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
        Debug.Log("Entrei 2");
        SceneManager.LoadScene("Game Scene");
    }

    public void CharacterSeletion()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
}
