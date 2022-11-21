using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using static ENUM;

public class ShopCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    Button button;

    [SerializeField]
    TMP_Text title;

    public Action onClick = null;
    public Action<ShopCell> onMoveEmoji = null;
    public Action<ShopCell> onBuyEmoji = null;

    public EmojiData emojiData;
    public int coord;
    public GameObject shopEmoji;


    [SerializeField] private CertainBoostItem m_tempBoostItem;
    [SerializeField] private bool m_isBoardCell;
    [SerializeField] private bool m_isClickOnBoardCell;
    [SerializeField] private bool m_isSelected;
    [SerializeField] private bool m_isEnter;
    [SerializeField] private bool m_isExit;
    [SerializeField] private GameObject m_infoPopup;
    [SerializeField] private TextMeshProUGUI m_infoTMP;
    [SerializeField] private TextMeshProUGUI m_nameTMP;
    [SerializeField] private TextMeshProUGUI m_costTMP;
    [SerializeField] private GameObject m_att;
    [SerializeField] private TextMeshProUGUI m_attTMP;
    [SerializeField] private GameObject m_hp;
    [SerializeField] private TextMeshProUGUI m_hpTMP;
    [SerializeField] private GameObject m_lv;
    [SerializeField] private TextMeshProUGUI m_lvTMP;
    [SerializeField] private Image m_expSlider;
    [SerializeField] private GameObject m_selectedFrame;
    [SerializeField] private Image m_shadowImage;
    [SerializeField] private GameObject m_upgradeInfoPopup;
    [SerializeField] private TextMeshProUGUI m_upgradeInfoTMP;
    [SerializeField] private TextMeshProUGUI m_upgradeNameTMP;
    [SerializeField] private UpgradeEffect upgradeEffect;
    [SerializeField] private CouldBeLeveledUpEffect m_leveledUpEffect;
    [SerializeField] private List<Sprite> m_emojisImage;
    [SerializeField] private int m_attStored;
    [SerializeField] private int m_hpStored;
    [SerializeField] private int m_lvStored;

    [SerializeField] private GameObject m_dice;
    [SerializeField] private GameObject m_diceTier1;
    [SerializeField] private GameObject m_diceTier2;
    [SerializeField] private GameObject m_diceTier3;
    [SerializeField] private GameObject m_diceTier4;

    [SerializeField] private ArrowAnimation m_arrow;
    [SerializeField] private UpgradeSelectedAnimation m_upgradeSelected;

    public static System.Action<ShopCell, bool> onBoardCellClick;
    public static System.Action<EmojiData> onLevelUp;
    public static System.Action onUpgradeSuccessfull;
    public static System.Action onNotEnoughCoin;
    public static System.Action<bool> onBoardCellHover;

    public bool IsBoardCell => m_isBoardCell;
    public CouldBeLeveledUpEffect CouldBeLeveledUpEffect => m_leveledUpEffect;

    public void Start()
    {
        button.onClick.AddListener(OnClickOnEmoji);
        CertainBoostCell.onBoost += OnGetBoostTemp;
        EmojiAbilityHandler.onGetAffection += OnGetAffection;
        ShopController.onTierChange += OnTierChange;
        DragDrop.onMoving += OnMoving;
        DragDrop.onRayCastHits += OnRayCastHits;
        CertainBoostCell.onEnableUpgradeSelectedAnimation += EnableUpgradeSelectedAnimation;
    }

    // setup empty cell
    public void Setup()
    {
        Destroy(shopEmoji);
        title.text = "";
        emojiData = null;
        EnableDamageHPLv(false, "", "", "");
        m_attStored = 0;
        m_hpStored = 0;
        m_lvStored = 0;
        if (m_dice != null)
        {
            m_dice.SetActive(false);
        }
        upgradeEffect.Hide();
        ExpSliderUp(0);
    }

    private void OnGetAffection()
    {
        if (emojiData != null)
        {
            EnableDamageHPLv(true, emojiData.damage.ToString(), emojiData.health.ToString(), emojiData.level.ToString());
        }
    }

    private void EnableDamageHPLv(bool isEnable, string att, string hp, string lv)
    {
        m_att.SetActive(isEnable);
        m_hp.SetActive(isEnable);
        if (m_isBoardCell)
        {
            m_lv.SetActive(isEnable);
        }

        if (!isEnable)
        {
            m_shadowImage.gameObject.SetActive(false);
        }

        if (isEnable)
        {

            m_attTMP.text = att;
            m_hpTMP.text = hp;
            if (lv == "3")
            {
                m_lvTMP.text = lv + "!";
            }
            else
            {
                m_lvTMP.text = lv;
            }

            if (Convert.ToInt32(att) != m_attStored)
            {
                m_attStored = Convert.ToInt32(att);

                ScaleUpTMP(m_attTMP.transform);
            }
            if (Convert.ToInt32(hp) != m_hpStored)
            {
                m_hpStored = Convert.ToInt32(hp);

                ScaleUpTMP(m_hpTMP.transform);
            }
            if (Convert.ToInt32(lv) != m_lvStored)
            {
                m_lvStored = Convert.ToInt32(lv);

                ScaleUpTMP(m_lvTMP.transform);
            }
        }
    }
    private void ScaleUpTMP(Transform target)
    {
        target.transform.DOScale(Vector3.one * 1.7f, 0.2f).OnComplete(() =>
        {
            target.transform.DOScale(Vector3.one * 1f, 0.4f);
        });
    }

    Tweener tween;
    private void ScaleUpEmoji(Transform target)
    {
        tween.Kill();
        tween = target.DOPunchScale(Vector3.one * 3, 0.3f);
    }

    // setup ship cell
    public void Setup(EmojiData emojiData)
    {
        if (emojiData == null)
        {
            Setup();
            return;
        }
        this.emojiData = emojiData;
        BaseEmojiData baseEmoji = DataController.instance.baseEmojis[emojiData.baseEmojiId];
        emojiData.health = emojiData.upgradedHealth;//temp fix ::SLINT:::
        emojiData.damage = emojiData.upgradedDamage;//temp fix ::SLINT:::
        emojiData.isAbilityExecuted = false;
        CreateEmoji(DataController.instance.currentPlayer, emojiData);
        EnableDamageHPLv(true, emojiData.damage.ToString(), emojiData.health.ToString(), emojiData.level.ToString());
        EnableDiceTier(emojiData);
        ExpSliderUp(emojiData.levelStackTimes);
        ScaleUpEmoji(shopEmoji.transform);
        if (emojiData.upgradeTime == 1)
        {
            upgradeEffect.SetImage(emojiData.Upgrade.Avatar);
            upgradeEffect.Show();
        }
    }

    private void EnableDiceTier(EmojiData emojiData)
    {
        if (m_dice != null)
        {
            m_dice.SetActive(true);

            m_diceTier1.SetActive(false);
            m_diceTier2.SetActive(false);
            m_diceTier3.SetActive(false);
            m_diceTier4.SetActive(false);

            if (emojiData.tier == 1)
            {
                m_diceTier1.SetActive(true);
            }
            if (emojiData.tier == 2)
            {
                m_diceTier2.SetActive(true);
            }
            if (emojiData.tier == 3)
            {
                m_diceTier3.SetActive(true);
            }
            if (emojiData.tier == 4)
            {
                m_diceTier4.SetActive(true);
            }
        }
    }

    private void CreateEmoji(PlayerData playerData, EmojiData emojiData)
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        shopEmoji = Instantiate(ModelUtils.GetDataFromResources(this.emojiData.baseEmojiId).gameObject, transform);

        foreach (var item in m_emojisImage)
        {
            if (emojiData.baseEmojiId == item.name)
            {
                m_shadowImage.sprite = item;
                m_shadowImage.gameObject.SetActive(false);
                break;
            }
        }

        EmojiController emojiController = shopEmoji.GetComponent<EmojiController>();
        emojiController.playerData = playerData;
        emojiController.data = emojiData;
        // emojiController.enabled = false;W

        DragDrop dragDrop = shopEmoji.AddComponent<DragDrop>();
        dragDrop.onClick = onClick;
        dragDrop.onMove = onMoveEmoji;
        dragDrop.onBuy = onBuyEmoji;

        shopEmoji.AddComponent<BoxCollider>();
        shopEmoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
        shopEmoji.tag = "Emoji";
        shopEmoji.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        shopEmoji.transform.localPosition = new Vector3(0, 9, 0);
        shopEmoji.transform.localRotation = Quaternion.Euler(0, 180, 0);
        shopEmoji.transform.localScale = Vector3.one * 15;
        shopEmoji.SetActive(true);
    }

    private void OnGetBoostTemp(CertainBoostItem boostItem)
    {
        if (m_isBoardCell && shopEmoji != null)
        {
            m_tempBoostItem = boostItem;
        }
        else
        {
            m_tempBoostItem = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_tempBoostItem != null)
        {
            OnBoost(m_tempBoostItem);
        }

        if (m_isBoardCell && emojiData != null)
        {
            if (!string.IsNullOrEmpty(emojiData.emojiId))
            {
                m_isClickOnBoardCell = true;

                DOVirtual.DelayedCall(0.05f, () =>
                {
                    onBoardCellClick?.Invoke(this, true);
                });
            }
        }
        else if (!m_isBoardCell)
        {
            onBoardCellClick?.Invoke(null, false);
        }
    }

    public void OnBoost(CertainBoostItem boostItem)
    {
        PlayerData player = DataController.instance.currentPlayer;
        if (player.gameState.coins >= boostItem.Data.Cost - ShopController.Instance.discountPriceForboost)
        {
            if (emojiData != null && boostItem != null)
            {
                if (emojiData.Upgrade != null && (emojiData.Upgrade.HealthBonus > 0 || emojiData.Upgrade.DamageBonus > 0))
                {
                    m_tempBoostItem = null;
                    return;
                }

                if (!string.IsNullOrEmpty(emojiData.emojiId))
                {
                    emojiData.Upgrade = boostItem.Data;

                    BaseEmojiData baseEmoji = DataController.instance.baseEmojis[emojiData.baseEmojiId];

                    PlayerGameState gameState = player.gameState;

                    onUpgradeSuccessfull?.Invoke();

                    emojiData.upgradeTime = 1;

                    upgradeEffect.Show();

                    upgradeEffect.SetImage(emojiData.Upgrade.Avatar);

                    if (emojiData.abilityType == ABILITY_TYPE.EmojiEatsUpgrade)
                    {
                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiData);
                    }

                    EnableDamageHPLv(true, (emojiData.damage + boostItem.Data.DamageBonus).ToString(), (emojiData.health + boostItem.Data.HealthBonus).ToString(), emojiData.level.ToString());

                    ShopController.Instance.PlayBoostEffect(this);

                    boostItem.transform.parent.GetComponent<CertainBoostCell>().DiceTier.SetActive(false);

                    Destroy(boostItem.gameObject);

                    if (m_isBoardCell)
                    {
                        m_upgradeInfoPopup.gameObject.SetActive(true);
                        m_upgradeInfoTMP.text = emojiData.Upgrade.Description;
                        m_upgradeNameTMP.text = emojiData.Upgrade.Name;
                    }
                    else
                    {
                        m_upgradeInfoPopup.gameObject.SetActive(false);
                    }

                }
            }
        }
        else
        {
            m_tempBoostItem = null;
            onNotEnoughCoin?.Invoke();
        }
    }

    private void OnClickOnEmoji()
    {
        Debug.Log("On Click Emoji");
        if (shopEmoji != null)
        {
            shopEmoji.transform.DOScale(Vector3.one * 20f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                shopEmoji.transform.DOScale(Vector3.one * 15f, 0.1f).SetEase(Ease.Linear);
            });
        }
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
            !RectTransformUtility.RectangleContainsScreenPoint(ShopController.Instance.m_sellingButton.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            if (m_isClickOnBoardCell)
            {
                Debug.Log("Click Outside");

                m_isClickOnBoardCell = false;

                onBoardCellClick?.Invoke(null, false);
            }

            if (m_isSelected && m_isExit)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    m_isSelected = false;

                    EnableTempInfoPopup(false);
                }
            }
        }
    }

    private void EnableUpgradeSelectedAnimation(bool isEnable)
    {
        if (!isEnable)
        {
            m_upgradeSelected.IsEnable(false);
        }
        else
        {
            if (m_isBoardCell && shopEmoji != null)
            {

                m_upgradeSelected.IsEnable(isEnable);
            }
        }
    }

    private void ExpSliderUp(int stackTimes)
    {
        switch (stackTimes)
        {
            case 0:
                ExpSliderUpAnimation(0f, false);
                break;
            case 1:
                ExpSliderUpAnimation(1f / 2f, false);
                break;
            case 2:
                ExpSliderUpAnimation(1f, true);
                break;
            case 3:
                ExpSliderUpAnimation(1f / 3f, false);
                break;
            case 4:
                ExpSliderUpAnimation(2f / 3f, false);
                break;
            case 5:
                ExpSliderUpAnimation(1f, false);
                break;
            default:
                break;
        }
    }

    private void ExpSliderUpAnimation(float endValue, bool isReset)
    {
        m_expSlider.DOFillAmount(endValue, 0.5f).OnComplete(() =>
        {
            if (isReset)
            {
                m_expSlider.DOFillAmount(0, 0f);
            }
        });
    }

    public bool OnCheckingLevelUp(ShopCell shopCell)
    {
        if (shopCell != this)
        {
            if (m_isBoardCell)
            {
                PlayerData player = DataController.instance.currentPlayer;

                if (shopCell.emojiData.baseEmojiId.CompareTo(emojiData.baseEmojiId) == 0)
                {
                    if (IsBoardCell && shopCell.IsBoardCell)
                    {
                        if (emojiData.level < 3 && shopCell.emojiData.level < 3)
                        {
                            emojiData.levelStackTimes++;
                            ExpSliderUp(emojiData.levelStackTimes);
                            if (emojiData.levelStackTimes == 2)
                            {
                                emojiData.level = 2;
                            }
                            else if (emojiData.levelStackTimes == 5)
                            {
                                emojiData.level = 3;
                            }
                            if (emojiData.level == 2 || emojiData.level == 3)
                            {
                                emojiData.health++;
                                emojiData.damage++;
                            }

                            var newHealth = emojiData.health;
                            var newDamage = emojiData.damage;

                            BaseEmojiData baseEmoji = DataController.instance.baseEmojis[emojiData.baseEmojiId];

                            EnableDamageHPLv(true, emojiData.damage.ToString(), emojiData.health.ToString(), emojiData.level.ToString());

                            foreach (var item in DataController.instance.currentPlayer.emojis)
                            {
                                if (item.emojiId.CompareTo(emojiData.emojiId) == 0)
                                {
                                    item.health = newHealth;
                                    item.upgradedHealth = newHealth;
                                    item.damage = newDamage;
                                    item.upgradedDamage = newDamage;
                                }
                            }

                            foreach (var item in DataController.instance.currentPlayer.board.placements)
                            {
                                if (shopCell.emojiData.emojiId == item.emojiId)
                                {
                                    DataController.instance.currentPlayer.board.placements.Remove(item);
                                    break;
                                }
                            }

                            ShopController.Instance.PlayLevelUpEffect(this);

                            onLevelUp?.Invoke(shopCell.emojiData);
                            shopCell.ResetData();
                            Destroy(shopCell.shopEmoji.gameObject);

                            ShopController.Instance.CheckingCouldBeLeveledUp();

                            return true;
                        }
                        else
                        {
                            ShopController.Instance.EnableNoticePopup(true, "The max level is level 3.");
                            return false;
                        }
                    }
                    else
                    {
                        //else 2 thang khac board thi check tien
                        if (player.gameState.coins >= shopCell.emojiData.cost)
                        {
                            if (emojiData.level < 3 && shopCell.emojiData.level < 3)
                            {
                                emojiData.levelStackTimes++;
                                ExpSliderUp(emojiData.levelStackTimes);
                                if (emojiData.levelStackTimes == 2)
                                {
                                    emojiData.level = 2;
                                    if (emojiData.abilityType == ABILITY_TYPE.LevelUp)
                                    {
                                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiData);
                                    }
                                }
                                else if (emojiData.levelStackTimes == 5)
                                {
                                    emojiData.level = 3;
                                    if (emojiData.abilityType == ABILITY_TYPE.LevelUp)
                                    {
                                        EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiData);
                                    }
                                }
                                if (emojiData.level == 2 || emojiData.level == 3)
                                {
                                    emojiData.health++;
                                    emojiData.damage++;
                                }

                                onUpgradeSuccessfull?.Invoke();

                                var newHealth = emojiData.health;
                                var newDamage = emojiData.damage;

                                BaseEmojiData baseEmoji = DataController.instance.baseEmojis[emojiData.baseEmojiId];

                                EnableDamageHPLv(true, emojiData.damage.ToString(), emojiData.health.ToString(), emojiData.level.ToString());

                                foreach (var item in DataController.instance.currentPlayer.emojis)
                                {
                                    if (item.emojiId.CompareTo(emojiData.emojiId) == 0)
                                    {
                                        item.health = newHealth;
                                        item.upgradedHealth = newHealth;
                                        item.damage = newDamage;
                                        item.upgradedDamage = newDamage;
                                    }
                                }

                                foreach (var item in DataController.instance.currentPlayer.board.placements)
                                {
                                    if (shopCell.emojiData.emojiId == item.emojiId)
                                    {
                                        DataController.instance.currentPlayer.board.placements.Remove(item);
                                        break;
                                    }
                                }

                                ShopController.Instance.PlayLevelUpEffect(this);

                                onLevelUp?.Invoke(shopCell.emojiData);
                                shopCell.ResetData();
                                Destroy(shopCell.shopEmoji.gameObject);

                                ShopController.Instance.CheckingCouldBeLeveledUp();

                                return true;
                            }
                            else
                            {
                                ShopController.Instance.EnableNoticePopup(true, "The max level is level 3.");
                                return false;
                            }
                        }
                        else
                        {
                            onNotEnoughCoin?.Invoke();
                            return false;
                        }
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Update()
    {
        HideIfClickedOutside(gameObject);
    }

    public void ResetData()
    {
        emojiData = null;
        title.text = "";
        EnableDamageHPLv(false, "", "", "");
        upgradeEffect.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_isEnter = true;
        m_isExit = false;

        EnableTempInfoPopup(true);

        m_upgradeSelected.OnSelect(true);

        if (IsBoardCell && shopEmoji != null)
        {
            onBoardCellHover?.Invoke(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_isEnter = false;
        m_isExit = true;

        EnableTempInfoPopup(false);

        m_upgradeSelected.OnSelect(false);

        if (IsBoardCell && shopEmoji != null)
        {
            onBoardCellHover?.Invoke(false);
        }
    }

    private void EnableTempInfoPopup(bool isEnable)
    {
        if (emojiData != null)
        {
            if (!string.IsNullOrEmpty(emojiData.emojiId))
            {
                m_infoPopup.transform.localScale = Vector3.one;
                m_infoTMP.text = AbilityDescription.Instance.GetDescription(emojiData);
                m_costTMP.text = emojiData.cost.ToString();
                m_nameTMP.text = emojiData.baseEmojiId;

                m_infoPopup.SetActive(isEnable);
                m_selectedFrame.SetActive(isEnable);
                if (isEnable && emojiData.upgradeTime == 1 && m_isBoardCell)
                {
                    m_upgradeInfoPopup.gameObject.SetActive(true);
                    m_upgradeInfoTMP.text = emojiData.Upgrade.Description;
                    m_upgradeNameTMP.text = emojiData.Upgrade.Name;
                }
                else
                {
                    m_upgradeInfoPopup.gameObject.SetActive(false);
                }
            }
        }
    }
    public void AbilityPopupEffect()
    {
        m_infoPopup.transform.localScale = Vector3.zero;
        m_infoPopup.SetActive(true);
        m_infoPopup.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                m_infoPopup.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { m_infoPopup.SetActive(false); });
            });
        });
    }

    private void OnTierChange()
    {
        if (m_isBoardCell && shopEmoji != null)
        {
            // if (emojiData == null) Debug.Log("OnTierChange::: No Emoji Data");
            // else Debug.Log("OnTierChange::: " + emojiData.abilityType.ToString());
            //Firs time fucntion is called emojidata is null, only the second time we go emojidata :::Slint:::
            if (emojiData != null)
            {
                if (emojiData.abilityType == ENUM.ABILITY_TYPE.TierChange)
                {
                    EmojiAbilityHandler.Instance.CheckEmojiAbility(emojiData);
                }
            }
        }
    }

    public static Action<bool> onSelectEmoji;

    private void OnMoving(bool isMoving, ShopCell cell)
    {
        if (cell == this)
        {
            if (isMoving)
            {
                if (!m_shadowImage.gameObject.activeSelf)
                {
                    m_shadowImage.gameObject.SetActive(true);

                    if (!m_isBoardCell)
                    {
                        PlayerData player = DataController.instance.currentPlayer;
                        if (player.gameState.coins >= cell.emojiData.cost)
                        {
                            onSelectEmoji?.Invoke(true);
                        }
                    }
                    else
                    {
                        onSelectEmoji?.Invoke(true);
                    }
                }
            }
            else
            {
                if (m_shadowImage.gameObject.activeSelf)
                {
                    m_shadowImage.gameObject.SetActive(false);

                    onSelectEmoji?.Invoke(false);
                }
            }
        }

        m_selectedFrame.gameObject.SetActive(false);
    }

    private void OnRayCastHits(ShopCell cell)
    {
        if (cell == this)
        {
            m_selectedFrame.gameObject.SetActive(true);

            m_arrow.OnSelect(true);
        }
        else
        {
            m_selectedFrame.gameObject.SetActive(false);

            m_arrow.OnSelect(false);
        }
    }

    private void OnDestroy()
    {
        CertainBoostCell.onBoost -= OnGetBoostTemp;
        EmojiAbilityHandler.onGetAffection -= OnGetAffection;
        ShopController.onTierChange -= OnTierChange;
        DragDrop.onMoving -= OnMoving;
        DragDrop.onRayCastHits -= OnRayCastHits;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
