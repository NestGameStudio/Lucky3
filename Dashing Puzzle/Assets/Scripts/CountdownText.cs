using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    public GameObject gameManager;
    public int SecondsToCompletion = 60; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        //currentTime / SecondsToCompletion
        //float time = gameManager.GetComponent<TimeRushController>().LittleBar.value - gameManager.GetComponent<TimeRushController>().currentTime;
        float time = SecondsToCompletion - gameManager.GetComponent<TimeRushController>().currentTime;
        gameObject.GetComponent<Text>().text = (time).ToString();
    }
}
