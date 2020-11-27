using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiocontrol : MonoBehaviour
{
    public GameObject Object ;
    public AudioClip[] music;
    private int i;

    void Awake(){
        AudioSource audio = Object.GetComponent<AudioSource>() ;
        i = Random.Range(0, 5);
        audio.clip = music[i];
        audio.Play();
        // Debug.Log(i);
        // Debug.Log(music[i]);

    }
    void FixedUpdate() 
    {
        if(Input.GetButtonDown("Next"))
        {
            if(i==5) i = 0;
            else i++;
            AudioSource audio = Object.GetComponent<AudioSource>() ;
            // audio.Stop();
            audio.clip = music[i];
            audio.Play();
            // Debug.Log(music[i]);
        }
        if(Input.GetButtonDown("back"))
        {
            if(i==0) i = 5;
            else i--;
            AudioSource audio = Object.GetComponent<AudioSource>() ;
            audio.clip = music[i];
            audio.Play();
            // Debug.Log(music[i]);
        }
    }

    
}
