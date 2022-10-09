using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }
    public Slider musicSlider;
    public Slider soundEffectsSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        musicVolume =  PlayerPrefs.GetFloat("SavedMusic");
        musicSlider.value = musicVolume;
        soundEffectsVolume = PlayerPrefs.GetFloat("SavedSoundEffects");
        soundEffectsSlider.value = soundEffectsVolume;
        AudioManager.instance.UpdateMusicMixerVolume();
        AudioManager.instance.UpdateSoundEffectsMixerVolume();
    }

    // Changing the music mixer value when music slider is changed
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("SavedMusic", musicVolume);
        PlayerPrefs.Save(); //Save values
        //musicSliderText.text = ((int)(value * 100)).ToString(); //To change mixer value decimal numbers if we want
        AudioManager.instance.UpdateMusicMixerVolume();
    }

    // Changing the sound effects mixer value when music slider is changed
    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        PlayerPrefs.SetFloat("SavedSoundEffects", soundEffectsVolume);
        PlayerPrefs.Save(); //Save values
        //soundEffectSliderText.text = ((int)(value * 100)).ToString(); //To change mixer value decimal numbers if we want
        AudioManager.instance.UpdateSoundEffectsMixerVolume();
    }
}
