using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveNewHighscore : MonoBehaviour
{
    // vinculado ao painel
    // ele vai receber o tempo restante no qual ojogador zerou o jogo
    // se ele for menor que todos os tempos registrados ele só mostra a tabela de highscore
    // se ele for maior que algum deles ele substitui a célula
    // se ele for igual substitui pra cima 

    // primeiro ele mostra todos os recordes salvos
    // depois ele avalia como descrito acima
    // depois ele salva ou não


    /// <summary>
    /// percorre de 0 avaliando o highscore
    /// </summary>
    /// 

    [SerializeField] public GameObject[] cells;

    private int currentHighscore;

    private bool Up = false;
    private bool Down = false;
    private bool EnterRight = false;
    private bool Left = false;

    private void Awake()
    {
        // Pega o valor de tempo restante do Timerushcontroller
        currentHighscore = (int)TimeRushController.Instance.SecondsToCompletion - (int)TimeRushController.Instance.currentTime;

        // se é a primeira vez em que se zera o jogo, cria um novo campo de save de Highscore
        if (!PlayerPrefs.HasKey("NumHighscores")) {
            PlayerPrefs.SetInt("NumHighscores", 1);
        } else {

            // avalia se o highscore do cara é equiparável com algum já salvo e mostra os highscores existentes
            for (int i=1; i <= 5; i++) {

                // avalia se existe um dado salvo naquele slot
                if (PlayerPrefs.HasKey ("Name" + i))  {

                    // compara para ver se o highscore atual é maior que o salvo do slot
                    if (PlayerPrefs.GetInt("Highscore" + i) < currentHighscore) {
                        // escreve por cima da celula e atualiza as restantes para baixo
                        activateCellToWrite(i);
                        break;
                    }

                } else {

                    // É a primeira vez que está escrevendo o recorde
                    if (i == 1) {
                        activateCellToWrite(i);
                    }
                    break;

                }

            }
        }

        displayHighscores();

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) { 
            Up = true;
        } else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            Down = true;
        } else if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
            EnterRight = true;
        } else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)){
            Left = true;
        }
    }

    // Mostra os recordes no painel
    private void displayHighscores() {

        Debug.Log("GRR");

        for (int i=0; i<=5; i++) {

            if (PlayerPrefs.HasKey("Name" + i + 1))
            {
                if (!cells[i].activeInHierarchy) {
                    cells[i].SetActive(true);
                }

                cells[i].transform.Find("Time").GetComponent<Text>().text = PlayerPrefs.GetInt("Highscore" + i + 1).ToString() + " s";
                cells[i].transform.Find("FirstLetter").Find("firstLetterText").GetComponent<Text>().text = PlayerPrefs.GetString("name" + i + 1).ToCharArray()[0].ToString();
                cells[i].transform.Find("SecondLetter").Find("SecondLetterText").GetComponent<Text>().text = PlayerPrefs.GetString("name" + i + 1).ToCharArray()[0].ToString();
                cells[i].transform.Find("ThirdLetter").Find("ThirdLetterText").GetComponent<Text>().text = PlayerPrefs.GetString("name" + i + 1).ToCharArray()[0].ToString();
            }
            break;
        }

    }

    // Começa a escrever por cima do valor da célula
    private void activateCellToWrite(int numCell) {

        // Atualiza o restante das celulas para baixo
        updateLastCells(numCell);

        // Escreve o highscore 
        cells[numCell - 1].transform.Find("Time").GetComponent<Text>().text = currentHighscore.ToString();
        cells[numCell - 1].transform.Find("FirstLetter").Find("firstLetterText").GetComponent<Text>().text = 'A'.ToString();
        cells[numCell - 1].transform.Find("SecondLetter").Find("SecondLetterText").GetComponent<Text>().text = 'A'.ToString();
        cells[numCell - 1].transform.Find("ThirdLetter").Find("ThirdLetterText").GetComponent<Text>().text = 'A'.ToString();

        // Permite alterar as três primeiras letras
        StartCoroutine(WriteInCell(numCell));
    }

    // coloca os valores das celulas anteriores abaixo do valor do novo recorde
    private void updateLastCells(int lastWrittencell) {

        string cellName = "";
        int cellHighscore = 0;
        int highscoreAux;
        string nameAux;

        if (PlayerPrefs.HasKey("Name" + lastWrittencell)) {
            cellName = PlayerPrefs.GetString("Name" + lastWrittencell);
            cellHighscore = PlayerPrefs.GetInt("Highscore" + lastWrittencell);
        }

        for (int i = lastWrittencell + 1; i <= 5; i++)  {

            // avalia se tem outro score em baixo
            if (PlayerPrefs.HasKey("Name" + i)) {
                nameAux = PlayerPrefs.GetString("Name" + i);
                highscoreAux = PlayerPrefs.GetInt("Highscore" + i);

                // existia um valor antes da celula ser escrita
                if (cellName != "")  {
                    PlayerPrefs.SetString("Name" + i, cellName);
                    PlayerPrefs.SetInt("Highscore" + i, cellHighscore);
                } else { break; }

            } else  {
                break;
            }
        }
    }

    // Assim que o jogador terminar de escrever a terceira letra, ele chama essa função para salvar o highscore
    private void SaveRecord(string name, int highscore) {
        PlayerPrefs.SetString("Name" + "NumHighscores", name);
        PlayerPrefs.SetInt("Highscore" + "NumHighscores", highscore);

        PlayerPrefs.Save();
    }

    // ativa as letras para serem mudadas uma a uma
    IEnumerator WriteInCell(int numcell) {

        char[] name = new char[3];

        char currentTextLetter = 'A';
        int letter = 1;

        foreach(Transform button in cells[numcell - 1].transform.Find("FirstLetter").GetComponentInChildren<Transform>(true)) {
            if (button.GetComponent<Button>()) {
                button.gameObject.SetActive(true);
            }
        }

        // simulate update
        while (true) {

            if (Up)
            {
                Up = false;

                // loop entra A e Z maiúsculos
                if (currentTextLetter == 'Z') {
                    currentTextLetter = 'A';
                } else {
                    currentTextLetter = (char)(((int)currentTextLetter) + 1);
                }

                if (letter == 1) {
                    cells[numcell - 1].transform.Find("FirstLetter").Find("firstLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                } else if (letter == 2) {
                    cells[numcell - 1].transform.Find("SecondLetter").Find("SecondLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                } else if (letter == 3) {
                    cells[numcell - 1].transform.Find("ThirdLetter").Find("ThirdLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                }

            } else if (Down) {

                Down = false;
                // loop entra A e Z maiúsculos
                if (currentTextLetter == 'A') {
                    currentTextLetter = 'Z';
                } else {
                    currentTextLetter = (char)(((int)currentTextLetter) - 1);
                }

                if (letter == 1) {
                    cells[numcell - 1].transform.Find("FirstLetter").Find("firstLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                } else if (letter == 2) {
                    cells[numcell - 1].transform.Find("SecondLetter").Find("SecondLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                } else if (letter == 3) {
                    cells[numcell - 1].transform.Find("ThirdLetter").Find("ThirdLetterText").GetComponent<Text>().text = (currentTextLetter).ToString();
                }

            } else if (EnterRight) {

                EnterRight = false;
                letter += 1;
                if (letter <= 3) {
                    name[letter - 1] = currentTextLetter;
                    currentTextLetter = name[letter - 1];
                }

                // Liga os botões que trocam a letra
                if (letter == 2) {

                    foreach (Transform button in cells[numcell - 1].transform.Find("FirstLetter").GetComponentInChildren<Transform>(true)) {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(false);
                    }
                    foreach (Transform button in cells[numcell - 1].transform.Find("SecondLetter").GetComponentInChildren<Transform>(true)) {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(true);
                    }
                } else if (letter == 3) {

                    foreach (Transform button in cells[numcell - 1].transform.Find("SecondLetter").GetComponentInChildren<Transform>(true))  {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(false);
                    } foreach (Transform button in cells[numcell - 1].transform.Find("ThirdLetter").GetComponentInChildren<Transform>(true)) {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(true);
                    }
                } else if (letter == 4) {
                    foreach (Transform button in cells[numcell - 1].transform.Find("ThirdLetter").GetComponentInChildren<Transform>(true)) {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(false);
                    }
                    break;
                }

            } else if (Left) {

                Left = false;
                if (letter > 1) {
                    letter -= 1;
                    name[letter - 1] = currentTextLetter;
                    currentTextLetter = name[letter - 1];
                }

                // Liga os botões que trocam a letra
                if (letter == 1)
                {
                    foreach (Transform button in cells[numcell - 1].transform.Find("FirstLetter").GetComponentInChildren<Transform>(true))
                    {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(true);
                    }
                    foreach (Transform button in cells[numcell - 1].transform.Find("SecondLetter").GetComponentInChildren<Transform>(true))  {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(false);
                    }
                }
                else if (letter == 2)
                {

                    foreach (Transform button in cells[numcell - 1].transform.Find("SecondLetter").GetComponentInChildren<Transform>(true))
                    {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(true);
                    }
                    foreach (Transform button in cells[numcell - 1].transform.Find("ThirdLetter").GetComponentInChildren<Transform>(true))
                    {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(false);
                    }
                }
                else if (letter == 3)
                {
                    foreach (Transform button in cells[numcell - 1].transform.Find("ThirdLetter").GetComponentInChildren<Transform>(true)) {
                        if (button.GetComponent<Button>())
                            button.gameObject.SetActive(true);
                    }
                }
            }

            yield return new WaitForEndOfFrame();

        }

        SaveRecord(name.ToString(), currentHighscore); 

    }
}
