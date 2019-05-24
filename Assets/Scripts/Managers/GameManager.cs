using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int levelCount = 2;

    private int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnStartGameButton()
    {
        currentLevel = 1;
        LoadLevel(currentLevel);
    }

    public void LoadNextLevel()
    {
        LoadLevel(++currentLevel);
    }


    public void RestartCurrentLevel()
    {
        LoadLevel(currentLevel);
    }

    private void LoadLevel(int levelNumber)
    {
        if(currentLevel <= levelCount)
        {
            SceneManager.LoadScene("Level" + levelNumber.ToString());
        } else
        {
            // TODO: sorry no more levels <--- this in case we dont have sea fight
        }
    }
}
