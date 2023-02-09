using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeRotation : MonoBehaviour
{
    #region Instance
    private static GyroscopeRotation instance;
    public static GyroscopeRotation Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GyroscopeRotation>();
                if(instance ==null)
                {
                    instance = new GameObject("Spawned GyroManager", typeof(GyroscopeRotation)).GetComponent<GyroscopeRotation>();
                }
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }
    #endregion

    [Header("Logic")]
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro()
    {
        //Already activated
        if (gyroActive)
            return;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
        }else
        {
            Debug.Log("Gyroscope is not supported on your mobile device");
        }

    }

    private void Update()
    {
        if(gyroActive)
        {
            rotation = gyro.attitude;
            Debug.Log(rotation);
        }
    }

    public Quaternion GetGyroRotation()
    {
        return rotation;
    }
}
