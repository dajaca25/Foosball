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
        //public Transform leftRacketSpawn;
        //public Transform rightRacketSpawn;
        GameObject ball;

        public float player1Input;
        public float player2Input;

        public bool isHost = false;

        //Accessed by the "GameLauncher" script.
        public string startNetworkType;
        public static Foosball_NetworkManager Instance;

        public GameObject player1;
        public GameObject player2;
        public GameObject playerController;

        public bool player1Paused;
        public bool player2Paused;



        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Foosball_NetworkManager>();
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
                //player1Input = player1.GetComponent<Player>().horizontalInput;
                print("player1Input: " + player1Input);
            }
            if (player2 != null)
            {
                //player2Input = player2.GetComponent<Player>().verticalInput;
                print("player2Input: " + player2Input);
            }
        }



        public void SetHost(bool host)
        {
            isHost = host;
        }
        


        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("Player Joined");
            // add player at correct spawn position
            //Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, Vector3.zero, quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);

            // spawn ball if two players
            if (numPlayers == 2)
            {
                // ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                // NetworkServer.Spawn(ball);
            }
        }



        public GameObject SpawnStars(Vector3 position, Transform parent)
        {
            GameObject newStar = Instantiate(spawnPrefabs[0], position, quaternion.identity, parent);
            NetworkServer.Spawn(newStar);
            return newStar;
        }
        


        public void SetNetworkType(string newType)
        {
            startNetworkType = newType;
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
            print("start host check 1");
            StartHost();
        }
    }
}