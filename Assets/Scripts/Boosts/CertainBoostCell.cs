using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
public class CertainBoostCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CertainBoostItem m_certainBoostItem;
    [SerializeField] private GameObject m_selectedFrame;
    [SerializeField] private GameObject m_infoObject;
    [SerializeField] private GameObject m_diceTier;
    [SerializeField] private GameObject m_diceTier1;
    [SerializeField] private GameObject m_diceTier2;
    [SerializeField] private GameObject m_diceTier3;
    [SerializeField] private TextMeshProUGUI m_infoTMP;
    [SerializeField] private TextMeshProUGUI m_nameTMP;
    [SerializeField] private TextMeshProUGUI m_costTMP;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Color m_disableColor;
    [SerializeField] private Color m_enableColor;
    [SerializeField] private Color m_hoverColor;
    [SerializeField] private bool m_isSelected;
    [SerializeField] private bool m_isEnter;
    [SerializeField] private bool m_isExit;
    string destinationTag = "DropArea";
    string emojiTag = "Emoji";

    public static System.Action<CertainBoostItem> onBoost;
    public static System.Action<bool> onSelectEmoji;
    public static System.Action<bool> onEnableUpgradeSelectedAnimation;
    public CertainBoostItem GetCertainBoostItem => m_certainBoostItem;
    public GameObject DiceTier => m_diceTier;


    private void Start()
    {
        ShopCell.onBoardCellHover += OnBoardCellHover;
    }
    private void OnEnable()
    {
        m_spriteRenderer.color = m_disableColor;

        m_selectedFrame.SetActive(false);

        m_infoObject.SetActive(false);

        m_diceTier.SetActive(false);
    }

    private void OnBoardCellHover(bool isBoardCellHover)
    {
        if (m_isSelected)
        {
            if (isBoardCellHover)
            {
                m_spriteRenderer.color = m_hoverColor;

                onSelectEmoji?.Invoke(true);
            }
            else
            {
                m_spriteRenderer.color = m_enableColor;

                onSelectEmoji?.Invoke(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_certainBoostItem)
        {
            if (!m_isSelected)
            {
                m_isSelected = true;

                EnableSeletectedFrame(true);

                m_spriteRenderer.color = m_enableColor;

                onBoost?.Invoke(m_certainBoostItem);

                Debug.Log("onEnableUpgradeSelectedAnimation send");

                onEnableUpgradeSelectedAnimation?.Invoke(true);

                m_certainBoostItem.transform.DOScale(Vector3.one * 20f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    m_certainBoostItem.transform.DOScale(Vector3.one * 15f, 0.1f).SetEase(Ease.Linear);
                });
            }
            else
            {
                m_isSelected = false;

                EnableSeletectedFrame(false);

                m_spriteRenderer.color = m_disableColor;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_isEnter = true;
        m_isExit = false;
        if (m_certainBoostItem)
        {
            EnableSeletectedFrame(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_isEnter = false;
        m_isExit = true;
        if (!m_isSelected)
        {
            EnableSeletectedFrame(false);
        }
    }

    public void SetCertainBoostItem(CertainBoostItem certainBoostItem)
    {
        m_certainBoostItem = certainBoostItem;

        m_infoTMP.text = certainBoostItem.Data.Description;
        m_nameTMP.text = certainBoostItem.Data.Name.ToUpper();
        m_costTMP.text = (certainBoostItem.Data.Cost - ShopController.Instance.discountPriceForboost).ToString();

        m_diceTier.SetActive(true);

        m_diceTier1.SetActive(false);
        m_diceTier2.SetActive(false);
        m_diceTier3.SetActive(false);

        if (certainBoostItem.Data.Tier == 1)
        {
            m_diceTier1.SetActive(true);
        }
        if (certainBoostItem.Data.Tier == 2)
        {
            m_diceTier2.SetActive(true);
        }
        if (certainBoostItem.Data.Tier == 3)
        {
            m_diceTier3.SetActive(true);
        }
    }

    private void EnableSeletectedFrame(bool isEnable)
    {
        m_selectedFrame.SetActive(isEnable);

        m_infoObject.SetActive(isEnable);

        if (!isEnable)
        {
            DOVirtual.DelayedCall(0.1f, () =>
            {
                onBoost?.Invoke(null);
            });

            onEnableUpgradeSelectedAnimation?.Invoke(false);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePositon = Input.mousePosition;
        mousePositon.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePositon);
    }

    private void Update()
    {
        if (m_isSelected && m_isExit)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_isSelected = false;

                EnableSeletectedFrame(false);

                m_spriteRenderer.color = m_disableColor;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = GetMousePosition() - rayOrigin;

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection);

        if (hits.Length > 0)
        {
            // Check ray cast hit on DropArea
            if (hits[0].transform.tag == destinationTag)
            {
                if (m_certainBoostItem)
                {
                    var cell = hits[0].transform.GetComponent<ShopCell>();
                    if (cell.IsBoardCell && cell.shopEmoji != null)
                    {
                        cell.OnBoost(m_certainBoostItem);
                    }
                }
            }
            // Check ray cast hit on Emoji
            else if (hits[0].transform.tag == emojiTag)
            {
                if (m_certainBoostItem)
                {
                    var cell = hits[0].transform.parent.transform.GetComponent<ShopCell>();
                    if (cell.IsBoardCell && cell.shopEmoji != null)
                    {
                        cell.OnBoost(m_certainBoostItem);
                    }
                }
            }
        }
    }
    private void OnDestroy()
    {
        ShopCell.onBoardCellHover -= OnBoardCellHover;
    }
}
