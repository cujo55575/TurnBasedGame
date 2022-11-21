using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static ENUM;

public class PanelListUtils
{
    public static void ResizeContainer(Transform container, int n)
    {
        var firstChild = container.GetChild(0).gameObject;
        if (container.childCount > n)  // in case children need to be hidden
        {
            for (var i = n; i < container.childCount; i++)
            {
                container.GetChild(i).gameObject.SetActive(false);
                //MonoBehaviour.Destroy(container.GetChild(i).gameObject);
            }
        }
        else if (container.childCount < n) // in case children need to be added
        {
            for (var i = container.childCount; container.childCount < n; i++)
            {
                GameObject.Instantiate(firstChild, container);
            }
        }

        // turn on (need to check condition first)
        int idx = 0;
        while (idx < n)
        {
            container.GetChild(idx).gameObject.SetActive(true);
            idx++;
        }
    }
}

public class ShopController : Singleton<ShopController>
{
    [SerializeField]
    Transform board, buyablePanel;

    [SerializeField]
    TMP_Text stats, emojiInfo;

    [SerializeField] TextMeshProUGUI m_rounds;
    [SerializeField] TextMeshProUGUI m_health;
    [SerializeField] TextMeshProUGUI m_loseHealth;
    [SerializeField] TextMeshProUGUI m_wins;
    public TextMeshProUGUI m_coin;
    [SerializeField] TextMeshProUGUI m_sellTMP;

    [SerializeField] private int m_roundsStored;
    [SerializeField] private int m_healthStored;
    [SerializeField] private int m_winsStored;
    [SerializeField] private int m_coinStored;

    [SerializeField] private Button startButton, rerollButton;

    [SerializeField] private ShopCell[] boardCells = new ShopCell[5];
    [SerializeField] private ShopCell[] buyableCells = new ShopCell[5];

    [SerializeField] public List<EmojiData> buyableEmojis;//emojis we can buy.

    private EmojiData emojiToBuy;
    private EmojiData emojiToMove;

    [SerializeField] public Button m_sellingButton;
    [SerializeField] private ShopCell m_currentShopCell;
    [SerializeField] private GameObject m_noticePopup;
    [SerializeField] private GameObject objBattleController;
    [SerializeField] private TextMeshProUGUI m_noticeDescription;
    [SerializeField] private Button m_closeNoticePopupButton;
    [SerializeField] private GameObject BoardBase;
    [SerializeField] private GameObject loseHelathPanel;
    [SerializeField] private GameObject objBoosts;
    [SerializeField] private RectTransform topCurtain;
    [SerializeField] private RectTransform bottomCurtain;
    [SerializeField] private CertainBoostController certainBoostController;
    [SerializeField] private ParticleSystem m_abilityEffectPrefab;
    [SerializeField] private ParticleSystem m_affectionEffectPrefab;
    [SerializeField] private ParticleSystem m_levelingUpEffectPrefab;
    [SerializeField] private ParticleSystem m_boostEffectPrefab;
    [SerializeField] private bool m_isBuying;
    [SerializeField] private int m_tierUnlockedStored;

    private Vector3 m_bottomCurtainPos;
    private Vector3 m_topCurtainPos;

    public int discountPriceForboost = 0;
    public static System.Action<int> onRollBoost;
    public static System.Action onTierChange;

