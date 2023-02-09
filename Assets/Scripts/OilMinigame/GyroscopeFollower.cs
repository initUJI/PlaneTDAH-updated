using UnityEngine;

public class GyroscopeFollower : MonoBehaviour
{
    [Header("Tweaks")]
    //[SerializeField] private Quaternion baseRotation = new Quaternion(0, 0, 0, 0);

    [SerializeField] private Vector3 baseRotation;
    [SerializeField] private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        GyroscopeRotation.Instance.EnableGyro();
    }

    // Update is called once per frame
    void Update()
    {
        //Primero se recoge el input con su orientación corregida
        Vector3 rotationGyro = new Vector3(GyroscopeRotation.Instance.GetGyroRotation().eulerAngles.y, 
            GyroscopeRotation.Instance.GetGyroRotation().eulerAngles.x, 
            GyroscopeRotation.Instance.GetGyroRotation().eulerAngles.z);

        //Luego se le suma un angulo para que sea acorde con la escena
        rotationGyro += baseRotation;

        //Se interpola el angulo final con el actual
        Vector3 finalRotation = AngleLerp(transform.eulerAngles, rotationGyro, rotationSpeed);

        //Finalmente, se actualizan los valores de rotación
        transform.localRotation = Quaternion.Euler(finalRotation);
    }

    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
