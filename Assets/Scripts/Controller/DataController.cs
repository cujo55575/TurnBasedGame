using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    public static int MAX_PLAYER_HEALTH = 10;
    public static int INITIAL_COINS = 10;
    public static int EACH_ROUND_COINS = 10;
}

[Serializable]
public class BaseEmojiData
{
    public string baseId;
    public Color color;
    public string name;
    public int health;
    // public int maxHealth;
    public int damage;
    public int cost = 3;
    public int tier;
    public ENUM.EMOJI type;
    public ENUM.Emoji_Level_Layer layer;
    public ENUM.ABILITY_TYPE abilityType;
    public string abilityDescription;
    public string abilityName;
    public List<string> weaponIds = new List<string>();
}

[Serializable]
public class CertainBoostData
{
    public string ID;
    public string Name;
    public int Tier;
    public int Cost;
    public int DamageBonus;
    public int HealthBonus;
    public int TakeLessDamage;
    public string Description;
    public bool IsRandomTarget;
    public int TargetAmount;
    public bool IsTemporary;
    public Sprite Avatar;
}

[Serializable]
public class StatModifier
{
    public string statName;
    public int boost = 0;
    public float multiplier = 0f;
}

[Serializable]
public class UpgradeData
{
    public string id;
    public string name;
    public List<StatModifier> modifiers = new List<StatModifier>();
}

[Serializable]
public class WeaponData : UpgradeData
{
    public Color attackColor;
    public float flightSpeed;
    public float SPS;
    public float DPS;
    public int Damage
    {
        get { return (int)(DPS / SPS); }
    }
}
[Serializable]
public class BaseTierData
{
    public string baseId;
    public List<ENUM.EMOJI> tier = new List<ENUM.EMOJI>();
}
[Serializable]
public class TierData
{
    public static List<EmojiData> GetCustomizedEmojiList()
    {
        List<EmojiData> tier = new List<EmojiData>();
        int tierCount = DataController.instance.baseTierList["Tier5"].tier.Count;
        for (int i = 0; i < tierCount; i++)
        {
            string randomEmojiId = DataController.instance.baseTierList["Tier5"].tier[i].ToString();
            EmojiData data = EmojiData.MakeEmoji(randomEmojiId);
            tier.Add(data);
        }
        return tier;
    }
    public static List<EmojiData> GetEmojiList(string tierId, int tierCount)
    {
        List<EmojiData> tier = new List<EmojiData>();
        for (int i = 0; i < tierCount; i++)
        {
            int count = DataController.instance.baseTierList[tierId].tier.Count;
            string randomEmojiId = DataController.instance.baseTierList[tierId].tier[UnityEngine.Random.Range(0, count)].ToString();
            EmojiData data = EmojiData.MakeEmoji(randomEmojiId);
            tier.Add(data);
        }
        return tier;
    }
    public static List<string> GetEmojisStringList(string tierId)
    {
        List<string> tier = new List<string>();
        int count = DataController.instance.baseTierList[tierId].tier.Count;
        for (int i = 0; i < count; i++)
        {
            string randomEmojiId = DataController.instance.baseTierList[tierId].tier[i].ToString();
            tier.Add(randomEmojiId);
        }
        return tier;
    }
}
[Serializable]
public class EmojiData
{
    public string emojiId;
    public string baseEmojiId;
    public List<string> upgradeIds = new List<string>();
    public ENUM.EMOJI type;
    public ENUM.ABILITY_TYPE abilityType;
    public int health;
    public int upgradedHealth;
    public int maxHealth;
    public int damage;
    public int upgradedDamage;
    public int level;
    public int levelStackTimes;
    public int cost;
    public int tier;
    public int upgradeTime;
    public int emojiLayerForLevel;
    public List<int> emojiLayerList = new List<int> { 2, 5 };
    public bool isAbilityExecuted = false;
    public string abilityDescription;
    public string abilityName;
    public List<int> levelLayer;
    public CertainBoostData Upgrade;
    public BaseEmojiData baseEmoji
    {
        get { return DataController.instance.baseEmojis[baseEmojiId]; }
    }

    public static EmojiData MakeEmoji(string baseEmojiId)
    {
        var baseEmoji = DataController.instance.baseEmojis[baseEmojiId];
        return new EmojiData
        {
            emojiId = baseEmojiId + "-" + Guid.NewGuid().ToString(),
            baseEmojiId = baseEmojiId,
            upgradeIds = baseEmoji.weaponIds,
            health = baseEmoji.health,
            upgradedHealth = baseEmoji.health,
            type = baseEmoji.type,
            abilityType = baseEmoji.abilityType,
            // damage = weapons[0].Damage,
            // upgradedDamage = weapons[0].Damage,
            damage = baseEmoji.damage,
            upgradedDamage = baseEmoji.damage,
            cost = baseEmoji.cost,
            tier = baseEmoji.tier,
            level = 1,
            levelStackTimes = 0,
            upgradeTime = 0,
            Upgrade = new CertainBoostData(),
            abilityDescription = baseEmoji.abilityDescription,
            abilityName = baseEmoji.abilityName,
        };
    }
}
[Serializable]
public class PlacementData
{
    public int index;
    public string emojiId;
    public string emojiName;
}

