using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuAudioManager : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField]
    private PlayerData playerProfile;

    [Header("Audio Mixer")]
    [SerializeField]
    private AudioMixer masterVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMenuSound(AudioClip selectedClip)
    {

    }

    public void MasterVolume(float volume)
    {
        float volumeValue = volume;

        if (volumeValue == 0)
        {
            masterVolume.SetFloat("Master Volume", -80);
        }

        else
        {
            masterVolume.SetFloat("Master Volume", -40f + (volumeValue * 100 / 2.25f));
        }

        playerProfile.masterVolume = volume;
    }

    public void GameVolume(float volume)
    {
        float volumeValue = volume;

        if (volumeValue == 0)
        {
            masterVolume.SetFloat("Game Volume", -80);
        }

        else
        {
            masterVolume.SetFloat("Game Volume", -40f + (volumeValue * 100 / 2.25f));
        }
        playerProfile.gameVolume = volume;
    }

    public void MusicVolume(float volume)
    {
        float volumeValue = volume;
        if (volumeValue == 0)
        {
            masterVolume.SetFloat("Music Volume", -80);
        }

        else
        {
            masterVolume.SetFloat("Music Volume", -40f + (volumeValue * 100 / 2.25f));
        }
        playerProfile.musicVolume = volume;
    }

    public void MenuVolume(float volume)
    {
        float volumeValue = volume;
        if (volumeValue == 0)
        {
            masterVolume.SetFloat("Menu Volume", -80);

        }

        else
        {
            masterVolume.SetFloat("Menu Volume", -40f + (volumeValue * 100 / 2.25f));

        }
        playerProfile.menuVolume = volume;
    }


}
