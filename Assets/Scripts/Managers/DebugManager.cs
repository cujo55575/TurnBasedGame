using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager ms_instance;
    private Rect Rect_DebugWin = new Rect(0, 0, 300, 500);
    private Rect Rect_AllWnd = new Rect(0, 130, 300, 450);
    private Rect Rect_CloseBtn = new Rect(250, 0, 50, 20);
    private Rect Rect_FullBtn = new Rect(0, 0, 300, 50);
    public static bool Show_DebugWin = false;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Show_DebugWin = !Show_DebugWin;
        }
        else if (Input.touchCount > 3)
        {
            Show_DebugWin = !Show_DebugWin;
        }

    }
    void OnGUI()
    {
        if (!Show_DebugWin)
            return;
        GUI.Window(0, Rect_DebugWin, doDebugWin, "--------------------Debug--------------------");

    }
    void doDebugWin(int windowId)
    {
        if (GUI.Button(Rect_CloseBtn, "x"))
        {
            Show_DebugWin = false;
        }


        if (GUI.Button(Rect_FullBtn, "Cheat Panel"))
        {
        }


        if (GUI.Button(new Rect(0, 100, 300, 50), "Increase coin to 100"))
        {
            GameConstants.EACH_ROUND_COINS = 100;
            GameConstants.INITIAL_COINS = 100;
            PlayerData player = DataController.instance.currentPlayer;
            PlayerGameState gameState = player.gameState;
            gameState.coins = GameConstants.INITIAL_COINS;
            ShopController.Instance.m_coin.text = gameState.coins.ToString();
        }
        if (GUI.Button(new Rect(0, 150, 300, 50), "Set coin to 10"))
        {
            GameConstants.EACH_ROUND_COINS = 10;
            GameConstants.INITIAL_COINS = 10;
            PlayerData player = DataController.instance.currentPlayer;
            PlayerGameState gameState = player.gameState;
            gameState.coins = GameConstants.INITIAL_COINS;
            ShopController.Instance.m_coin.text = gameState.coins.ToString();
        }
    }
}
