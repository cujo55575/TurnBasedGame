using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    ShopController shopController;
    [SerializeField]
    BattleController battleController;
    [SerializeField]
    DataController dataController;
    [SerializeField]
    Transform loadingScreen;
    [SerializeField]
    GameEndGui gameEndGui;

    public static GameController instance;
    public GameController()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    IEnumerator LoadNextRoundCoroutine()
    {
        instance.shopController.Hide();
        instance.battleController.Hide();
        loadingScreen.gameObject.SetActive(true);
        while (!DataController.instance.nextRoundLoaded)
        {
            yield return new WaitForEndOfFrame();
        }
        loadingScreen.gameObject.SetActive(false);
        GoToBattle();
    }

    IEnumerator LoadDataCoroutine()
    {
        instance.shopController.Hide();
        instance.battleController.Hide();
        loadingScreen.gameObject.SetActive(true);

        dataController.StartLoadingData("me");
        while (!DataController.instance.isReady)
        {
            yield return new WaitForEndOfFrame();
        }

        loadingScreen.gameObject.SetActive(false);
        instance.dataController.StartLoadingNextRoundSnapshots(DataController.instance.currentPlayer.gameState.round);
        GoToShop();
    }
    // GameConstants gameConstants = new GameConstants();
    public void StartNextRound()
    {
        var player = DataController.instance.currentPlayer;
        if (player.gameState.health <= 0)
        {
            GoToEndGame(false);
        }
        else
        {
            player.gameState.round += 1;
            if (player.gameState.winRound == 10)
            {
                GoToEndGame(true);
                return;
            }
            //player.gameState.coins += GameConstants.EACH_ROUND_COINS;
            player.gameState.coins = GameConstants.EACH_ROUND_COINS;
            instance.dataController.StartLoadingNextRoundSnapshots(player.gameState.round);
            GoToShop();
        }
    }

    public void GoToShop()
    {
        instance.shopController.Show();
        instance.battleController.Hide();
    }
    public void GoToEndGame(bool win)
    {
        instance.shopController.Hide();
        instance.battleController.Hide();
        instance.gameEndGui.Show(win);
        ShopController.Instance.baseEmojiIds = new List<string>();
        GameConstants.EACH_ROUND_COINS = 10;
        GameConstants.INITIAL_COINS = 10;
    }

    PlayerData FindPlayerSnapshotToFight(int index)
    {
        List<PlayerData> roundPlayers = DataController.instance.nextRoundData;
        if (roundPlayers == null)
        {
            return null;
        }

        List<PlayerData> notMePlayers = new List<PlayerData>();
        foreach (PlayerData player in roundPlayers)
        {
            if (player.id != DataController.instance.currentPlayerId)
            {
                notMePlayers.Add(player);
            }
        }

        if (notMePlayers.Count == 0)
        {
            return null;
        }

        return notMePlayers[index];
    }

    public void GoToBattle()
    {
        if (!DataController.instance.nextRoundLoaded)
        {
            instance.StartCoroutine(instance.LoadNextRoundCoroutine());
            return;
        }

        var player = DataController.instance.currentPlayer;
        DataController.instance.SavePlayerSnapshot(player);

        var opponent = instance.FindPlayerSnapshotToFight(player.gameState.round);
        if (opponent == null)
        {
            GoToEndGame(true);
            return;
        }
        EmojiAbilityHandler.Instance.enemyEmojis = new List<EmojiData>();
        for (int i = 0; i < opponent.emojis.Count; i++)
        {
            EmojiAbilityHandler.Instance.enemyEmojis.Add(opponent.emojis[i]);
        }

        instance.shopController.Hide();
        instance.battleController.gameObject.SetActive(true);
        instance.battleController.SetupBattle(
            player,
            opponent);
    }

    public void PlayAgain()
    {
        DataController.instance.currentPlayer.Reset();
        GoToShop();
    }
}
