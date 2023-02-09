using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterOfGroupOfPoints : MonoBehaviour
{
    public List<GameObject> grupitosOfPoints;
    private Painting_GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Painting_GameManager>();
        int pointsToDo = 0;

        foreach (Transform child in transform)
        {
            grupitosOfPoints.Add(child.gameObject);
            pointsToDo += child.GetComponent<GroupOfPoints>().objectivesToPaint.Count;
            child.gameObject.SetActive(false);
        }
        //Activar primera figura
        grupitosOfPoints[0].SetActive(true);
        gameManager.objectiveColor = grupitosOfPoints[0].GetComponent<GroupOfPoints>().colorToPaint;
        if(gameManager.difficultMode == "Medium" || gameManager.difficultMode == "Hard")
        {
            grupitosOfPoints[0].GetComponent<GroupOfPoints>().ocultarColor();
        }

        //Comunicar al manager el numero total de puntos a hacer
        gameManager.totalPointsToDo = pointsToDo;
    }

    public void siguienteGrupoDePuntos()
    {
        //Se ha completado el grupo actual por lo que se elimina
        grupitosOfPoints[0].SetActive(false);
        grupitosOfPoints.RemoveAt(0);
        gameManager.figuresDone++;

        //Ya se han completado todos los grupos de puntos
        if (grupitosOfPoints.Count == 0)
        {
            //Fin del juego
            return;
        }
        //Aun quedan grupos de puntos por completar
        else
        {
            //Se activa el siguiente grupo de puntos
            grupitosOfPoints[0].SetActive(true);
            gameManager.objectiveColor = grupitosOfPoints[0].GetComponent<GroupOfPoints>().colorToPaint;

            if (gameManager.difficultMode == "Medium" || gameManager.difficultMode == "Hard")
            {
                grupitosOfPoints[0].GetComponent<GroupOfPoints>().ocultarColor();
            }
        }

    }
}
