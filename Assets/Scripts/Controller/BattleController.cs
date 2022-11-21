using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using static ENUM;

[Serializable]
public class BoardInfo
{
    public string playerId;
    public List<EmojiController> emojis;
    public List<EmojiData> dataList;
    public List<Transform> emojiTranList;
    public float direction;
    public float yRotation;

    public void EnableEmoji(int index, EmojiController enemy)
    {
        emojis[index].enabled = true;
        emojis[index].isCurrentEmoji = true;
    }
}
public class ModelUtils
{
    public static EmojiController GetDataFromResources(string name)
    {
        return Resources.Load<EmojiController>("Prefab/Emojis/" + name);
    }
}

public class BattleController : MonoBehaviour
{
    [SerializeField] MatchEndGui matchEndGui;
    [SerializeField] GameEndGui gameEndGui;
    public List<Transform> leftEmojiPositions;
    public List<Transform> rightEmojiPositions;
    public List<BoardInfo> boardInfos;
    [SerializeField] private TextMeshProUGUI m_teamName;
    [SerializeField] private TextMeshProUGUI m_teamOppositeName;
    [SerializeField] private GameObject BoardBase;
    public enum BattleState { NONE, ATTACK, FINISH }
    public BattleState state;
    public static int readyPlayerCount;
    public PlayerData currentPlayer;
    public PlayerData currentEnemy;
    [SerializeField] private bool m_isFinish;
    public static BattleController instance;

    public BattleController()
    {
        instance = this;
        state = BattleState.NONE;
    }


