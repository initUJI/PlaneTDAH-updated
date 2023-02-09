using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotationAnimation : MonoBehaviour
{
    float rotationSpeedDetection, rotationSpeedShooting = 0,
        mediumSpeed = 3, fastSpeed = 15;

    public int state = 0; // 0 --> No rotation, 1 --> SurfaceDetected, 2 --> SurfaceShooting

    private void Start()
    {
        rotationSpeedShooting = 0;
        rotationSpeedDetection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedRegulation();
        ApplyRotation();
    }

    void SpeedRegulation()
    {
        rotationSpeedShooting = Mathf.Lerp(rotationSpeedShooting, rotationSpeedDetection, Time.deltaTime * 5);
    }

    void ApplyRotation()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + rotationSpeedShooting);
    }

    public void ChangeState(int newState)
    {
        state = newState;
        
        switch (state)
        {
            case 0:
                rotationSpeedDetection = 0;
                break;
            case 1:
                rotationSpeedDetection = mediumSpeed;
                break;
            case 2:
                rotationSpeedDetection = fastSpeed;
                break;
        }
    }
}
