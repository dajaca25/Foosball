using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mirror.Examples.Pong
{
    public class GameLauncher : MonoBehaviour
    {
        public GameObject gameManager;
        public GameObject otherPlayerPausedScreen;
        
        bool serverActive;
        string networkType;

        private IEnumerator coroutine;

        void Update()
        {
            //serverActive = Foosball_NetworkManager.Instance.serverActive;
            networkType = Foosball_NetworkManager.Instance.startNetworkType;

            if (gameManager == null)
            {
                gameManager = Foosball_NetworkManager.Instance.gameObject;
                print("network type: " + networkType);
                if (networkType == "Host")
                {
                    if (!serverActive)
                    {
                        coroutine = StartGame();
                        StartCoroutine(coroutine);
                        print("host!");
                    }
                }
                if (networkType == "Client")
                {
                    //if(serverActive)
                    //{
                    Foosball_NetworkManager.Instance.StartClient();
                        print("client!");
                    //}
                    /*
                    else
                    {
                        NetworkManagerPong.Instance.startNetworkType = "";
                        egSceneManager.Instance.GetComponent<egSceneManager>().LoadScene("eag_MainMenu");
                        print("no network :(");
                    }
                    */
                }
            }
        }




        public void StopHost()
        {
            Foosball_NetworkManager.Instance.StopHost();
        }




        public void StopClient()
        {
            Foosball_NetworkManager.Instance.StopClient();
        }




        public void PauseOtherPlayer(bool isPaused)
        {
            otherPlayerPausedScreen.SetActive(isPaused);
        }



        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(0.1f);
            print("I waited 1 second!");
            Foosball_NetworkManager.Instance.StartNewHost();
        }
    }
}
