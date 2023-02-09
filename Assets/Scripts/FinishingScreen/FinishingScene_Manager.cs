using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishingScene_Manager : MonoBehaviour
{
    public GameObject panel_UI, extraGame_panel;
    
    SessionManager sessionManager;

    public List<Sprite> markImages;

    int pointsMal, pointsNormal, pointsBien, pointsExcelente;
    void Start()
    {
        if (SessionManager.instance != null)
            sessionManager = SessionManager.instance;

        //Modificando el titulo
        
        panel_UI.transform.Find("Title").GetComponent<Text>().text = "¡Sesión " + sessionManager.difficultMode + " finalizada!";


        //Crea limites para las puntuaciones
        if(sessionManager.difficultMode == "Corta")
        {
            pointsMal = 150;
            pointsNormal = 200;
            pointsBien = 250;
            pointsExcelente = 300;
        }else if(sessionManager.difficultMode == "Media")
        {
            pointsMal = 300;
            pointsNormal = 400;
            pointsBien = 475;
            pointsExcelente = 550;
        }
        else
        {
            pointsMal = 600;
            pointsNormal = 785;
            pointsBien = 900;
            pointsExcelente = 1000;
        }

        string colorCode;

        //¿El jugador ha ganado el juego extra?
        if(sessionManager.puntosTotales >= pointsExcelente)
        {
            //Cambiar el nombre del botón y cambiar su funcion.
            panel_UI.transform.Find("Buttons/ContinueButton").GetComponent<Button>().onClick.AddListener(() => juegoExtraPanel());
            panel_UI.transform.Find("Buttons/ContinueButton/Title").GetComponent<Text>().text = "¡JUEGO EXTRA!";
            colorCode = "#008000ff";
        }
        else
        {
            //El jugador no ha ganado el premio
            panel_UI.transform.Find("Buttons/ContinueButton").GetComponent<Button>().onClick.AddListener(() => continueButton());
            panel_UI.transform.Find("Buttons/ContinueButton/Title").GetComponent<Text>().text = "Continuar";
            colorCode = "#ff0000ff";
        }

        //Mostrando informacion del trascurso de la sesion
        panel_UI.transform.Find("Information/TiempoTotal").GetComponent<Text>().text = "Juego Extra: " + "<color=" +colorCode+ ">" + pointsExcelente + "</color>" + " Puntos";
        panel_UI.transform.Find("Information/PuntosTotal").GetComponent<Text>().text = "Puntos totales: " + sessionManager.puntosTotales + " Puntos";

        //Crear marca para determinar como lo ha hecho
        if (sessionManager.puntosTotales > 0 && sessionManager.puntosTotales < pointsMal)
        {
            //Lo ha hecho mal
            panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[0];
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "Ups...";
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(166, 9, 1, 255);
        }
        else if (sessionManager.puntosTotales >= pointsMal && sessionManager.puntosTotales < pointsNormal)
        {
            //Lo ha hecho medio mal
            panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[1];
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "No esta mal";
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(166, 77, 1, 255);
        }
        else if (sessionManager.puntosTotales >= pointsNormal && sessionManager.puntosTotales < pointsBien)
        {
            //Lo ha hecho normal
            panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[2];
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "Bien";
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(142, 140, 10, 255);
        }
        else if (sessionManager.puntosTotales >= pointsBien && sessionManager.puntosTotales < pointsExcelente)
        {
            //Lo ha hecho bien
            panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[3];
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "¡Genial!";
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(96, 173, 0, 255);
        }
        else
        {
            //Lo ha hecho muy bien
            panel_UI.transform.Find("CalificationImage/Image").GetComponent<Image>().sprite = markImages[4];
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().text = "¡Excelente!";
            panel_UI.transform.Find("CalificationImage/CalificationText").GetComponent<Text>().color = new Color32(0, 138, 27, 255);
        }

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
}
