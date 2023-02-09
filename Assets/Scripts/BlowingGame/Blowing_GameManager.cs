using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blowing_GameManager : MonoBehaviour
{
    private GameObject parent_fan;
    private Slider powerPercentage_Slider;
    private Text objectiveText;

    public int timesToGo, timesGoal;
   
    // Start is called before the first frame update
    void Start()
    {
        timesToGo = 0;

        //Obtaining the fan_object to reference their children
        parent_fan = GameObject.Find("/Plane/Fan_Object");
        powerPercentage_Slider = GameObject.Find("/Canvas/PercentageSlider").GetComponent<Slider>();

        objectiveText = GameObject.Find("/Canvas/objectiveText").GetComponent<Text>();
        objectiveText.text = "Times to go: " + timesToGo + "/" + timesGoal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePowerPercentageSlider(float min, float max, float actualPower)
    {
        float diff = max - min;
        float actualDataNormalized = actualPower - min;

        powerPercentage_Slider.value = actualDataNormalized / diff * 100;
    }

    public void microphoneInputToFanMovement(float power = 0)
    {
        if(power != 0)
            parent_fan.GetComponent<Fan_Controller>().increaseFanVelocity(power);
        else
            parent_fan.GetComponent<Fan_Controller>().increaseFanVelocity();

    }

    public void updateObjectiveText()
    {
        objectiveText.text = "Times to go: " + timesToGo + "/" + timesGoal;
    }
}
