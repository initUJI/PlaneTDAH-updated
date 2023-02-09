using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConfigurationScript : MonoBehaviour
{

    public List<GameObject> listOfGroupPoints;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject newGroupOfPoints = new GameObject();
        int selection = Random.Range(0, listOfGroupPoints.Count);

        if (SessionManager.instance == null || (SessionManager.instance != null && SessionManager.instance.playingSession == false))
        {
            //El jugador no se encuentra en una session
            newGroupOfPoints = (GameObject)Instantiate(listOfGroupPoints[selection], this.transform, false);
        }
        else
        {
            //El jugador se encuentra en una sesion
            MinijuegoLevel thisLevel = SessionManager.instance.sucesionDeJuegos[0];

            switch (thisLevel.difficulty)
            {
                case "Easy":
                    newGroupOfPoints = (GameObject) Instantiate(listOfGroupPoints[selection], this.transform, false);
                    break;

                case "Medium":
                    newGroupOfPoints = (GameObject)Instantiate(listOfGroupPoints[selection], this.transform, false);
                    break;

                case "Hard":
                    newGroupOfPoints = (GameObject)Instantiate(listOfGroupPoints[selection], this.transform, false);
                    break;
            }
        }
    }
}
