using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [SerializeField] private List<UIBase> __UIHistoryList = new List<UIBase>();
    [SerializeField] private List<UIBase> _UIBaseList = new List<UIBase>();
    private Dictionary<string, UIBase> _UIDict = new Dictionary<string, UIBase>();

    private Transform _UIRootTF;
    #endregion

    #region Unity Functions
    public void Start()
    {
        //if(_UIBaseList.Count != 0)
        //{
        //    foreach(UIBase each in _UIBaseList)
        //    {
        //        _UIDict.Add(each.name, each);
        //    }
        //}

        _UIRootTF = transform.GetChild(0).transform;
        DontDestroyOnLoad(_UIRootTF);
    }

    public override void Initialize()
    {
        base.Initialize();
        _UIRootTF = transform.GetChild(0).transform;
        DontDestroyOnLoad(_UIRootTF);
    }
    #endregion

    #region Public Functions
    public UIBase GetUI(string uiName)
    {
        if (_UIDict.ContainsKey(uiName))
        {
            return _UIDict[uiName];
        }

        return null;
    }

    public void ShowUIWithName(string uiName, params object[] objects)
    {
        if (!_UIDict.ContainsKey(uiName))
        {
            CreateUIBase(uiName);
        }

        if (_UIDict[uiName] == null) return;

        _UIDict[uiName].Show(objects);

        // Save opened ui in UIHistoryList
        __UIHistoryList.Add(_UIDict[uiName]);
    }

    public void ShowUI(string folderName, string uiName, params object[] objects)
    {
        if (!_UIDict.ContainsKey(uiName))
        {
            CreateUIBase(uiName, folderName);
        }

        if (_UIDict[uiName] == null) return;

        if (_UIDict[uiName].Opened) return;

        _UIDict[uiName].Show(objects);

        // Save opened ui in UIHistoryList
        __UIHistoryList.Add(_UIDict[uiName]);
    }

    public bool OpenedUI(string uiName, params object[] objects)
    {
        if (!_UIDict.ContainsKey(uiName)) return false;

        if (_UIDict[uiName] == null) return false;

        if (_UIDict[uiName].Opened) return true;

        return false;
    }

    public void CloseUI(string uiName)
    {
        if (!_UIDict.ContainsKey(uiName))
        {
            return;
        }

        _UIDict[uiName].Close();
        __UIHistoryList.Remove(_UIDict[uiName]);
    }

    public void RemoveUI(string uiName)
    {
        if (_UIDict.ContainsKey(uiName))
        {
            _UIDict.Remove(uiName);
        }
    }

    public void DestroyUI(string uiName)
    {
        if (_UIDict.ContainsKey(uiName))
        {
            UIBase uiBase = GetUI(uiName);
            Destroy(uiBase.gameObject);

            _UIDict.Remove(uiName);
        }
    }

    public void CloseAllUI(string exceptionUI = "")
    {
        Dictionary<string, UIBase> tempDict = new Dictionary<string, UIBase>();

        foreach (KeyValuePair<string, UIBase> each in _UIDict)
        {
                if (!each.Value == null)
                {
                    Debug.Log("Null ");
                    continue;
                }

                each.Value.Close();

                tempDict.Add(each.Key, each.Value);
        }

        //foreach (KeyValuePair<string, UIBase> each in tempDict)
        //{
        //    _UIDict.Remove(each.Key);
        //    __UIHistoryList.Remove(each.Value);
        //}

        //_UIDict.Clear();
        //__UIHistoryList.Clear();
    }

    //public void UIBotto
    public void ShowPreviousUI(params object[] objects)
    {
        UIBase previousUI = __UIHistoryList[__UIHistoryList.Count - 1];
        previousUI.Show(objects);
    }
    #endregion

    #region Private Functions
    private void CreateUIBase(string uiName, string folderName = "")
    {
        // Edited Here
        UIBase UIPrefab = LoadUIPreab(uiName, folderName);

        //UIBase UIPrefab = _UIBaseList.Find(each => each.name == uiName);

        UIBase UI = Instantiate(UIPrefab, _UIRootTF, false) as UIBase;

        UI.gameObject.name = uiName;
        UI.gameObject.SetActive(false);

        // Register new UI
        _UIDict.Add(uiName, UI);
    }

    private UIBase LoadUIPreab(string uiName, string uiPath = "")   
    {
        string formattedPath = string.Format("{0}{1}", uiPath, uiName);

        GameObject UIPrefabGO = Resources.Load<GameObject>(formattedPath);

        if (UIPrefabGO == null)
        {
            Debug.LogError(string.Format("Load UI Prefab failed: {0}", uiName));
            return null;
        }

        if (!UIPrefabGO.TryGetComponent<UIBase>(out UIBase uiBase))
        {
            Debug.Log(System.Type.GetType(uiName).ToString());
            return UIPrefabGO.AddComponent(System.Type.GetType(uiName)) as UIBase;
        }

        return UIPrefabGO.GetComponent<UIBase>();
    }

    #endregion
}
