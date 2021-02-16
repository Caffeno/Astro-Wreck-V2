using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ScoreDisplay scoreKeeper;


    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = (Time.timeScale + 1) % 2;
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void ResetGame()
    {
        UnpauseGame();
        scoreKeeper.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        UnpauseGame();
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("MainMenu");
    }
}
