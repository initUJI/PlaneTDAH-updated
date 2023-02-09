using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    private GameObject GyroControl;
    private Quaternion rot;
    private Quaternion adjustrot;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        GyroControl = new GameObject("Gyro Control");
        GyroControl.transform.position = transform.position;
        transform.SetParent(GyroControl.transform);
        gyroEnabled = EnableGyro();
    }

    // Update is called once per frame
    void Update()
    {
        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            //GyroControl.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
            GyroControl.transform.LookAt(target);
            rot = new Quaternion(0, 0, 1, 0);
            return true;
        }
        return false;
    }
}
