using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public int puntosTotales = 0;
    public int arosFallados = 0;
    public int anillasEnTotalJugadas = 0;
    public float timePlayed;
    public Text textoPuntuacion;
    public Text textoTiempo;
    public GameObject gameplayUI;
    public GameObject finishingPanel;
    public string mode;

    private PartidaJugada partidaActual;
    public bool hasStarted = false;

    //Variables que definen dificultad de la partida
    public int totalRingsToCatch , matchDurationTime;

    // Start is called before the first frame update
    void Start()
    {
        CameraObjectFollower skyboxManager = GameObject.Find("Main Camera").GetComponent<CameraObjectFollower>();

        if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
        {
            //El jugador no se encuentra en una session
            matchDurationTime = 60;
            totalRingsToCatch = 200;
            mode = "Easy";
            skyboxManager.actualizarEscenario("Easy");
        }
        else
        {
            //El jugador se encuentra en una sesion, poner el juego acorde a ello
            MinijuegoLevel thisLevel = SessionManager.instance.sucesionDeJuegos[0];
            mode = thisLevel.difficulty;
            partidaActual = new PartidaJugada(thisLevel.difficulty);
            DataManager.instancia.añadirPartidaGuardada(partidaActual);
            DataManager.instancia.Guardar();

            this.gameObject.GetComponent<UI_InGame_Manager>().setPauseScreenModeToPractise();

            switch (mode)
            {
                case "Easy":
                    matchDurationTime = 60;
                    totalRingsToCatch = 200;
                    skyboxManager.actualizarEscenario(mode);
                    break;

                case "Medium":
                    matchDurationTime = 60;
                    totalRingsToCatch = 200;
                    skyboxManager.actualizarEscenario(mode);
                    break;

                case "Hard":
                    matchDurationTime = 60;
                    totalRingsToCatch = 200;
                    skyboxManager.actualizarEscenario(mode);
                    break;
            }
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if(hasStarted)
        {
            ActualizarTiempoPartida();
            ComprobarFinalPartida();
        }
    }

    private void ComprobarFinalPartida()
    {
        if((int)timePlayed >= matchDurationTime || anillasEnTotalJugadas >= totalRingsToCatch)
        {
            //End of the game
            hasStarted = false;
            //Show finishing panel
            
            finishingPanel.SetActive(true);
            GetComponent<UI_InGame_Manager>().setPauseButtonState(false);

            LeanTween.scale(finishingPanel.gameObject, new Vector3(1, 1, 1), 1);
           
            //Make buttons functional
            //finishingPanel.transform.Find("Panel/Buttons/MenuButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());

            if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
            {
                finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Práctica terminada!";

                //Editando texto de resultado
                finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";
                finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = "¡Excelente!";

                //Editando texto de resultado adicional
                finishingPanel.transform.Find("Panel/Information/AdditionalText").gameObject.GetComponent<Text>().text = "Aros recogidos: " + (puntosTotales / 10);
                finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";

                //Editando texto de puntos
                finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "";


                //Editando las funciones de los botones
                finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());
                finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Menu";
            }
            else
            {
                //Se suman puntos y tiempo al global (REVISAR)
                SessionManager.instance.SumarTiempoAlTotal((int)timePlayed);
                partidaActual.aciertos = puntosTotales / 10;
                partidaActual.fallos = arosFallados;
                partidaActual.juegoTerminado = true;
                DataManager.instancia.sustituirPartidaGuardada(partidaActual);
                DataManager.instancia.Guardar();

                string resultadoPrueba = "";
                if(puntosTotales >= 360)
                {
                    resultadoPrueba = "¡Los estás haciendo excelente!";
                    SessionManager.instance.SumarPuntosAlTotal(100);

                }
                else if(puntosTotales >= 200)
                {
                    resultadoPrueba = "¡Vas mejorando!";
                    SessionManager.instance.SumarPuntosAlTotal(50);

                }
                else if (puntosTotales >= 1)
                {
                    resultadoPrueba = "¡Ánimo! ¡Sigue así!";
                    SessionManager.instance.SumarPuntosAlTotal(25);
                }
                else
                {
                    resultadoPrueba = "¡Vuelve a intentarlo! ¡Tu puedes!";
                }

                //Se prepara la Interfaz
                SessionManager.instance.sucesionDeJuegos.RemoveAt(0);

                //Editando titulo de la pantalla final
                int completed = SessionManager.instance.totalGamesInSession - SessionManager.instance.sucesionDeJuegos.Count;
                finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Terminado! " + completed + " / " + SessionManager.instance.totalGamesInSession;

                //Editando texto de resultado
                finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";
                finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = resultadoPrueba;

                //Editando texto de resultado adicional
                finishingPanel.transform.Find("Panel/Information/AdditionalText").gameObject.GetComponent<Text>().text = "Aros recogidos: " + (puntosTotales / 10);
                finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";

                //Editando texto de puntos
                finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "";

                //Editando las funciones de los botones y su texto correspondiente
                finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => SessionManager.instance.chargeNextScene());
                finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Siguiente";
                
            }

        }
    }

    void replayMatch()
    {
        SceneManager.LoadScene("Avion");
    }

    private void ActualizarTiempoPartida()
    {
        timePlayed += Time.deltaTime;
        float timeToShow = matchDurationTime - timePlayed;
        if (timeToShow <= 0)
        {
            timeToShow = 0;
        }
        textoTiempo.text = "Tiempo: " + Mathf.Floor(timeToShow).ToString("00");
    }

    IEnumerator StartCountdown()
    {
        Transform countdownObject = GameObject.Find("Canvas/Countdown").transform;

        foreach(Transform child in countdownObject)

        {
            LeanTween.scale(child.gameObject, new Vector3(1, 1, 1), 0.75f);
            yield return new WaitForSeconds(0.75f);
            LeanTween.scale(child.gameObject, new Vector3(0, 0, 0), 0.25f);
            yield return new WaitForSeconds(0.25f);
        }
        hasStarted = true;
        gameplayUI.SetActive(true);

        yield return null;
    }

    public void SumarPuntos(int puntosASumar)
    {
        if (hasStarted)
        {
            puntosTotales += puntosASumar;
            anillasEnTotalJugadas++;


            double percent = ((double)puntosTotales / (double)anillasEnTotalJugadas) * 10;
            percent = Math.Truncate(percent);
            textoPuntuacion.text = "Aros Recogidos: " + percent + " %";
        }
    }
}
