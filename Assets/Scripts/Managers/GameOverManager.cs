using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public int MiningTime = 10;
        public GameObject lootingOnChainScreen;
        public Button continueButton;
        public GameObject menuScreen;

        public static GameOverManager instance
        {
            get
            { 
                return singletonInstance;
            }
        }

        private void Start()
        {
            if (singletonInstance == null)
            {
                singletonInstance = this;
            } else
            {
                Debug.LogError("Detected multiple instances accross scenes");
            }
            lootingOnChainScreen.gameObject.SetActive(false);
        }
        private static GameOverManager singletonInstance;

        private GameManager gameManager = GameManager.Instance;

        public void FinishGame(bool victory)
        {
            StartCoroutine(TransferFunds(victory ? gameManager.playerAccount : gameManager.enemyAccount,
                victory ? gameManager.enemyAccount : gameManager.playerAccount));
        }

        private IEnumerator TransferFunds(ERC20TokenContractClient.KeyPair winer, ERC20TokenContractClient.KeyPair looser)
        {
            int treasureAmount = Random.RandomRange(100, 1000);
            lootingOnChainScreen.gameObject.SetActive(true);
            yield return gameManager.erc20.Transfer(looser.priv, looser.pub, winer.pub, treasureAmount);
            yield return new WaitForSeconds(MiningTime);
            lootingOnChainScreen.gameObject.SetActive(false);
            menuScreen.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);
            yield return 13;
                
        }

    }
}