using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public static SceneController Instance { get { return instance; } }

    public AudioMixer mixer;

    public GameObject VolumeSlider;

    public AudioMixerSnapshot snapshot;

    public bool fadeOut = false;
    float volumeDown = 1;
    //public float seconds = 1;

    private void Awake() {
        Debug.Log(VolumeSlider.GetComponent<Slider>().value - volumeDown);
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
        
    }/*
    public void Update()
    {
        if (fadeOut)
        {

            volumeDown -= 100f * Time.deltaTime;

            mixer.SetFloat("Volume", VolumeSlider.GetComponent<Slider>().value - volumeDown);
            if (VolumeSlider.GetComponent<Slider>().value - volumeDown == -50)
            {
                fadeOut = false;
                mixer.SetFloat("Volume", VolumeSlider.GetComponent<VolumeSlider>().SlideLastValue);
            }
        }
    }*/

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
        StartCoroutine(DelayStart(1.5f));
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
    public  IEnumerator DelayStart(float seconds)
    {
        fadeOut = true;
        Destroy(VolumeSlider);
        snapshot.TransitionTo(seconds - 0.3f);
        yield return new WaitForSeconds(seconds);       
        SceneManager.LoadScene("Test de Levels");
    }
}
