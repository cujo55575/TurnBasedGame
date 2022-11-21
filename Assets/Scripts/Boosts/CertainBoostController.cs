using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CertainBoostController : MonoBehaviour
{
    [SerializeField] private List<CertainBoostItem> m_listOfCertainBoost;
    [SerializeField] private List<CertainBoostCell> m_listOfSpot;
    [SerializeField] private List<CertainBoostItem> m_listOfCurrentBoost;
    [SerializeField] private List<GameObject> m_listBoostPos;

    private void Awake()
    {
        ShopController.onRollBoost += RandomBoosts;
    }

    private void Start()
    {
        RandomBoosts(1);
    }

    private void RandomBoosts(int amount)
    {
        if (amount >= 3)
        {
            amount = 3;
        }
        else if (amount <= 1)
        {
            amount = 1;
        }

        foreach (var item in m_listBoostPos)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < amount; i++)
        {
            m_listBoostPos[i].SetActive(true);
        }

        foreach (var item in m_listOfCurrentBoost)
        {
            if (item)
            {
                Destroy(item.gameObject);
            }
        }
        m_listOfCurrentBoost.Clear();

        var tierUnlock = DataController.instance.currentPlayer.gameState.tierUnlock;

        Debug.Log("tierUnlock = " + tierUnlock);

        for (int i = 0; i < amount; i++)
        {
            var randNum = UnityEngine.Random.Range(0, m_listOfCertainBoost.Count);

            while (m_listOfCertainBoost[randNum].Data.Tier > tierUnlock)
            {
                randNum = UnityEngine.Random.Range(0, m_listOfCertainBoost.Count);
            }

            if (randNum == m_listOfCertainBoost.Count)
            {
                randNum -= 1;
            }

            var newBoost = Instantiate(m_listOfCertainBoost[randNum], m_listOfSpot[i].transform);
            newBoost.transform.localPosition = new Vector3(0, 9, 0);
            newBoost.transform.localScale = Vector3.one * 15f;

            m_listOfSpot[i].SetCertainBoostItem(newBoost);

            m_listOfCurrentBoost.Add(newBoost);
        }
    }

    private void OnDestroy()
    {
        ShopController.onRollBoost -= RandomBoosts;
    }
}
