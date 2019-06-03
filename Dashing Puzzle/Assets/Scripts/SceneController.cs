using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public static SceneController Instance { get { return instance; } }


    private void Awake() {

        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene("Defeat");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("Victory");
    }

    public void StartGame()
    { 
        SceneManager.LoadScene("Test de Levels");
    }

    public void CharacterSeletion()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    /*public void TesteDeLevels()
    {
        SceneManager.LoadScene("Test de Levels");
    }*/

    public void HighScore()
    {
        SceneManager.LoadScene("Highscore");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
