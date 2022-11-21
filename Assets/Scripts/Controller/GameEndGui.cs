using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameEndGui : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Button okayButton;
    [SerializeField] Button menuButton;
    [SerializeField] Image img;
    [SerializeField] GameObject shopControllerBoost;

    public static System.Action<int> onResetUpgrade;

    public static GameEndGui instance;
    void Awake()
    {
        if (instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        if (shopControllerBoost != null)
            shopControllerBoost.SetActive(false);

        okayButton.onClick.AddListener(OnClickPlayAgain);
        menuButton.onClick.AddListener(OnClickBackToMenu);
    }

    public void Show(bool win)
    {
        gameObject.SetActive(true);
        var player = DataController.instance.currentPlayer;
        if (win)
        {
            Debug.Log("GameEndGui::SHOW");
            img.gameObject.SetActive(true);
            img.sprite = GetSprites("Asset " + Random.Range(0, 23));
            text.text = string.Format(
                "you set a new world record\n" +
                "No other player has made it to {0} rounds",
                player.gameState.winRound);
        }
        else
        {
            img.gameObject.SetActive(false);
            string winRound = player.gameState.winRound > 0 ? player.gameState.winRound == 1 ? string.Format("and you won {0} round", player.gameState.winRound) :
            string.Format("and you won {0} rounds", player.gameState.winRound) : "";
            text.text = string.Format(
            "game over\n" +
            "you played {0} rounds\n",
            player.gameState.round) + winRound;
        }
    }

    public void OnClickPlayAgain()
    {
        shopControllerBoost.SetActive(true);
        gameObject.SetActive(false);
        GameController.instance.PlayAgain();
    }

    public void OnClickBackToMenu()
    {
        OnClickPlayAgain();
        // gameObject.SetActive(false);
        // Debug.LogWarning("can't go back to menu because it doesn't exist yet");
        // TODO: implement main menu
    }
    public Sprite GetSprites(string Name)
    {
        return Resources.Load<Sprite>("Stickers/" + Name);
    }
}
