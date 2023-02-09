using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCollisionScript : MonoBehaviour
{
    FluidGameManager gameManager;

    public GameObject rellenoJarra;
    public Transform colliderPoint;
    int conteo;

    Vector3 scaleWithoutFill, scaleFilled;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = (FluidGameManager) FindObjectOfType(typeof(FluidGameManager));
        scaleWithoutFill = new Vector3(1, 0.05f, 0.05f);
        scaleFilled = new Vector3(1, 1, 1);
        rellenoJarra.transform.localScale = scaleWithoutFill;
    }

    private void Update()
    {
        transform.position = colliderPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "particleObject")
        {
            if(other.gameObject.GetComponent<DontGoThroughThings>().activated && gameManager.hasStarted)
            {
                conteo++;
                gameManager.actualizarValorSlider(conteo);
                actualizarFormaRelleno();
                other.GetComponent<DontGoThroughThings>().DesactivateRigidbody(true);
            }
        }
    }

    private void actualizarFormaRelleno()
    {
        float percentage = gameManager.particleCounter / gameManager.particlesToReach;
        if (percentage > 1)
            percentage = 1;

        rellenoJarra.transform.localScale = Vector3.Lerp(scaleWithoutFill, scaleFilled, percentage);
    }
}
