using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UI_Manager_PipeGame : MonoBehaviour
{
    public Text dirtyPointsText, timeText, errorsText;
    public Slider heatingSlider;

    public bool userAccuarcyError;
    public int playerErrors;

    public float totalDirtyPoints, actualDirtyPoints;
    public int destroyedPoints = 0;
    public float percentageDirtyErased;

    public float maxHeating = 200, actualHeating, deacreasingHeatFactor;

    public void increasingHeatOfPipe()
    {
        if(!userAccuarcyError)
        {
            userAccuarcyError = true;
            playerErrors++;
            SoundEffects_Manager.instancia.PlaySound("playerFail");
            errorsText.text = "Errores: " + playerErrors;
        }
    }

    void regulationOfHeat()
    {
        if (actualHeating > 0)
        {
            actualHeating -= deacreasingHeatFactor * Time.deltaTime;
        }
        else if (actualHeating > maxHeating)
        {
            //Game Over
        } else
        {
            actualHeating = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        regulationOfHeat();
        refreshingHeatSliderState();
    }

    public void setTotalDirtyPoints(int totalPoints)
    {
        totalDirtyPoints = totalPoints;
        /*actualDirtyPoints = totalDirtyPoints;
        percentageDirtyErased = actualDirtyPoints / totalDirtyPoints * 100f;*/

        dirtyPointsText.text = "Suciedades eliminadas: " + destroyedPoints;
    }

    public void restDirtyPoints(int pointsToRest)
    {
        destroyedPoints += pointsToRest;
        actualDirtyPoints -= pointsToRest;
        //percentageDirtyErased = actualDirtyPoints / totalDirtyPoints * 100f;

        dirtyPointsText.text = "Suciedades eliminadas: " + destroyedPoints;
    }

    private void refreshingHeatSliderState()
    {
        float percentagePipeHeat = actualHeating / maxHeating;
        heatingSlider.value = percentagePipeHeat;
    }

    public void ActualizarTiempo(float actualTime)
    {
        timeText.text = "Tiempo: " + Mathf.Floor(actualTime).ToString("00");
    }
}
