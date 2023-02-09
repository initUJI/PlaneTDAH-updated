using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSoundScript : MonoBehaviour
{
    AudioSource objectAudio;

    // Start is called before the first frame update
    void Awake()
    {
        objectAudio.Play();
        Invoke("DestroyTheObject", 1);
    }

    void DestroyTheObject()
    {
        Destroy(this.gameObject);
    }
}
