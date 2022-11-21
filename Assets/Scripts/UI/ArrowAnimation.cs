using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ArrowAnimation : MonoBehaviour
{
    [SerializeField] ShopCell m_shopCell;
    [SerializeField] RectTransform m_arrowRect;
    [SerializeField] Image m_arrowImage;
    [SerializeField] Color m_normalColor;
    [SerializeField] Color m_selectedColor;

    private void Awake()
    {
        ShopCell.onSelectEmoji += OnSelectEmoji;
    }
    private void Start()
    {
        Moving();
    }

    private void Moving()
    {
        m_arrowRect.DOAnchorPos3DY(m_arrowRect.anchoredPosition.y - 40, 0.8f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnSelectEmoji(bool isSelect)
    {
        if (m_shopCell.shopEmoji == null)
        {
            m_arrowImage.color = m_normalColor;
            m_arrowImage.gameObject.SetActive(isSelect);
        }
    }
    public void OnSelect(bool isSelect)
    {
        if (isSelect)
        {
            m_arrowImage.color = m_selectedColor;
        }
        else
        {
            m_arrowImage.color = m_normalColor;
        }
    }
}
