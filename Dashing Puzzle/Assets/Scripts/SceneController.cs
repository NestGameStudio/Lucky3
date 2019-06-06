using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public static SceneController Instance { get { return instance; } }

    public AudioMixerSnapshot AudioMixerSnapshot;

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
        StartCoroutine(DelayStart(1f));
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
    IEnumerator DelayStart(float seconds)
    {
        AudioMixerSnapshot.TransitionTo(seconds);
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Test de Levels");
    }
}
