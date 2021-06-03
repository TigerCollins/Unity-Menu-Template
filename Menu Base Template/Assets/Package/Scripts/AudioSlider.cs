using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField]
    private Slider audioSlider;
    [SerializeField]
    private AudioMixer slidersMixerGroup;
    [SerializeField]
    private PlayerData playerProfile;
    [SerializeField]
    private MenuAudioManager menuAudioManager;

    [Header("Local Values")]
    public float volume;
    [SerializeField]
    private AudioType audioType;

    public enum AudioType
    {
        MasterVolume,
        MusicVolume,
        GameVolume,
        MenuVolume
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Sets volume display fromm saved data
        switch (audioType)
        {
            case AudioType.MasterVolume:
                volume = playerProfile.masterVolume;
                break;
            case AudioType.MusicVolume:
                volume = playerProfile.musicVolume;
                break;
            case AudioType.GameVolume:
                volume = playerProfile.gameVolume;
                break;
            case AudioType.MenuVolume:
                volume = playerProfile.menuVolume;
                break;
            default:
                break;
        }


        UpdateVolumeDisplay(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeUp()
    {
        if (volume + 1 <= 10)
        {
            volume++;
            UpdateVolumeDisplay(true);
        }

    }

    public void VolumeDown()
    {
        if(volume -1 >= 0)
        {
            volume -=1;
            UpdateVolumeDisplay(true);
        }
       
    }

    public void UpdateVolumeDisplay(bool buttonPress)
    {
        //-------ALLOWS THE SLIDER TO CHANGE IF NOT A BUTTON PRESS CHANGE-----
        if(buttonPress)
        {
            audioSlider.value = volume;
        }
       
        else
        {
            volume = audioSlider.value;
        }

        //----------DETERMINES WHAT SLIDER IS FOR WHAT------
        switch (audioType)
        {
            case AudioType.MasterVolume:
                menuAudioManager.MasterVolume(volume);
                break;
            case AudioType.MusicVolume:
                menuAudioManager.MusicVolume(volume);
                break;
            case AudioType.GameVolume:
                menuAudioManager.GameVolume(volume);
                break;
            case AudioType.MenuVolume:
                menuAudioManager.MenuVolume(volume);
                break;
            default:
                break;
        }

    }


}
