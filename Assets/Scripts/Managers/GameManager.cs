using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int levelCount = 2;

    private int currentLevel = 1;

    public ERC20TokenContractClient.KeyPair playerAccount;
    public ERC20TokenContractClient.KeyPair enemyAccount;
    public ERC20TokenContractClient erc20;

    public static GameManager Instance {
        get {
            return singletonInstance;
        }
    }
    private static GameManager singletonInstance;

    // Start is called before the first frame update
    void Start()
    {
        if(singletonInstance != null)
        {
            Debug.LogError("Two game managers detected!");
        } else
        {
            singletonInstance = this;
        }
        DontDestroyOnLoad(gameObject);
        erc20 = GetComponent<ERC20TokenContractClient>();
        //StartCoroutine(erc20.DeployAndFillAccounts());
        playerAccount = erc20.getPlayerAccount();
        playerAccount = erc20.GetRandomAccount();
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
