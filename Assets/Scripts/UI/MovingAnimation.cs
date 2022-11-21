using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class MovingAnimation : MonoBehaviour
{
    public Transform m_target;
    [Button("Move")]
    public void Move()
    {
        transform.DOJump(m_target.position, 10f, 4, 1f).SetEase(Ease.Linear);
    }
}
