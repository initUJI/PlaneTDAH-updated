using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterParticle : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> dropSounds;

    void Awake()
    {
        Invoke("DestroyTheObject", 1);
        int num = Random.Range(0, 3);
        GetComponent<AudioSource>().clip = dropSounds[num];
        GetComponent<AudioSource>().Play();
    }

    void DestroyTheObject()
    {
        Destroy(this.gameObject);
    }
}
