using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    [SerializeField]
    private AudioMixer audioMixer;


    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
           
    }

   
  
}
