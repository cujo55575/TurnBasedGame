using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData
{
    public Dictionary<string, WeaponData> weapons;
    public Dictionary<string, BaseEmojiData> baseEmojis;
    public Dictionary<string, BaseTierData> baseTiers;
}

public interface IBackendInterface
{
    public delegate void GetPlayerSnapshotCallback(List<PlayerData> playerData);
    public delegate void GetConstantsCallback(ConstantData constantData);
    public delegate void GetPlayerDataCallback(PlayerData playerData);

    public void GetRoundData(int roundNumber, GetPlayerSnapshotCallback callback);
    public void GetPlayerData(string playerId, GetPlayerDataCallback callback);  // can only load my own data
    public void GetConstants(GetConstantsCallback callback);
    public void SavePlayerSnapshot(PlayerData playerSnapshot);  // can only save my own data as a snapshot, also save it to my player data
}

public class MockBackend : MonoBehaviour, IBackendInterface
{
    static List<List<PlayerData>> playerSnapshots = new List<List<PlayerData>>();
    private List<List<EmojiData>> emojijDataList = new List<List<EmojiData>>();
    private List<string> improveTierList = new List<string>();

    private void InititalizeFakePlayerSnapshots()
    {
        improveTierList = new List<string>();
        if (DataController.instance.isCustomizedTierList)
        {
            for (int i = 0; i < DataController.instance.totalRound; i++)
            {
                emojijDataList.Add(GetCustomizedEmojiTierList());
            }
            playerSnapshots = new List<List<PlayerData>>();
            for (int i = 0; i <= DataController.instance.totalRound; i++)
            {
                playerSnapshots.Add(PlayerDataList(DataController.instance.totalRound));
            }
        }
        else
        {
            for (int i = 0; i < DataController.instance.totalRound; i++)
            {
                emojijDataList.Add(GetEmojiTierList(DataController.instance.GetTierName(i)));
                // emojijDataList.Add(GetImproveEmojisTierList(DataController.instance.tierName[i]));
            }
            playerSnapshots = new List<List<PlayerData>>();
            for (int i = 0; i <= DataController.instance.totalRound; i++)
            {
                playerSnapshots.Add(PlayerDataList(DataController.instance.totalRound));
            }
        }
    }
    private List<PlayerData> PlayerDataList(int round)
    {
        List<PlayerData> playerDataList = new List<PlayerData>();
        for (int i = 0; i < round; i++)
        {
            playerDataList.Add(SetUpPlayerData(GetEmojis(i), i));
        }
        return playerDataList;
    }
    private List<EmojiData> GetEmojiTierList(string tierIndex)
    {
        var list = new List<EmojiData>();
        for (int i = 0; i < TierData.GetEmojiList(tierIndex, EmojiCount(tierIndex)).Count; i++)
        {
            list.Add(TierData.GetEmojiList(tierIndex, EmojiCount(tierIndex))[i]);
        }
        return list;
    }

    private void AddDataToEmojiImproveTierList(string tierIndex)
    {
        for (int i = 0; i < TierData.GetEmojisStringList(tierIndex).Count; i++)
        {
            improveTierList.Add(TierData.GetEmojisStringList(tierIndex)[i]);
        }
    }

    private List<EmojiData> GetImproveEmojisTierList(string tierIndex)
    {
        AddDataToEmojiImproveTierList(tierIndex);
        List<EmojiData> tier = new List<EmojiData>();
        for (int i = 0; i < improveTierList.Count; i++)
        {
            string randomEmojiId = improveTierList[Random.Range(0, improveTierList.Count)];
            EmojiData data = EmojiData.MakeEmoji(randomEmojiId);
            tier.Add(data);
        }
        return tier;
    }
    private List<EmojiData> GetCustomizedEmojiTierList()
    {
        var list = new List<EmojiData>();
        for (int i = 0; i < TierData.GetCustomizedEmojiList().Count; i++)
        {
            list.Add(TierData.GetCustomizedEmojiList()[i]);
        }
        return list;
    }
    private int EmojiCount(string tierIndex)
    {
        if (tierIndex == "Tier1") return 3;
        else if (tierIndex == "Tier2" || tierIndex == "Tier3") return 4;
        return 5;
    }
    public void GetConstants(IBackendInterface.GetConstantsCallback callback)
    {
        var constants = new ConstantData();

        // upgrades
        constants.weapons = GetWeaponDatas();

        // base emojis
        constants.baseEmojis = GetEmojiDatas();

        //base tierdatas
        constants.baseTiers = GetTierDatas();

        callback(constants);

        InititalizeFakePlayerSnapshots();
    }

