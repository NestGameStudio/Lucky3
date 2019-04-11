using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextCamera : MonoBehaviour
{
    [SerializeField] GameObject[] cameraPos;
    [SerializeField] Text textLevel;
    int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        updateText();
        counter = 0;
        
        this.transform.position = cameraPos[counter].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(counter < 2)
            {
                counter++;
            }            
            this.transform.position = cameraPos[counter].transform.position;
            updateText();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {            
            this.transform.position = cameraPos[counter - 1].transform.position;
            counter--;
            updateText();
        }

    }

    private void updateText()
    {
        textLevel.text = "Level " + (counter+1);
    }
}
