using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextCamera : MonoBehaviour
{
    [SerializeField] GameObject[] cameraPos;
    [SerializeField] GameObject[] players;

    [SerializeField] Text textLevel;
    int counter;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i+1].SetActive(false);
        }
        counter = 0;       
        
        this.transform.position = cameraPos[counter].transform.position;

        updateText();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(counter < (cameraPos.Length-1))
            {
                players[counter].SetActive(false);
                counter++;
                players[counter].SetActive(true);
                this.transform.position = cameraPos[counter].transform.position;
                updateText();
            }         
                
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (counter > 0)
            {
                players[counter].SetActive(false);
                this.transform.position = cameraPos[counter - 1].transform.position;
                counter--;
                players[counter].SetActive(true);
                updateText();
            }
            
        }

    }

    private void updateText()
    {
        textLevel.text = "Level " + (counter+1);
    }
}
