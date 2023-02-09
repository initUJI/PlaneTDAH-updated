using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    GameManagerScript gameManager;

    public PatronesSpawns patronesToSpawn;

    public int totalRingSpawned = 0;
    public int totalRingDestroyed = 0;
    int lastAnimation = 0;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        patronesToSpawn = (PatronesSpawns) FindObjectOfType(typeof(PatronesSpawns));
        StartCoroutine("SetGuard");
    }

    IEnumerator SetGuard()
    {
        while(!gameManager.hasStarted)
            yield return new WaitForSeconds(0.5f);

        while (gameManager.hasStarted)
        {
            //Proceso de patrones basandose en script que los almacena
            //1º Decision de que patron se va a procesar
            int numAnimation = lastAnimation;
            while (numAnimation == lastAnimation)
            {
                numAnimation = UnityEngine.Random.Range(1, patronesToSpawn.posicionamientosSpawn.Count) - 1;
            }
            lastAnimation = numAnimation;

            //2º Procesamiento e instancia del patron
            yield return StartCoroutine(ProcesarArraySpawn(patronesToSpawn.posicionamientosSpawn[numAnimation])); 

            //3º Parada antes del siguiente proceso
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator ProcesarArraySpawn(string[] arrayToProcess)
    {
        //Se recorre el array que contiene la posicion del patron y se instancian segun su contenido
        foreach ( string position in  arrayToProcess)
        {
            Transform positionToSpawn = SearchChild("Res" + position);

            if (positionToSpawn != null)
            {
                Instantiate(objectToSpawn, positionToSpawn.position, positionToSpawn.rotation);
                totalRingSpawned++;
            }

            yield return new WaitForSeconds(1);
        }
    }

    public Transform SearchChild(string name)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == name)
            {
                return transform.GetChild(i);
            }
        }
        return null;
    }
}
