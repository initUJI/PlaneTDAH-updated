using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObject : MonoBehaviour
{
    public float ringSpeed;

    public int puntosSuma;
    bool ringCrossed;
    bool matchEnded;
    public GameManagerScript gameManager;

    public List<AudioClip> sounds;
    public List<Material> iddleMaterials;
    public Material wrongMaterial;
    public Material successMaterial;

    public Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        matchEnded = false;
        correctTheMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject();
        if(!gameManager.hasStarted)
        {
            if (!matchEnded)
            {
                PlayAnimation("dissapearRingAnimation");
                matchEnded = true;
            }
        }
    }

    void MoveObject()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (ringSpeed * Time.deltaTime));
    }

    public void correctTheMaterial()
    {
        GameObject ringObject = transform.GetChild(0).gameObject;
        if(gameManager.mode != null)
        {
            switch(gameManager.mode)
            {
                case "Easy":
                    ringObject.GetComponent<MeshRenderer>().material = iddleMaterials[0];
                    break;
                case "Medium":
                    ringObject.GetComponent<MeshRenderer>().material = iddleMaterials[1];
                    break;
                case "Hard":
                    ringObject.GetComponent<MeshRenderer>().material = iddleMaterials[2];
                    break;
            }
        }
    }

    public void updateSuccessMaterial()
    {
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = successMaterial;
    }

    public void updateWrongMaterial()
    {
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = wrongMaterial;
    }

    public void PlayErrorSound()
    {
        GetComponent<AudioSource>().clip = sounds[1];
        GetComponent<AudioSource>().Play();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().clip = sounds[0];
            GetComponent<AudioSource>().Play();

            PlayAnimation("successAnimation");
            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            gameManager.SumarPuntos(puntosSuma);
        }
    }

    public void PlayAnimation(string animationName)
    {
        anim.Play(animationName, -1, 0f);
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
