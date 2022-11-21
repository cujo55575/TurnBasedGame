using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static ENUM;
using NaughtyAttributes;
public class EmojiAbilityHandler : MonoBehaviour
{
    [SerializeField] GameObject smokePrefab;

    public List<EmojiData> playerEmojis;
    public List<EmojiData> enemyEmojis;
    public static System.Action onGetAffection;
    public static System.Action<string, string> onGetAbilityEffect;
    public static System.Action<string, List<string>> onGetAbilityEffects;
    public static System.Action<int> onGetCertainboostPriceChange;
    public static System.Action<EmojiData> onAbilityUsing;
    public static EmojiAbilityHandler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    //Use this function for checking Ability in all cases
    public void CheckEmojiAbility(EmojiData emojiData)
    {
        Debug.Log("Check Ability Emoji " + emojiData.baseEmojiId);

        onAbilityUsing?.Invoke(emojiData);

        ABILITY_TYPE type = emojiData.abilityType;

        switch (type)
        {
            case ABILITY_TYPE.Sell:
                ExecuteAbilityOnSell(emojiData);
                break;
            case ABILITY_TYPE.Buy:
                ExecuteAbilityOnBuy(emojiData);
                break;
            case ABILITY_TYPE.Faint:
                ExecuteAbilityOnFaint(emojiData);
                break;
            case ABILITY_TYPE.BeforeFaint:
                ExecuteAbilityBeforeFaint(emojiData);
                break;
            case ABILITY_TYPE.FaintSummon:
                ExecuteAbiliyOnFaintSummon(emojiData);
                break;
            case ABILITY_TYPE.LevelUp:
                ExecuteAbilityOnLevelUp(emojiData);
                break;
            case ABILITY_TYPE.StartOfBattle:
                ExecuteAbilityOnStartBattle(emojiData);
                break;
            case ABILITY_TYPE.BeforeAttack:
                ExecuteAbilityBeforeAttack(emojiData);
                break;
            //case ABILITY_TYPE.FriendHurt:
            //    ExecuteAbilityOnFriendHurt(emojiData);
            //    break;
            case ABILITY_TYPE.Hurt:
                ExecuteAbilityOnHurt(emojiData);
                break;
            //case ABILITY_TYPE.TierChange:
            //    ExecuteAbilityOnTierChange(emojiData);
            //    break;
            case ABILITY_TYPE.StartOfTurn:
                ExecuteAbilityStartOfTurn(emojiData);
                break;
            case ABILITY_TYPE.EndTurn:
                ExecuteAbilityEndTurn(emojiData);
                break;
            case ABILITY_TYPE.FriendAheadAttacks:
                ExecuteAbilityFriendAheadAttacks(emojiData);
                break;
            case ABILITY_TYPE.FriendAheadFaints:
                ExecuteAbilityFriendAheadFaints(emojiData);
                break;
            case ABILITY_TYPE.EmojiEatsUpgrade:
                ExecuteAbilityEmojiEatsUpgrade(emojiData);
                break;
            case ABILITY_TYPE.KnockOut:
                ExecuteAbilityKnockOut(emojiData);
                break;
            case ABILITY_TYPE.FriendSummoned:
                ExecuteAbilityFriendSummoned(emojiData);
                break;
            default:
                break;
        }
    }

    //There are 2 types of abilities, the one that happens in Battle only affects emojis in Battle, and the one that happens in Shop affects emojis till the end of 10 matches




