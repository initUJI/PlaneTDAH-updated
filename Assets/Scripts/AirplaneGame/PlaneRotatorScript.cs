using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRotatorScript : MonoBehaviour
{
    public float fanVelocity = 0;
    public float angleToTurn;
    public float maxSpeed;
    public float percentage;

    public int multiplier;

    public Fan_Controller fanController;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
        fanController = GetComponentInChildren<Fan_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        percentage = oscillate(Time.time, maxSpeed, 1);

        Vector3 rotationToReach = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 2 * percentage);
        Vector3 newRotation = rotationToReach;

        transform.eulerAngles = newRotation;
    }

    float calcularVelocidadGiro()
    {
        float value = fanController.normalizedFanSpeed();

        float percentageSpeed = 1 - (value / 100);

        return percentageSpeed * maxSpeed;
    }

    float oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }
}
