using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    FluidGameManager gameManager;
    public float cameraRotation;
    public float faucetRotation = 3;

    public GameObject playerCamera;

    public Image rightArrow, leftArrow;
    public float rightLimitToCorrect, leftLimitToCorrect;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<FluidGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.hasStarted)
        {
            updateCameraState();
            //comprobacionInterfaz(); NOT WORKING
        }
    }

    void updateCameraState()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Gyro Control");
        }
        else
        {
            cameraRotation = playerCamera.transform.localEulerAngles.y;
        }
    }

    void comprobacionInterfaz()
    {
        if(cameraRotation >= rightLimitToCorrect && cameraRotation < 180)
        {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(true);
        }
        else if (cameraRotation <= leftLimitToCorrect && cameraRotation >= 180)
        {
            rightArrow.gameObject.SetActive(true);
            leftArrow.gameObject.SetActive(false);
        }
        else
        {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(false);
        }
    }
}
