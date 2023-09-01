using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCursor : Aiming
{
    private void Update()
    {
        if (active)
        {
            Vector3 mouseDirection = CameraManager.instance.GetMouseDirection(transform.position);
            AimWeapon(mouseDirection);
        }
    }
}
