using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isGamePaused = false;

    [SerializeField] private GameObject creditsObject;
    private UIManager _uiManager;
    private LevelChanger _levelChanger;
    [SerializeField] private GameObject pauseMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("The UIManager on GameManager is <null>");
        }
        _levelChanger = GameObject.Find("Level_Changer").GetComponent<LevelChanger>();
        if(_levelChanger == null)
        {
            Debug.LogError("The LevelChanger on GameManager is <null>");
        }
        
        AudioManager.instance.Play("Game");
        CursorLockOn();
    }

    // Update is called once per frame
    void Update()
    {
        //Open or close Pause Menu when conditions are met
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isGamePaused && pauseMenuPanel.activeSelf)
            {
                ResumeGame();
            }
            else if(!isGamePaused)
            {
                PauseGame();
            }
        }
    }

    //Losing game process
    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        isGameOver = true;
        StartCoroutine("GameOverRoutine");
    }

    //Winning game process
    public void WinGame()
    {
        Debug.Log("GameManager::WinGame() Called");
        isGameOver = true;
        _levelChanger.FadeToLevel();
        creditsObject.SetActive(true);
        StartCoroutine("WinGameRoutine");
    }

    //Pause game process
    public void PauseGame()
    {
        CursorLockOff();
        _uiManager.HideInPause();
        pauseMenuPanel.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    //Resume game process
    public void ResumeGame()
    {
        CursorLockOn();
        pauseMenuPanel.SetActive(false);
        _uiManager.ShowOutPause();
        isGamePaused = false;
        Time.timeScale = 1.0f;
    }

    //To go main menu scene
    public void GoMainMenu()
    {
        //AudioManager.instance.Stop("Game");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }

    //Locking cursor when necessary    
    public void CursorLockOn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //Unlocking cursor when necessary
    public void CursorLockOff()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1f);
        _levelChanger.FadeToLevel();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator WinGameRoutine()
    {
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("Main");
    }


}
