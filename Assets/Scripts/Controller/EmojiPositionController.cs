using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class EmojiPositionController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private EmojiController m_emojiController;
    [SerializeField] private TextMeshProUGUI m_abilityName;
    [SerializeField] private TextMeshProUGUI m_abilityDescription;
    [SerializeField] private TextMeshProUGUI m_upgradeName;
    [SerializeField] private TextMeshProUGUI m_upgradeDescription;
    [SerializeField] private TextMeshProUGUI m_level;
    [SerializeField] private UpgradeEffect upgradeEffect;
    [SerializeField] private GameObject m_abilityContent;
    [SerializeField] private GameObject m_upgradeContent;
    [SerializeField] private GameObject m_levelContent;
    [SerializeField] private Image m_expSlider;

    [SerializeField] private GameObject m_abilityPopup;
    [SerializeField] private TextMeshProUGUI m_abilityPopupName;
    [SerializeField] private TextMeshProUGUI m_abilityPopupDescription;

    public EmojiController EmojiController => m_emojiController;

    private void Start()
    {
        EmojiAbilityHandler.onAbilityUsing += AbilityPopupEffect;
    }
    public void SetEmojiController(EmojiController emojiController)
    {
        upgradeEffect.Hide();
        m_emojiController = emojiController;

        if (emojiController != null)
        {
            m_abilityName.text = emojiController.data.baseEmojiId;
            m_abilityDescription.text = AbilityDescription.Instance.GetDescription(emojiController.data);
            m_abilityDescription.fontStyle = TMPro.FontStyles.Bold;
            m_upgradeName.text = emojiController.data.Upgrade.Name;
            m_upgradeDescription.text = emojiController.data.Upgrade.Description;
            m_upgradeDescription.fontStyle = TMPro.FontStyles.Bold;

            m_abilityPopupName.text = emojiController.data.baseEmojiId;
            m_abilityPopupDescription.text = AbilityDescription.Instance.GetDescription(emojiController.data);
            m_abilityPopupDescription.fontStyle = TMPro.FontStyles.Bold;

            m_level.text = emojiController.data.level.ToString();
            m_levelContent.SetActive(true);

            ExpSliderUp(emojiController.data.levelStackTimes);

            if (emojiController.data.upgradeTime == 1)
            {
                upgradeEffect.SetImage(emojiController.data.Upgrade.Avatar);
                upgradeEffect.Show();
            }
        }
        else
        {
            ExpSliderUp(0);

            m_levelContent.SetActive(false);

            upgradeEffect.Hide();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_emojiController != null)
        {
            m_abilityContent.SetActive(true);

            if (m_emojiController.data.Upgrade != null)
            {
                if (!string.IsNullOrEmpty(m_emojiController.data.Upgrade.ID))
                {
                    m_upgradeContent.SetActive(true);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_emojiController != null)
        {
            m_abilityContent.SetActive(true);

            if (m_emojiController.data.Upgrade != null)
            {
                if (!string.IsNullOrEmpty(m_emojiController.data.Upgrade.ID))
                {
                    m_upgradeContent.SetActive(true);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_abilityContent.SetActive(false);
        m_upgradeContent.SetActive(false);
    }

    private void Awake()
    {
        m_emojiController = GetComponentInChildren<EmojiController>();
    }

    public void AbilityPopupEffect(EmojiData emojiData)
    {
        if (m_emojiController == null)
        {
            return;
        }

        if (emojiData.emojiId == m_emojiController.data.emojiId)
        {
            m_abilityPopup.transform.localScale = Vector3.zero;
            m_abilityPopup.SetActive(true);
            upgradeEffect.Hide();
            m_abilityPopup.transform.DOScale(Vector3.one * 0.1225714f, 0.5f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    m_abilityPopup.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { m_abilityPopup.SetActive(false); });
                });
            });
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



    public void Update()
    {
        if (m_emojiController != null)
        {
            m_levelContent.transform.localPosition = new Vector2(m_emojiController.transform.localPosition.x, m_emojiController.transform.localPosition.y + 8);

            upgradeEffect.transform.localPosition = new Vector2(m_emojiController.transform.localPosition.x, m_emojiController.transform.localPosition.y - 7);

            if (m_emojiController.data.health <= 0)
            {
                m_levelContent.SetActive(false);
            }
        }
    }
}
