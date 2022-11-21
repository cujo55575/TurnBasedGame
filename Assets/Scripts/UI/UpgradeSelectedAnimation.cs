using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UpgradeSelectedAnimation : MonoBehaviour
{
    [SerializeField] ShopCell m_shopCell;
    [SerializeField] RectTransform m_arrowRect;
    [SerializeField] Image m_circle;
    [SerializeField] Color m_normalColor;
    [SerializeField] Color m_selectedColor;

    private void Awake()
    {
        //CertainBoostCell.onEnableUpgradeSelectedAnimation += OnSelectEmoji;
    }
    private void Start()
    {
        Moving();
    }

    private void Moving()
    {
        m_arrowRect.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void IsEnable(bool isSelect)
    {
        if (m_shopCell.shopEmoji != null)
        {
            m_arrowRect.gameObject.SetActive(isSelect);
        }
    }
    public void OnSelect(bool isSelect)
    {
        if (isSelect)
        {
            m_circle.color = m_selectedColor;
        }
        else
        {
            m_circle.color = m_normalColor;
        }
    }
}
