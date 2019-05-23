using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public int MiningTime = 10;
        public static GameOverManager instance
        {
            get
            {
                if(singletonInstance == null)
                {
                    singletonInstance = new GameOverManager();
                }
                return singletonInstance;
            }
        }
        private static GameOverManager singletonInstance;

        public void FinishGame(bool victory)
        {
            StartCoroutine(TransferFunds());
        }

        private IEnumerator TransferFunds()
        {
            // TODO: add winner/looser wallets as function params
            // TODO: show popup: You are harvesting treasure from blockchain
            yield return new WaitForSeconds(MiningTime);
            yield return 13;
                
        }

    }
}