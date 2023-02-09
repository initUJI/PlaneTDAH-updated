using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOfPoints : MonoBehaviour
{
    private masterOfGroupOfPoints parentMaster;
    public List<GameObject> objectivesToPaint;
    public Color colorToPaint;

    public GameObject lastPointOnUse;
    private float scaleOfPoint, speed;
    private bool active;
    public bool completed = false;

    public SpriteRenderer showingSprite;

    private void OnEnable()
    {
        parentMaster = transform.parent.gameObject.GetComponent<masterOfGroupOfPoints>();

        Color nocolor = new Color(0, 0, 0, 0);
        if (colorToPaint == nocolor)
            colorToPaint = Color.yellow;

        lastPointOnUse = objectivesToPaint[0];
        lastPointOnUse.SetActive(true);
        active = false;
        speed = 0.01f;
    }


    // Update is called once per frame
    void Update()
    {
        if(lastPointOnUse != null)
        {
            actualizarAnimacionDeLuz();
        }
    }

    private void actualizarAnimacionDeLuz()
    {
        if(active)
        {
            scaleOfPoint += speed;
            if(scaleOfPoint >= 0.40f)
            {
                active = false;
                return;
            }
            lastPointOnUse.gameObject.GetComponent<Light>().range = scaleOfPoint;
        }
        else
        {
            scaleOfPoint -= speed;
            if(scaleOfPoint <= 0)
            {
                active = true;
                return;
            }
            lastPointOnUse.gameObject.GetComponent<Light>().range = scaleOfPoint;
        }
    }

    public void siguientePunto()
    {
        objectivesToPaint.Remove(lastPointOnUse);
        Destroy(lastPointOnUse);

        if(objectivesToPaint.Count != 0)
        {
            lastPointOnUse = objectivesToPaint[0];
            lastPointOnUse.SetActive(true);
        }
        else
        {
            parentMaster.siguienteGrupoDePuntos();
            completed = true;
            return;
        }
    }

    public void ocultarColor()
    {
        showingSprite.color = Color.black;
        foreach(GameObject obj in objectivesToPaint)
        {
            obj.GetComponent<Light>().color = Color.white;
        }
    }

    public void mostrarColor()
    {
        showingSprite.color = Color.white;
        foreach (GameObject obj in objectivesToPaint)
        {
            obj.GetComponent<Light>().color = colorToPaint;
        }
    }

}
