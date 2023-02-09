using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorCollider : MonoBehaviour
{
    public FaucetObjectScript parentScript;

    // Start is called before the first frame update
    void Start()
    {
        parentScript = gameObject.GetComponentInParent(typeof(FaucetObjectScript)) as FaucetObjectScript;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            parentScript.AbrirGrifo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            parentScript.CerrarGrifo();
        }
    }
}
