using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRushController : MonoBehaviour
{
    public static TimeRushController instance;


    public Image LittleBar;

    private int currentLevel;

    // Preencher a barrinha de tempo
    // Pegar a pocentagem da posicao do player nos levels, atualizar por level se possivel atualizar no update
    // pegar numero de levels e trabalhar a completude deles em porcentagem, se possivel ver a 
    // distancia do player da porta que leva ao final do jogo e ver a porcentagem
    // dele conforme a aproximacao dessa porta
    // pegar numero de levels
    // pegar posicao do player

    // tempo dinamico, quanto mais rapido ele avanca, mais rapido o tempo avanca

    public static TimeRushController Instance { get { return instance; } }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = ChamberController.Instance.currentChamberNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
