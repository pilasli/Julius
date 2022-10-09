using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

//Auido system to make sounds and sound effects tidy
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private Sound[] sounds;  

    void Awake()
    {
        instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.music:
                s.source.outputAudioMixerGroup = musicMixerGroup;
                break;
                case Sound.AudioTypes.soundEffect:
                s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                break;
            }
        }        
    }
    public void Play (string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if(s == null)
        {
            Debug.LogError("Sound" + clipname + " does not exist!");
            return;
        }
        s.source.Play();
    }

    public void Stop (string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if(s == null)
        {
            Debug.LogError("Sound" + clipname + " does not exist!");
        }
        s.source.Stop();
    }

    public void UpdateMusicMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
    }
    public void UpdateSoundEffectsMixerVolume()
    {
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(AudioOptionsManager.soundEffectsVolume) * 20); 
    }
}
