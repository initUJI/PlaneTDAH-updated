using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainMovement : MonoBehaviour
{
    public float mountainSpeed;
    public bool isOnTheRight = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MountainMove();
    }

    void MountainMove()
    {
        transform.Translate(-Vector3.forward * mountainSpeed * Time.deltaTime);
    }
}
