using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CouldBeLeveledUpEffect : MonoBehaviour
{
    [SerializeField] private GameObject m_content;

    [SerializeField] private CanvasGroup m_sprite_1;
    [SerializeField] private Transform m_startPos_1;
    [SerializeField] private Transform m_finishPos_1;

    private void Awake()
    {
        ResetTween();
    }
    private void ResetTween()
    {
        m_sprite_1.transform.position = m_startPos_1.position;
        m_sprite_1.transform.localScale = Vector3.zero;

        tween1.Kill();
        tween1_1.Kill();
    }
    Tweener tween1;
    Tweener tween1_1;
    private void Run()
    {
        tween1 = m_sprite_1.transform.DOMove(m_finishPos_1.position, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        tween1_1 = m_sprite_1.transform.DOScale(1, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
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
