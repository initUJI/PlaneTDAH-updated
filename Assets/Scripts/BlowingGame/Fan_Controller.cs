using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_Controller : MonoBehaviour
{
    private Blowing_GameManager gameManager;
    public Transform inner_fan, outter_fan;
    public SaturationIndicator_UI saturationIndicator;

    public float partialLimitUp, partialLimitDown;
    public float fan_actual_speed, fan_max_speed, fan_min_speed, decreaseCoeficient;

    public bool blowing, saturated;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Blowing_GameManager>();

        inner_fan = transform.GetChild(0);
        outter_fan = transform.GetChild(1);

        partialLimitDown = 20;
        partialLimitUp = 90;
    }


    void Update()
    {

        //Updating fan velocity
        UpdateFanVelocity();

        UpdateUI_Percentage_Slider();

        //Moving inner fan
        inner_fan.Rotate(0, 0, fan_actual_speed * Time.deltaTime);
    }

    private void UpdateUI_Percentage_Slider()
    {
        gameManager.UpdatePowerPercentageSlider(fan_min_speed, fan_max_speed, fan_actual_speed);
    }

    private void UpdateFanVelocity()
    {
        if(fan_actual_speed > fan_min_speed)
        {
            fan_actual_speed -= decreaseCoeficient * Time.deltaTime;

            float fanNormalizedSpeed = normalizedFanSpeed();
            if (fanNormalizedSpeed <= partialLimitDown)
            {
                saturated = false;
                saturationIndicator.changeStateForIndicator(false);
            }
        }
        else
        {
            fan_actual_speed = fan_min_speed;
        }
    }

    public void increaseFanVelocity(float power = 0)
    {
        if(power != 0)
        {
            blowing = true;
            if (!saturated)
            {
                if (fan_actual_speed < fan_max_speed)
                    fan_actual_speed += power;
                else
                {
                    fan_actual_speed = fan_max_speed;
                    saturated = true;
                    bool countRound = saturationIndicator.changeStateForIndicator(true);
                    if (countRound)
                    {
                        gameManager.timesToGo++;
                        gameManager.updateObjectiveText();
                    }
                }
            }
        }else
        {
            //Esto funciona ya que el slider va desde el valor 0 a 100
            float fanNormalizedSpeed = normalizedFanSpeed();

            if (fanNormalizedSpeed >= partialLimitUp)
            {
                saturated = true;
                blowing = false;
                bool countRound = saturationIndicator.changeStateForIndicator(true);
                if (countRound)
                {
                    gameManager.timesToGo++;
                    gameManager.updateObjectiveText();
                }
            }
        }
    }

    public float normalizedFanSpeed()
    {
        float fanNormalizedSpeed = fan_actual_speed / fan_max_speed * 100;
        return fanNormalizedSpeed;
    }
}
