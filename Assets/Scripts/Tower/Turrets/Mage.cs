using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : TurretManager
{

    public GameObject turretAnchor;
    public Vector3 rotationSpeed = new Vector3(0, 10, 0);

    public float bobbingSpeed = 1f;
    public float bobbingAmplitude = 0.1f;

    private void AnimateTurret(){
        transform.Rotate(rotationSpeed * Time.deltaTime);
        float bobY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude;
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + bobY, originalPosition.z); 
    }
    protected override Vector3 RotateTurret(){
        AnimateTurret();
        
        if (turretAnchor == null)
        {
            return Vector3.zero;
        }

        if (currentTarget == null)
        {
            ResetXZRotation();
            return Vector3.zero;
        }

        Vector3 direction = currentTarget.transform.position - turretAnchor.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turretAnchor.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        float clampedX = Mathf.Clamp(rotation.x, -90f, 90f);
        float clampedZ = Mathf.Clamp(rotation.z, -90f, 90f);

        turretAnchor.transform.rotation = Quaternion.Euler(clampedX, rotation.y, clampedZ);
        return direction;
    }

}
