using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetActivator : MonoBehaviour
{

    bool activateState = false;

    public GameObject particlesEmmiter;

    private void Start()
    {
        particlesEmmiter.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activateState = true;
            particlesEmmiter.SetActive(true);
        }
    }
}