    void Start()
    {
        startButton.onClick.AddListener(() => StartBattle());
        rerollButton.onClick.AddListener(Reroll);
        ShopCell.onBoardCellClick += OnBoardCellClick;
        ShopCell.onLevelUp += OnLevelUp;
        ShopCell.onUpgradeSuccessfull += OnUpgradeSuccessfull;
        ShopCell.onNotEnoughCoin += OnNotEnoughCoin;
        DragDrop.onNotEnoughCoin += OnNotEnoughCoin;
        DragDrop.onDrag += OnDragging;
        DragDrop.onCheckingLeveledUpAfterBuy += CheckingCouldBeLeveledUp;
        EmojiAbilityHandler.onGetAbilityEffect += OnGetAbilityEffect;
        EmojiAbilityHandler.onGetAffection += RefreshStats;
        EmojiAbilityHandler.onGetAbilityEffects += OnGetAbilityEffects;
        EmojiAbilityHandler.onGetCertainboostPriceChange += onGetCertainboostPriceChange;
        m_sellingButton.onClick.AddListener(OnSellingEmoji);
        m_closeNoticePopupButton.onClick.AddListener(CloseNoticePoppup);
        m_tierUnlockedStored = DataController.instance.currentPlayer.gameState.tierUnlock;
        m_bottomCurtainPos = bottomCurtain.anchoredPosition;
        m_topCurtainPos = topCurtain.anchoredPosition;

    }
    public void onGetCertainboostPriceChange(int discountPrice)
    {
        Debug.Log("CertainBoostController:: discount price:: " + discountPrice);
        discountPriceForboost = discountPrice;
    }
    void OnEnable()
    {
        Debug.Log("Disable BattleController Gameobject::: ");
        objBoosts.SetActive(true);
        objBattleController.SetActive(false);
    }

