using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Microphone_Settings_Panel : MonoBehaviour
{
    AudioClip microphoneInput;
    bool microphoneInitialized;
    public float sensitivity = 0, powerOfMic = 0;

    public float rmsValue = 0, DbValue = 0;
    public float RefValue = 0.1f;

    public Slider sensitivitySlider, powerSlider, resultSlider;
    public Text dbText;

    public GameSettings game_data;


    // Start is called before the first frame update
    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);

            microphoneInitialized = true;
        }
        else
        {
            print("There is no mic availiable");
        }

        sensitivitySlider.onValueChanged.AddListener(delegate {
            sensivityValueChangedHandler(sensitivitySlider);
        });

        powerSlider.onValueChanged.AddListener(delegate
        {
            powerValueChangedHandler(powerSlider);
        });

        if(game_data.sensitivity == 0 && game_data.micPower == 0)
        {
            resultSlider.maxValue = resultSlider.minValue + 10;
            sensitivity = game_data.sensitivity;
            
            powerOfMic = 10;
            game_data.micPower = 10;
            powerSlider.value = powerOfMic;
        }
        else
        {
            sensitivity = game_data.sensitivity;
            powerOfMic = game_data.micPower;

            sensitivitySlider.value = sensitivity;
            powerSlider.value = powerOfMic;
        }
    }

    // Update is called once per frame
    void Update()
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
            dbText.text = "Actual db: " + DbValue.ToString("00");

            if(DbValue >= resultSlider.minValue && DbValue <= resultSlider.maxValue)
            {
                resultSlider.value = DbValue;
            }
        }
    }

    private void powerValueChangedHandler(Slider powerSlider)
    {
        //Cambiar valor de la variable
        game_data.micPower = (int) powerSlider.value;
        powerOfMic = powerSlider.value;

        //Ajustar la barra de resultados
        resultSlider.maxValue = resultSlider.minValue + powerOfMic + 10;
    }

    private void sensivityValueChangedHandler(Slider sensitivitySlider)
    {
        //Cambiar el valor de la variable
        game_data.sensitivity = (int) sensitivitySlider.value;
        sensitivity = sensitivitySlider.value;

        //Ajustar la barra de resultado
        resultSlider.minValue = sensitivity - 45;
        resultSlider.maxValue = resultSlider.minValue + powerOfMic + 10;
    }

    
}
