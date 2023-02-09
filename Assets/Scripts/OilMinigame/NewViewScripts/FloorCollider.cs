using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    FluidGameManager gameManager;
    int conteoFallido;

    void Start()
    {
        gameManager = (FluidGameManager)FindObjectOfType(typeof(FluidGameManager));
        conteoFallido = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "particleObject")
        {
            if(collision.gameObject.GetComponent<DontGoThroughThings>().activated && gameManager.hasStarted)
            {
                conteoFallido++;
                gameManager.actualizarFailedDrops(conteoFallido);
                collision.gameObject.GetComponent<DontGoThroughThings>().DesactivateRigidbody(false);
            }
        }
    }
}
