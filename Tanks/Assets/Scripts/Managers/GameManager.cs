using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] m_Tanks;

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
        int seconds = Mathf.RoundToInt(m_gameTime);

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
        }
    }

    private void ManageStateGameOver()
    {
        if(Input.GetKeyUp(KeyCode.Return) == true)
        {
            m_gameTime = 0;
            m_GameState = GameState.Playing;

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


}