    //------------------In Shop------------------------- 
    //Buy
    //Sell
    //Tier Change
    //Friend sold
    //Start of battle
    #region In Shop
    private void ExecuteAbilityOnSell(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Angel:
                AngleRework(emojiData);
                break;
            //case EMOJI.Dead:
            //    Deadv2(emojiData);
            //    break;
            //case EMOJI.Suprise:
            //    Suprisev2(emojiData);
            //    break;
            case EMOJI.AngryDevil:
                AngryDevilReword(emojiData);
                break;
            case EMOJI.Wink:
                WinkRework(emojiData);
                break;
        }
    }
    private void ExecuteAbilityOnTierChange(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Sick:
                //Sick(emojiData);
                break;
        }
    }
    private void ExecuteAbilityOnBuy(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Amaze:
                Amaze(emojiData);
                break;
            case EMOJI.HoldingTears:
                HoldingTearsRework(emojiData);
                break;
            case EMOJI.Chuckle:
                Chuckle(emojiData);
                break;
        }
    }
    public void ExecuteAbilityOnFriendSold(EmojiData emojiData)
    {
        Crazy(emojiData);
        // EMOJI type = emojiData.type;
        // switch (type)
        // {
        //     case EMOJI.Crazy:

        //         break;
        // }
    }

    public void ExecuteAbilityEmojiEatsUpgrade(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Kiss:
                Kiss(emojiData);
                break;
            case EMOJI.Enjoy:
                Enjoy(emojiData);
                break;
        }
    }
    #endregion

    //------------------In Battle------------------------- 
    //Before Attack
    //Hurt
    //Friend Hurt
    //Faint
    #region In Battle

    public void ExecuteAbilityOnStartBattle(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Dead:
                DeadRework(emojiData);
                break;
            case EMOJI.Sick:
                SickRework(emojiData);
                break;
            case EMOJI.Roar:
                Roar(emojiData);
                break;
            case EMOJI.Think:
                Think(emojiData);
                break;
            case EMOJI.LoveFace:
                Loveface(emojiData);
                break;
            //Deal 2/4/6 damage to a random enemy.
            case EMOJI.Heart:
                Heart(emojiData);
                break;
        }
    }

    public void ExecuteAbilityFriendSummoned(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Silence:
                Silence(emojiData);
                break;
            case EMOJI.Hot:
                Hot(emojiData);
                break;
            default:
                break;
        }
    }
    public void ExecuteAbilityKnockOut(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            default:
                break;
        }
    }
    public void ExecuteAbilityFriendAheadFaints(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Cheerful:
                Cheerful(emojiData);
                break;
            default:
                break;
        }
    }
    public void ExecuteAbilityFriendAheadAttacks(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Happy:
                Happy(emojiData);
                break;
            default:
                break;
        }
    }
    public void ExecuteAbilityEndTurn(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Nonchalant:
                Nonchalant(emojiData);
                break;
            case EMOJI.RollEyes:
                RollEyes(emojiData);
                break;
            case EMOJI.Sleep:
                Sleep(emojiData);
                break;
            case EMOJI.Nap:
                Nap(emojiData);
                break;
            //Give right most friend 2/4/6 damage and 3/6/9 hp
            case EMOJI.Poop:
                Poop(emojiData);
                break;
            default:
                break;
        }

    }
    public void ExecuteAbilityStartOfTurn(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Cry:
                Cry(emojiData);
                break;
            //Discount shop food(upgrades) by 1/2/3 gold
            case EMOJI.Like:
                Like(emojiData);
                break;
            default:
                break;
        }
    }

    public void ExecuteAbilityBeforeAttack(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            //Deal 1/2/3 Damage to friend behind. Before Attack
            case EMOJI.Monocle:
                MonocleRework(emojiData);
                break;
        }
    }
    public void ExecuteAbilityOnHurt(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            //Gain +4/8/12 damage on Hurt
            case EMOJI.StarStruck:
                StarStruckRework(emojiData);
                break;
            case EMOJI.Teary:
                Teary(emojiData);
                break;
            case EMOJI.Drunk:
                Drunk(emojiData);
                break;
            case EMOJI.Fever:
                Fever(emojiData);
                break;
            case EMOJI.Money:
                Money(emojiData);
                break;
        }
    }
    public void ExecuteAbilityOnFaint(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            //Give a random friend +2/4/6 damage & +1/2/3 hp after faint
            case EMOJI.Angry:
                AngryRework(emojiData);
                break;
            //Deal Damge to ALL +2/4/6 damage on Faint
            case EMOJI.Tongue:
                Tongue(emojiData);
                break;
            //Summon  Sad  +5/10/15 damage +5/10/15 hp on Faint
            case EMOJI.Sad:
                Sad(emojiData);
                break;
        }
    }
    public void ExecuteAbiliyOnFaintSummon(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            //Summon 1/2/3 Dirty surprise with 1dmg &1 hp on Faint
            case EMOJI.Surprise:
                Suprise(emojiData);
                break;
            //Summon 1 zombie attracted +1/2/3 damage +1/2/3 hp on Faint
            case EMOJI.Attracted:
                Attracted(emojiData);
                break;
            //Summon Level  tier 3 pet with 2/4/6 hp and 2/4/6 damage
            case EMOJI.Laugh:
                Laugh(emojiData);
                break;
            //Two kisses with health and damage  2/4/6 hp and 2/4/6 damage
            case EMOJI.Smooch:
                Smooch(emojiData);
                break;
            //Summon 1/2/3 Surprise with 1hp and 2 attack
            case EMOJI.Disgust:
                Disgust(emojiData);
                break;
        }
    }
    public void ExecuteAbilityBeforeFaint(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            //Give the two nearest friends behind +1/2/3 damage and +1/2/3 hp before Faint
            case EMOJI.Glasses:
                Glasses(emojiData);
                break;
            case EMOJI.Worry:
                Worry(emojiData);
                break;
            // Give 1/2/3 Armor(HP) friends behind.
            case EMOJI.Dislike:
                Dislike(emojiData);
                break;
        }
    }
    private void ExecuteAbilityOnLevelUp(EmojiData emojiData)
    {
        EMOJI type = emojiData.type;
        switch (type)
        {
            case EMOJI.Cold:
                Cold(emojiData);
                break;
        }
    }
    #endregion
    //Give a random friend +1/2/3 Hp On Friend Sold
    private void Crazy(EmojiData emojiData)
    {
        List<EmojiData> emojis = DataController.instance.currentPlayer.emojis;
        EmojiData friendSoldEmojiData = null;
        if (emojis.Count > 1)
        {
            foreach (EmojiData emoji in emojis)
            {
                if (emoji.type == EMOJI.Crazy)
                {
                    friendSoldEmojiData = emoji;
                    break;
                }
            }
            if (friendSoldEmojiData != null)
            {
                EmojiData m_emojiData = null;
                List<EmojiData> m_emojis = new List<EmojiData>();
                foreach (EmojiData item in emojis)
                {
                    if (item.emojiId != friendSoldEmojiData.emojiId && item.emojiId != emojiData.emojiId)
                    {
                        m_emojis.Add(item);
                    }
                }
                if (m_emojis.Count > 0) m_emojiData = m_emojis[Random.Range(0, m_emojis.Count)];
                if (m_emojiData != null)
                {
                    m_emojiData.health += friendSoldEmojiData.level;
                    m_emojiData.upgradedHealth = m_emojiData.health;
                    onGetAbilityEffect?.Invoke(m_emojiData.emojiId, m_emojiData.emojiId);
                    onGetAffection?.Invoke();
                    Debug.Log("Executed Crazy Ability::: " + m_emojiData.emojiId);
                }
            }
        }
    }
    //Discount shop food(upgrades) by 1/2/3 gold
    private void Like(EmojiData emojiData)
    {
        if (!emojiData.isAbilityExecuted)
        {
            onGetCertainboostPriceChange?.Invoke(emojiData.level);
            foreach (var item in BattleController.instance.boardInfos[0].emojis)
            {
                //Check Is my team
                if (item.data.emojiId == emojiData.emojiId)
                {
                    OnGetBuff?.Invoke(item);
                    break;
                }
            }
            emojiData.isAbilityExecuted = true;
            Debug.Log("Executed Like Ability ");
        }
    }
    //Deal 2/4/6 damage to a random enemy.
    private void Heart(EmojiData emojiData)
    {
        var damage = emojiData.level * 2;
        var player = BattleController.instance.boardInfos[0];
        var enemy = BattleController.instance.boardInfos[1];
        var myEmoji = player.emojis[Random.Range(0, player.emojis.Count)];
        var opponentEmoji = enemy.emojis[Random.Range(0, enemy.emojis.Count)];

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                //Deal damage to their team
                opponentEmoji.data.health = opponentEmoji.data.health - damage;
                opponentEmoji.txtHealth.text = opponentEmoji.data.health <= 0 ? 0.ToString() : opponentEmoji.data.health.ToString();
                OnGetDamage?.Invoke(opponentEmoji);
                OnGetRealDamage?.Invoke(opponentEmoji);
                Debug.Log("Executed Heart Ability to Enemy Team");
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                //Deal damage to my team
                myEmoji.data.health = myEmoji.data.health - damage;
                myEmoji.txtHealth.text = myEmoji.data.health <= 0 ? 0.ToString() : myEmoji.data.health.ToString();
                OnGetDamage?.Invoke(myEmoji);
                OnGetRealDamage?.Invoke(opponentEmoji);
                Debug.Log("Executed Heart Ability to Player Team");
                break;
            }
        }
    }
    //Give right most friend 2/4/6 damage and 3/6/9 hp
    private void Poop(EmojiData emojiData)
    {
        BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
        BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];
        if (playerBoardInfo.dataList.Contains(emojiData))
        {
            PoopAbilityHelper(playerBoardInfo, emojiData);
        }
        else if (enemyBoardInfo.dataList.Contains(emojiData))
        {
            PoopAbilityHelper(enemyBoardInfo, emojiData);
        }
    }
    private void PoopAbilityHelper(BoardInfo boardInfo, EmojiData emojiData)
    {
        if (boardInfo.dataList.IndexOf(emojiData) != boardInfo.dataList.Count - 1)
        {
            EmojiController emoji = boardInfo.emojis[boardInfo.emojis.Count - 1];
            emoji.data.health += emojiData.level * 3;
            emoji.data.damage += emojiData.level * 2;
            OnGetBuff?.Invoke(emoji);
            Debug.Log("Executed Dislike Ability ");
        }
    }
    // Give 1/2/3 Armor(HP) friends behind.
    private void Dislike(EmojiData emojiData)
    {
        BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
        BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];
        if (playerBoardInfo.dataList.Contains(emojiData))
        {
            DislikeAbilityHelper(playerBoardInfo, emojiData);
        }
        else if (enemyBoardInfo.dataList.Contains(emojiData))
        {
            DislikeAbilityHelper(enemyBoardInfo, emojiData);
        }
    }
    private void DislikeAbilityHelper(BoardInfo boardInfo, EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();
        if (boardInfo.dataList.IndexOf(emojiData) != boardInfo.dataList.Count - 1)
        {
            foreach (EmojiController item in boardInfo.emojis)
            {
                if (item.data.emojiId != emojiData.emojiId)
                {
                    emojis.Add(item);
                }
            }
        }
        if (emojis.Count != 0)
        {
            foreach (EmojiController item in emojis)
            {
                item.data.health += emojiData.level;
                OnGetBuff?.Invoke(item);
            }
            Debug.Log("Executed Dislike Ability ");
        }
    }
    //Give the two nearest friends behind +1/2/3 damage and +1/2/3 hp before Faint
    private void Glasses(EmojiData emojiData)
    {
        if (BattleController.instance.boardInfos[0].dataList.Contains(emojiData))
        {
            List<EmojiController> m_playerEmojis = BattleController.instance.boardInfos[0].emojis;
            if (m_playerEmojis.Count <= 1) return;
            if (m_playerEmojis.Count >= 3)
            {
                EmojiController emojisOne = m_playerEmojis[1];
                emojisOne.data.health += emojiData.level;
                emojisOne.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojisOne);
                EmojiController emojisTwo = m_playerEmojis[2];
                emojisTwo.data.health += emojiData.level;
                emojisTwo.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojisTwo);
            }
            else if (m_playerEmojis.Count >= 2)
            {
                EmojiController emojis = m_playerEmojis[1];
                emojis.data.health += emojiData.level;
                emojis.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojis);
            }

        }
        else if (BattleController.instance.boardInfos[1].dataList.Contains(emojiData))
        {
            List<EmojiController> m_enemyEmojis = BattleController.instance.boardInfos[1].emojis;
            if (m_enemyEmojis.Count <= 1) return;
            if (m_enemyEmojis.Count >= 3)
            {
                EmojiController emojisOne = m_enemyEmojis[1];
                emojisOne.data.health += emojiData.level;
                emojisOne.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojisOne);
                EmojiController emojisTwo = m_enemyEmojis[2];
                emojisTwo.data.health += emojiData.level;
                emojisTwo.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojisTwo);
            }
            else if (m_enemyEmojis.Count >= 2)
            {
                EmojiController emojis = m_enemyEmojis[1];
                emojis.data.health += emojiData.level;
                emojis.data.damage += emojiData.level;
                OnGetBuff?.Invoke(emojis);
            }
        }
        Debug.Log("Executed Glasses Ability");
    }

    //Gives a random friend +1/2/3 hp and +1/2/3 damage on Buy
    private void Amaze(EmojiData emojiData)
    {
        List<EmojiData> emojis = new List<EmojiData>();
        List<string> idList = new List<string>();
        EmojiData data = null;
        foreach (EmojiData emoji in DataController.instance.currentPlayer.emojis)
        {
            if (emoji.emojiId != emojiData.emojiId)
            {
                emojis.Add(emoji);
            }
        }
        if (emojis.Count >= 1) data = emojis[Random.Range(0, emojis.Count)];
        if (data != null)
        {
            data.health += emojiData.level;
            data.damage += emojiData.level;
            data.upgradedHealth = data.health;
            data.upgradedDamage = data.damage;
            idList.Add(data.emojiId);
            Debug.Log("Executed Amaze Ability");
        }
        if (idList.Count > 0)
        {
            onGetAbilityEffects?.Invoke(emojiData.emojiId, idList);
            onGetAffection?.Invoke();
        }
    }
    //Summon 1/2/3 Surprise with 1hp and 2 attack
    private void Disgust(EmojiData emojiData)
    {
        if (emojiData.health <= 0)
        {
            if (!emojiData.isAbilityExecuted)
            {
                BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
                BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];

                if (playerBoardInfo.dataList.Contains(emojiData))
                {
                    EmojiData m_emojiData = EmojiData.MakeEmoji(EMOJI.Surprise.ToString());
                    DisgustAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, m_emojiData, emojiData);
                }
                else if (enemyBoardInfo.dataList.Contains(emojiData))
                {
                    EmojiData m_emojiData = EmojiData.MakeEmoji(EMOJI.Surprise.ToString());
                    DisgustAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, m_emojiData, emojiData);
                }
                Debug.Log("Executed Disgust Ability ");
            }
        }
    }
    private void DisgustAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData m_emojiData, EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();
        foreach (EmojiController emoji in boardInfo.emojis)
        {
            if (emoji.data.emojiId != emojiData.emojiId)
            {
                emojis.Add(emoji);
            }
        }
        for (int i = 0; i < emojiData.level; i++)
        {
            EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(m_emojiData.baseEmojiId), boardInfo.emojiTranList[i]);
            emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
            emoji.name = emoji.name.Replace("(Clone)", " Suprise");
            emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
            emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
            boardInfo.emojis[i] = emoji;
            emoji.data.baseEmojiId = m_emojiData.baseEmojiId;
            emoji.data.emojiId = m_emojiData.emojiId;
            emoji.data.type = m_emojiData.type;
            emoji.data.abilityType = m_emojiData.abilityType;
            emoji.data.tier = 2;
            emoji.data.health = 1;
            emoji.data.damage = 2;
            OnGetBuff?.Invoke(emoji);
        }
        List<EmojiController> m_emojis = new List<EmojiController>();
        if (emojiData.level == 2 || emojiData.level == 3)
        {
            for (int i = 0; i < emojiData.level; i++)
            {
                m_emojis.Add(boardInfo.emojis[i]);
            }
        }
        if (emojiData.level == 2 || emojiData.level == 3)
        {
            boardInfo.emojis = new List<EmojiController>();
            foreach (EmojiController item in m_emojis)
            {
                boardInfo.emojis.Add(item);
            }
        }
        Debug.Log("SupriseAbilityHelper:: boardinfo.emojis count " + boardInfo.emojis.Count);
        if (emojiData.level >= 2)
        {
            foreach (EmojiController emoji in emojis)
            {
                boardInfo.emojis.Add(emoji);
            }
        }
    }
    //Two kisses with health and damage  2/4/6 hp and 2/4/6 damage
    private void Smooch(EmojiData emojiData)
    {
        if (emojiData.health <= 0)
        {
            if (!emojiData.isAbilityExecuted)
            {
                BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
                BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];
                EmojiData m_emojiData = EmojiData.MakeEmoji(EMOJI.Kiss.ToString());
                if (playerBoardInfo.dataList.Contains(emojiData))
                {
                    SmoochAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, m_emojiData, emojiData);
                }
                else if (enemyBoardInfo.dataList.Contains(emojiData))
                {
                    SmoochAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, m_emojiData, emojiData);
                }
                Debug.Log("Executed Smooch Ability");
            }
        }
    }
    private void SmoochAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData m_emojiData, EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();
        foreach (EmojiController emoji in boardInfo.emojis)
        {
            if (emoji.data.emojiId != emojiData.emojiId)
            {
                emojis.Add(emoji);
            }
        }
        for (int i = 0; i < 2; i++)
        {
            EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(m_emojiData.baseEmojiId), boardInfo.emojiTranList[i]);
            emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
            emoji.name = emoji.name.Replace("(Clone)", " Kiss");
            emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
            emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
            boardInfo.emojis[i] = emoji;
            emoji.data.baseEmojiId = m_emojiData.baseEmojiId;
            emoji.data.emojiId = m_emojiData.emojiId;
            emoji.data.type = m_emojiData.type;
            emoji.data.abilityType = m_emojiData.abilityType;
            emoji.data.tier = 3;
            emoji.data.health = emojiData.level * 2;
            emoji.data.damage = emojiData.level * 2;
            OnGetBuff?.Invoke(emoji);
            emoji.data.isAbilityExecuted = true;
        }
        List<EmojiController> m_emojis = new List<EmojiController>();
        for (int i = 0; i < 2; i++)
        {
            m_emojis.Add(boardInfo.emojis[i]);
        }

        boardInfo.emojis = new List<EmojiController>();
        foreach (EmojiController item in m_emojis)
        {
            boardInfo.emojis.Add(item);
        }
        Debug.Log("SmoochAbilityHelper:: boardinfo.emojis count " + boardInfo.emojis.Count);
        foreach (EmojiController emoji in emojis)
        {
            boardInfo.emojis.Add(emoji);
        }
    }
    private string[] tier3Emojis = new string[] { EMOJI.Worry.ToString(), EMOJI.Teary.ToString(), EMOJI.Drunk.ToString() ,
    EMOJI.Silence.ToString(),EMOJI.Nonchalant.ToString(),EMOJI.Happy.ToString(),EMOJI.Cheerful.ToString(),EMOJI.Kiss.ToString(),
    EMOJI.Smooch.ToString(),EMOJI.Chuckle.ToString(),EMOJI.Roar.ToString()};
    //Summon Level  tier 3 pet with 2/4/6 hp and 2/4/6
    private void Laugh(EmojiData emojiData)
    {
        if (emojiData.health <= 0)
        {
            if (!emojiData.isAbilityExecuted)
            {
                BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
                BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];

                if (playerBoardInfo.dataList.Contains(emojiData))
                {
                    EmojiData m_emojiData = EmojiData.MakeEmoji(tier3Emojis[Random.Range(0, tier3Emojis.Length)]);
                    LaughAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, m_emojiData, emojiData);
                }
                else if (enemyBoardInfo.dataList.Contains(emojiData))
                {
                    EmojiData m_emojiData = EmojiData.MakeEmoji(tier3Emojis[Random.Range(0, tier3Emojis.Length)]);
                    LaughAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, m_emojiData, emojiData);
                }
                Debug.Log("Executed Laugh Ability ");
            }
        }
    }
    private void LaughAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData m_emojiData, EmojiData emojiData)
    {
        EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(m_emojiData.baseEmojiId), boardInfo.emojiTranList[0]);
        emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
        emoji.name = emoji.name.Replace("(Clone)", " Tier3 Emoji");
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
        emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
        boardInfo.emojis[0] = emoji;
        emoji.data.baseEmojiId = m_emojiData.baseEmojiId;
        emoji.data.emojiId = m_emojiData.emojiId;
        emoji.data.type = m_emojiData.type;
        emoji.data.abilityType = m_emojiData.abilityType;
        emoji.data.tier = 3;
        emoji.data.health = emojiData.level * 2;
        emoji.data.damage = emojiData.level * 2;
        OnGetBuff?.Invoke(emoji);
        emoji.data.isAbilityExecuted = true;
    }
    //Summon 1/2/3 Dirty surprise with 1dmg &1 hp on Faint
    private void Suprise(EmojiData emojiData)
    {
        if (emojiData.health <= 0)
        {
            if (!emojiData.isAbilityExecuted)
            {
                BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
                BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];

                if (playerBoardInfo.dataList.Contains(emojiData))
                {
                    SupriseAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, emojiData);
                }
                else if (enemyBoardInfo.dataList.Contains(emojiData))
                {
                    SupriseAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, emojiData);
                }
                Debug.Log("Executed Suprise Ability ");
            }
        }
    }
    private void SupriseAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();
        foreach (EmojiController emoji in boardInfo.emojis)
        {
            if (emoji.data.emojiId != emojiData.emojiId)
            {
                emojis.Add(emoji);
            }
        }
        for (int i = 0; i < emojiData.level; i++)
        {
            EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(emojiData.baseEmojiId), boardInfo.emojiTranList[i]);
            emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
            emoji.name = emoji.name.Replace("(Clone)", "Dirty Suprise");
            emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
            emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
            emoji.transform.GetChild(0).GetChild(0).GetChild(0).
            GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 147, 255, 255);
            boardInfo.emojis[i] = emoji;
            emoji.data.health = 1;
            emoji.data.damage = 1;
            OnGetBuff?.Invoke(emoji);
            emoji.data.isAbilityExecuted = true;
        }
        List<EmojiController> m_emojis = new List<EmojiController>();
        if (emojiData.level == 2 || emojiData.level == 3)
        {
            for (int i = 0; i < emojiData.level; i++)
            {
                m_emojis.Add(boardInfo.emojis[i]);
            }
        }
        if (emojiData.level == 2 || emojiData.level == 3)
        {
            boardInfo.emojis = new List<EmojiController>();
            foreach (EmojiController item in m_emojis)
            {
                boardInfo.emojis.Add(item);
            }
        }
        Debug.Log("SupriseAbilityHelper:: boardinfo.emojis count " + boardInfo.emojis.Count);
        if (emojiData.level >= 2)
        {
            foreach (EmojiController emoji in emojis)
            {
                boardInfo.emojis.Add(emoji);
            }
        }
    }
    //Summon  Sad  +5/10/15 damage +5/10/15 hp on Faint
    private void Sad(EmojiData emojiData)
    {
        if (emojiData.health <= 0)
        {
            if (!emojiData.isAbilityExecuted)
            {
                BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
                BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];

                if (playerBoardInfo.dataList.Contains(emojiData))
                {
                    SadAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, emojiData);
                }
                else if (enemyBoardInfo.dataList.Contains(emojiData))
                {
                    SadAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, emojiData);
                }
                Debug.Log("Executed Attracted Ability ");
            }
        }
    }
    private void SadAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData emojiData)
    {
        EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(emojiData.baseEmojiId), boardInfo.emojiTranList[0]);
        emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
        emoji.name = emoji.name.Replace("(Clone)", "Sad");
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
        emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 147, 255, 255);
        boardInfo.emojis[0] = emoji;
        emoji.data.health = emojiData.level * 5;
        emoji.data.damage = emojiData.level * 5;
        OnGetBuff?.Invoke(emoji);
        emoji.data.isAbilityExecuted = true;
    }
    //Summon 1 zombie attracted +1/2/3 damage +1/2/3 hp on Faint
    private void Attracted(EmojiData emojiData)
    {
        if (!emojiData.isAbilityExecuted)
        {
            BoardInfo playerBoardInfo = BattleController.instance.boardInfos[0];
            BoardInfo enemyBoardInfo = BattleController.instance.boardInfos[1];

            if (playerBoardInfo.dataList.Contains(emojiData))
            {
                if (emojiData.health <= 0)
                {
                    AttractedAbilityHelper(playerBoardInfo, BattleController.instance.currentPlayer, emojiData);
                }
            }
            else if (enemyBoardInfo.dataList.Contains(emojiData))
            {
                if (emojiData.health <= 0)
                {
                    AttractedAbilityHelper(enemyBoardInfo, BattleController.instance.currentEnemy, emojiData);
                }
            }
            Debug.Log("Executed Attracted Ability ");
        }
    }
    private void AttractedAbilityHelper(BoardInfo boardInfo, PlayerData playerData, EmojiData emojiData)
    {
        EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(emojiData.baseEmojiId), boardInfo.emojiTranList[0]);
        emoji.Setup(boardInfo, playerData, playerData.GetEmoji(emojiData.emojiId));
        emoji.name = emoji.name.Replace("(Clone)", "Zombie Attracted");
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
        emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>().color = new Color(200, 200, 200, 255);
        emoji.transform.GetChild(0).GetChild(0).GetChild(0).
        GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 147, 255, 255);
        boardInfo.emojis[0] = emoji;
        emoji.data.health = emojiData.level;
        emoji.data.damage = emojiData.level;
        OnGetBuff?.Invoke(emoji);
        emoji.data.isAbilityExecuted = true;
    }
    //Give all friends +1/2/3 hp and +1/2/3 damage on LevelUp.
    private void Cold(EmojiData emojiData)
    {
        List<EmojiData> emojis = DataController.instance.currentPlayer.emojis;
        List<string> idList = new List<string>();
        foreach (EmojiData data in emojis)
        {
            if (data.emojiId != emojiData.emojiId)
            {
                data.health += emojiData.level;
                data.damage += emojiData.level;
                data.upgradedHealth = data.health;
                data.upgradedDamage = data.damage;
                idList.Add(data.emojiId);
            }
        }
        onGetAbilityEffects?.Invoke(emojiData.emojiId, idList);
        onGetAffection?.Invoke();
        Debug.Log("Executed Cold Ability");
    }
    //Deal Damge to ALL +2/4/6 damage on Faint
    private void Tongue(EmojiData emojiData)
    {
        List<EmojiController> playerEmojis = BattleController.instance.boardInfos[0].emojis;
        List<EmojiController> enemyEmojis = BattleController.instance.boardInfos[1].emojis;
        bool isPlayer = false;
        bool isEnemy = false;
        foreach (EmojiController item in playerEmojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                isPlayer = true;
                break;
            }
        }
        if (isPlayer)
        {
            foreach (EmojiController item in enemyEmojis)
            {
                item.data.health -= emojiData.level * 2;
                OnGetDamage?.Invoke(item);
                OnGetRealDamage?.Invoke(item);
            }
        }

        foreach (EmojiController item in enemyEmojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                isEnemy = true;
                break;
            }
        }
        if (isEnemy)
        {
            foreach (EmojiController item in playerEmojis)
            {
                item.data.health -= emojiData.level * 2;
                OnGetDamage?.Invoke(item);
                OnGetRealDamage?.Invoke(item);
            }
        }
    }
    #region An

    //Action only for battle
    public System.Action<EmojiController> OnGetBuff;
    public System.Action<EmojiController> OnGetDamage;
    public System.Action<EmojiController> OnGetRealDamage;

    //Faint Give a random friend Damage and Health
    private void AngryRework(EmojiData emojiData)
    {
        Debug.Log("Ability Angry");

        List<EmojiController> emojis = new List<EmojiController>();

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[0].emojis;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[1].emojis;
                break;
            }
        }
        Debug.Log(emojis.Count);

        var emoji = emojis[Random.Range(0, emojis.Count)];

        if (emojis.Count > 1)
        {
            while (emoji.data.emojiId == emojiData.emojiId)
            {
                emoji = emojis[Random.Range(0, emojis.Count)];
            }

            var bonusDamage = 2 * emoji.data.level;
            var bonusHealth = emoji.data.level;

            foreach (var item in emojis)
            {
                if (item.data.emojiId.CompareTo(emoji.data.emojiId) == 0)
                {
                    item.data.damage += bonusDamage;
                    item.data.health += bonusHealth;

                    OnGetBuff?.Invoke(item);
                    break;
                }
            }
        }
    }
    //Sell Give 2 random friends Health
    private void AngryDevilReword(EmojiData emojiData)
    {
        var emojis = DataController.instance.currentPlayer.board.placements;
        var listID = new List<string>();

        if (emojis.Count > 1)
        {
            var emojiFriend1 = emojis[Random.Range(0, emojis.Count)];

            while (emojiFriend1.emojiId == emojiData.emojiId)
            {
                emojiFriend1 = emojis[Random.Range(0, emojis.Count)];
            }
            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (emojiFriend1.emojiId == item.emojiId)
                {
                    item.health += emojiData.level;
                    item.upgradedHealth += emojiData.level;
                }
            }
            listID.Add(emojiFriend1.emojiId);

            emojis.Remove(emojiFriend1);

            var emojiFriend2 = emojis[Random.Range(0, emojis.Count)];

            if (emojis.Count > 1)
            {
                while (emojiFriend2.emojiId == emojiData.emojiId)
                {
                    emojiFriend2 = emojis[Random.Range(0, emojis.Count)];
                }
                foreach (var item in DataController.instance.currentPlayer.emojis)
                {
                    if (emojiFriend2.emojiId == item.emojiId)
                    {
                        item.health += emojiData.level;
                        item.upgradedHealth += emojiData.level;
                    }
                }
                listID.Add(emojiFriend2.emojiId);
            }

            onGetAbilityEffects?.Invoke(emojiData.emojiId, listID);
            onGetAffection?.Invoke();
        }

    }

    //Sell Give shop pets HP Damage
    private void AngleRework(EmojiData emojiData)
    {
        var listID = new List<string>();
        var emojis = ShopController.Instance.buyableEmojis;
        foreach (var item in emojis)
        {
            item.health += emojiData.level;
            item.upgradedHealth += emojiData.level;
            listID.Add(item.emojiId);
        }

        onGetAbilityEffects?.Invoke(emojiData.emojiId, listID);
        onGetAffection?.Invoke();
    }

    //Deal damage to first enemy 
    private void DeadRework(EmojiData emojiData)
    {
        var damage = emojiData.level;
        var myEmoji = BattleController.instance.boardInfos[0].emojis[0];
        var opponentEmoji = BattleController.instance.boardInfos[1].emojis[0];

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                //Deal damage to their team
                opponentEmoji.data.health = opponentEmoji.data.health - damage;
                opponentEmoji.txtHealth.text = opponentEmoji.data.health <= 0 ? 0.ToString() : opponentEmoji.data.health.ToString();
                OnGetDamage?.Invoke(opponentEmoji);
                OnGetRealDamage?.Invoke(opponentEmoji);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                //Deal damage to my team
                myEmoji.data.health = myEmoji.data.health - damage;
                myEmoji.txtHealth.text = myEmoji.data.health <= 0 ? 0.ToString() : myEmoji.data.health.ToString();
                OnGetDamage?.Invoke(myEmoji);
                OnGetRealDamage?.Invoke(opponentEmoji);
                break;
            }
        }
    }

    //Sell Gain1/2/3 gold 
    private void WinkRework(EmojiData emojiData)
    {
        PlayerData enemy = DataController.instance.currentPlayer;
        PlayerGameState gameState = enemy.gameState;
        var newCoin = gameState.coins + emojiData.level;
        Debug.Log("newCoin before = " + newCoin);
        gameState.coins = newCoin;
        Debug.Log("newCoin after = " + gameState.coins);

        onGetAbilityEffect?.Invoke(emojiData.emojiId, emojiData.emojiId);
        onGetAffection?.Invoke();
    }

    //Start of battle Give friend behind Damage
    private void SickRework(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[0].emojis;
                var index = BattleController.instance.boardInfos[0].emojis.IndexOf(item);
                //have more than 1 emoji and emojiData is not the last one
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = (int)System.Math.Round(0.5f * emojiData.level * emojiData.damage) + friend.data.damage;
                    OnGetBuff?.Invoke(friend);
                }
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[1].emojis;
                var index = BattleController.instance.boardInfos[1].emojis.IndexOf(item);
                //have more than 1 emoji and emojiData is not the last one
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = (int)System.Math.Round(0.5f * emojiData.level * emojiData.damage) + friend.data.damage;
                    OnGetBuff?.Invoke(friend);
                }
                break;
            }
        }
    }

    //Before Attack Deal Damage to friend behind 
    private void MonocleRework(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[0].emojis;
                var index = BattleController.instance.boardInfos[0].emojis.IndexOf(item);
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = emojiData.level + emojiData.damage;
                    OnGetBuff?.Invoke(friend);
                }
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[1].emojis;
                var index = BattleController.instance.boardInfos[1].emojis.IndexOf(item);
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = emojiData.level + emojiData.damage;
                    OnGetBuff?.Invoke(friend);
                }
                break;
            }
        }
    }

    //Buy Copy 50%/100%/150% HP of the friend who has highest HP
    private void HoldingTearsRework(EmojiData emojiData)
    {
        var highiestHP = 1f;
        var lv = 1f;

        List<EmojiData> emojis = DataController.instance.currentPlayer.emojis;

        if (emojis.Count > 1)
        {
            for (int i = 0; i < emojis.Count; i++)
            {
                if (emojis[i].emojiId != emojiData.emojiId)
                {
                    if (emojis[i].health > highiestHP)
                    {
                        highiestHP = emojis[i].health;
                        lv = emojis[i].level;
                    }
                }
            }

            float bonusHP = highiestHP * ((50f / 100f) * lv);
            if (bonusHP > 0 && bonusHP < 1)
            {
                bonusHP = 1;
            }

            var bonusHPInt = (int)System.Math.Round(bonusHP);

            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (item.emojiId.CompareTo(emojiData.emojiId) == 0)
                {
                    item.health += bonusHPInt;
                    item.upgradedHealth = item.health;

                    onGetAbilityEffect?.Invoke(item.emojiId, item.emojiId);
                    onGetAffection?.Invoke();

                    break;
                }
            }
        }

    }

    //Hurt Gain Damage
    private void StarStruckRework(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 4 + item.data.damage;
                item.data.upgradedDamage = item.data.damage;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 4 + item.data.damage;
                item.data.upgradedDamage = item.data.damage;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Start of turn Gain Coin
    private void Cry(EmojiData emojiData)
    {
        GameConstants.EACH_ROUND_COINS += emojiData.level;

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Before faint Deal 50%/100%/150% ATK to adjacent pets.
    private void Worry(EmojiData emojiData)
    {
        var damage = emojiData.level * (50f / 100f);
        if (damage > 0 && damage < 1)
        {
            damage = 1;
        }

        var bonusDamageInt = (int)System.Math.Round(damage);

        if (BattleController.instance.boardInfos[0].emojis.Count > 0)
        {
            var myEmoji = BattleController.instance.boardInfos[0].emojis[0];

            foreach (var item in BattleController.instance.boardInfos[1].emojis)
            {
                //Check Is opponent team
                if (item.data.emojiId == emojiData.emojiId)
                {
                    //Deal damage to my team
                    myEmoji.data.health = myEmoji.data.health - bonusDamageInt;
                    myEmoji.txtHealth.text = myEmoji.data.health <= 0 ? 0.ToString() : myEmoji.data.health.ToString();
                    OnGetDamage?.Invoke(myEmoji);
                    OnGetRealDamage?.Invoke(myEmoji);
                    break;
                }
            }
        }

        if (BattleController.instance.boardInfos[1].emojis.Count > 0)
        {
            var opponentEmoji = BattleController.instance.boardInfos[1].emojis[0];
            foreach (var item in BattleController.instance.boardInfos[0].emojis)
            {
                //Check Is my team
                if (item.data.emojiId == emojiData.emojiId)
                {
                    //Deal damage to their team
                    opponentEmoji.data.health = opponentEmoji.data.health - bonusDamageInt;
                    opponentEmoji.txtHealth.text = opponentEmoji.data.health <= 0 ? 0.ToString() : opponentEmoji.data.health.ToString();
                    OnGetDamage?.Invoke(opponentEmoji);
                    OnGetRealDamage?.Invoke(opponentEmoji);
                    break;
                }
            }
        }

    }

    //Hurt Deal 2/4/6 damage to a random enemy.
    private void Teary(EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[1].emojis;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[0].emojis;
                break;
            }
        }
        Debug.Log(emojis.Count);

        var emoji = emojis[Random.Range(0, emojis.Count)];

        var damage = 2 * emoji.data.level;

        emoji.data.health = emoji.data.health - damage;

        OnGetDamage?.Invoke(emoji);
        OnGetRealDamage?.Invoke(emoji);
    }

    //Hurt Give friend behind Damage HP
    private void Drunk(EmojiData emojiData)
    {
        Debug.Log("Drunk");
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[0].emojis;
                var index = BattleController.instance.boardInfos[0].emojis.IndexOf(item);
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = emojiData.level * 2 + friend.data.damage;
                    friend.data.health = emojiData.level * 2 + friend.data.health;
                    OnGetBuff?.Invoke(friend);
                    break;
                }
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                var emojis = BattleController.instance.boardInfos[1].emojis;
                var index = BattleController.instance.boardInfos[1].emojis.IndexOf(item);
                if (emojis.Count > 1 && index != emojis.Count - 1)
                {
                    var friend = emojis[index + 1];
                    friend.data.damage = emojiData.level * 2 + friend.data.damage;
                    friend.data.health = emojiData.level * 2 + friend.data.health;
                    OnGetBuff?.Invoke(friend);
                    break;
                }
            }
        }
    }


    //Friend summoned Gain Damage HP
    private void Silence(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 2 + item.data.damage;
                item.data.health = item.data.level * 2 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 2 + item.data.damage;
                item.data.health = item.data.level * 2 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //End turn Gain Damage HP
    private void Nonchalant(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level + item.data.damage;
                item.data.health = item.data.level + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level + item.data.damage;
                item.data.health = item.data.level + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Friend ahead attacks Gain Damage HP
    //When A attacks, check the one behind A, will be in Attack part in the Battle
    private void Happy(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 2 + item.data.damage;
                item.data.health = item.data.level * 2 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 2 + item.data.damage;
                item.data.health = item.data.level * 2 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Friend ahead faints Gain Armor 
    //When A faints, check the one behind A, will be in Attack part in the Battle
    private void Cheerful(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.health = item.data.level + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.health = item.data.level + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Emoji eats upgrade , Give it HP
    private void Kiss(EmojiData emojiData)
    {
        List<EmojiData> emojis = DataController.instance.currentPlayer.emojis;

        foreach (var item in DataController.instance.currentPlayer.emojis)
        {
            if (item.emojiId.CompareTo(emojiData.emojiId) == 0)
            {
                item.health += emojiData.level;
                item.upgradedHealth = item.health;

                onGetAbilityEffect?.Invoke(item.emojiId, item.emojiId);
                onGetAffection?.Invoke();

                break;
            }
        }
    }

    //Buy If you lost last battle give all friends damage + hp
    private void Chuckle(EmojiData emojiData)
    {
        if (DataController.instance.currentPlayer.gameState.lastMatchResult == MatchEndGui.Result.LOSE)
        {
            List<string> listID = new List<string>();
            List<EmojiData> emojis = DataController.instance.currentPlayer.emojis;

            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (item.emojiId.CompareTo(emojiData.emojiId) != 0)
                {
                    item.health += emojiData.level;
                    item.upgradedHealth = emojiData.level;

                    item.damage += emojiData.level;
                    item.upgradedDamage = emojiData.level;

                    listID.Add(item.emojiId);
                }
            }

            onGetAbilityEffects?.Invoke(emojiData.emojiId, listID);
            onGetAffection?.Invoke();
        }
    }

    //Start of the battle : Deal 3 damage to the lowest HP enemy. Triggers 1/2/3 times
    private void Roar(EmojiData emojiData)
    {
        var lowestHP = 100f;
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        EmojiController emojiHasLowestHP = new EmojiController();
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isOpponentTeam = true;
                break;
            }
        }
        //Emoji has ability is my team
        if (isMyTeam)
        {
            if (BattleController.instance.boardInfos[1].emojis.Count > 0)
            {
                lowestHP = BattleController.instance.boardInfos[1].emojis[0].data.health;

                //Find weakest in opponent team
                foreach (var item in BattleController.instance.boardInfos[1].emojis)
                {
                    if (item.data.health <= lowestHP)
                    {
                        lowestHP = item.data.health;
                        emojiHasLowestHP = item;
                    }
                }

                emojiHasLowestHP.data.health = emojiHasLowestHP.data.health - emojiData.level * 3;

                OnGetDamage?.Invoke(emojiHasLowestHP);
                OnGetRealDamage?.Invoke(emojiHasLowestHP);
            }
        }

        //Emoji has ability is opponent team
        if (isOpponentTeam)
        {
            if (BattleController.instance.boardInfos[0].emojis.Count > 0)
            {
                lowestHP = BattleController.instance.boardInfos[0].emojis[0].data.health;

                //Find weakest in my team
                foreach (var item in BattleController.instance.boardInfos[0].emojis)
                {
                    if (item.data.health <= lowestHP)
                    {
                        lowestHP = item.data.health;
                        emojiHasLowestHP = item;
                    }
                }

                emojiHasLowestHP.data.health = emojiHasLowestHP.data.health - emojiData.level * 3;

                OnGetDamage?.Invoke(emojiHasLowestHP);
                OnGetRealDamage?.Invoke(emojiHasLowestHP);
            }

        }
    }

    private void RollEyes(EmojiData emojiData)
    {
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        EmojiController emoji = new EmojiController();
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                emoji = item;
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                emoji = item;
                isOpponentTeam = true;
                break;
            }
        }
        //Emoji has ability is my team
        if (isMyTeam)
        {
            //Find friend has level 3 in my team
            foreach (var item in BattleController.instance.boardInfos[0].emojis)
            {
                if (item.data.level == 3)
                {
                    emojiData.health += emojiData.level * 2;
                    emojiData.damage += emojiData.level * 2;

                    OnGetBuff?.Invoke(emoji);
                }
            }
        }

        //Emoji has ability is opponent team
        if (isOpponentTeam)
        {
            //Find weakest in my team
            foreach (var item in BattleController.instance.boardInfos[1].emojis)
            {
                if (item.data.level == 3)
                {
                    emojiData.health += emojiData.level * 2;
                    emojiData.damage += emojiData.level * 2;

                    OnGetBuff?.Invoke(emoji);
                }
            }
        }
    }

    //Friend summoned Give it Damage
    private void Hot(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                BattleController.instance.boardInfos[0].emojis[0].data.damage = item.data.level * 2 + BattleController.instance.boardInfos[0].emojis[0].data.damage;
                OnGetBuff?.Invoke(BattleController.instance.boardInfos[0].emojis[0]);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                BattleController.instance.boardInfos[1].emojis[0].data.damage = item.data.level * 2 + BattleController.instance.boardInfos[1].emojis[0].data.damage;
                OnGetBuff?.Invoke(BattleController.instance.boardInfos[1].emojis[0]);
                break;
            }
        }
    }

    //End Turn Copy ability from friend ahead as lvl 1/2/3 until the end of battle.
    private void Sleep(EmojiData emojiData)
    {
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        EmojiController emoji = new EmojiController();
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                emoji = item;
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                emoji = item;
                isOpponentTeam = true;
                break;
            }
        }

        if (isMyTeam)
        {
            var emojiIndex = BattleController.instance.boardInfos[0].emojis.IndexOf(emoji);
            //It means the list count > 1 because index > 1 means there are more than 1 emoji in list, if only have one emoji, this index must be = 1
            if (emojiIndex > 1)
            {
                var emojiTarget = BattleController.instance.boardInfos[0].emojis[emojiIndex - 1];
                if (emoji.data.level == emojiTarget.data.level)
                {
                    emoji.data.health = emojiTarget.data.health;
                    emoji.data.damage = emojiTarget.data.damage;
                    OnGetBuff(emoji);
                }
            }
        }
        else if (isOpponentTeam)
        {
            var emojiIndex = BattleController.instance.boardInfos[1].emojis.IndexOf(emoji);
            //It means the list count > 1 because index > 1 means there are more than 1 emoji in list, if only have one emoji, this index must be = 1
            if (emojiIndex > 1)
            {
                var emojiTarget = BattleController.instance.boardInfos[1].emojis[emojiIndex - 1];
                if (emoji.data.level == emojiTarget.data.level)
                {
                    emoji.data.health = emojiTarget.data.health;
                    emoji.data.damage = emojiTarget.data.damage;
                    OnGetBuff(emoji);
                }
            }
        }
    }

    //End turn Give two level 2 and 3 friends 
    private void Nap(EmojiData emojiData)
    {
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isOpponentTeam = true;
                break;
            }
        }

        if (isMyTeam)
        {
            var listFriend = new List<EmojiController>();
            foreach (var item in BattleController.instance.boardInfos[0].emojis)
            {
                //Not include emoji self 
                if (item.data.emojiId != emojiData.emojiId)
                {
                    //Only level 2 or 3
                    if (item.data.level == 2 || item.data.level == 3)
                    {
                        //Only accept 2 friends
                        if (listFriend.Count < 3)
                        {
                            listFriend.Add(item);
                        }
                    }
                }
            }

            foreach (var item in listFriend)
            {
                item.data.health = emojiData.level + item.data.health;
                item.data.damage = emojiData.level + item.data.damage;
                OnGetBuff(item);
            }
        }

        else if (isOpponentTeam)
        {
            var listFriend = new List<EmojiController>();
            foreach (var item in BattleController.instance.boardInfos[1].emojis)
            {
                //Not include emoji self 
                if (item.data.emojiId != emojiData.emojiId)
                {
                    //Only level 2 or 3
                    if (item.data.level == 2 || item.data.level == 3)
                    {
                        //Only accept 2 friends
                        if (listFriend.Count < 3)
                        {
                            listFriend.Add(item);
                        }
                    }
                }
            }

            foreach (var item in listFriend)
            {
                item.data.health = emojiData.level + item.data.health;
                item.data.damage = emojiData.level + item.data.damage;
                OnGetBuff(item);
            }
        }
    }

    //Start of battle Reduce Hp of the highest enemy by 33%/66%/99%
    public void Think(EmojiData emojiData)
    {
        var highestHP = 0f;
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        EmojiController emojiHasHighestHP = new EmojiController();
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isOpponentTeam = true;
                break;
            }
        }
        //Emoji has ability is my team
        if (isMyTeam)
        {
            if (BattleController.instance.boardInfos[1].emojis.Count > 0)
            {
                highestHP = BattleController.instance.boardInfos[1].emojis[0].data.health;

                //Find weakest in opponent team
                foreach (var item in BattleController.instance.boardInfos[1].emojis)
                {
                    if (item.data.health >= highestHP)
                    {
                        highestHP = item.data.health;
                        emojiHasHighestHP = item;
                    }
                }

                var damage = (int)System.Math.Round((33f / 100f) * emojiData.level);
                emojiHasHighestHP.data.health = emojiHasHighestHP.data.health - damage;

                OnGetDamage?.Invoke(emojiHasHighestHP);
                OnGetRealDamage?.Invoke(emojiHasHighestHP);
            }
        }

        //Emoji has ability is opponent team
        if (isOpponentTeam)
        {
            if (BattleController.instance.boardInfos[0].emojis.Count > 0)
            {
                highestHP = BattleController.instance.boardInfos[0].emojis[0].data.health;

                //Find weakest in my team
                foreach (var item in BattleController.instance.boardInfos[0].emojis)
                {
                    if (item.data.health >= highestHP)
                    {
                        highestHP = item.data.health;
                        emojiHasHighestHP = item;
                    }
                }

                var damage = (int)System.Math.Round((33f / 100f) * emojiData.level);
                emojiHasHighestHP.data.health = emojiHasHighestHP.data.health - damage;
            }


            OnGetDamage?.Invoke(emojiHasHighestHP);
            OnGetRealDamage?.Invoke(emojiHasHighestHP);
        }
    }
    //Hurt Gain Damage & HP
    private void Fever(EmojiData emojiData)
    {
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 3 + item.data.damage;
                item.data.health = item.data.level * 3 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                item.data.damage = item.data.level * 3 + item.data.damage;
                item.data.health = item.data.level * 3 + item.data.health;
                OnGetBuff?.Invoke(item);
                break;
            }
        }
    }

    //Hurt Deal 6/12/18 damage to a random enemy
    private void Money(EmojiData emojiData)
    {
        List<EmojiController> emojis = new List<EmojiController>();

        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[1].emojis;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            if (item.data.emojiId == emojiData.emojiId)
            {
                emojis = BattleController.instance.boardInfos[0].emojis;
                break;
            }
        }

        var emoji = emojis[Random.Range(0, emojis.Count)];

        var damage = 6 * emoji.data.level;

        emoji.data.health = emoji.data.health - damage;

        OnGetDamage?.Invoke(emoji);
        OnGetRealDamage?.Invoke(emoji);
    }

    //Emoji eats upgrade Give 2 random friends
    private void Enjoy(EmojiData emojiData)
    {
        var emojis = DataController.instance.currentPlayer.board.placements;
        var listID = new List<string>();

        var emojiFriend1 = emojis[Random.Range(0, emojis.Count)];

        if (emojis.Count > 1)
        {
            while (emojiFriend1.emojiId == emojiData.emojiId)
            {
                emojiFriend1 = emojis[Random.Range(0, emojis.Count)];
            }
            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (emojiFriend1.emojiId == item.emojiId)
                {
                    item.health += emojiData.level;
                    item.upgradedHealth += emojiData.level;

                    item.damage += emojiData.level;
                    item.upgradedDamage += emojiData.level;
                }
            }
            listID.Add(emojiFriend1.emojiId);
        }

        emojis.Remove(emojiFriend1);

        var emojiFriend2 = emojis[Random.Range(0, emojis.Count)];

        if (emojis.Count > 1)
        {
            while (emojiFriend2.emojiId == emojiData.emojiId)
            {
                emojiFriend2 = emojis[Random.Range(0, emojis.Count)];
            }
            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (emojiFriend2.emojiId == item.emojiId)
                {
                    item.health += emojiData.level;
                    item.upgradedHealth += emojiData.level;

                    item.damage += emojiData.level;
                    item.upgradedDamage += emojiData.level;
                }
            }
            listID.Add(emojiFriend2.emojiId);
        }

        onGetAbilityEffects?.Invoke(emojiData.emojiId, listID);
        onGetAffection?.Invoke();
    }

    //Start of battle Deal 8 damage to last enemy. Repeat 1/2/3 time(s).
    private void Loveface(EmojiData emojiData)
    {
        bool isMyTeam = false;
        bool isOpponentTeam = false;
        foreach (var item in BattleController.instance.boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isMyTeam = true;
                break;
            }
        }
        foreach (var item in BattleController.instance.boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiData.emojiId)
            {
                isOpponentTeam = true;
                break;
            }
        }

        //Is My Team
        if (isMyTeam)
        {
            //Find last emoji in Opponent Team
            if (BattleController.instance.boardInfos[1].emojis.Count > 0)
            {
                var lastEnemy = BattleController.instance.boardInfos[1].emojis[BattleController.instance.boardInfos[1].emojis.Count - 1];
                lastEnemy.data.health = lastEnemy.data.health - 8 * emojiData.level;

                OnGetDamage?.Invoke(lastEnemy);
                OnGetRealDamage?.Invoke(lastEnemy);

            }
        }
        //Is Opponent Team
        if (isOpponentTeam)
        {
            // Find last emoji in My Team
            if (BattleController.instance.boardInfos[0].emojis.Count > 0)
            {
                var lastEnemy = BattleController.instance.boardInfos[0].emojis[BattleController.instance.boardInfos[0].emojis.Count - 1];
                lastEnemy.data.health = lastEnemy.data.health - 8 * emojiData.level;

                OnGetDamage?.Invoke(lastEnemy);
                OnGetRealDamage?.Invoke(lastEnemy);

            }
        }
    }

    #endregion
}