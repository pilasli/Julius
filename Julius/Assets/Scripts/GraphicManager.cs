using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GraphicManager : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown resolutionDropdown;
    private string fullScreenStr;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("SavedResolution");
        fullScreenStr = PlayerPrefs.GetString("SavedFullscreen");

        // To set resolution on start
        if(resolutionDropdown.value == 0)
        {
            Screen.SetResolution(640, 360, Screen.fullScreen);
        }
        if(resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
        if(resolutionDropdown.value == 2)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }

        // To set fullscreen on start 
        if(fullScreenStr == "true")
        {
            Screen.fullScreen = true;
            fullScreenToggle.isOn = true;
        }
        else if(fullScreenStr == "false")
        {
            Screen.fullScreen = false;
            fullScreenToggle.isOn = false;
        }
        QualitySettings.SetQualityLevel(5); //To set quality as ultra       
    }

    // To change fullscreen status when toggle (in canvas) is changed
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        QualitySettings.SetQualityLevel(5); //To set quality as ultra
        if(isFullScreen)
        {
            PlayerPrefs.SetString("SavedFullscreen", "true");
        }
        else
        {
            PlayerPrefs.SetString("SavedFullscreen", "false");
        }
        PlayerPrefs.Save(); //Save values
    }

    // To change resolution status when dropdown is changed
    public void SetResolution(int resolutionIndex)
    {
        if(resolutionIndex == 0)
        {
            Screen.SetResolution(640, 360, Screen.fullScreen);
        }
        if(resolutionIndex == 1)
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
        if(resolutionIndex == 2)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        PlayerPrefs.SetInt("SavedResolution", resolutionIndex);
        PlayerPrefs.Save(); //Save values
        QualitySettings.SetQualityLevel(5);
    }

}
