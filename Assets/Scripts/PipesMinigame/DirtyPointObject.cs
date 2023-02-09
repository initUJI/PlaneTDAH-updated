using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPointObject : MonoBehaviour
{

    public enum RotationAxis
    {
        All,
        Y,
        X,
        Z
    }


    public RotationAxis axis;
    public float dirtyPointLifeQuant = 200;
    GameObject shieldsAroundVirus;
    public float scalationSpeed, rotationSpeed;
    public Transform dirtyPointModel;
    public bool growing;
    public bool gettingDamage;
    public GameObject endingParticles;


    private void Awake()
    {
        dirtyPointModel = transform.GetChild(0);
        gettingDamage = false;
    }

    public void SetDifficultMode(string difficultMode)
    {
        switch(difficultMode)
        {
            case ("Easy"):
                break;
            case ("Medium"):
                shieldsAroundVirus = dirtyPointModel.Find("MediumShields").gameObject;
                shieldsAroundVirus.SetActive(true);
                shieldsAroundVirus.transform.rotation = UnityEngine.Random.rotation;
                break;
            case ("Hard"):
                shieldsAroundVirus = dirtyPointModel.Find("HardShields").gameObject;
                shieldsAroundVirus.SetActive(true);
                shieldsAroundVirus.transform.rotation = UnityEngine.Random.rotation;
                break;
        }
    }

    private void Update()
    {
        AnimateTheParticle();
    }

    private void AnimateTheParticle()
    {
        //Make rotation animation
        if(shieldsAroundVirus != null)
        {
            float rot = Time.deltaTime * rotationSpeed;

            switch (axis)
            {
                default:
                case RotationAxis.All:
                    // Debug.Log("Rotating All");
                    shieldsAroundVirus.transform.Rotate(new Vector3(rot, 0,rot));
                    break;

                case RotationAxis.X:
                    //Debug.Log("Rotating X");
                    shieldsAroundVirus.transform.Rotate(new Vector3(rot, 0f, 0f));
                    break;

                case RotationAxis.Y:
                    //Debug.Log("Rotating Y");
                    shieldsAroundVirus.transform.Rotate(new Vector3(0f, rot, 0f));
                    break;

                case RotationAxis.Z:
                    //Debug.Log("Rotating Z");
                    shieldsAroundVirus.transform.Rotate(new Vector3(0f, 0f, rot));
                    break;

            }
        }
        

        //Make scalation animation
        if (gettingDamage)
            scalationSpeed = 0.01f;
        else
            scalationSpeed = 0.0005f;

        float scaleOfPoint = dirtyPointModel.transform.localScale.x;

        if (growing)
        {
            scaleOfPoint += scalationSpeed;
            if (scaleOfPoint >= 1.664615f)
            {
                growing = false;
                return;
            }
            dirtyPointModel.transform.localScale = new Vector3(scaleOfPoint, scaleOfPoint, scaleOfPoint);
        }
        else
        {
            scaleOfPoint -= scalationSpeed;
            if (scaleOfPoint <= 1f)
            {
                growing = true;
                return;
            }
            dirtyPointModel.transform.localScale = new Vector3(scaleOfPoint, scaleOfPoint, scaleOfPoint);
        }

    }

    public bool restLife(float lifeToRest)
    {
        dirtyPointLifeQuant -= lifeToRest;

        if (dirtyPointLifeQuant <= 0)
        {
            DestruccionDirtyPoint();
            
            return true;
        }
        else
            return false;
    }

    private void DestruccionDirtyPoint()
    {
        SoundEffects_Manager.instancia.PlaySound("dirtyPointDestroyed");

        GameObject.Find("/GameManager").GetComponent<PipesGameManager>().dirtyPointErased();


        //Crear Particulas
        Instantiate(endingParticles, this.transform.position, Quaternion.identity);

        //Destruir el objeto de punto sucio
        Destroy(this.gameObject);
    }
}
