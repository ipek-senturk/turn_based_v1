using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip musicSound;
    public AudioSource musicSource;

    private void Start()
    {
        musicSource.clip = musicSound;
        musicSource.Play();
    }

}
