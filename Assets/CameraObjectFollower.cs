using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectFollower : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Material> skyboxPerMode;
    public Light luzEscenario;

    public Skybox box;
    public float speed = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        box.material.SetFloat("_Rotation", Time.time * speed);
    }

    public void actualizarEscenario(string dificultad = "Easy")
    {
        Color lightColor;

        switch (dificultad)
        {
            case "Easy":
                ColorUtility.TryParseHtmlString("#FFF4D6", out lightColor);
                luzEscenario.color = lightColor;
                box.material = skyboxPerMode[0];
                break;
            case "Medium":
                ColorUtility.TryParseHtmlString("#E7943C", out lightColor);
                luzEscenario.color = lightColor;
                box.material = skyboxPerMode[1];
                break;
            case "Hard":
                ColorUtility.TryParseHtmlString("#2D2E33", out lightColor);
                luzEscenario.color = lightColor;
                box.material = skyboxPerMode[2];
                break;
        }
    }
}