    private void StartBattle()
    {
        discountPriceForboost = 0;
        //DataController.instance.currentPlayer.gameState.round++;
        PlayerData player = DataController.instance.currentPlayer;
        // if (player.emojis.Count == 0)
        // {
        //     EnableNoticePopup(true, "Need to buy at least one emoji to play!");
        //     return;
        // }

        player.emojis = new List<EmojiData>();
        EmojiAbilityHandler.Instance.playerEmojis = new List<EmojiData>();
        for (int i = boardCells.Length - 1; i >= 0; i--)
        {
            if (boardCells[i].emojiData != null)
            {
                boardCells[i].emojiData.health += boardCells[i].emojiData.Upgrade.HealthBonus;
                boardCells[i].emojiData.damage += boardCells[i].emojiData.Upgrade.DamageBonus;

                player.emojis.Add(boardCells[i].emojiData);
                EmojiAbilityHandler.Instance.playerEmojis.Add(boardCells[i].emojiData);
            }
        }
        StartCoroutine(SceneTransition(1.9f));
    }
    private IEnumerator SceneTransition(float timer)
    {
        objBoosts.SetActive(false);
        TweenManager.Instance.SlideUpAndDown(bottomCurtain, m_bottomCurtainPos.y + 540f, timer);
        TweenManager.Instance.SlideUpAndDown(topCurtain, m_topCurtainPos.y - 540f, timer);
        yield return new WaitForSeconds(timer);
        GameController.instance.GoToBattle();
        TweenManager.Instance.SlideUpAndDown(bottomCurtain, m_bottomCurtainPos.y, timer);
        TweenManager.Instance.SlideUpAndDown(topCurtain, m_topCurtainPos.y, timer);

    }
    public List<string> baseEmojiIds = new List<string>();
    List<EmojiData> RollEmojis()
    {
        BoardBase.SetActive(true);
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round <= DataController.instance.totalRound)
        {
            for (int i = 0; i < DataController.instance.emojiDataList[round].Count; i++)
            {
                baseEmojiIds.Add(DataController.instance.emojiDataList[round][i]);
            }
        }
        var emojis = new List<EmojiData>();
        for (var i = 0; i < TotalEmojis(); i++)
        {
            string randomEmojiId = baseEmojiIds[Random.Range(0, baseEmojiIds.Count)];
            EmojiData emoji = EmojiData.MakeEmoji(randomEmojiId);
            emojis.Add(emoji);
        }
        return emojis;
    }
    private int TotalEmojis()
    {
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round >= 0 && round <= 1) return 3;
        else if (round >= 2 && round <= 4) return 4;
        else return 5;
    }

    public void CheckingCouldBeLeveledUp()
    {
        foreach (var item in buyableCells)
        {
            item.CouldBeLeveledUpEffect.Hide();
        }

        foreach (var item in buyableCells)
        {
            if (item.emojiData != null)
            {
                var emoji = DataController.instance.currentPlayer.emojis.Find(x => x.baseEmojiId == item.emojiData.baseEmojiId);

                if (emoji != null)
                {
                    item.CouldBeLeveledUpEffect.Show();
                }
            }
        }
    }
    public void Show()
    {
        m_sellingButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
        buyableEmojis = RollEmojis();

        onRollBoost?.Invoke(DataController.instance.currentPlayer.gameState.tierUnlock);
        Refresh();

        int tierUnlock = DataController.instance.currentPlayer.gameState.tierUnlock;
        Debug.Log("SHOPCONTROLLER::: Outside if statement m_tierUnlockedStored " + m_tierUnlockedStored + " tierunlock " + tierUnlock);
        if (tierUnlock != m_tierUnlockedStored)
        {
            m_tierUnlockedStored = tierUnlock;
            Debug.Log("SHOPCONTROLLER::: Inside if statement m_tierUnlockedStored " + m_tierUnlockedStored + " tierunlock " + tierUnlock);
            onTierChange?.Invoke();
        }

        CheckingCouldBeLeveledUp();
    }

    private void SetupGrids()
    {
        // setup grids
        PanelListUtils.ResizeContainer(board, boardCells.Length);
        for (var i = 0; i < boardCells.GetLength(0); i++)
        {
            ShopCell cell = board.GetChild(i).GetComponent<ShopCell>();
            cell.Setup();
            cell.coord = i;

            //cell.SetClick(() =>
            //{
            //    Debug.Log("SetClick()");
            //});

            boardCells[i] = cell;
        }

        PanelListUtils.ResizeContainer(buyablePanel, buyableCells.Length);
        for (var i = 0; i < buyableCells.Length; i++)
        {
            ShopCell cell = buyablePanel.GetChild(i).GetComponent<ShopCell>();
            cell.Setup();
            cell.coord = i;
            //cell.SetClick(() =>
            //{
            //    OnClickStoreCell(cell);
            //});
            cell.onClick = () => OnClickStoreCell(cell);
            cell.onMoveEmoji = (cell) => OnMoveEmoji(cell);
            cell.onBuyEmoji = (cell) => OnBuyEmoji(cell);

            buyableCells[i] = cell;
        }
    }
    private void OnUpgradeSuccessfull()
    {
        PlayerData player = DataController.instance.currentPlayer;
        PlayerGameState gameState = player.gameState;
        gameState.coins -= 3;
        m_coin.text = gameState.coins.ToString();
        if (m_coinStored != gameState.coins)
        {
            m_coinStored = gameState.coins;

            ScaleUpTMP(m_coin.transform);
        }
    }
    void RefreshStats()
    {
        PlayerData player = DataController.instance.currentPlayer;
        PlayerGameState gameState = player.gameState;

        m_rounds.text = (gameState.round).ToString();
        m_coin.text = gameState.coins.ToString();
        m_wins.text = gameState.wins.ToString() + "\\10";
        m_health.text = gameState.health <= 0 ? 0.ToString() : gameState.health.ToString();
        if (gameState.round >= 2)
        {
            loseHelathPanel.SetActive(true);
            m_loseHealth.text = "Lose -> - " + ReduceHealth().ToString();
        }
        else
        {
            loseHelathPanel.gameObject.SetActive(false);
        }

        if (m_roundsStored != gameState.round)
        {
            m_roundsStored = gameState.round;

            ScaleUpTMP(m_rounds.transform);
        }
        if (m_coinStored != gameState.coins)
        {
            m_coinStored = gameState.coins;

            ScaleUpTMP(m_coin.transform);
        }
        if (m_winsStored != gameState.wins)
        {
            m_winsStored = gameState.wins;

            ScaleUpTMP(m_wins.transform);
        }
        if (m_healthStored != gameState.health)
        {
            m_healthStored = gameState.health;

            ScaleUpTMP(m_health.transform);
        }
    }

    private void ScaleUpTMP(Transform target)
    {
        target.transform.DOScale(Vector3.one * 2f, 0.2f).OnComplete(() =>
        {
            target.transform.DOScale(Vector3.one * 1f, 0.5f);
        });
    }

    //public void RefreshData()
    //{

    //}
    public void Refresh()
    {
        RefreshStats();
        SetupGrids();

        // show my emojis
        PlayerData player = DataController.instance.currentPlayer;
        if (player.board.placements.Count > 0)
        {
            foreach (PlacementData placement in player.board.placements)
            {
                ShopCell cell = boardCells[placement.index];
                EmojiData emojiData = player.GetEmoji(placement.emojiId);
                cell.Setup(emojiData);
            }
        }
        // show buyable emojis
        RefreshBuyablePanel();

    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="cell"></param>
    //buying emojis
    void OnMoveEmoji(ShopCell cell)
    {
        //var player = DataController.currentPlayer;
        PlayerData player = DataController.instance.currentPlayer;

        if (emojiToMove != null)  // stop moving a emoji
        {
            Debug.Log("emojiToMove");

            if (cell.emojiData == null)
            {
                foreach (var placement in player.board.placements)
                {
                    if (placement.emojiId == emojiToMove.emojiId)
                    {
                        placement.index = cell.coord;
                        break;
                    }
                }
                emojiToMove = null;
                Refresh();
            }
        }
        //else   // start moving a emoji
        {
            Debug.Log("emojiToMove is null");
            if (cell.emojiData != null)
            {
                emojiToMove = cell.emojiData;
            }
        }
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="cell"></param>

    void OnBuyEmoji(ShopCell cell)
    {
        PlayerData player = DataController.instance.currentPlayer;

        if (emojiToBuy != null)  // stop buying a emoji
        {
            if (cell.emojiData == null)
            {
                // TODO: subtract money
                if (player.gameState.coins < emojiToBuy.baseEmoji.cost)
                {
                    emojiToBuy = null;
                    // TODO: show gui warning
                    Debug.LogWarning("cannot buy emoji=, not enough money");
                    EnableNoticePopup(true, "Not enough coins");
                    //Refresh();
                    return;
                }

                player.gameState.coins -= emojiToBuy.baseEmoji.cost;
                for (var i = 0; i < buyableEmojis.Count; i++)
                {
                    if (buyableEmojis[i] == emojiToBuy)
                    {
                        //buyableEmojis[i] = null;
                        buyableEmojis.Remove(buyableEmojis[i]);
                        break;
                    }
                }

                //Debug.Log("Before buy > Emoji Count : " + player.emojis.Count);
                player.emojis.Add(emojiToBuy);
                //Debug.Log("After buy > Emoji Count : " + player.emojis.Count);
                player.board.placements.Add(new PlacementData
                {
                    index = cell.coord,
                    emojiId = emojiToBuy.emojiId,
                    emojiName = emojiToBuy.baseEmojiId
                });

                if (emojiToBuy.abilityType == ENUM.ABILITY_TYPE.Buy)
                {
                    ShopController.Instance.SetCurrentShopCell(cell);
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiToBuy);
                }

                //Debug.Log(cell.name);
                emojiToBuy = null;
                RefreshStats();
                m_currentShopCell = cell;//Whenever we buy emoji, that emoji should be in the m_currentShopcell,to prevent player forget 
                                         //to select the emoji he want to level up ,


            }
        }
        else
        {
            Debug.Log("emojiToBuy is null");
        }
    }

    void OnClickStoreCell(ShopCell cell)
    {
        if (emojiToBuy == null && emojiToMove == null)  // start buying emoji
        {
            if (cell.emojiData != null)
            {
                emojiToBuy = cell.emojiData;
            }
        }
    }

    private void OnLevelUp(EmojiData emojiData)
    {
        emojiToBuy = null;
        //emojiToMove = null;
        for (var i = 0; i < buyableEmojis.Count; i++)
        {
            if (buyableEmojis[i].emojiId.CompareTo(emojiData.emojiId) == 0)
            {
                buyableEmojis.Remove(buyableEmojis[i]);
                break;
            }
        }
    }

    public void Reroll()
    {
        var player = DataController.instance.currentPlayer;
        if (player.gameState.coins <= 0)
        {
            emojiToBuy = null;
            Refresh();
            // TODO: show gui warning
            Debug.LogWarning("cannot reroll, not enough money");
            EnableNoticePopup(true, "Not enough coins");
            return;
        }
        player.gameState.coins -= 1;
        RefreshStats();//RefreshStats need to call in this scope,Refresh has all sorts of recreateing emojis model and duplating the models.
        buyableEmojis = RollEmojis();
        onRollBoost?.Invoke(DataController.instance.currentPlayer.gameState.tierUnlock);
        RefreshBuyablePanel();
        CheckingCouldBeLeveledUp();
    }

    void RefreshBuyablePanel()
    {
        for (var i = 0; i < buyableEmojis.Count; i++)
        {
            if (buyableEmojis[i] == null)
            {
                buyableCells[i].Setup();
            }
            else
            {
                buyableCells[i].Setup(buyableEmojis[i]);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBoardCellClick(ShopCell shopCell, bool isInside)
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            if (!m_sellingButton.gameObject.activeInHierarchy && !isInside)
            {
                return;
            }

            m_sellingButton.gameObject.SetActive(isInside);

            m_sellTMP.text = "Sell (" + shopCell.emojiData.level + ")";

            m_currentShopCell = shopCell;

        });
    }

    public void SetCurrentShopCell(ShopCell cell)
    {
        Debug.Log("HUH");

        m_currentShopCell = cell;
        m_isBuying = true;
    }


    private void OnDragging()
    {
        OnBoardCellClick(null, false);
    }

    private void OnSellingEmoji()
    {
        m_isBuying = false;

        if (m_currentShopCell != null)
        {
            if (m_currentShopCell.emojiData.abilityType == ENUM.ABILITY_TYPE.Sell)
            {
                EmojiAbilityHandler.Instance.CheckEmojiAbility(m_currentShopCell.emojiData);
            }
            //whenever any emojis got sold, this method will be called and check if there is "FriendSold" aka "Crazy emoji" exist.:::SLINT:
            EmojiAbilityHandler.Instance.ExecuteAbilityOnFriendSold(m_currentShopCell.emojiData);

            var abilityEffect = Instantiate(m_abilityEffectPrefab, m_currentShopCell.transform);

            abilityEffect.transform.localPosition = Vector3.zero;

            abilityEffect.gameObject.SetActive(true);

            abilityEffect.Play();

            DataController.instance.currentPlayer.gameState.coins += m_currentShopCell.emojiData.level;

            foreach (var item in DataController.instance.currentPlayer.emojis)
            {
                if (item.emojiId.CompareTo(m_currentShopCell.emojiData.emojiId) == 0)
                {
                    DataController.instance.currentPlayer.emojis.Remove(item);
                    break;
                }
            }

            foreach (var item in DataController.instance.currentPlayer.board.placements)
            {
                if (item.emojiId.CompareTo(m_currentShopCell.emojiData.emojiId) == 0)
                {
                    DataController.instance.currentPlayer.board.placements.Remove(item);
                    break;
                }
            }

            m_sellTMP.text = "Sell";

            RefreshStats();

            m_currentShopCell.ResetData();

            Destroy(m_currentShopCell.shopEmoji);

            m_sellingButton.gameObject.SetActive(false);

            CheckingCouldBeLeveledUp();
        }
    }
    private void OnNotEnoughCoin()
    {
        EnableNoticePopup(true, "Not enough coins");
    }

    public void EnableNoticePopup(bool isEnable, string description)
    {
        m_noticePopup.SetActive(isEnable);
        m_noticeDescription.text = description;
    }

    private void CloseNoticePoppup()
    {
        EnableNoticePopup(false, "");
    }

    public void OnGetAbilityEffect(string abilityEmojiID, string effectionEmojiID)
    {
        var affectionCell = new ShopCell();
        foreach (var item in boardCells)
        {
            if (item.shopEmoji != null)
            {
                if (item.emojiData.emojiId.CompareTo(effectionEmojiID) == 0)
                {
                    affectionCell = item;
                }
            }
        }

        foreach (var item in buyableCells)
        {
            if (item.shopEmoji != null)
            {
                if (item.emojiData.emojiId.CompareTo(effectionEmojiID) == 0)
                {
                    affectionCell = item;
                }
            }
        }

        if (m_isBuying)
        {
            PlayAbilityEffect(m_currentShopCell, m_currentShopCell);
        }
        else
        {
            PlayAbilityEffect(m_currentShopCell, affectionCell);
        }
    }

    private void OnGetAbilityEffects(string abilityEmojiID, List<string> effectionEmojiIDs)
    {
        var affectionCells = new List<ShopCell>();
        foreach (var item in boardCells)
        {
            if (item.shopEmoji != null)
            {
                var a = effectionEmojiIDs.Find(x => x == item.emojiData.emojiId);
                if (!string.IsNullOrEmpty(a))
                {
                    affectionCells.Add(item);
                }
            }
        }

        foreach (var item in buyableCells)
        {
            if (item.shopEmoji != null)
            {
                var a = effectionEmojiIDs.Find(x => x == item.emojiData.emojiId);
                if (!string.IsNullOrEmpty(a))
                {
                    affectionCells.Add(item);
                }
            }
        }

        PlayAbilityEffects(m_currentShopCell, affectionCells);
    }

    public void PlayAbilityEffects(ShopCell abilityCell, List<ShopCell> affectionCells)
    {
        var abilityEffect = Instantiate(m_abilityEffectPrefab, abilityCell.transform);

        abilityEffect.transform.localPosition = Vector3.zero;

        abilityEffect.gameObject.SetActive(true);

        abilityEffect.Play();


        var affectionEffects = new List<ParticleSystem>();
        foreach (var item in affectionCells)
        {
            var affectionEffect = Instantiate(m_affectionEffectPrefab, item.transform);

            affectionEffect.transform.localPosition = Vector3.zero;

            affectionEffects.Add(affectionEffect);
        }


        abilityCell.AbilityPopupEffect();
        DOVirtual.DelayedCall(1f, () =>
        {
            foreach (var item in affectionEffects)
            {
                item.gameObject.SetActive(true);
                item.Play();
            }
        });
    }

    public void PlayAbilityEffect(ShopCell abilityCell, ShopCell affectionCell)
    {
        var abilityEffect = Instantiate(m_abilityEffectPrefab, abilityCell.transform);
        var affectionEffect = Instantiate(m_affectionEffectPrefab, affectionCell.transform);
        abilityEffect.transform.localPosition = Vector3.zero;
        affectionEffect.transform.localPosition = Vector3.zero;
        abilityEffect.gameObject.SetActive(true);
        abilityEffect.Play();
        abilityCell.AbilityPopupEffect();
        DOVirtual.DelayedCall(1f, () =>
        {
            affectionEffect.gameObject.SetActive(true);
            affectionEffect.Play();
        });
    }

    public void PlayLevelUpEffect(ShopCell cell)
    {
        var abilityEffect = Instantiate(m_levelingUpEffectPrefab, cell.transform);

        abilityEffect.transform.localPosition = Vector3.zero;

        abilityEffect.gameObject.SetActive(true);

        abilityEffect.Play();
    }
    public void PlayBoostEffect(ShopCell cell)
    {
        var abilityEffect = Instantiate(m_boostEffectPrefab, cell.transform);

        abilityEffect.transform.localPosition = Vector3.zero;

        abilityEffect.gameObject.SetActive(true);

        abilityEffect.Play();
    }
    private int ReduceHealth()
    {
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round >= 0 && round < 2) return 1;
        else if (round >= 2 && round <= 3) return 2;
        else return 3;
    }
    private void OnDestroy()
    {
        ShopCell.onBoardCellClick -= OnBoardCellClick;
        ShopCell.onLevelUp -= OnLevelUp;
        ShopCell.onUpgradeSuccessfull -= OnUpgradeSuccessfull;
        DragDrop.onNotEnoughCoin -= OnNotEnoughCoin;
        DragDrop.onDrag -= OnDragging;
        EmojiAbilityHandler.onGetAbilityEffect -= OnGetAbilityEffect;
        EmojiAbilityHandler.onGetAffection -= RefreshStats;
        EmojiAbilityHandler.onGetAbilityEffects -= OnGetAbilityEffects;
        EmojiAbilityHandler.onGetCertainboostPriceChange -= onGetCertainboostPriceChange;
    }
}
