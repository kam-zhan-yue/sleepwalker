using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCursor : Aiming
{
    protected override void Update()
    {
        Vector3 mouseDirection = CameraManager.instance.GetMouseDirection(transform.position);
        AimWeapon(mouseDirection);
    }
}
