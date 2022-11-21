using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class BattleUI : MonoBehaviour
{
    [SerializeField] private Button m_pauseButton;

    [SerializeField] private Button m_playButton;

    [SerializeField] private Button m_fastButton;
    [SerializeField] private Button m_autoPlayButton;
    [SerializeField] private TextMeshProUGUI m_fastTMP;
    [SerializeField] private Image m_fastIcon;
    [SerializeField] private Image m_fastIcon2;
    [SerializeField] private Image m_autoPlayIcon;

    [SerializeField] private Color m_normal;
    [SerializeField] private Color m_active;

    [SerializeField] private TextMeshProUGUI m_myEmoJiDamageTMP;
    [SerializeField] private TextMeshProUGUI m_opponentEmojiDamageTMP;
    public static System.Action<bool> onIsAutoPlay;

    private bool m_isPause;
    [SerializeField] private bool m_isFast;
    [SerializeField] private bool m_isAutoPlay;
    private int m_timeScale;
    private bool m_isRightPlace;

    private void OnEnable()
    {
        UpdateSpeed(1);
        m_isFast = false;
        m_isPause = false;
        m_isAutoPlay = false;
        m_isRightPlace = false;
        m_playButton.gameObject.SetActive(false);
        m_pauseButton.gameObject.SetActive(true);
        m_fastIcon.color = m_normal;
        m_fastIcon2.color = m_normal;
    }

    private void OnDisable()
    {
        UpdateSpeed(1);
        m_isFast = false;
        m_isPause = false;
        m_isAutoPlay = false;
        m_isRightPlace = false;
    }
    private void Start()
    {
        m_pauseButton.onClick.AddListener(Pause);
        m_playButton.onClick.AddListener(Play);
        m_fastButton.onClick.AddListener(Fast);
        m_autoPlayButton.onClick.AddListener(AutoPlay);

        BattleController.instance.onShowMyEmojiDamageText += ShowMyEmojiDamageText;
        BattleController.instance.onShowOpponentEmojiDamageText += ShowOpponentEmojiDamageText;
        BattleController.instance.onIsRightPlace += EmojiIsRightPlace;
    }
    private void EmojiIsRightPlace(bool isRightPlace)
    {
        m_isRightPlace = isRightPlace;

        if (isRightPlace)
        {
            m_playButton.gameObject.SetActive(true);
            m_pauseButton.gameObject.SetActive(false);
        }
        else
        {
            m_playButton.gameObject.SetActive(true);
            m_pauseButton.gameObject.SetActive(false);
        }
    }

    private void Play()
    {
        //if (!m_isRightPlace)
        {
            m_playButton.gameObject.SetActive(false);
            m_pauseButton.gameObject.SetActive(true);

            m_isPause = false;

            Time.timeScale = 1;
            EnlargeButtons(0);
        }
    }

    private void Pause()
    {
        m_playButton.gameObject.SetActive(true);
        m_pauseButton.gameObject.SetActive(false);

        m_isPause = true;

        m_isAutoPlay = false;
        onIsAutoPlay?.Invoke(m_isAutoPlay);

        m_isFast = false;
        m_fastTMP.color = m_normal;
        m_fastIcon.color = m_normal;
        m_fastIcon2.color = m_normal;


        Time.timeScale = 0;
        EnlargeButtons(1);
    }

    private void Fast()
    {
        m_playButton.gameObject.SetActive(true);
        m_pauseButton.gameObject.SetActive(false);

        m_isFast = !m_isFast;
        if (m_isFast)
        {
            Time.timeScale = 2;
            m_fastIcon.color = m_active;
            m_fastIcon2.color = m_active;
        }
        else
        {
            Time.timeScale = 1;
            m_fastIcon.color = m_normal;
            m_fastIcon2.color = m_normal;
        }
        EnlargeButtons(3);
    }

    private void AutoPlay()
    {
        m_playButton.gameObject.SetActive(false);
        m_pauseButton.gameObject.SetActive(true);

        m_isPause = false;

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        m_isAutoPlay = !m_isAutoPlay;
        onIsAutoPlay?.Invoke(m_isAutoPlay);
        if (m_isAutoPlay)
        {
            m_autoPlayIcon.color = m_active;
        }
        else
        {
            m_autoPlayIcon.color = m_normal;
        }
        EnlargeButtons(2);
    }
    private void EnlargeButtons(int i)
    {
        Vector3 scaleValue = new Vector3(1.2f, 1.2f, 1.2f);
        if (i == 0)
        {
            m_playButton.gameObject.transform.localScale = scaleValue;
            m_pauseButton.gameObject.transform.localScale = Vector3.one;
            m_autoPlayButton.gameObject.transform.localScale = Vector3.one;
            m_fastButton.gameObject.transform.localScale = Vector3.one;
        }
        if (i == 1)
        {
            m_playButton.gameObject.transform.localScale = Vector3.one;
            m_pauseButton.gameObject.transform.localScale = scaleValue;
            m_autoPlayButton.gameObject.transform.localScale = Vector3.one;
            m_fastButton.gameObject.transform.localScale = Vector3.one;
        }
        if (i == 2)
        {
            m_playButton.gameObject.transform.localScale = Vector3.one;
            m_pauseButton.gameObject.transform.localScale = Vector3.one;
            m_autoPlayButton.gameObject.transform.localScale = scaleValue;
            m_fastButton.gameObject.transform.localScale = Vector3.one;
        }
        if (i == 3)
        {
            m_playButton.gameObject.transform.localScale = Vector3.one;
            m_pauseButton.gameObject.transform.localScale = Vector3.one;
            m_autoPlayButton.gameObject.transform.localScale = Vector3.one;
            m_fastButton.gameObject.transform.localScale = scaleValue;
        }
    }
    private void UpdateSpeed(int speed)
    {
        Time.timeScale = speed;
    }

    private DOTween tween;
    public void ShowMyEmojiDamageText(string damage)
    {
        m_myEmoJiDamageTMP.text = damage;
        m_myEmoJiDamageTMP.gameObject.SetActive(true);

        m_myEmoJiDamageTMP.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
        {
            m_myEmoJiDamageTMP.transform.DOScale(Vector3.one * 1f, 0.5f).OnComplete(() => { m_myEmoJiDamageTMP.gameObject.SetActive(false); });
        });
    }
    public void ShowOpponentEmojiDamageText(string damage)
    {
        m_opponentEmojiDamageTMP.text = damage;
        m_opponentEmojiDamageTMP.gameObject.SetActive(true);

        m_opponentEmojiDamageTMP.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
        {
            m_opponentEmojiDamageTMP.transform.DOScale(Vector3.one * 1f, 0.5f).OnComplete(() => { m_opponentEmojiDamageTMP.gameObject.SetActive(false); });
        });
    }

    private void OnDestroy()
    {
        BattleController.instance.onIsRightPlace -= EmojiIsRightPlace;
    }
}
