
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    public GameManagerScript gameManager;

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
        if(other.tag == "obstacle")
        {
            other.GetComponent<RingObject>().PlayAnimation("failedAnimation");
            other.GetComponent<RingObject>().PlayErrorSound();
            if(gameManager.hasStarted)
            {
                gameManager.arosFallados++;
                gameManager.SumarPuntos(0);
            }
        }
    }
}
