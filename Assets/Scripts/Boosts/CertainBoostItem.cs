using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CertainBoostItem : MonoBehaviour
{
    [SerializeField] private CertainBoost m_certainBoostScriptableObjectData;

    public CertainBoostData Data => m_certainBoostScriptableObjectData.CertainBoostData;
}
