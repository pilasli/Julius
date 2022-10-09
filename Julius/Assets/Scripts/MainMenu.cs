using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("Main_Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // To load Game scene
    public void LoadGame()
    {
        //AudioManager.instance.Stop("Main_Menu");
        Debug.Log("Game is Loading...");
        SceneManager.LoadScene("Game");
    }

    // Exit game to windows
    public void ExitGame()
    {
        Debug.Log("Game is Closing...");
        Application.Quit();
    }
}
