using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPanel_Decorator : MonoBehaviour
{
    public Text date_Text;
    public Text panelInformation_Text;

    public Button previousPage_Button, nextPage_Button;
    [SerializeField] Button sendButton;

    PartidaJugada[] datosParaMostrar;

    DateTime firstDay, lastDay, actualDayOnPage;

    // Start is called before the first frame update
    void Awake()
    {
        datosParaMostrar = DataManager.instancia.datosPorPartida;
        int daysPlaying = CountDaysPlaying();
        if(daysPlaying >= 7)
        {
            sendButton.interactable = true;
        }

        //Si todo va bien cargará los elementos y los mostrará por pantalla
        if (datosParaMostrar != null && datosParaMostrar.Length != 0)
        {
            //Vemos los días en los que se ha hecho el ejercicio
            firstDay = datosParaMostrar[datosParaMostrar.Length - 1].fechaEjercicio;
            lastDay = datosParaMostrar[0].fechaEjercicio;
            actualDayOnPage = firstDay;

            if (firstDay.Date == lastDay.Date)
            {
                date_Text.text = firstDay.Date.ToString("yyyy/MM/dd");
                previousPage_Button.interactable = false;
                nextPage_Button.interactable = false;
            }
            else
            {
                date_Text.text = firstDay.Date.ToString("yyyy/MM/dd");
                previousPage_Button.interactable = false;
                nextPage_Button.interactable = true;
            }

            foreach (PartidaJugada partida in datosParaMostrar)
            {
                if(partida.fechaEjercicio.Date == actualDayOnPage.Date)
                {
                    string nuevaInfo;

                    string partidaTerminada = partida.juegoTerminado ? "Juego terminado." : "Se abandonó la partida";
                    double percentage = (double)partida.aciertos / (double)(partida.aciertos + partida.fallos) * 100;


                    nuevaInfo = "Juego: Avion\n" + "Fecha de Juego: " + partida.fechaEjercicio.ToString() + "\n" +
                        "Dificultad: " + partida.dificultad + "\n" +
                        "Partida terminada: " + partidaTerminada + "\n" +
                        "Precisión de la partida (Aciertos/Fallos): " + Math.Truncate(percentage) + "% (" + partida.aciertos + " / " + partida.fallos + ") \n\n\n";

                    panelInformation_Text.text += nuevaInfo;
                }
            }
        }
        //En caso de que no hayan datos aún almacenados o no encuentre la instancia de dataManager
        else
        {
            panelInformation_Text.text = "No hay datos a mostrar en estos momentos.";

            previousPage_Button.interactable = false;
            nextPage_Button.interactable = false;
            date_Text.text = "xxxx/xx/xx";
        }
    }

    int CountDaysPlaying()
    {
        List<DateTime> diasJugados = new List<DateTime>();

        foreach(PartidaJugada partida in datosParaMostrar)
        {
            //Si no hay ningun día añadido a la lista se añade (en caso de que haya algun ejercicio ya realizado)
            if(diasJugados.Count == 0)
            {
                diasJugados.Add(partida.fechaEjercicio.Date);
                continue;
            }

            //Se comprueba si el día de la partida ya está registrado y en caso de que sí, no lo cuenta
            bool addToList = true;
            foreach(DateTime dia in diasJugados)
            {
                if(partida.fechaEjercicio.Date == dia.Date)
                {
                    addToList = false;
                }
            }

            //Si es un día diferente a todos los que hay se añade a la lista
            if (addToList)
                diasJugados.Add(partida.fechaEjercicio.Date);
        }

        Debug.Log("Dias jugados en total: " + diasJugados.Count);
        return diasJugados.Count;
    }

    string preparingMailToSend()
    {
        string result = "";

        foreach (PartidaJugada partida in datosParaMostrar)
        {
            
            string nuevaInfo;

            string partidaTerminada = partida.juegoTerminado ? "Juego terminado." : "Se abandonó la partida";
            double percentage = (double)partida.aciertos / (double)(partida.aciertos + partida.fallos) * 100;


            nuevaInfo = "Juego: Avion\n" + "Fecha de Juego: " + partida.fechaEjercicio.ToString() + "\n" +
                "Dificultad: " + partida.dificultad + "\n" +
                "Partida terminada: " + partidaTerminada + "\n" +
                "Precisión de la partida (Aciertos/Fallos): " + Math.Truncate(percentage) + "% (" + partida.aciertos + " / " + partida.fallos + ") \n\n\n";

            result += nuevaInfo;
        }

        return result;
    }

    public void sendEmail()
    {
        string subject = "Información prueba de juegos: Fly The Plane";

        string body = "La información recopilada del ejercicio realizado es la siguiente: \n\n";

        body += preparingMailToSend() + "\n\n";

        body += "Prueba realizada en colaboración con el INIT (Institute of New Imaging Technologies).";
    }

    public void nextPage()
    {
        //Se actualizan los botones que mueven de día
        actualDayOnPage = actualDayOnPage.AddDays(1);
        Debug.Log(actualDayOnPage.Date.ToString() + " / " + lastDay.Date.ToString());

        previousPage_Button.interactable = true;
        nextPage_Button.interactable = true;

        if (actualDayOnPage.Date >= lastDay.Date)
        {
            nextPage_Button.interactable = false;
        }
        date_Text.text = actualDayOnPage.Date.ToString("yyyy/MM/dd");

        bool exerciseOnDay = false;
        //Llenar el panel de los datos necesarios
        panelInformation_Text.text = "";
        foreach (PartidaJugada partida in datosParaMostrar)
        {
            if (partida.fechaEjercicio.Date == actualDayOnPage.Date)
            {
                exerciseOnDay = true;
                string nuevaInfo;

                string partidaTerminada = partida.juegoTerminado ? "Juego terminado." : "Se abandonó la partida";
                double percentage = (double)partida.aciertos / (double)(partida.aciertos + partida.fallos) * 100;

                nuevaInfo = "Juego: Avion\n" + "Fecha de Juego: " + partida.fechaEjercicio.ToString() + "\n" +
                    "Dificultad: " + partida.dificultad + "\n" +
                    "Partida terminada: " + partidaTerminada + "\n" +
                    "Precisión de la partida (Aciertos/Fallos): " + Math.Truncate(percentage) + "% (" + partida.aciertos + " / " + partida.fallos + ") \n\n\n";

                panelInformation_Text.text += nuevaInfo;
            }
        }

        //Si no había ejercicios hechos ese día, saltamos al siguiente
        if (exerciseOnDay == false)
        {
            nextPage();
        }
    }

    public void previousPage()
    {
        //Se actualizan los botones que mueven de día
        actualDayOnPage = actualDayOnPage.AddDays(-1);
        Debug.Log(actualDayOnPage.Date.ToString() + " / " + firstDay.Date.ToString());

        previousPage_Button.interactable = true;
        nextPage_Button.interactable = true;

        if (actualDayOnPage.Date <= firstDay.Date)
        {
            previousPage_Button.interactable = false;
        }
        date_Text.text = actualDayOnPage.Date.ToString("yyyy/MM/dd");


        //Llenar el panel de los datos necesarios
        bool exerciseOnDay = false;
        panelInformation_Text.text = "";
        foreach (PartidaJugada partida in datosParaMostrar)
        {
            if (partida.fechaEjercicio.Date == actualDayOnPage.Date)
            {
                exerciseOnDay = true;
                string nuevaInfo;

                string partidaTerminada = partida.juegoTerminado ? "Juego terminado." : "Se abandonó la partida";
                double percentage = (double)partida.aciertos / (double)(partida.aciertos + partida.fallos) * 100;

                nuevaInfo = "Juego: Avion\n" + "Fecha de Juego: " + partida.fechaEjercicio.ToString() + "\n" +
                    "Dificultad: " + partida.dificultad + "\n" +
                    "Partida terminada: " + partidaTerminada + "\n" +
                    "Precisión de la partida (Aciertos/Fallos): " + Math.Truncate(percentage) + "% (" + partida.aciertos + " / " + partida.fallos + ") \n\n\n";

                panelInformation_Text.text += nuevaInfo;
            }
        }

        if(!exerciseOnDay)
        {
            previousPage();
        }
    }

}
