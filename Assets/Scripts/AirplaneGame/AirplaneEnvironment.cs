using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneEnvironment : MonoBehaviour
{
    public GameObject contenedorMontañasIzq, contenedorMontañasDer;

    public GameObject[] mountains;
    public Vector3 initialPositionRight, initialPositionLeft;

    Transform actualMontañaIzq, actualMontañaDer;

    // Start is called before the first frame update
    void Start()
    {
        initialPositionRight = new Vector3(36.1f, -12, 0);
        initialPositionLeft = new Vector3(-36.1f, -12, 0);
        inciarMontañasEscenario();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void inciarMontañasEscenario()
    {
        //Creando las montañas zona derecha
        int numMountain = UnityEngine.Random.Range(0, mountains.Length);
        
        GameObject montañaDer = (GameObject)Instantiate(mountains[numMountain], initialPositionRight, Quaternion.Euler(0, -2, 0));
        montañaDer.transform.parent = contenedorMontañasDer.transform;
        montañaDer.GetComponent<MountainMovement>().isOnTheRight = true;
        actualMontañaDer = montañaDer.transform;

        for (int i = 0; i < 10; i++)
        {
            Transform puntoDeInstancia = actualMontañaDer.Find("EndPoint");

            numMountain = UnityEngine.Random.Range(0, mountains.Length);
            GameObject newMountain = (GameObject)Instantiate(mountains[numMountain], puntoDeInstancia.position, Quaternion.Euler(0, -2, 0));
            newMountain.GetComponent<MountainMovement>().isOnTheRight = true;
            newMountain.transform.parent = contenedorMontañasDer.transform;

            actualMontañaDer = newMountain.transform;
        }

        //Creando las montañas de la izquierda
        numMountain = UnityEngine.Random.Range(0, mountains.Length);

        GameObject montañaIzq = (GameObject)Instantiate(mountains[numMountain], initialPositionLeft, Quaternion.Euler(0, 2, 0));
        montañaIzq.transform.parent = contenedorMontañasIzq.transform;
        montañaIzq.GetComponent<MountainMovement>().isOnTheRight = false;
        actualMontañaIzq = montañaIzq.transform;

        for (int i = 0; i < 10; i++)
        {
            Transform puntoDeInstancia = actualMontañaIzq.Find("EndPoint");

            numMountain = UnityEngine.Random.Range(0, mountains.Length);
            GameObject newMountain = (GameObject)Instantiate(mountains[numMountain], puntoDeInstancia.position, Quaternion.Euler(0, 2, 0));
            newMountain.GetComponent<MountainMovement>().isOnTheRight = false;
            newMountain.transform.parent = contenedorMontañasIzq.transform;

            actualMontañaIzq = newMountain.transform;
        }
    }

    public void crearNuevaMontaña(bool isOnTheRight)
    {
        if (isOnTheRight)
        {
            Transform puntoDeInstancia = actualMontañaDer.Find("EndPoint");

            int numMountain = UnityEngine.Random.Range(0, mountains.Length);
            GameObject newMountain = (GameObject)Instantiate(mountains[numMountain], puntoDeInstancia.position, Quaternion.Euler(0, -2, 0));
            newMountain.GetComponent<MountainMovement>().isOnTheRight = true;
            newMountain.transform.parent = contenedorMontañasDer.transform;

            actualMontañaDer = newMountain.transform;
        }
        else
        {
            Transform puntoDeInstancia = actualMontañaIzq.Find("EndPoint");

            int numMountain = UnityEngine.Random.Range(0, mountains.Length);
            GameObject newMountain = (GameObject)Instantiate(mountains[numMountain], puntoDeInstancia.position, Quaternion.Euler(0, 2, 0));
            newMountain.GetComponent<MountainMovement>().isOnTheRight = false;
            newMountain.transform.parent = contenedorMontañasIzq.transform;

            actualMontañaIzq = newMountain.transform;
        }
    }
}
