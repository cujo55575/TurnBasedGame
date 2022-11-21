using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MatchEndGui : MonoBehaviour
{
    public enum Result
    {
        WIN,
        LOSE,
        DRAW
    }

    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text txtUnlockTierTitle;
    [SerializeField] private Button okayButton;
    [SerializeField] private Button btncloseUnlockTierPanel;
    [SerializeField] private GameObject unlockTierPanel;
    [SerializeField] private GameObject winLoseDrawPanel;
    [SerializeField] private Transform skullPlaceHolder;
    [SerializeField] private Transform starPlaceHolder;
    [SerializeField] private Transform unlockTiersPlaceHolders;
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private Sprite[] emojiSprites = new Sprite[3];
    [SerializeField] private Image resultImage;

    [Header("Anim Sequence Objs")]
    [SerializeField] Image mainPanelImg;
    [SerializeField] Transform dullStarsHolder, brightStarsHolder, dullSkullsHolder, brighSkullsHolder;
    [SerializeField] CanvasGroup centerInfoPanel;

    public static System.Action<int> onRollBoost;
    private void Start()
    {
        okayButton.onClick.AddListener(() => OnClickOkay());
        unlockTierPanel.SetActive(false);
        btncloseUnlockTierPanel.onClick.AddListener(() => CloseUnlockPanel());
        placeHolder.SetActive(false);
    }

    public void Show(Result result)
    {
        gameObject.SetActive(true);
        winLoseDrawPanel.SetActive(true);

        var gameState = DataController.instance.currentPlayer.gameState;
        // var stats = string.Format("round: {0} \n hp: {1}/{2} \n wins: {3}",
        //     gameState.round + 1, gameState.health, GameConstants.MAX_PLAYER_HEALTH, gameState.wins);
        // text.text = "\n" + stats;
        HideSkull();
        ShowSkull(gameState.health, gameState.wins);
        if (result == Result.WIN)
        {
            resultImage.GetComponent<Image>().sprite = emojiSprites[0];
            gameState.winRound++;
            text.text = "Victory";
            // text.text += "\n You won :)";
        }
        else if (result == Result.LOSE)
        {
            resultImage.GetComponent<Image>().sprite = emojiSprites[1];
            gameState.loseRound++;
            text.text = "Defeat";
            // text.text += "\n Defeat :(";
        }
        else
        {
            resultImage.GetComponent<Image>().sprite = emojiSprites[2];
            if (gameState.wins >= 1)
            {
                gameState.wins--;
            }
            text.text = "Draw";
            //     var mstats = string.Format("round: {0} \n hp: {1}/{2} \n wins: {3}",
            // gameState.round + 1, gameState.health, GameConstants.MAX_PLAYER_HEALTH, gameState.wins);
            //     text.text = "\n" + mstats;
            //     text.text += "\n Draw";
        }

        StartAnimSequence();
    }
    private void ShowSkull(int skullcount, int starCount)
    {
        for (int i = 0; i < skullcount; i++)
        {
            skullPlaceHolder.GetChild(i).GetComponent<Image>().enabled = true;
        }
        for (int i = 0; i < starCount; i++)
        {
            starPlaceHolder.GetChild(i).GetComponent<Image>().enabled = true;
        }
    }
    private void HideSkull()
    {
        for (int i = 0; i < 10; i++)
        {
            skullPlaceHolder.GetChild(i).GetComponent<Image>().enabled = false;
            starPlaceHolder.GetChild(i).GetComponent<Image>().enabled = false;
        }
        Reset();
    }
    public void OnClickOkay()
    {
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round == 0 || round % 2 == 0 || round > 5)
        {
            gameObject.SetActive(false);
            HideSkull();
            GameController.instance.StartNextRound();
        }
        else
        {
            winLoseDrawPanel.SetActive(false);
            ShowUnlockedTier();
        }
    }
    private void CloseUnlockPanel()
    {
        for (int i = 0; i < unlockTiersPlaceHolders.transform.childCount; i++)
        {
            Destroy(unlockTiersPlaceHolders.transform.GetChild(i).gameObject);
        }
        unlockTierPanel.SetActive(false);
        gameObject.SetActive(false);
        HideSkull();
        GameController.instance.StartNextRound();
    }
    const float TAU = 6.28318530718f;
    private void ShowUnlockedTier()
    {
        int round = DataController.instance.currentPlayer.gameState.round;
        if (round % 2 != 0 && round <= 5)
        {
            Debug.Log("Inside ShowUnlockedTierMethod ::: round " + round);
            unlockTierPanel.SetActive(true);
            List<string> baseEmojiIds = new List<string>();
            txtUnlockTierTitle.text = "Tier " + tierIndex(round) + " Emojis Unlocked!";
            baseEmojiIds = DataController.instance.EmojiNameList(DataController.instance.GetTierName(round + 1));
            Debug.Log("Inside ShowUnlockedTierMethod ::: GetTierName value " + DataController.instance.GetTierName(round + 1));
            for (int i = 0; i < baseEmojiIds.Count; i++)
            {
                float t = i / (float)baseEmojiIds.Count;
                float angRad = t * TAU;
                float x = Mathf.Cos(angRad);
                float y = Mathf.Sin(angRad);
                Vector2 point = new Vector2(x, y);
                GameObject m_placeHolder = Instantiate(placeHolder, new Vector3(point.x * 230, point.y * 230, 0), Quaternion.identity);
                m_placeHolder.transform.SetParent(unlockTiersPlaceHolders, false);
                m_placeHolder.SetActive(true);
                CreateEmoji(EmojiData.MakeEmoji(baseEmojiIds[i]), m_placeHolder.transform);
            }
            DataController.instance.currentPlayer.gameState.tierUnlock = tierIndex(round);
            Debug.Log("ShowUnlockedTier:: tierIndex " + tierIndex(round) + " Round " + round + " tierUnlocIndex " + DataController.instance.currentPlayer.gameState.tierUnlock);
        }
    }
    //need to refactor
    private int tierIndex(int index)
    {
        if (index == 1) return 2;
        else if (index == 3) return 3;
        else if (index == 5) return 4;
        return 0;
    }
    private void CreateEmoji(EmojiData emojiData, Transform parent)
    {
        GameObject unlockEmoji = Instantiate(ModelUtils.GetDataFromResources(emojiData.baseEmojiId).gameObject);
        unlockEmoji.transform.SetParent(parent, false);
        parent.transform.GetChild(0).GetComponent<TMP_Text>().text = emojiData.baseEmojiId;
        unlockEmoji.AddComponent<BoxCollider>();
        unlockEmoji.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().speed = 0.5f;
        unlockEmoji.tag = "Emoji";
        unlockEmoji.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        unlockEmoji.transform.localPosition = new Vector3(0, 9, 0);
        unlockEmoji.transform.localRotation = Quaternion.Euler(0, 180, 0);
        unlockEmoji.transform.localScale = Vector3.one * 15;
        unlockEmoji.SetActive(true);
    }

    private async void StartAnimSequence()
    {
        mainPanelImg.DOFade(0.75f, 1f).OnComplete(() =>
        {
            AnimateStars();
            AnimateSkulls();
        });
    }

    private async void AnimateStars()
    {
        for (int i = 0; i < brightStarsHolder.childCount; i++)
        {
            Transform dullStar = dullStarsHolder.GetChild(i);
            Transform brightStar = brightStarsHolder.GetChild(i);

            dullStar.localScale = Vector3.zero;
            brightStar.localScale = Vector3.zero;

            dullStar.GetComponent<Image>().DOFade(1, 0.75f);
            dullStar.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            brightStar.GetComponent<Image>().DOFade(1, 0.75f);
            brightStar.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            await System.Threading.Tasks.Task.Delay(100);
        }
    }

    private async void AnimateSkulls()
    {
        for (int i = dullSkullsHolder.childCount - 1; i >= 0; i--)
        {
            Transform dullSkull = dullSkullsHolder.GetChild(i);
            Transform brightSkull = brighSkullsHolder.GetChild(i);

            dullSkull.localScale = Vector3.zero;
            brightSkull.localScale = Vector3.zero;

            dullSkull.GetComponent<Image>().DOFade(1, 0.75f);
            dullSkull.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            if (i != 0)
            {
                brightSkull.GetComponent<Image>().DOFade(1, 0.75f);
                brightSkull.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            }
            else
            {
                brightSkull.GetComponent<Image>().DOFade(1, 0.75f);
                brightSkull.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    centerInfoPanel.DOFade(1, 0.1f).OnComplete(() =>
                    {
                        centerInfoPanel.interactable = true;
                    });
                });
            }

            await System.Threading.Tasks.Task.Delay(100);
        }
    }

    private void Reset()
    {
        for (int i = 0; i < brightStarsHolder.childCount; i++)
        {
            Transform dullStar = dullStarsHolder.GetChild(i);
            Transform brightStar = brightStarsHolder.GetChild(i);

            dullStar.localScale = Vector3.zero;
            brightStar.localScale = Vector3.zero;

            dullStar.GetComponent<Image>().DOFade(0, 0);
            brightStar.GetComponent<Image>().DOFade(0, 0);
        }

        for (int i = dullSkullsHolder.childCount - 1; i >= 0; i--)
        {
            Transform dullSkull = dullSkullsHolder.GetChild(i);
            Transform brightSkull = brighSkullsHolder.GetChild(i);

            dullSkull.localScale = Vector3.zero;
            brightSkull.localScale = Vector3.zero;

            dullSkull.GetComponent<Image>().DOFade(0, 0);
            brightSkull.GetComponent<Image>().DOFade(0, 0);         
        }
        centerInfoPanel.DOFade(0, 0);
    }
}
