using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Painting_GameManager : MonoBehaviour
{
    public Transform cameraPointer;
    public Transform sprayModel;

    public GameObject Gameplay_UI;
    public GameObject placeObjectPanel;
    public string difficultMode;

    public bool painting, nearOfTheWall;
    public bool playing;
    public int distance;
    public float gameplayTime, totalTimeToGo;
    public float totalPointsToDo, totalPointsDone;

    private float lerpDuration = 1;
    private float startTime, speed, journeyLength;
    private Color colorToPaint;
    public P3dPaintSphere sprayManager;
    public AudioSource sprayAudio;
    public Material plasticSprayMaterial;

    public GameObject finishingPanel;
    public Text timeText;
    public Text percentageText;

    public AudioClip errorSound;
    private int paintErrors = 0;
    public int figuresDone;

    [SerializeField]
    private Image _graffiti;

    public Color userColor, objectiveColor;

    void Awake()
    {
        cameraPointer.localPosition = new Vector3(0, 0, 0);
        sprayModel.localPosition = new Vector3(0, -1, sprayModel.localPosition.z);
        sprayModel.gameObject.SetActive(false);
        totalPointsDone = 0;
        totalPointsToDo = 0;

        Gameplay_UI.SetActive(false);
        placeObjectPanel.SetActive(true);

        journeyLength = Vector3.Distance(sprayModel.localPosition, new Vector3(0, -0.24f, 2.07f));
        speed = 2;


        colorToPaint = new Color(0, 0, 0, 0);
        sprayManager.Color = colorToPaint;
        IniciarPartida();
    }

    private void IniciarPartida()
    {
        if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
        {
            //El jugador no se encuentra en una session
            difficultMode = "Easy";
            totalTimeToGo = 120;
        }
        else
        {
            //El jugador se encuentra en una sesion
            MinijuegoLevel thisLevel = SessionManager.instance.sucesionDeJuegos[0];
            difficultMode = thisLevel.difficulty;

            /*switch (difficultMode)
            {
                case "Easy":
                    totalTimeToGo = 60;
                    break;

                case "Medium":
                    totalTimeToGo = 60;
                    break;

                case "Hard":
                    totalTimeToGo = 60;
                    break;
            }*/
            totalTimeToGo = 120;
        }
    }

    void Update()
    {
        if (playing)
        {
            EjecutarRaycast();
            AnimarSpray();

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButton(0))
            {
                //Si el puntero está encima de un boton no llega a pintar
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                else
                {
                    StartPainting();
                }
#endif

                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }
                else
                {
                    StartPainting();
                }
            }
            else if (painting && Input.touchCount == 0)
            {
                FinishPainting();
            }

            ActualizarTiempoGameplay();
            ComprobarFinalDeJuego();
        }
    }

    public void f_finishgGame()
    {
        gameplayTime = 120;
    }

    private void ComprobarFinalDeJuego()
    {
        if (gameplayTime >= totalTimeToGo || (totalPointsDone / totalPointsToDo) >= 1)
        {
            playing = false;
            FinishPainting();

            GetComponent<UI_InGame_Manager>().setPauseButtonState(false);
            GetComponent<UI_InGame_Manager>().setSaveButtonState(false);
            finishingPanel.SetActive(true);

            LeanTween.scale(finishingPanel, new Vector3(1, 1, 1), 1);

            //Fill the panel with information

            int percentage = (int)((totalPointsDone / totalPointsToDo) * 100);


            //Make the buttons functional
            //finishingPanel.transform.Find("Panel/Buttons/MenuButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());

            if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
            {
                finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Extra terminado!";

                //Editando texto de resultado
                //int paintedWall = (int)((totalPointsDone / totalPointsToDo) * 100);
                //finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "";
                //finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = "";

                //Editando texto de resultado adicional
                finishingPanel.transform.Find("Panel/ResultSprite").gameObject.GetComponent<Image>().sprite = finishingPanel.transform.parent.GetComponent<Painting_Interface_Manager>().copyTextureToImage();

                //Editando texto de puntos
                //finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "Errores cometidos: " + paintErrors;


                //Editando las funciones de los botones
                finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<UI_InGame_Manager>().exitButtonPressed());
                finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Menu";
            }
            else
            {
                int totalDone = (int)(totalPointsDone / totalPointsToDo) * 100;

                SessionManager.instance.SumarTiempoAlTotal((int)gameplayTime);

                string resultadoPrueba = "";
                if (totalDone >= 75)
                {
                    resultadoPrueba = "¡Excelente!";
                    SessionManager.instance.SumarPuntosAlTotal(100);

                }
                else if (totalDone >= 40)
                {
                    resultadoPrueba = "Bien";
                    SessionManager.instance.SumarPuntosAlTotal(50);

                }
                else
                {
                    resultadoPrueba = "¡Practica para mejorar!";
                    SessionManager.instance.SumarPuntosAlTotal(25);
                }

                //Se prepara la Interfaz
                SessionManager.instance.sucesionDeJuegos.RemoveAt(0);

                //Editando titulo de la pantalla final
                //int completed = SessionManager.instance.totalGamesInSession - SessionManager.instance.sucesionDeJuegos.Count;
                //finishingPanel.transform.Find("Panel/Title").gameObject.GetComponent<Text>().text = "¡Terminado! " + completed + " / " + SessionManager.instance.totalGamesInSession;

                //Editando texto de resultado
                /*finishingPanel.transform.Find("Panel/Information/PointsText").gameObject.GetComponent<Text>().text = "Resultado: ";
                finishingPanel.transform.Find("Panel/Information/PointsText/ResolutionText").gameObject.GetComponent<Text>().text = resultadoPrueba;*/

                //Editando texto de resultado adicional
                //finishingPanel.transform.Find("Panel/Information/AdditionalText").gameObject.GetComponent<Text>().text = "Figuras pintadas: " + figuresDone;

                //Editando texto de puntos
                //finishingPanel.transform.Find("Panel/Information/TimeText").gameObject.GetComponent<Text>().text = "Errores cometidos: " + paintErrors;

                //Editando las funciones de los botones y su texto correspondiente
                //finishingPanel.transform.Find("Panel/Buttons/NextButton").gameObject.GetComponent<Button>().onClick.AddListener(() => SessionManager.instance.chargeNextScene());
                //finishingPanel.transform.Find("Panel/Buttons/NextButton/Information2").GetComponent<Text>().text = "Siguiente";
            }
        }
    }

    private void playAgain()
    {
        SceneManager.LoadScene("Graffiti");
    }

    private void ActualizarTiempoGameplay()
    {

        gameplayTime += Time.deltaTime;
        float timeToShow = totalTimeToGo - gameplayTime;
        if (timeToShow <= 0)
        {
            timeToShow = 0;
        }
        timeText.text = "Tiempo: " + Mathf.Floor(timeToShow).ToString("00");
    }

    private void AnimarSpray()
    {
        if (startTime > lerpDuration)
            return;

        if (nearOfTheWall)
        {
            float fractionOfJourney = startTime / lerpDuration;

            sprayModel.localPosition = Vector3.Lerp(sprayModel.localPosition, new Vector3(0, -0.01f, 0.26f), fractionOfJourney);
            startTime += speed * Time.deltaTime;
        }
        else
        {
            float fractionOfJourney = startTime / lerpDuration;

            sprayModel.localPosition = Vector3.Lerp(sprayModel.localPosition, new Vector3(0, -0.142f, 0.26f), fractionOfJourney);
            startTime += speed * Time.deltaTime;
        }
    }

    private void EjecutarRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.collider.tag == "Drawable")
            {
                if (!nearOfTheWall)
                {
                    startTime = 0;
                }

                nearOfTheWall = true;
                return;
            }
            else if (hit.collider.tag == "dirtyPoint")
            {
                if (!nearOfTheWall)
                {
                    startTime = 0;
                }
                nearOfTheWall = true;

                if (painting)
                {
                    GroupOfPoints actualGroup = hit.collider.transform.parent.gameObject.GetComponent<GroupOfPoints>();
                    if (sprayManager.Color == actualGroup.colorToPaint)
                    {
                        totalPointsDone++;
                        actualGroup.siguientePunto();
                        actualizarTextoUI();
                    }
                }
                return;
            }
        }
        if (nearOfTheWall)
        {
            nearOfTheWall = false;
            startTime = 0;
        }
    }

    private void actualizarTextoUI()
    {
        int percentage = (int)totalPointsDone;
        //percentageText.text = "Puntos pintados: " + percentage;
    }

    public void StartTheGame()
    {
        placeObjectPanel.SetActive(false);
        sprayModel.gameObject.SetActive(true);
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

        playing = true;
        Gameplay_UI.SetActive(true);

        yield return null;
    }

    private void FinishPainting()
    {
        painting = false;
        cameraPointer.localPosition = new Vector3(0, 0, 0);
        sprayAudio.Stop();
    }

    public void StartPainting()
    {
        if (!painting)
        {
            painting = true;
            cameraPointer.localPosition = new Vector3(0, 0, 50);
            sprayAudio.Play();
        }
    }

    public void ChangeTheColor(Color newColor)
    {
        sprayManager.Color = newColor;
        plasticSprayMaterial.color = newColor;
        userColor = newColor;
    }

    //Assets/Models/Materials/Textures/EmptyTexture.png
    public void f_saveGraffiti()
    {
        string name = "Captura_Graffiti_Pipes_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss") + ".png";
        Texture2D image = finishingPanel.transform.parent.GetComponent<Painting_Interface_Manager>().copyTextureToImage().texture;
        NativeGallery.SaveImageToGallery(image, "Mis Grafittis", name, (success, path) => _ShowAndroidToastMessage("Media save result: The Graffiti " + (success ? "was successfully saved in " + path : "could NOT be saved.")));
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
