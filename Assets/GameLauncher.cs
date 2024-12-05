using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mirror
{
    public class GameLauncher : MonoBehaviour
    {
        public GameObject gameManager;

        
        public GameObject otherPlayerPausedScreen;
        public GameObject waitingScreen;

        bool serverActive;
        string networkType;

        private IEnumerator coroutine;

        bool isPanelOff;


        void Update()
        {
            if (NetworkServer.active)
            {
                if (Foosball_NetworkManager.Instance.startNetworkType == "Host")
                {
                    if (NetworkManager.singleton.GetComponent<Foosball_NetworkManager>().numPlayers == 2)
                    {
                        print("there are 2 players now, and I'm the host! Let's start the game.");
                        StartCoroutine(TogglePanel());
                    }
                }
                else /*if(Foosball_NetworkManager.Instance.isHost == false)*/
                {
                    if (!isPanelOff && waitingScreen.activeSelf)
                    {
                        print("there are 2 players now, and I'm the client! Let's start the game.");
                        StartCoroutine(TogglePanel());
                    }
                }
            }



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
                    print("starting network client...");
                    Foosball_NetworkManager.Instance.StartClient();
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



        IEnumerator TogglePanel()
        {
            //if (!isServer) yield break;

            yield return new WaitForSeconds(2);
            waitingScreen.SetActive(false);
            yield return new WaitForSeconds(2);
            isPanelOff = false;
            StopCoroutine(TogglePanel());
        }
    }
}
