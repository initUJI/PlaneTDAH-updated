using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public List<MinijuegoLevel> sucesionDeJuegos;
    public int totalGamesInSession = 3;
    public bool playingSession = false;

    public int puntosTotales, tiempoTotal, puntosAConseguir;

    public string difficultMode;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void setSessionMode(string newMode)
    {
        //Comprobar que no se hayan completado todas las sesiones del día


        //Se definen los diferentes ejercicios a hacer
        difficultMode = newMode;
        playingSession = true;

        //Cantidad de ejercicios en base a la duración
        switch (difficultMode)
        {
            case "Corta":
                createSucesionList(3);
                puntosAConseguir = 300;
                break;

            case "Media":
                createSucesionList(8);
                puntosAConseguir = 650;
                break;

            case "Larga":
                createSucesionList(12);
                puntosAConseguir = 1000;
                break;
        }

        //Debug Information
        foreach (MinijuegoLevel obj in sucesionDeJuegos)
            Debug.Log(obj.typeOfGame + " , " + obj.difficulty);

        Debug.Log("Total de ejercicios: " +sucesionDeJuegos.Count);
        totalGamesInSession = sucesionDeJuegos.Count;

        //Comenzar la sesión
        chargeNextScene();
    }

    public void createSucesionList(int duration)
    {
        sucesionDeJuegos = new List<MinijuegoLevel>();

        sucesionDeJuegos.Add(RandomizeALevel("Easy"));
        sucesionDeJuegos.Add(RandomizeALevel("Medium"));
        sucesionDeJuegos.Add(RandomizeALevel("Hard"));

    }

    public void SumarPuntosAlTotal(int points)
    {
        puntosTotales += points;
    }

    public void SumarTiempoAlTotal(int time)
    {
        tiempoTotal += time;
    }

    public void chargeNextScene()
    {
        //Se han hecho ya todas las misiones
        if (sucesionDeJuegos.Count == 0)
        {
            //Cargar escena final con resultados
            SceneManager.LoadScene("FinishingTheSession");
            return;
        }
            

        MinijuegoLevel newScene = sucesionDeJuegos[0];

        SceneManager.LoadScene(newScene.typeOfGame);
    }

    public void clearData()
    {
        puntosTotales = 0;
        tiempoTotal = 0;
    }

    public void clearSuccesionList()
    {
        playingSession = false;
        sucesionDeJuegos = new List<MinijuegoLevel>();
    }

    private MinijuegoLevel RandomizeALevel(string mode)
    {
        /*bool correct = false;
        MinijuegoLevel newMinigame = new MinijuegoLevel();

        while (!correct)
        {
            int i = UnityEngine.Random.Range(0, 4);
            

            switch (i)
            {
                case 0:
                    newMinigame = new MinijuegoLevel("Avion", mode);
                    break;

                case 1:
                    newMinigame = new MinijuegoLevel("Fluid", mode);
                    break;

                case 2:
                    newMinigame = new MinijuegoLevel("Pipes", mode);
                    break;

                case 3:
                    newMinigame = new MinijuegoLevel("Graffiti", mode);
                    break;
            }

            //Comprueba que el nivel no es repetido
            bool repetido = false;
            foreach (MinijuegoLevel level in sucesionDeJuegos)
            {
                if (newMinigame.typeOfGame == level.typeOfGame && newMinigame.difficulty == level.difficulty)
                    repetido = true;
            }

            //Si no esta repetido saldrá del bucle
            if (!repetido)
                correct = true;
            else
                newMinigame = new MinijuegoLevel();
        }*/
        MinijuegoLevel newMinigame = new MinijuegoLevel();
        newMinigame = new MinijuegoLevel("Avion", mode);


        return newMinigame;
    }
}

public class  MinijuegoLevel
{
    public string typeOfGame { get; set; }
    public string difficulty { get; set; }

    public MinijuegoLevel(string gameMode = "", string DifficultyLevel = "")
    {
        typeOfGame = gameMode;
        difficulty = DifficultyLevel;
    }
}
