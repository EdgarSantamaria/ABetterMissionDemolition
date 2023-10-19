using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundClip;

    void Awake()
    {
        //Check if there is already an instance of AudioManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy the new AudioManager
            Destroy(gameObject);
        }
        //Make sure that the AudioManager is not destroyed between scenes
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        //Play the sound
        soundClip.PlayOneShot(clip);
    }
}
