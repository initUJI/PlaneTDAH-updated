using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishTheSession : MonoBehaviour
{
    public GameObject panel_UI, extraGame_panel;
    public AudioSource celebrationSound_Effect;
    
    SessionManager sessionManager;

    public List<Sprite> markImages;

    int pointsMal, pointsNormal, pointsBien, pointsExcelente;


    private void Awake()
    {
        if (SessionManager.instance != null)
            sessionManager = SessionManager.instance;

        //Modificando el titulo
        panel_UI.transform.Find("Title").GetComponent<Text>().text = "¡Sesión finalizada!";
        panel_UI.transform.Find("Information/TiempoTotal").GetComponent<Text>().text = "";
        panel_UI.transform.Find("CalificationImage").gameObject.SetActive(false);

        //Se establecen los limites de la puntuación
        pointsMal = 150;
        pointsNormal = 200;
        pointsBien = 250;
        pointsExcelente = 300;

        //Se implementa el evento para continuar y salir al menú principal
        string colorCode = "#ff0000ff";
        panel_UI.transform.Find("Buttons/ContinueButton").GetComponent<Button>().onClick.AddListener(() => continueButton());
        panel_UI.transform.Find("Buttons/ContinueButton/Title").GetComponent<Text>().text = "Continuar";

        //Mostrando informacion del trascurso de la sesion
        panel_UI.transform.Find("Information/PuntosTotal").GetComponent<Text>().text = "Puntos actuales: " + sessionManager.puntosTotales + " Puntos";
        
        if(DataManager.instancia != null && DataManager.instancia.ultimaPuntuacion != 0)
        {
            panel_UI.transform.Find("Information/TiempoTotal").GetComponent<Text>().text = "Puntos anterior partida: " + "<color=" + colorCode + ">" + DataManager.instancia.ultimaPuntuacion + "</color>" + " Puntos";

            if(sessionManager.puntosTotales >= DataManager.instancia.ultimaPuntuacion)
            {
                panel_UI.transform.Find("CalificationImage").gameObject.SetActive(true);
                celebrationSound_Effect.Play();
                panel_UI.transform.Find("Buttons/ContinueButton").GetComponent<Button>().onClick.RemoveAllListeners();
                panel_UI.transform.Find("Buttons/ContinueButton").GetComponent<Button>().onClick.AddListener(() => juegoExtraPanel());

                panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[4];
                panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "¡Guau!";
                panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(0, 138, 27, 255);
            }
        }

        DataManager.instancia.ultimaPuntuacion = sessionManager.puntosTotales;
        DataManager.instancia.Guardar();
    }
    public void continueButton()
    {
        sessionManager.clearData();
        sessionManager.clearSuccesionList();

        SceneManager.LoadScene("MainMenu");
    }

    public void juegoExtraPanel()
    {
        extraGame_panel.SetActive(true);
    }

    public void jugarAJuegoSpray()
    {
        sessionManager.clearData();
        sessionManager.clearSuccesionList();

        SceneManager.LoadScene("Graffiti");
    }
}
