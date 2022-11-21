using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static ENUM;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponStatus
{
    WeaponData data;
    public int damage = 0;
    public System.DateTime nextShootTime;

    public WeaponStatus(WeaponData weaponData)
    {
        data = weaponData;
        damage = (int)(data.DPS / data.SPS);
        Reset();
    }

    public void Reset()
    {
        nextShootTime = System.DateTime.Now.AddSeconds(1 / data.SPS);
    }
}

public class EmojiController : MonoBehaviour
{
    public PlayerData playerData;
    public EmojiData data;
    public BoardInfo boardInfo;
    public string playerId;
    [SerializeField] GameObject smokePrefab;
    [SerializeField] private Outline battleHighlight;

    public TMP_Text txtHealth;
    public TMP_Text txtDamage;

    public bool isCurrentEmoji = false;
    public bool onBattleGround = false;
    void Start()
    {
        EmojiAbilityHandler.onGetAffection += ModifyHpAndDamage;
        m_body = transform.GetChild(0).gameObject;
        m_isAlive = true;
    }
    void OnDestroy()
    {
        EmojiAbilityHandler.onGetAffection -= ModifyHpAndDamage;
    }
    private void ModifyHpAndDamage()
    {
        if (data != null)
        {
            txtHealth.text = data.health <= 0 ? 0.ToString() : data.health.ToString();
            txtDamage.text = data.damage <= 0 ? 0.ToString() : data.damage.ToString();
            if (onBattleGround && data.health <= 0)
            {
                // onDeath.Invoke();
                // Destroy(gameObject);
            }
        }
    }

    public void Setup(BoardInfo boardInfo, PlayerData playerData, EmojiData data)
    {
        this.boardInfo = boardInfo;
        this.playerData = playerData;
        this.playerId = this.playerData.id;
        this.data = data;
        onBattleGround = true;
        txtHealth.text = this.data.health <= 0 ? 0.ToString() : this.data.health.ToString();
        txtDamage.text = this.data.damage.ToString();
    }

    #region Shoot Code
    //void Shoot(WeaponData weapon)
    //{
    //    print("shooting weapon " + weapon.name);
    //    var enemyShips = BattleController.getEnemyShips(playerId);
    //    if (enemyShips.Count == 0)
    //    {
    //        return;
    //    }

    //    var randomEnemy = enemyShips[Random.Range(0, enemyShips.Count)];
    //    ShootAt(weapon, randomEnemy, false);
    //}

    //void ShootAt(WeaponData weapon, EmojiController target, bool miss)
    //{
    //    var projectile = Instantiate(projectilePrefab);
    //    projectile.Setup(transform, target.transform, weapon);
    //    if (!miss)
    //    {
    //        target.TakeDamage(target, weapon.Damage);
    //    }
    //}
    private void GenerateDeadSmokeEffect(Vector3 direction)
    {
        GameObject smoke = Instantiate(smokePrefab, transform);
        smoke.transform.rotation = Quaternion.Euler(0, 90 * direction.x, 0);
    }

    #endregion

    #region An

    [SerializeField] private bool m_isAlive;
    [SerializeField] private bool m_isHurt;
    [SerializeField] private GameObject m_body;
    [SerializeField] private EmojiPositionController m_emojiPosController;
    Tweener tween;

    public EmojiPositionController EmojiPosController
    {
        get { return m_emojiPosController; }
        set { m_emojiPosController = value; }
    }
    public bool IsAlive
    {
        get { return m_isAlive; }
        set { m_isAlive = value; }
    }

    //If Hurt = true, it means can't kill this emoji in one turn, this emoji can't be knockout
    public bool IsHurt
    {
        get { return m_isHurt; }
        set { m_isHurt = value; }
    }

    public void MovingAnimation(bool isMyTeam, System.Action onMovingComplete = null)
    {
        var startPosX = -100f;
        if (!isMyTeam)
        {
            startPosX = startPosX * -1f;
        }

        transform.position = new Vector2(transform.position.x + startPosX, transform.position.y);

        transform.DOLocalJump(Vector3.zero, 5f, 5, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onMovingComplete?.Invoke();
        });
    }

    public void ShakeAnimation(bool isMyTeam)
    {
        tween.Kill(); 

        float rotateZ = 0;
        if (isMyTeam)
        {
            rotateZ = 15f;
        }
        else
        {
            rotateZ = -15f;
        }

        transform.DORotate(new Vector3(0, 0, rotateZ), 0.1f).OnComplete(() =>
        {
            tween = transform.DOShakePosition(100f, 1, 5).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }).SetUpdate(false);
    }

    public void FightAnimation(bool isMyTeam)
    {
        tween.Kill();

        float moveX = 0;
        float rotateZ = 15;

        if (isMyTeam)
        {
            moveX = 6;
            rotateZ = -15f;
        }
        else
        {
            moveX = -6;
            rotateZ = 15f;
        }

        transform.DOLocalMoveX(moveX, 0.1f).SetUpdate(false);
        transform.DORotate(new Vector3(0, 0, rotateZ), 0.1f).OnComplete(() =>
        {
            BattleController.instance.OnPlayFightParticle();
        }).SetUpdate(false);
    }

    [SerializeField] private Vector3 direction;
    public void FlyAway(bool isMyTeam)
    {
        tween.Kill();

        DOVirtual.DelayedCall(0.1f, () =>
        {
            float moveX = 0f;
            float moveY = 30;

            if (isMyTeam)
            {
                moveX = -80f;
            }
            else
            {
                moveX = 80f;
            }

            direction = new Vector3(moveX, moveY, 0);

            GenerateDeadSmokeEffect(direction);

            transform.DOLocalMove(direction, 0.2f).OnComplete(() =>
            {
                m_body.SetActive(false);
                BattleController.instance.OnPlayExplosionParticle(direction);
                DOVirtual.DelayedCall(1f, () => { Destroy(gameObject); }).SetUpdate(false);
            }).SetUpdate(false);
        });
    }

    public void BackToSeat(System.Action onComplete = null)
    {
        tween.Kill();

        if (m_isAlive)
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                transform.DOLocalMove(Vector3.zero, 0.1f).SetUpdate(false);
                transform.DORotate(Vector3.zero, 0.1f).SetUpdate(false);
                onComplete();
            }).SetUpdate(false);
        }
    }
    #endregion

    public void SetBattleHighlight(bool isActive)
    {
        battleHighlight.enabled = isActive;
    }
}
