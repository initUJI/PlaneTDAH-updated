using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaturationIndicator_UI : MonoBehaviour
{
    public Image background, indicator;
    public float changingSpeed;
    public bool activated = false;
    float multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            
            if(indicator.color.a <= 0)
            {
                multiplier = 1;
            }else if(indicator.color.a >= 0.7f)
            {
                multiplier = -1;
            }

            Color newColor = indicator.color;
            newColor.a += changingSpeed * Time.deltaTime * multiplier;
            indicator.color = newColor;
        }
        
    }

    public bool changeStateForIndicator (bool newState)
    {
        bool result = false;

        if(activated == false && newState == true)
        {
            result = true;
        }
        activated = newState;
        
        if(activated == false)
        {
            Color newColor = indicator.color;
            newColor.a = 0;
            indicator.color = newColor;
        }

        return result;
    }
}
