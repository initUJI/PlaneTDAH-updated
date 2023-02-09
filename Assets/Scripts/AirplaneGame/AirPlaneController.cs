using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneController : MonoBehaviour
{
    private Rigidbody rigid;
    private GameManagerScript gameManager;

    public float planeSpeed;
    public float rotationSpeed;
    public float rotationMagnitude;
    [SerializeField] float movementMultiplier;

    public float leftLimit = -15;
    public float rightLimit = 15;
    public float topLimit = 5;
    public float bottomLimit = -4;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        if(gameManager.hasStarted)
        {
            //Se recoge el Input
            #if UNITY_EDITOR
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            #elif UNITY_ANDROID
            Vector3 movement = new Vector3(Input.acceleration.x * (movementMultiplier * 2), Input.acceleration.z * movementMultiplier, 0);
            #endif

            //Se le da movimiento al Avión en caso de que esté dentro de los limites de la pantalla
            if (bottomLimit <= transform.position.y && transform.position.y <= topLimit &&
                leftLimit <= transform.position.x && transform.position.x <= rightLimit)
            {
                rigid.velocity = movement * planeSpeed * Time.deltaTime;
            }
            //En caso de que se salga por alguno de los 4 limites (bordes de la pantalla)
            else if (transform.position.y < bottomLimit)
            {
                transform.position = new Vector3(transform.position.x, bottomLimit + .5f, transform.position.z);
            }
            else if (transform.position.y > topLimit)
            {
                transform.position = new Vector3(transform.position.x, topLimit - .5f, transform.position.z);
            }
            else if (transform.position.x < leftLimit)
            {
                transform.position = new Vector3(leftLimit + .5f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > rightLimit)
            {
                transform.position = new Vector3(rightLimit - 0.5f, transform.position.y, transform.position.z);
            }

            //Se rota el avión en la direccion en la que se está moviendo
            Vector3 rotationVector = new Vector3(rigid.velocity.normalized.y * rotationMagnitude, 0, rigid.velocity.normalized.x * rotationMagnitude);
            //Debug.Log("Vector is ( " + rigid.velocity.x + " , " + rigid.velocity.y + " )");

            transform.eulerAngles = AngleLerp(transform.eulerAngles, rotationVector, Time.deltaTime * rotationSpeed);
        }else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rigid.velocity = new Vector3(0, 0, 0);
        }
    }

    //Funcion personalizada para interpolar valores ya que Vector3.Lerp() da problemas con los angulos
    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
