using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects_Manager : MonoBehaviour
{
    public AudioClip dirtyPointDestroyed_Sound;
    public AudioClip playerFail_Sound;
    public AudioClip error_Sound;
    public AudioClip deselect_Sound;
    public static SoundEffects_Manager instancia;

    AudioSource soundManager;

    private void Awake()
    {
        soundManager = GetComponent<AudioSource>();
        
        if (instancia == null)
        {
            DontDestroyOnLoad(gameObject);
            instancia = this;
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
        
    }

    public void PlaySound(string sound_Code)
    {
        switch (sound_Code)
        {
            case "dirtyPointDestroyed":
                soundManager.PlayOneShot(dirtyPointDestroyed_Sound);
                break;
            case "playerFail":
                soundManager.PlayOneShot(playerFail_Sound);
                break;
            case "Error":
                soundManager.PlayOneShot(error_Sound);
                break;
            case "Deselect":
                soundManager.PlayOneShot(deselect_Sound);
                break;
        }
    }
}
