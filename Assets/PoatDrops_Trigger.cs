using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoatDrops_Trigger : MonoBehaviour
{
    public GameObject fillingIndicator_model;
    public FluidGameManager gameManager;
    public Transform cameraTransform, spawnPoint;
    public GameObject waterDrop;
    public float indicatorScale;

    int maxParticles;
    int actualParticles;

    public float poat_rotation;
    public bool activated;

    private void Awake()
    {
        maxParticles = 100;
        actualParticles = 0;
        indicatorScale = 0;
    }

    private void Update()
    {
        poat_rotation = cameraTransform.localEulerAngles.z;

        if(poat_rotation >= 120 && poat_rotation <= 283)
        {
            if(!activated)
                StartCoroutine(SpawnWater());
        }
        else
        {
            activated = false;
        }
    }

    IEnumerator SpawnWater()
    {
        activated = true;

        while (activated && gameManager.hasStarted && indicatorScale >= 0)
        {
            GameObject newObject = (GameObject)Instantiate(waterDrop, spawnPoint.position, spawnPoint.rotation);
            actualParticles--;
            actualizarEscalaIndicador();
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "particleObject")
        {
            actualParticles++;
            if(actualizarEscalaIndicador())
            {
                Destroy(other.gameObject);
            }
        }
    }

    private bool actualizarEscalaIndicador()
    {
        indicatorScale = (float) actualParticles / maxParticles;

        if(indicatorScale >= 1)
        {
            indicatorScale = 1;
            fillingIndicator_model.transform.localScale = new Vector3(fillingIndicator_model.transform.localScale.x, indicatorScale, fillingIndicator_model.transform.localScale.z);
            return false;
        }else
        {
            fillingIndicator_model.transform.localScale = new Vector3(fillingIndicator_model.transform.localScale.x, indicatorScale, fillingIndicator_model.transform.localScale.z);
            return true;
        }
    }
}
