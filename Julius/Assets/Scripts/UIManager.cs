using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] livesImage;
    [SerializeField] private Sprite fullLive;
    [SerializeField] private Sprite emptyLive;
    [SerializeField] private GameObject livesPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Text gameOverText;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager on UIManager is <null>");
        }

        gameOverText.gameObject.SetActive(false);
    }

    // To change active heart numbers on canvas when player gets liveUpgrade
    public void UpdateNumOfLives(int numOfLives)
    {
        for(int i= 0; i < numOfLives; i++)
        {
            livesImage[i].gameObject.SetActive(true);
            livesImage[i].sprite = fullLive;
        }
    }

    // To change hearts status to full or empty
    public void UpdateLives(int lives, int numOfLives)
    {
        for(int i = 0 ; i < lives; i++)
        {
            livesImage[i].sprite = fullLive;
        }
        for(int i = lives ; i < numOfLives; i++)
        {
            livesImage[i].sprite = emptyLive;
        }
    }

    // Game over process
    public void GameOverSequence()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    // To hide canvas gameplay objects (like hearts, points etc.)
    public void HideInPause()
    {
        livesPanel.gameObject.SetActive(false);
    }

    // To show out canvas gameplay objects (like hearts, points etc.)
    public void ShowOutPause()
    {
        livesPanel.gameObject.SetActive(true);
    }


}