    private int ReduceHealth()
    {
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round >= 0 && round < 2) return 1;
        else if (round >= 2 && round <= 3) return 2;
        else return 3;
    }
    IEnumerator ShowMatchEndUI(MatchEndGui.Result result)
    {
        yield return new WaitForSeconds(2f);
        if (DataController.instance.currentPlayer.gameState.winRound == 10)
        {
            Debug.Log("MATCHENDGUI::: WIN");
            BoardBase.SetActive(false);
            gameEndGui.gameObject.SetActive(true);
            gameEndGui.Show(true);
        }
        else
        {
            BoardBase.SetActive(false);
            matchEndGui.Show(result);
        }
    }
    private void ClearBoards()
    {
        // clear board
        ClearEmojis(leftEmojiPositions);
        ClearEmojis(rightEmojiPositions);

    }
    public void ClearEmojis(List<Transform> emojiPositions)
    {
        foreach (var emojiPos in emojiPositions)
        {
            foreach (Transform child in emojiPos.transform)
            {
                if (child.GetComponent<EmojiController>())
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void SetupBattle(PlayerData player1, PlayerData player2)
    {
        m_isFinish = false;

        currentPlayer = player1;
        currentEnemy = player2;

        ClearBoards();
        Debug.Log(player1.emojis.Count + " SetupBattle");
        AddBoardInfoData(player1, player2);

        // RestoreEmojis(player1);
        RestoreEmojis(player2);

        SetupBoard(boardInfos[0], player1);
        SetupBoard(boardInfos[1], player2);

        //boardInfos[0].EnableEmoji(0, boardInfos[1].emojis[0]);
        //boardInfos[1].EnableEmoji(0, boardInfos[0].emojis[0]);

        gameObject.SetActive(true);

        //boardInfos[0].emojis[0].ReadyToAttackAnimation();
        //boardInfos[1].emojis[0].ReadyToAttackAnimation();

        readyPlayerCount = 0;
        state = BattleState.ATTACK;

        m_teamName.text = boardInfos[0].playerId;
        m_teamOppositeName.text = boardInfos[1].playerId;

        if (boardInfos[0].emojis.Count == 0)
        {
            CheckResult();

            return;
        }

        foreach (var item in boardInfos[0].emojis)
        {
            item.MovingAnimation(true, () =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    RunAbilitiyBeforeStartBattle();
                });
            });
        }
        foreach (var item in boardInfos[1].emojis)
        {
            item.MovingAnimation(false);
        }
    }
    private void AddBoardInfoData(PlayerData player1, PlayerData player2)
    {
        boardInfos = new List<BoardInfo>() { new BoardInfo { dataList=new List<EmojiData>(player1.emojis),yRotation = 170, direction = -82,emojiTranList=leftEmojiPositions},
                                             new BoardInfo { dataList=new List<EmojiData>(player2.emojis), yRotation = -170, direction = 82,emojiTranList=rightEmojiPositions} };
    }
    private void RestoreEmojis(PlayerData player)
    {
        foreach (var emoji in player.emojis)
        {
            emoji.health = emoji.baseEmoji.health;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetupBoard(BoardInfo boardInfo, PlayerData player)
    {
        ClearEmojis(boardInfo.emojiTranList);
        boardInfo.emojis = new List<EmojiController>();
        for (int i = 0; i < boardInfo.dataList.Count; i++)
        {
            EmojiController emoji = Instantiate(ModelUtils.GetDataFromResources(boardInfo.dataList[i].baseEmojiId), boardInfo.emojiTranList[i]);
            emoji.Setup(boardInfo, player, player.GetEmoji(boardInfo.dataList[i].emojiId));
            emoji.name = emoji.name.Replace("(Clone)", i.ToString());
            emoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
            emoji.transform.GetChild(0).localRotation = Quaternion.Euler(0, boardInfo.yRotation, 0);
            emoji.EmojiPosController = boardInfo.emojiTranList[i].GetComponent<EmojiPositionController>();
            boardInfo.emojis.Add(emoji);
            boardInfo.emojiTranList[i].GetComponent<EmojiPositionController>().SetEmojiController(emoji);
        }
        boardInfo.playerId = player.id;
    }

    //End of game,
    //plus one more round : done
    //calculate tier unlocked
    //check player health, health is 0 = lose instanly,
    //after finish 10 rounds, receive a sticker, reset everything

    #region An

    [SerializeField] private ParticleSystem m_fightParticle;
    [SerializeField] private ParticleSystem m_explosionParticlePrefab;
    [SerializeField] private ParticleSystem m_wasHitParticlePrefab;
    [SerializeField] private ParticleSystem m_buffParticlePrefab;
    [SerializeField] private ParticleSystem m_starParticlePrefab;
    private bool m_emojisInRightPlaces;
    private bool m_isAutoPlay;
    public Action<bool> onIsRightPlace;
    public Action<string> onShowMyEmojiDamageText;
    public Action<string> onShowOpponentEmojiDamageText;
    private void Start()
    {
        m_isAutoPlay = false;
        m_emojisInRightPlaces = false;
        onIsRightPlace?.Invoke(m_emojisInRightPlaces);
        BattleUI.onIsAutoPlay += OnIsAutoPlay;
        EmojiAbilityHandler.Instance.OnGetBuff += OnEmojiGetBuff;
        EmojiAbilityHandler.Instance.OnGetDamage += OnEmojiGetDamage;
        EmojiAbilityHandler.Instance.OnGetRealDamage += OnWasHit;
    }
    private void OnEmojiGetBuff(EmojiController m_emoji)
    {
        m_emoji.txtDamage.text = m_emoji.data.damage.ToString();
        m_emoji.txtHealth.text = m_emoji.data.health <= 0 ? 0.ToString() : m_emoji.data.health.ToString();
        OnPlayBuffParticle(m_emoji.transform);
    }

    private void OnEmojiGetDamage(EmojiController m_emoji)
    {
        m_emoji.txtDamage.text = m_emoji.data.damage.ToString();
        m_emoji.txtHealth.text = m_emoji.data.health <= 0 ? 0.ToString() : m_emoji.data.health.ToString();
        OnPlayWasHitParticle(m_emoji.transform.transform);
    }
    private void OnIsAutoPlay(bool isAutoPlay)
    {
        m_isAutoPlay = isAutoPlay;

        if (m_emojisInRightPlaces)
        {
            StartTurn();
        }
    }

    private void ResetPos()
    {
        foreach (var item in leftEmojiPositions)
        {
            item.GetComponent<EmojiPositionController>().SetEmojiController(null);
        }
        foreach (var item in rightEmojiPositions)
        {
            item.GetComponent<EmojiPositionController>().SetEmojiController(null);
        }

        for (int i = 0; i < boardInfos[0].emojis.Count; i++)
        {
            boardInfos[0].emojis[i].transform.parent = leftEmojiPositions[i];
            leftEmojiPositions[i].GetComponent<EmojiPositionController>().SetEmojiController(boardInfos[0].emojis[i]);
            boardInfos[0].emojis[i].BackToSeat();
        }

        for (int i = 0; i < boardInfos[1].emojis.Count; i++)
        {
            boardInfos[1].emojis[i].transform.parent = rightEmojiPositions[i];
            rightEmojiPositions[i].GetComponent<EmojiPositionController>().SetEmojiController(boardInfos[1].emojis[i]);
            boardInfos[1].emojis[i].BackToSeat();
        }
    }

    //This must be run before start battle
    //All abilities before start battle will stay here, gonna split it turn by turn later
    private void RunAbilitiyBeforeStartBattle()
    {
        m_emojisInRightPlaces = false;
        onIsRightPlace?.Invoke(m_emojisInRightPlaces);

        //must do some animations here and check after run abilitiy before start battle, the opponent's emoji or my emoji is still alive or not

        var myEmoji = boardInfos[0].emojis[0];
        var opponentEmoji = boardInfos[1].emojis[0];

        ResetPos();

        //eg : Ability "Start of battle " : deal 1/2/3 damage to the first enemy
        //just for testing, logic maybe not true
        ///////////////////////////////////////////////////////////

        //var damage = 1;
        //opponentEmoji.data.health = opponentEmoji.data.health - damage;
        //opponentEmoji.txtHealth.text = opponentEmoji.data.health.ToString();
        //OnPlayWasHitParticle(opponentEmoji.transform);

        //////////////////////////////////////////////////////////


        //eg : Ability "Start of battle " : Give friend ahead 50/100/150% damage
        //just for testing, logic maybe not true

        //var bonusDamage = 1;
        //myEmoji.data.damage = myEmoji.data.damage + bonusDamage;
        //myEmoji.txtDamage.text = myEmoji.data.damage.ToString();
        //OnPlayBuffParticle(myEmoji.transform);

        //////////////////////////////////////////////////////////
        ///

        foreach (var item in boardInfos[0].emojis)
        {
            if (item.data.abilityType == ENUM.ABILITY_TYPE.StartOfBattle)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(item.data);
            }
        }

        foreach (var item in boardInfos[1].emojis)
        {
            if (item.data.abilityType == ENUM.ABILITY_TYPE.StartOfBattle)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(item.data);
            }
        }

        EndTurn(myEmoji, opponentEmoji);
    }

    private int m_myEmojiHP = 0;
    private int m_opponentEmojiHP = 0;

    //Do something with ability Start of battle before run this function
    private void ReadyToStartTurn()
    {
        m_emojisInRightPlaces = false;
        onIsRightPlace?.Invoke(m_emojisInRightPlaces);

        var myEmoji = boardInfos[0].emojis[0];
        var opponentEmoji = boardInfos[1].emojis[0];

        // ADDED FIGHT OUTLINE HIGHLIGHT ENABLE
        myEmoji.SetBattleHighlight(true);
        opponentEmoji.SetBattleHighlight(true);
        //

        m_myEmojiHP = myEmoji.data.health;
        m_opponentEmojiHP = opponentEmoji.data.health;

        myEmoji.IsAlive = true;
        opponentEmoji.IsAlive = true;

        ResetPos();

        DOVirtual.DelayedCall(0.3f, () =>
        {
            myEmoji.ShakeAnimation(true);
            opponentEmoji.ShakeAnimation(false);

            //after 2 sides setup in the right places and emojis shakes shakes, press Play right now makes Start Turn
            m_emojisInRightPlaces = true;
            onIsRightPlace?.Invoke(m_emojisInRightPlaces);

            //Start Of Turn
            if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.StartOfTurn)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
            }
            if (opponentEmoji.data.abilityType == ENUM.ABILITY_TYPE.StartOfTurn)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
            }

            //Control this by Play Button an Auto Play Button;
            if (m_isAutoPlay)
            {
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    StartTurn();
                });
            }
        }).SetUpdate(false);
    }

    //Control this by Play Button an Auto Play Button;
    public void StartTurn()
    {
        if (m_emojisInRightPlaces)
        {
            var myEmoji = boardInfos[0].emojis[0];
            var opponentEmoji = boardInfos[1].emojis[0];

            m_myEmojiHP = myEmoji.data.health;
            m_opponentEmojiHP = opponentEmoji.data.health;

            //compare damage and health between these

            //Do something with ability Before Attack
            if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.BeforeAttack)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
            }
            //Opponent Emoji attacks, my Emoji gets hurt
            //Check friend of the opponent
            if (boardInfos[1].emojis.Count > 1)
            {
                //It must be 2 emojis above so we have Behind And Ahead
                //Emoji at position 0 attack, check emoji at position 1 in list has Ability Friend Ahead Attacks or not
                if (boardInfos[1].emojis[1].data.abilityType == ENUM.ABILITY_TYPE.FriendAheadAttacks)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(boardInfos[1].emojis[1].data);
                }
            }
            //Opponent Emoji attacks, my Emoji gets hurt
            myEmoji.data.health = myEmoji.data.health - opponentEmoji.data.damage;
            myEmoji.txtHealth.text = myEmoji.data.health <= 0 ? 0.ToString() : myEmoji.data.health.ToString();
            onShowMyEmojiDamageText?.Invoke(opponentEmoji.data.damage.ToString());
            opponentEmoji.FightAnimation(false);

            ///
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            //Do something with ability Before Attack
            if (opponentEmoji.data.abilityType == ENUM.ABILITY_TYPE.BeforeAttack)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);
            }
            //My Emoji attacks, opponent Emoji gets hurt
            //Check friend of my emoji
            if (boardInfos[0].emojis.Count > 1)
            {
                //It must be 2 emojis above so we have Behind And Ahead
                //Emoji at position 0 attack, check emoji at position 1 in list has Ability Friend Ahead Attacks or not
                if (boardInfos[0].emojis[1].data.abilityType == ENUM.ABILITY_TYPE.FriendAheadAttacks)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(boardInfos[0].emojis[1].data);
                }
            }
            //My Emoji attacks, opponent Emoji gets hurt
            opponentEmoji.data.health = opponentEmoji.data.health - myEmoji.data.damage;
            opponentEmoji.txtHealth.text = opponentEmoji.data.health <= 0 ? 0.ToString() : opponentEmoji.data.health.ToString();
            onShowOpponentEmojiDamageText?.Invoke(myEmoji.data.damage.ToString());
            myEmoji.FightAnimation(true);

            EndTurn(myEmoji, opponentEmoji);

            m_emojisInRightPlaces = false;
            onIsRightPlace?.Invoke(m_emojisInRightPlaces);
        }
    }
    private void EndTurn(EmojiController myEmoji, EmojiController opponentEmoji)
    {
        Debug.Log("After combat, my emoji HP = " + myEmoji.data.health);
        Debug.Log("After combat, opponent emoji HP = " + opponentEmoji.data.health);

        //Do something with ability Before Faint 
        if (myEmoji.data.health <= 0)
        {
            if (myEmoji.data.abilityType == ABILITY_TYPE.BeforeFaint)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
            }
            myEmoji.IsAlive = false;

            //Animation fight then check flies or stays
            myEmoji.FlyAway(true);


            //Do something with ability Faint
            if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.Faint)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
            }
            else if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.FaintSummon)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);

                //this ability Friend Summoned only work when someone Summon
                //check in my team
                foreach (var item in boardInfos[0].emojis)
                {
                    if (item.data.abilityType == ENUM.ABILITY_TYPE.FriendSummoned)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(item.data);
                    }
                }
            }
            //It must be 2 emojis above so we have Behind And Ahead
            //Emoji at position 0 attack, check emoji at position 1 in list has Ability Friend Ahead Faint or not
            if (boardInfos[0].emojis.Count > 1)
            {
                if (boardInfos[0].emojis[1].data.abilityType == ENUM.ABILITY_TYPE.FriendAheadFaints)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(boardInfos[0].emojis[1].data);
                }
            }
            //remove out of list after emoji controller was destroyed
            boardInfos[0].emojis.Remove(myEmoji);
        }
        //Still alive but gets hurt
        //Do something with ability Hurt
        //This is also End Turn
        else if (myEmoji.data.health < m_myEmojiHP)
        {
            myEmoji.IsHurt = true;

            myEmoji.IsAlive = true;

            myEmoji.BackToSeat(() =>
            {
                if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.Hurt)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
                }
                else if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.EndTurn)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(myEmoji.data);
                }
            });
        }

        //Do something with ability Before Faint 
        if (opponentEmoji.data.health <= 0)
        {
            if (opponentEmoji.data.abilityType == ABILITY_TYPE.BeforeFaint)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);
            }
            opponentEmoji.IsAlive = false;

            //Animation fight then check flies or stays
            opponentEmoji.FlyAway(false);

            //Do something with ability Faint
            if (opponentEmoji.data.abilityType == ENUM.ABILITY_TYPE.Faint)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);
            }
            else if (opponentEmoji.data.abilityType == ENUM.ABILITY_TYPE.FaintSummon)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);

                //this ability Friend Summoned only work when someone Summon
                //check in my team
                foreach (var item in boardInfos[1].emojis)
                {
                    if (item.data.abilityType == ENUM.ABILITY_TYPE.FriendSummoned)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(item.data);
                    }
                }
            }
            //It must be 2 emojis above so we have Behind And Ahead
            //Emoji at position 0 attack, check emoji at position 1 in list has Ability Friend Ahead Faint or not
            if (boardInfos[1].emojis.Count > 1)
            {
                if (boardInfos[1].emojis[1].data.abilityType == ENUM.ABILITY_TYPE.FriendAheadFaints)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(boardInfos[1].emojis[1].data);
                }
            }
            //remove out of listafter emoji controller was destroyed
            boardInfos[1].emojis.Remove(opponentEmoji);
        }
        //Still alive but gets hurt
        //Do something with ability Hurt
        //This is also End Turn
        else if (opponentEmoji.data.health < m_opponentEmojiHP)
        {
            opponentEmoji.IsHurt = true;

            opponentEmoji.IsAlive = true;

            opponentEmoji.BackToSeat(() =>
            {
                Debug.Log("Hurt");
                if (myEmoji.data.abilityType == ENUM.ABILITY_TYPE.Hurt)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);
                }
                else if (opponentEmoji.data.abilityType == ENUM.ABILITY_TYPE.EndTurn)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(opponentEmoji.data);
                }
            });
        }

        CheckResult();
    }

    private void CheckResult()
    {
        //Lose
        //My team have no any emojis, but opponents still have
        if (boardInfos[0].emojis.Count == 0 && boardInfos[1].emojis.Count > 0)
        {
            Debug.Log("Lose");

            DataController.instance.currentPlayer.gameState.lastMatchResult = MatchEndGui.Result.LOSE;

            Lose();
        }
        //Win, opposite
        else if (boardInfos[1].emojis.Count == 0 && boardInfos[0].emojis.Count > 0)
        {
            Debug.Log("Win");

            DataController.instance.currentPlayer.gameState.lastMatchResult = MatchEndGui.Result.WIN;

            Win();
        }
        //Draw, no one have any emojis left
        else if (boardInfos[1].emojis.Count == 0 && boardInfos[0].emojis.Count == 0)
        {
            Debug.Log("Draw");

            DataController.instance.currentPlayer.gameState.lastMatchResult = MatchEndGui.Result.DRAW;

            Draw();
        }
        //Start a new turn till there are one of two teams has no emoji
        //Actually, must check the abilities first, have to run all abilities before start battle
        //In this case, I only test with 1 ability
        else
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                //EmojisDestroy(boardInfos[0], true);
                //EmojisDestroy(boardInfos[1], false);
                ReadyToStartTurn();
            }).SetUpdate(false);
        }
    }

    private void Win()
    {
        PlayerData player = DataController.instance.currentPlayer;
        player.gameState.wins += 1;

        state = BattleState.FINISH;
        m_isFinish = true;

        StartCoroutine(ShowMatchEndUI(MatchEndGui.Result.WIN));
    }

    private void Lose()
    {
        PlayerData player = DataController.instance.currentPlayer;

        state = BattleState.FINISH;
        m_isFinish = true;

        player.gameState.health -= ReduceHealth();
        if (player.gameState.health <= 0)
        {
            BoardBase.SetActive(false);
            instance.gameEndGui.Show(false);
        }
        else
        {
            StartCoroutine(ShowMatchEndUI(MatchEndGui.Result.LOSE));
        }
    }

    private void Draw()
    {
        state = BattleState.FINISH;
        m_isFinish = true;
        StartCoroutine(ShowMatchEndUI(MatchEndGui.Result.DRAW));
    }

    public void OnPlayFightParticle()
    {
        m_fightParticle.gameObject.SetActive(true);
        m_fightParticle.Play();
    }

    public void OnPlayExplosionParticle(Vector3 pos)
    {
        var starParticle = Instantiate(m_starParticlePrefab);
        starParticle.transform.position = pos;
        starParticle.gameObject.SetActive(true);
        starParticle.Play();

    }

    public void OnPlayWasHitParticle(Transform target)
    {
        var particle = Instantiate(m_wasHitParticlePrefab, target);
        particle.transform.localPosition = Vector2.zero;
        particle.gameObject.SetActive(true);
        particle.Play();
    }
    public void OnPlayBuffParticle(Transform target)
    {
        var particle = Instantiate(m_buffParticlePrefab, target);
        particle.transform.localPosition = Vector2.zero;
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    public void OnWasHit(EmojiController emojiController)
    {
        foreach (var item in boardInfos[0].emojis)
        {
            //Check Is my team
            if (item.data.emojiId == emojiController.data.emojiId)
            {
                //Do something with ability Before Faint 
                if (emojiController.data.health <= 0)
                {
                    if (emojiController.data.abilityType == ABILITY_TYPE.BeforeFaint)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }
                    emojiController.IsAlive = false;

                    //Animation fight then check flies or stays
                    emojiController.FlyAway(true);

                    //Do something with ability Faint
                    if (emojiController.data.abilityType == ENUM.ABILITY_TYPE.Faint)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }
                    //remove out of list after emoji controller was destroyed
                    boardInfos[0].emojis.Remove(emojiController);
                }
                //Still alive but gets hurt
                //Do something with ability Hurt
                else
                {
                    if (emojiController.data.abilityType == ENUM.ABILITY_TYPE.Hurt)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }

                    emojiController.IsAlive = true;

                    emojiController.BackToSeat();
                }
                break;
            }
        }
        foreach (var item in boardInfos[1].emojis)
        {
            //Check Is opponent team
            if (item.data.emojiId == emojiController.data.emojiId)
            {
                //Do something with ability Before Faint 
                if (emojiController.data.health <= 0)
                {
                    if (emojiController.data.abilityType == ABILITY_TYPE.BeforeFaint)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }
                    emojiController.IsAlive = false;

                    //Animation fight then check flies or stays
                    emojiController.FlyAway(false);

                    //Do something with ability Faint
                    if (emojiController.data.abilityType == ENUM.ABILITY_TYPE.Faint)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }

                    //remove out of listafter emoji controller was destroyed
                    boardInfos[1].emojis.Remove(emojiController);
                }
                //Still alive but gets hurt
                //Do something with ability Hurt
                else
                {
                    if (emojiController.data.abilityType == ENUM.ABILITY_TYPE.Hurt)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiController.data);
                    }

                    emojiController.IsAlive = true;

                    emojiController.BackToSeat();
                }
                break;
            }
        }

        CheckResult();
    }
    #endregion
}
