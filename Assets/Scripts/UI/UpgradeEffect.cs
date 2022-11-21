using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UpgradeEffect : MonoBehaviour
{
    [SerializeField] private GameObject m_content;

    [SerializeField] private CanvasGroup m_sprite_1;
    [SerializeField] private Transform m_startPos_1;
    [SerializeField] private Transform m_finishPos_1;

    [SerializeField] private CanvasGroup m_sprite_2;
    [SerializeField] private Transform m_startPos_2;
    [SerializeField] private Transform m_finishPos_2;

    [SerializeField] private CanvasGroup m_sprite_3;
    [SerializeField] private Transform m_startPos_3;
    [SerializeField] private Transform m_finishPos_3;

    Tweener tween;
    private void Awake()
    {
        ResetTween();
    }

    private void ResetTween()
    {
        m_sprite_1.transform.position = m_startPos_1.position;
        m_sprite_1.transform.localScale = Vector3.zero;

        m_sprite_2.transform.position = m_startPos_2.position;
        m_sprite_2.transform.localScale = Vector3.zero;

        m_sprite_3.transform.position = m_startPos_3.position;
        m_sprite_3.transform.localScale = Vector3.zero;

        tween1.Kill();
        tween1_1.Kill();
        tween2.Kill();
        tween2_2.Kill();
        tween3.Kill();
        tween3_3.Kill();
    }

    Tweener tween1;
    Tweener tween1_1;
    Tweener tween2;
    Tweener tween2_2;
    Tweener tween3;
    Tweener tween3_3;
    private void Run()
    {
        ResetTween();

        tween1 = m_sprite_1.transform.DOMove(m_finishPos_1.position, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        tween1_1 = m_sprite_1.transform.DOScale(1, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            tween2 = m_sprite_2.transform.DOMove(m_finishPos_2.position, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            tween2_2 = m_sprite_2.transform.DOScale(1, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });

        DOVirtual.DelayedCall(1f, () =>
        {
            tween3 = m_sprite_3.transform.DOMove(m_finishPos_3.position, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            tween3_3 = m_sprite_3.transform.DOScale(1, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
    }

    public void SetImage(Sprite sprite)
    {
        m_sprite_1.GetComponent<Image>().sprite = sprite;
        m_sprite_2.GetComponent<Image>().sprite = sprite;
        m_sprite_3.GetComponent<Image>().sprite = sprite;

    }

    public void Show()
    {
        m_content.SetActive(true);

        Run();
    }

    public void Hide()
    {
        m_content.SetActive(false);
    }
}
