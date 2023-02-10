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
            Vector3 _movement = Vector3.zero;
#if UNITY_EDITOR
            _movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
#elif UNITY_ANDROID
            _movement = new Vector3(Input.acceleration.x * (movementMultiplier * 2), Input.acceleration.z * movementMultiplier, 0);
#endif
            
            Vector3 _velocityInAxis = _movement * planeSpeed * Time.deltaTime;

            //Se rota el avión en la direccion en la que se está moviendo
            rigid.velocity = _velocityInAxis;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit + .5f, rightLimit - 0.5f), Mathf.Clamp(transform.position.y, bottomLimit + .5f, topLimit - .5f), transform.position.z);

            Vector3 _rotationVector = new Vector3(rigid.velocity.normalized.y * rotationMagnitude, 0, rigid.velocity.normalized.x * rotationMagnitude);

            transform.eulerAngles = AngleLerp(transform.eulerAngles, _rotationVector, Time.deltaTime * rotationSpeed);
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
