using Unity.Mathematics;
using UnityEngine;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Mirror
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]

    public class Foosball_NetworkManager : NetworkManager
    {
        public static Foosball_NetworkManager _instance;

        public bool levelFinish = false;

        public GameObject ball;

        public float player1Input;
        public float player2Input;

        public GameObject player1;
        public GameObject player2;
        public GameObject playerController;

        //Accessed by the "GameLauncher" script.
        public string startNetworkType;

        public bool player1Paused;
        public bool player2Paused;



        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // add player at correct spawn position
            string controlOrientation = numPlayers == 0 ? "Horizontal" : "Vertical";
            GameObject player = Instantiate(playerPrefab, this.transform.position, this.transform.rotation);
            player.GetComponent<Player>().controlDirection = controlOrientation;

            //Set the player1 and player2 object variables in this script.
            if (player.GetComponent<Player>().controlDirection == "Horizontal")
            {
                player1 = player;
            }
            if (player.GetComponent<Player>().controlDirection == "Vertical")
            {
                player2 = player;
            }

            NetworkServer.AddPlayerForConnection(conn, player);

            print("numPlayers: " + numPlayers);

            // spawn ball if two players
            if (numPlayers == 2)
            {
                //ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                //NetworkServer.Spawn(ball);
            }
        }



        public void SetNetworkType(string newType)
        {
            startNetworkType = newType;
        }


        
        public static Foosball_NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Foosball_NetworkManager>();
                    if (_instance == null)
                    {
                        // still no GameManager present, raise awareness:
                        Debug.LogError("An instance of type Foosball_NetworkManager is needed in the scene, but there is none!");
                    }
                }
                return _instance;
            }
        }
        

        
        private void Awake()
        {
            if (Instance == null)
            {
                _instance = FindObjectOfType<Foosball_NetworkManager>();
            }
            if(this == Instance)
                DontDestroyOnLoad(Instance);
            else
            {
                Destroy(this.gameObject);
            }
        }
        


        public void FixedUpdate()
        {
            if (player1 != null)
            {
                player1Input = player1.GetComponent<Player>().horizontalInput;
                print("horizontalInput: " + player1Input);
            }
            if (player2 != null)
            {
                player2Input = player2.GetComponent<Player>().verticalInput;
                print("verticalInput: " + player2Input);
            }
        }



        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }



        public void StartNewHost()
        {
            //if (!serverActive)
            //{
            print("starting network host...");
                StartHost();
            //}
        }
    }
}