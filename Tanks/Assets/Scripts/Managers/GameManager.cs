using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores;
    
    // Reference to the overlay Text to display winning text, etc
    public Text m_MessageText;
    public Text m_TimerText;
    public Text m_TimeToBeatText;

    public GameObject m_HighScorePanel;
    public Text m_HighScoresText;

    public Button m_NewGameButton;
    public Button m_HighScoresButton;

    private int seconds;

    public GameObject[] m_Tanks;

    ArrayList m_gameTimeList = new ArrayList();

    private float m_gameTime = 0f;
    public float GameTime { get { return m_gameTime; } }

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    private void Awake()
    {
        m_GameState = GameState.Start;
    }

    private void Start()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_MessageText.text = "Get Ready";
        m_TimeToBeatText.text = "";

        //m_HighScorePanel.gameObject.SetActive(false);
        //m_NewGameButton.gameObject.SetActive(false);
        //m_HighScoresButton.gameObject.SetActive(false);

        DisplayButtons(false);
    }

    private void Update()
    {
        ManageGameState();
    }

    private void ManageGameState()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                ManageStateStart();
                break;
            case GameState.Playing:
                ManageStatePlaying();
                break;
            case GameState.GameOver:
                ManageStateGameOver();
                break;
            default:
                break;
        }
    }

    private void ManageStateStart()
    {
        if(Input.GetKeyUp(KeyCode.Return) == true)
        {
            m_TimerText.gameObject.SetActive(true);
            m_MessageText.text = "";
            m_TimeToBeatText.text = "";

            m_GameState = GameState.Playing;

            for(int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].SetActive(true);
            }
        }
    }

    private void ManageStatePlaying()
    {
        bool isGameOver = false;

        m_gameTime += Time.deltaTime;
        seconds = Mathf.RoundToInt(m_gameTime);

        m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

        if (OneTankLeft() == true)
        {
            isGameOver = true;
        }
        else if (IsPlayerDead() == true)
        {
            isGameOver = true;
        }

        if(isGameOver == true)
        {
            m_GameState = GameState.GameOver;
            m_TimerText.gameObject.SetActive(false);


            m_gameTimeList.Add(seconds);

            //m_NewGameButton.gameObject.SetActive(true);
            //m_HighScoresButton.gameObject.SetActive(true);

            DisplayButtons(true);

            if (IsPlayerDead() == true)
            {
                m_MessageText.text = "TRY AGAIN";
            }
            else
            {
                m_MessageText.text = "WINNER!";

                // Save the score
                m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
                m_HighScores.SaveScoresToFile();
            }

            int timeToBeat = FindTimeToBeat();

            m_TimeToBeatText.text = "Time To Beat: " + string.Format("{0:D2}:{1:D2}", (timeToBeat / 60), (timeToBeat % 60));
        }
    }

    private void ManageStateGameOver()
    {
        if(Input.GetKeyUp(KeyCode.Return) == true)
        {
            m_gameTime = 0;
            m_GameState = GameState.Playing;

            m_MessageText.text = "";
            m_TimeToBeatText.text = "";
            m_TimerText.gameObject.SetActive(true);

            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].SetActive(true);
            }
        }
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if(m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }

        return numTanksLeft <= 1;
    }

    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if(m_Tanks[i].activeSelf == false)
            {
                if(m_Tanks[i].tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private int FindTimeToBeat()
    {
        int timeToBeat = int.MaxValue;

        foreach (int time in m_gameTimeList)
        {
            if(time < timeToBeat)
            {
                timeToBeat = time;
            }
        }

        return timeToBeat;
    }

    private void DisplayButtons(bool toggle)
    {
        m_NewGameButton.gameObject.SetActive(toggle);
        m_HighScoresButton.gameObject.SetActive(toggle);

        if(toggle == false)
        {
            m_HighScorePanel.SetActive(false);
        }
    }

    public void OnNewGame()
    {
        DisplayButtons(false);
    // m_HighScorePanel.SetActive(false);

        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_MessageText.text = "";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(true);
        }
    }

    public void OnHighScores()
    {
        m_MessageText.text = "";
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);

        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n", (seconds/60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }

    public void OnBackButton()
    {
        m_HighScorePanel.SetActive(false);
    }
}