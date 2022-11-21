using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileController : MonoBehaviour
{
    WeaponData weaponData;
    Transform target;
    DateTime startTime;
    DateTime endTime;

    public void Setup(Transform source, Transform target, WeaponData weaponData)
    {
        this.target = target;
        this.weaponData = weaponData;
        startTime = DateTime.Now;
        transform.position = source.position;

        var dist = (target.position - transform.position).magnitude;
        var flightTime = dist / weaponData.flightSpeed;
        endTime = startTime.AddSeconds(flightTime);
    }

    void Update()
    {
        if (target == null)
        {
            Kill();
            return;
        }

        var now = DateTime.Now;
        var remainingSeconds = (float)(endTime - now).TotalSeconds;
        if (remainingSeconds <= 0)
        {
            Kill();
            return;
        }

        transform.LookAt(target);
        var dir = target.position - transform.position;
        transform.position += dir * Time.deltaTime / remainingSeconds;
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
