using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TweenManager : Singleton<TweenManager>
{
    #region For Testing
    public Transform target;
    public Transform other;

    public Sequence readyToAttackSequence;
    public Ease ease;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //Shake(target);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //Change(target.GetComponent<MeshRenderer>());
        }
        if (Input.GetKey(KeyCode.W))
        {
            //Punch(target, other, Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Jump(target);
        }
    }
    #endregion

    public void Jump(Transform target, Vector3 direction, Action callback)
    {
        // Direction = Vector3.left / Vector3.right
        target.DOJump(
            endValue: new Vector3(120f * direction.x, 50, 0),
            jumpPower: 3,
            numJumps: 1,
            duration: 0.75f)
            .SetEase(Ease.OutExpo).SetUpdate(false)
            .OnComplete(() => callback());
    }

    public void ShakeAll(Transform target, Action callback = null)
    {
        const float duration = 0.5f;
        const float strength = 1f;

        Tween shakePosition = target.DOShakePosition(duration, strength).SetUpdate(false);
        Tween shakeRotation = target.DOShakeRotation(duration, strength).SetUpdate(false);
        Tween shakeScale = target.DOShakeScale(duration, strength).SetUpdate(false);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(shakePosition);
        sequence.Join(shakeRotation);
        sequence.Join(shakeScale);

        if (callback != null) sequence.OnComplete(() => callback());
    }

    public void Blink(MeshRenderer renderer, Action callback = null)
    {
        //Color colorDown = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.5f);
        //Color colorUp = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);

        Tween blinkDown = renderer.material.DOColor(Color.red, 0.2f);
        Tween blinkUp = renderer.material.DOColor(Color.white, 0.2f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(blinkDown);
        sequence.Append(blinkUp);
        sequence.SetLoops(2);

        if (callback != null) sequence.OnComplete(() => callback());
    }

    public void ShakeAndBlink(Transform target, Action callback = null)
    {
        const float duration = 0.5f;
        const float strength = 0.5f;

        Tween shakePosition = target.DOShakePosition(duration, strength).SetUpdate(false);
        Tween shakeRotation = target.DOShakeRotation(duration, strength).SetUpdate(false);
        Tween shakeScale = target.DOShakeScale(duration, strength).SetUpdate(false);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(shakePosition);
        sequence.Join(shakeRotation);
        sequence.Join(shakeScale);

        MeshRenderer renderer = target.GetChild(0).GetComponent<MeshRenderer>();

        //Color colorDown = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.5f);
        //Color colorUp = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);

        Tween blinkDown = renderer.material.DOColor(Color.red, 0.2f);
        Tween blinkUp = renderer.material.DOColor(Color.white, 0.2f);

        sequence.Join(blinkDown);
        sequence.Append(blinkUp);

        if (callback != null) sequence.OnComplete(() => callback());
    }

    public void ShakePosition(Transform target, Action callback = null)
    {
        const float duration = 0.1f;
        const float strength = 1f;

        Tween shakePosition = target.DOShakePosition(duration, strength).SetUpdate(false);

        if (callback != null) shakePosition.OnComplete(() => callback());
    }

    public void Punch(Transform target, Transform other, Vector3 direction)
    {
        // Direction [Vector3.left, Vector3.right]
        const float duration = 0.5f;
        const float strength = 0.5f;

        target.DOPunchPosition(
            punch: direction * 2,
            duration: duration,
            vibrato: 0,
            elasticity: 0).SetUpdate(false);

        other.DOShakePosition(
            duration: duration,
            strength: strength,
            vibrato: 10).SetDelay(duration * 0.5f).SetUpdate(false);
    }

    public void Rotate30(Transform target, Vector3 direction, Action callback = null)
    {
        Vector3 defaultRotation = target.rotation.eulerAngles;
        Vector3 rotation = new Vector3(defaultRotation.x, defaultRotation.y, 30 * direction.x);

        //Debug.Log("Target Rotation = " + rotation);

        Tween rotate = target.DORotate(rotation, 0.3f).SetUpdate(false);

        if (callback != null) rotate.OnComplete(() => callback());
    }

    public void PunchAndBackToNormal(Transform target, Vector3 direction)
    {
        readyToAttackSequence.Kill();

        // Direction [Vector3.left, Vector3.right]
        const float duration = 0.5f;

        Vector3 defaultRotation = target.rotation.eulerAngles;

        Tween punch = target.DOPunchPosition(
                        punch: direction * 2,
                        duration: duration,
                        vibrato: 0,
                        elasticity: 0).SetUpdate(false);

        Vector3 normalRotation = new Vector3(defaultRotation.x, defaultRotation.y, 0);
        Tween backToNormalRotation = target.DORotate(normalRotation, 0.5f).SetUpdate(false);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(punch);
        sequence.Append(backToNormalRotation);
    }

    // Move, Punch, Damage - ShakeAndBlink, MoveBack
    public void MoveAndPunch(Transform target, Vector3 direction, Action callback = null)
    {
        Vector3 defaultPos = target.transform.position;
        //Debug.Log("defaultPos : " + defaultPos);

        //Direction[Vector3.left, Vector3.right]
        Vector3 defaultRotation = target.rotation.eulerAngles;
        Vector3 position = target.position + direction * 10;

        Tween move = target.DOMove(position, 0.1f).SetEase(Ease.InElastic).SetUpdate(false);

        //Tween bounce2Back = target.DOMove(position - direction * 2, 0.4f);
        //Tween bounce2Forth = target.DOMove(position + direction * 2, 0.4f);
        //Tween bounce1Back = target.DOMove(position - direction * 1, 0.2f);
        //Tween bounce1Forth = target.DOMove(position + direction * 1, 0.2f);

        // Move and bounce
        Sequence sequence = DOTween.Sequence();
        sequence.Join(move);
        //sequence.Append(bounce2Back);
        //sequence.Append(bounce2Forth);
        //sequence.Append(bounce1Back);
        //sequence.Append(bounce1Forth);

        sequence.OnComplete(() =>
        {
            const float duration = 0.2f;
            const float strength = 1f;

            Tween shakePosition = target.DOShakePosition(duration, strength).SetUpdate(false);
            Tween shakeRotation = target.DOShakeRotation(duration, strength).SetUpdate(false);
            Tween shakeScale = target.DOShakeScale(duration, strength).SetUpdate(false);

            Sequence shakeSequence = DOTween.Sequence();
            // shakeSequence.Join(shakePosition);
            // shakeSequence.Join(shakeRotation);
            // shakeSequence.Join(shakeScale);
            shakeSequence.OnComplete(() => MoveBack(target, direction, callback));

            //ShakeAll(transform, callback);

            // MeshRenderer renderer = target.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            // for (int i = 0; i < target.GetChild(0).GetChild(0).childCount; i++)
            // {
            //     MeshRenderer renderer = target.GetChild(0).GetChild(0).GetChild(i).GetComponent<MeshRenderer>();
            //     Blink(renderer);
            // }

        });
    }
    public void MoveBack(Transform target, Vector3 direction, Action callback = null)
    {
        Vector3 defaultRotation = target.rotation.eulerAngles;
        Vector3 normalRotation = new Vector3(defaultRotation.x, defaultRotation.y, 0);
        Vector3 normalPosition = target.position - direction * 10;

        Tween backToNormalRotation = target.DORotate(normalRotation, 0.5f).SetUpdate(false);
        Tween backToNormalPosition = target.DOMove(normalPosition, 0.5f).SetUpdate(false);

        Sequence sequence = DOTween.Sequence();
        //sequence.Append(backToNormalRotation);
        sequence.Append(backToNormalPosition);

        if (callback != null) sequence.OnComplete(() => callback());
    }

    public void Change(MeshRenderer targetRenderer)
    {
        const float duration = 0.3f;
        targetRenderer.material.DOColor(UnityEngine.Random.ColorHSV(), duration);
    }
    public void SlideUpAndDown(RectTransform target, float value, float timer)
    {
        target.DOAnchorPosY(value, timer)
       .SetEase(Ease.OutCubic);
    }
}
