using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetObjectScript : MonoBehaviour
{
    public AudioSource faucetSound;
    public GameObject waterDrop;
    Transform spawnPoint;
    public int particlesQuantity;

    bool activated;
    List<GameObject> waterObjects = new List<GameObject>();
    FluidGameManager gameManager;

    void Start()
    {
        spawnPoint = transform.Find("SpawnLiquidPoint");
        gameManager = GameObject.Find("GameManager").GetComponent<FluidGameManager>();
    }

    public void AbrirGrifo()
    {
        if(gameManager.hasStarted)
        {
            //Marcar como activado
            if (!activated)
            {
                activated = true;
                faucetSound.Play();
                StartCoroutine("SpawnWater"); //Crear corutina que haga spawn de las gotas
            }
            else
                return;
        }
    }

    public void CerrarGrifo()
    {
        if (activated)
        {
            activated = false;
            faucetSound.Stop();
        }
        else
            return;


    }


    IEnumerator SpawnWater()
    {
        while(activated && gameManager.hasStarted)
        {
            GameObject newObject = (GameObject) Instantiate(waterDrop, spawnPoint.position, spawnPoint.rotation);
            waterObjects.Add(newObject);
            yield return new WaitForSeconds(0.15f);
        }

        yield return null;
    }
}
