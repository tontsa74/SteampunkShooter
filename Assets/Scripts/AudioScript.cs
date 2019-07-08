﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    AudioSource aSource;

    public bool playing = false;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            timer += Time.deltaTime;

            if (timer >= aSource.clip.length + 1 && !aSource.loop)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlaySound(AudioClip ac, bool looping, float volume)
    {
        aSource = GetComponent<AudioSource>();
        aSource.clip = ac;
        aSource.loop = looping;
        aSource.volume = volume;
        aSource.Play();
        playing = true;
    }

    public void PauseSound()
    {
        aSource.Pause();
        playing = false;
    }
}
