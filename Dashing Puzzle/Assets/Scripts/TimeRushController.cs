using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeRushController : MonoBehaviour
{
    public static TimeRushController instance;

    public Slider LittleBar;
    public float SecondsToCompletion = 60;

    private float currentLevel;
    public float currentTime = 0;

    public static TimeRushController Instance { get { return instance; } }

    private void Awake() {

        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        StartCoroutine(FillTheBar());
    }

    // Percentage of player completion within the map
    public void PlayerCompletionPosition()
    {
        currentLevel = ChamberController.Instance.currentChamberNumber + 1;

        LittleBar.value = (float) (currentLevel) / (float)(ChamberController.Instance.ChambersInGame.Length);
    }

    IEnumerator FillTheBar()
    {
        while(currentTime < SecondsToCompletion) {
            currentTime += 1;
            LittleBar.value = currentTime / SecondsToCompletion;
            yield return new WaitForSeconds(1);
        }

        SceneController.Instance.LoadLoseScene();
    }
}