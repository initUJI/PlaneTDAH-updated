using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroBlowingManager : MonoBehaviour
{
    AudioClip microphoneInput;
    Blowing_GameManager gameManager;
    bool microphoneInitialized;
    public float sensitivity = 0;

    public float rmsValue = 0, DbValue = 0;
    public float RefValue = 0.1f;

    public Slider sensivitySlider;
    public Text debugText;
    private float increasingCoeficient  = 20;

    public GameSettings game_data;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Blowing_GameManager>();
        sensitivity = game_data.sensitivity;

       /* sensivitySlider.onValueChanged.AddListener(delegate {
            sensivityValueChangedHandler(sensivitySlider);
        });*/

        if (Microphone.devices.Length > 0)
        {
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);

            microphoneInitialized = true;
        }
        else
        {
            print("There is no mic availiable");
        }
    }

    private void sensivityValueChangedHandler(Slider sensivitySlider)
    {
        sensitivity = sensivitySlider.value;
    }

    private void Update()
    {
        if (microphoneInitialized == true)
        {
            // get mic volume
            int dec = 512;
            float[] waveData = new float[dec];
            int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
            microphoneInput.GetData(waveData, micPosition);


            //Getting a peak on the last 128 samples
            float wavePeak = 0;
            float levelMax = 0;
            for (int i = 0; i < dec; i++)
            {
                wavePeak += waveData[i] * waveData[i];
            }

            rmsValue = Mathf.Sqrt(wavePeak / dec);
            DbValue = 20 * Mathf.Log10(rmsValue / RefValue);

            float minimumValue = -45 + sensitivity;

            if (DbValue < minimumValue) DbValue = minimumValue;

            float powerToGive = (DbValue - minimumValue) * Time.deltaTime * increasingCoeficient;

            if(powerToGive >= 1)
            {
                gameManager.microphoneInputToFanMovement(powerToGive);
            }else
            {
                gameManager.microphoneInputToFanMovement();
            }
        }

    }

    
}

