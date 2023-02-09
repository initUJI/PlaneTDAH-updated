using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class instructionInformationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string gameModeSelected;
    public List<string> gameModeInstructions;

    public Text instructionsTitle;
    public TextMeshProUGUI instructionsInfo;
    void Awake()
    {
        string avionInformation = "¡Orienta el avión con tu móvil para recoger la máxima cantidad de aros posibles! Controla el avión rotando el móvil en la dirección que quieras moverlo." +
            " \n \n¡Si consigues el objetivo de aros antes de tiempo conseguirás un bonus de puntos!";

        string fluidInformation = "Coloca la jarra debajo del grifo para llenarla de combustible. Luego desplaza la jarra lentamente y con cuidado hasta depositarla en el recipiente. " +
            "Realiza este proceso varias veces hasta que el recipiente esté completamente lleno. \n \n¡Cuidado de que no se caiga nada al suelo!";

        string pipesInformation = "Apunta a los puntos sucios de las tuberías y mantén pulsado un dedo en la pantalla para lanzar un rayo limpiador. Cuidado con disparar a un lugar que no esté sucio,\n \n¡Podrías romper la máquina!";

        string grafitiInformation = "Lo primero será encontrar una superficie espaciosa y plana para colocar el área de juego virtual. Una vez colocada, se deben completar las diversas figuras formadas por puntos." +
            " Para ello, hay que pulsar la pantalla cuando se apunta al muro para pintar y hacer un trazo siguiendo todos los puntos marcados. \n \n¡Conviértete en un gran artista!";

        gameModeInstructions.Add(avionInformation);
        gameModeInstructions.Add(fluidInformation);
        gameModeInstructions.Add(pipesInformation);
        gameModeInstructions.Add(grafitiInformation);
    }


    public void setInformation(string gameMode)
    {
        switch (gameMode)
        {
            case "Avion":
                instructionsTitle.text = "Dirige el avión";
                instructionsInfo.text = gameModeInstructions[0];
                break;
            case "Fluid":
                instructionsTitle.text = "Llena el recipiente";
                instructionsInfo.text = gameModeInstructions[1];

                break;
            case "Pipes":
                instructionsTitle.text = "Elimina la suciedad";
                instructionsInfo.text = gameModeInstructions[2];

                break;
            case "Graffiti":
                instructionsTitle.text = "Pinta la pared";
                instructionsInfo.text = gameModeInstructions[3];

                break;
        }
    }
}
