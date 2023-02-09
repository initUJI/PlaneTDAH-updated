using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeColliderManager : MonoBehaviour
{
    public Transform masterPipeParent;
    public string nameOfModel;
    public GameObject modelReferenceToCollider;

    public int pipeLifeQuant = 100;
    public Color baseColor, damagedColor;
    private float time = 0, durationToChange = 0.6f;

    private bool isBeingDamaged = false, repeating = false;

    public GameObject ModelReferenceToCollider { get => modelReferenceToCollider; set => modelReferenceToCollider = value; }

    void Start()
    {
        modelReferenceToCollider = masterPipeParent.Find(this.transform.name + "_model").gameObject;
        baseColor = this.GetComponent<Renderer>().material.color;
    }

    public void StartDamageToPipe ()
    {
        //StartCoroutine("ChangeColor");
        isBeingDamaged = true;
    }

    public void FinishDamageToPipe()
    {
        isBeingDamaged = false;
        StartCoroutine("FinishLaserInteraction");
    }
     
    IEnumerator FinishLaserInteraction()
    {
        //Inicializacion de variables cuando se frena el daño
        float timeToGo = 0;
        float duration = 0.2f;

        //Reseteo de las variables que controlan la animación cuando esta siendo dañado
        time = 0;
        repeating = false;

        //Se resetea el color al suyo base propio
        while(timeToGo < 1)
        {
            ModelReferenceToCollider.GetComponent<Renderer>().material.color = Color.Lerp(ModelReferenceToCollider.GetComponent<Renderer>().material.color, baseColor, timeToGo);

            timeToGo += Time.deltaTime / duration;
            yield return null;
        }
    }


    private void Update()
    {
        //Si esta siendo dañado comienza la animacion
        if(isBeingDamaged)
        {
            if(!repeating)
            {
                ModelReferenceToCollider.GetComponent<Renderer>().material.color = Color.Lerp(baseColor, damagedColor, time);

                if (time < 1)
                {
                    time += Time.deltaTime / durationToChange;
                }
                else
                {
                    time = 0;
                    repeating = true;
                }
            }
            else
            {
                ModelReferenceToCollider.GetComponent<Renderer>().material.color = Color.Lerp(damagedColor, baseColor, time);

                if (time < 1)
                {
                    time += Time.deltaTime / durationToChange;
                }
                else
                {
                    time = 0;
                    repeating = false;
                }
            }
        }
    }
 }
