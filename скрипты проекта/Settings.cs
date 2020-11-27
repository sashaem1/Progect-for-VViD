using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{
    // public AudioMixer am;
    bool isFullScreen;
    public int q;
    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
    public void Quality(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }
    public void AudioVolume(float sliderValue)
    {
        // am.SetFloat("masterVolume", sliderValue);
    }
}
