using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FluidGameManager : MonoBehaviour
{
    //public Slider percentageSlider;
    public Text percentageText, failedDropsText, timeText;
    public float particleCounter, particlesToReach;
    int failedDrops, totalDrops;

    public string difficulty;
    public GameObject poatToFill;
    public bool movingPoat;

    public bool hasStarted = false;
    public GameObject gameplay_UI;

    public float gameplayTime, totalTimeToGo;
    public GameObject finishingPanel;

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "00";
        actualizarValorSlider(0);
        actualizarFailedDrops(0);
        DefineGameMode();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        Transform countdownObject = GameObject.Find("Canvas/Countdown").transform;

        foreach (Transform child in countdownObject)
        {
            LeanTween.scale(child.gameObject, new Vector3(1, 1, 1), 0.75f);
            yield return new WaitForSeconds(0.75f);
            LeanTween.scale(child.gameObject, new Vector3(0, 0, 0), 0.25f);
            yield return new WaitForSeconds(0.25f);
        }
        hasStarted = true;
        gameplay_UI.SetActive(true);
        gameplayTime = 0;

        yield return null;
    }

    private void DefineGameMode()
    {
        if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
        {
            //No se encuentra en una sesion
            poatToFill.transform.localScale = new Vector3(19.85877f, 21.01058f, 24.73227f);
            totalTimeToGo = 60;
            particlesToReach = 350;
        }
        else
        {
            //Si que se encuentra en una sesion
            difficulty = SessionManager.instance.sucesionDeJuegos[0].difficulty;
            switch (difficulty)
            {
                case "Easy":
                    {
                        totalTimeToGo = 60;
                        particlesToReach = 250;
                        break;
                    }

                case "Medium":
                    {
                        totalTimeToGo = 60;
                        particlesToReach = 250;
                        break;
                    }
                case "Hard":
                    {
                        particlesToReach = 60;
                        totalTimeToGo = 250;
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        if(hasStarted)
        {
            actualizarTiempo();
            comprobarFinalDeJuego();
        }
    }

    private void comprobarFinalDeJuego()
    {
        if(particleCounter/particlesToReach >= 1 || gameplayTime >= totalTimeToGo)
        {
            //Finish the game
            hasStarted = false;

            finishingPanel.SetActive(true);
            GetComponent<UI_InGame_Manager>().setPauseButtonState(false);


            LeanTween.scale(finishingPanel, new Vector3(1, 1, 1), 1);

            //Fill the panel with the info
            float valueToShow = (particleCounter / particlesToReach) * 100;

            //Make buttons functional
            finishingPanel.transform.Find("Panel/Buttons/MenuButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());
            
            if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
            {
                finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Práctica terminada!";

                //Editando texto de resultado
                finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";
                finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = "";

                //Editando texto de resultado adicional
                finishingPanel.transform.Find("Panel/Information/AdditionalText").gameObject.GetComponent<Text>().text = "Gotas acertadas: " + particleCounter;

                double percent = (particleCounter / (particleCounter + failedDrops)) * 100;
                finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "Precisión: "
                    + Math.Round(percent) + " %";


                finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Menu";
                finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());
            }
            else
            {
                SessionManager.instance.SumarTiempoAlTotal((int)gameplayTime);
                
                string resultadoPrueba = "";
                if (particleCounter >= 190)
                {
                    resultadoPrueba = "¡Excelente!";
                    SessionManager.instance.SumarPuntosAlTotal(100);
                }
                else if (particleCounter >= 150)
                {
                    resultadoPrueba = "Bien";
                    SessionManager.instance.SumarPuntosAlTotal(50);
                }
                else
                {
                    resultadoPrueba = "¡Practica para mejorar!";
                    SessionManager.instance.SumarPuntosAlTotal(25);
                }

                SessionManager.instance.sucesionDeJuegos.RemoveAt(0);
                //Editando titulo de la pantalla final
                int completed = SessionManager.instance.totalGamesInSession - SessionManager.instance.sucesionDeJuegos.Count;
                finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Terminado! " + completed + " / " + SessionManager.instance.totalGamesInSession;

                //Editando texto de resultado
                /*finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "Resultado: ";
                finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = resultadoPrueba;*/

                //Editando texto de resultado adicional
                finishingPanel.transform.Find("Panel/Information/AdditionalText").gameObject.GetComponent<Text>().text = "Gotas acertadas: " + particleCounter;

                //Editando texto de puntos
                /*finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "Gotas falladas: "
                    + SessionManager.instance.puntosTotales + "/" + SessionManager.instance.puntosAConseguir;*/
                double percent = (particleCounter / (particleCounter + failedDrops)) * 100;
                finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "Precisión: "
                    + Math.Round(percent) + " %";

                finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => SessionManager.instance.chargeNextScene());
                finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Siguiente";
                
            }
        }
    }

    private void actualizarTiempo()
    {
        gameplayTime += Time.deltaTime;
        float timeToShow = totalTimeToGo - gameplayTime;
        if (timeToShow <= 0)
        {
            timeToShow = 0;
        }
        timeText.text = Mathf.Floor(timeToShow).ToString("00");
    }

    public void actualizarValorSlider(int newCount)
    {
        particleCounter = newCount;

        float valueToShow = particleCounter / 100;
        
        //percentageSlider.value = valueToShow;
        percentageText.text = valueToShow.ToString("F2") + " Litros llenos";
    }

    public void actualizarFailedDrops(int newCount)
    {
        failedDrops = newCount;
        failedDropsText.text = newCount.ToString();
    }

    void playAgain()
    {
        SceneManager.LoadScene("Fluid");
    }
}
