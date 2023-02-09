using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mountain")
        {
            bool isRight = other.gameObject.GetComponent<MountainMovement>().isOnTheRight;
            GameObject.FindObjectOfType<AirplaneEnvironment>().crearNuevaMontaña(isRight);
            Destroy(other.gameObject);
        }
    }
}