[Serializable]
public class BoardData
{
    public List<PlacementData> placements = new List<PlacementData>();
}

[Serializable]
public class PlayerGameState
{
    public int health;
    public int round;
    public int winRound;
    public int loseRound;
    public int wins;
    public int coins;
    public int tierUnlock;
    public MatchEndGui.Result lastMatchResult;
    public PlayerGameState()
    {
        Reset();
    }
    // GameConstants gameConstants = new GameConstants();
    public void Reset()
    {
        health = GameConstants.MAX_PLAYER_HEALTH;
        round = 0;
        wins = 0;
        coins = GameConstants.INITIAL_COINS;
        tierUnlock = 1;
        lastMatchResult = MatchEndGui.Result.DRAW;
        GameConstants.EACH_ROUND_COINS = GameConstants.INITIAL_COINS;
    }
}

[Serializable]
public class PlayerData
{
    public string id;
    public string name;
    public List<EmojiData> emojis = new List<EmojiData>();
    public BoardData board = new BoardData();
    public PlayerGameState gameState = new PlayerGameState();
    public void Reset()
    {
        ShopController.Instance.baseEmojiIds = new List<string>();
        GameConstants.EACH_ROUND_COINS = 10;
        GameConstants.INITIAL_COINS = 10;
        board = new BoardData();
        emojis = new List<EmojiData>();
        gameState = new PlayerGameState();
    }

    public EmojiData GetEmoji(string emojiId)
    {
        foreach (var emoji in emojis)
        {
            if (emoji.emojiId == emojiId)
            {
                return emoji;
            }
        }
        return null;
    }
}

public class DataController : MonoBehaviour
{
    public Dictionary<string, UpgradeData> upgrades;
    public Dictionary<string, BaseEmojiData> baseEmojis;
    public Dictionary<string, BaseTierData> baseTierList;
    public List<List<string>> emojiDataList = new List<List<string>>();
    public string currentPlayerId;
    public int totalRound;
    public static DataController instance;
    private void Awake()
    {
        totalRound = 300;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public bool isReady
    {
        get { return _isReady; }
    }
    private bool _isReady = false;

    IBackendInterface backend;

    [SerializeField] PlayerData _currentPlayer;
    public PlayerData currentPlayer
    {
        get { return _currentPlayer; }
    }

    public List<PlayerData> nextRoundData;
    public bool nextRoundLoaded = false;
    public bool isCustomizedTierList = false;
    public void StartLoadingData(string myPlayerId)
    {
        currentPlayerId = myPlayerId;
        StartCoroutine(LoadDataCoroutine());
    }

    public void StartLoadingNextRoundSnapshots(int roundNumber)
    {
        nextRoundLoaded = false;
        backend.GetRoundData(roundNumber, (List<PlayerData> snapshots) =>
        {
            nextRoundData = snapshots;
            nextRoundLoaded = true;
        });
    }

    public IEnumerator LoadDataCoroutine()
    {
        backend = new MockBackend();

        _isReady = false;
        while (!_isReady)
        {
            if (upgrades != null && baseEmojis != null && baseTierList != null && currentPlayer != null)
            {
                _isReady = true;
                break;
            }

            backend.GetConstants((ConstantData constants) =>
            {
                upgrades = new Dictionary<string, UpgradeData>();
                foreach (var key in constants.weapons.Keys)
                {
                    upgrades[key] = constants.weapons[key];
                }
                baseEmojis = constants.baseEmojis;
                baseTierList = constants.baseTiers;
            });

            CreateTierListForLocalPlayer();

            backend.GetPlayerData(currentPlayerId, (PlayerData playerData) =>
            {
                _currentPlayer = playerData;
            });

            yield return new WaitForEndOfFrame();
        }
    }
    // Modified for 10 wins :::SLINT:::
    private void CreateTierListForLocalPlayer()
    {
        for (int i = 0; i < totalRound; i++)
        {
            emojiDataList.Add(EmojiNameList(GetTierName(i)));
        }
    }
    public string GetTierName(int i)
    {
        if (i >= 0 && i <= 1) return "Tier1";
        else if (i >= 2 && i <= 3) return "Tier2";
        else if (i >= 4 && i <= 5) return "Tier3";
        else if (i >= 6 && i <= 7) return "Tier4";
        else return "Tier5";
    }
    public List<string> EmojiNameList(string tierId)
    {
        List<string> tier = new List<string>();
        for (int i = 0; i < baseTierList[tierId].tier.Count; i++)
        {
            string data = baseTierList[tierId].tier[i].ToString();
            tier.Add(data);
        }
        return tier;
    }
    public void SavePlayerSnapshot(PlayerData playerData)
    {
        backend.SavePlayerSnapshot(playerData);
    }
}