    public void GetPlayerData(string playerId, IBackendInterface.GetPlayerDataCallback callback)
    {
        var me = new PlayerData
        {
            id = "me",
            name = "It's Me"
        };
        callback(me);
    }

    public static List<PlayerData> getPlayerSnapshot(int round)
    {
        if (round >= playerSnapshots.Count)
        {
            return null;
        }

        return playerSnapshots[round];
    }
    public void GetRoundData(int roundNumber, IBackendInterface.GetPlayerSnapshotCallback callback)
    {
        if (roundNumber >= playerSnapshots.Count)
        {
            callback(null);
            return;
        }

        callback(playerSnapshots[roundNumber]);
    }
    public void SavePlayerSnapshot(PlayerData playerSnapshot)
    {
        while (playerSnapshot.gameState.round >= playerSnapshots.Count)
        {
            playerSnapshots.Add(new List<PlayerData>());
        }
        playerSnapshots[playerSnapshot.gameState.round].Add(playerSnapshot);
    }

    #region Private Functions
    private Dictionary<string, BaseEmojiData> GetEmojiDatas()
    {
        Dictionary<string, BaseEmojiData> baseEmojiDict = new Dictionary<string, BaseEmojiData>();

        string emojiPath = string.Format(GLOBALCONST.FORMAT_SCRIPTABLE_PATH, GLOBALCONST.FOLDER_EMOJIS);
        Emoji[] emojis = Resources.LoadAll<Emoji>(emojiPath);
        foreach (Emoji emoji in emojis)
        {
            baseEmojiDict[emoji.emojiData.baseId] = emoji.emojiData;
        }

        return baseEmojiDict;
    }
    private Dictionary<string, BaseTierData> GetTierDatas()
    {
        Dictionary<string, BaseTierData> baseTierDict = new Dictionary<string, BaseTierData>();

        string tierPath = string.Format(GLOBALCONST.FORMAT_SCRIPTABLE_PATH, GLOBALCONST.FOLDER_TIERS);
        Tier[] tiers = Resources.LoadAll<Tier>(tierPath);
        foreach (Tier tier in tiers)
        {
            baseTierDict[tier.tierData.baseId] = tier.tierData;
        }

        return baseTierDict;
    }
    private Dictionary<string, WeaponData> GetWeaponDatas()
    {
        Dictionary<string, WeaponData> weaponDict = new Dictionary<string, WeaponData>();

        string weaponPath = string.Format(GLOBALCONST.FORMAT_SCRIPTABLE_PATH, GLOBALCONST.FOLDER_WEAPONS);
        Weapon[] weapons = Resources.LoadAll<Weapon>(weaponPath);

        foreach (Weapon weapon in weapons)
        {
            weaponDict[weapon.weaponData.id] = weapon.weaponData;
        }

        return weaponDict;
    }

    private PlayerData SetUpPlayerData(List<EmojiData> emojis, int index)
    {
        List<EmojiData> data = new List<EmojiData>();
        for (int i = 0; i < emojis.Count; i++)
        {
            data.Add(emojis[i]);
        }
        PlayerData playerData = new PlayerData
        {
            id = "player" + index,
            name = "Player " + index,
            emojis = new List<EmojiData>(data),
            board = new BoardData
            {
                placements = SetupPlacementDataList(data),
            },
        };
        return playerData;
    }
    private List<EmojiData> GetEmojis(int index)
    {
        List<EmojiData> emojis = new List<EmojiData>();
        for (int i = 0; i < TotalEmojis(index); i++)
        {
            emojis.Add(emojijDataList[index][i]);
        }
        return emojis;
    }
    //0,1,t1, 2,3,t2  4,5,t3 6,7,t4
    private int TotalEmojis(int index)
    {
        if (index >= 0 && index <= 1) return 3;
        else if (index >= 2 && index <= 5) return 4;
        else return 5;
    }
    private List<PlacementData> SetupPlacementDataList(List<EmojiData> emojis)
    {
        List<PlacementData> placements = new List<PlacementData>();
        for (int i = 0; i < emojis.Count; i++)
        {
            placements.Add(SetUpPlacementData(emojis[i], i));
        }
        return placements;
    }
    private PlacementData SetUpPlacementData(EmojiData emojiData, int index)
    {
        PlacementData placementData = new PlacementData
        {
            index = index,
            emojiId = emojiData.emojiId,
            emojiName = emojiData.baseEmojiId,
        };
        return placementData;
    }
    #endregion 
}
