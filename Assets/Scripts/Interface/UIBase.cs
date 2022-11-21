using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    #region Public Functions
    public bool Opened
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }

    public virtual void OnShow(object[] objects)
    {

    }
    public void Show(object[] objects)
    {
        if (Opened) return;
        gameObject.SetActive(true);
        OnShow(objects);
    }

    public void Close()
    {
        if (!Opened) return;
        gameObject.SetActive(false);
    }
    #endregion
}
