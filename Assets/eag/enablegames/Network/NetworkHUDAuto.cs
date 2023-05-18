using System;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkHUDAuto")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NetworkHUDAuto : MonoBehaviour
    {
        public NetworkManager manager;
        [SerializeField] public bool showGUI = true;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;

        // Runtime variable
        bool m_ShowServer;

        void Awake()
        {
            GetNetworkIPs();
            // NetworkClientConnect.Instance.Connect();
            manager = GetComponent<NetworkManager>();
            if (!showGUI)
            {
                //   NetworkClientConnect.Instance.Connect();
                //manager.StartClient();
                //if(GameObject.Find("Player") != null){
                //     GameObject.Find("Player").SetActive(false);
                //  }
            }
            DontDestroyChildOnLoad(this.gameObject);
        }

        public static void DontDestroyChildOnLoad(GameObject child)
        {
            Transform parentTransform = child.transform;

            // If this object doesn't have a parent then its the root transform.
            while (parentTransform.parent != null)
            {
                // Keep going up the chain.
                parentTransform = parentTransform.parent;
            }
            GameObject.DontDestroyOnLoad(parentTransform.gameObject);
        }

        void Update()
        {
            bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);
           if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                        timer++;
//                        print("timer="+ timer);
                if (noConnection)
                {
                        if (timer > 130){
                            timer=0;
                            print("timer:starthost");
                            manager.StartHost();
                        }
}
                else
                {
                        if (timer > 120)
                        {
                            print("timer:stopclient");
//                            manager.StopClient();
                            manager.StopHost();

                        }
                }
            }
            else
            {

//                        timer++;
//                        print("serveractive timer="+ timer);

//                        timer++;
 //                       print("serveractive timer="+ timer);

                if (manager.IsClientConnected())
                {
//                        print("serveractive2 timer="+ timer);

                }else{
                            print("timer:notconnected");
                            timer=0;
                    manager.StopHost();
                }
            }


            if (!showGUI)
                return;

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        manager.StartServer();
                    }
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        manager.StartHost();
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    print("-------------------------------START CLIENT2");

                    manager.StartClient();
                }
            }
            if (NetworkServer.active && manager.IsClientConnected())
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    manager.StopHost();
                }
            }
        }

public static string IPs = "";
public static void GetNetworkIPs()
{
    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                 {
                     if (ni.OperationalStatus == OperationalStatus.Up)
                     {
                         if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                         {
                             foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                             {
//                                if (ip != null)
//                                     print("IP=" + ip.Address);
                                 if (ip != null && ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                 {
                                     IPs += ip.Address + "\n";
                                     print("IP Inter=" + ip.Address);
                                     // My addresses
                                     if (ni.Description.ToLower().Contains("vpn"))
                                     {
                                         //foundVPN = true;
                                     }
                                 }    
                             }
                         }
                     }

                 }
}

    public static string GetLocalIPAddress()
    {
        GetNetworkIPs();
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    float timer = 0f;
    /*
    public string GetLocalIPv4()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(
                    f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString();
        } 
        */
        void OnGUI()
        {
            //print("NM:client=" + manager.client);
            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            const int spacing = 24;

            bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);

            manager.useWebSockets = GUI.Toggle(new Rect(10, 10, 120, 30), manager.useWebSockets, "Use Websockets");
            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                /*
                        timer++;
                        print("timer="+ timer);*/
                if (noConnection)
                {
                    /*
                        if (timer > 130){
                            timer=0;
                            print("timer:starthost");
                            manager.StartHost();
                        }
                        */
                    if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                        {
                            manager.StartHost();
                        }
                        ypos += spacing;
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
                    {
                        print("-------------------------------START CLIENT");
                        manager.StartClient();
                    }

                    manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
                    ypos += spacing;

                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUI.Box(new Rect(xpos, ypos, 200, 25), "(  WebGL cannot be server  )");
                        ypos += spacing;
                    }
                    else
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                        {
                            manager.StartServer();
                        }
                        ypos += spacing;
                    }
                }
                else
                {
                    GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..");
                    ypos += spacing;
                    /*
                        if (timer > 120)
                        {
                            print("timer:stopclient");
//                            manager.StopClient();
                            manager.StopHost();

                        }
                        */
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt"))
                    {
                    
                        manager.StopClient();
                    }
                }
            }
            else
            {

//                        timer++;
//                        print("serveractive timer="+ timer);
                if (NetworkServer.active)
                {
                        print("serveractive1 timer="+ timer);
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
                    {
                        manager.StopHost();
                    }
                    ypos += spacing;
                    string serverMsg = "Server: addresses :\n" + IPs + "port=" + manager.networkPort;
                    if (manager.useWebSockets)
                    {
                        serverMsg += " (Using WebSockets)";
                    }
                    GUI.Label(new Rect(xpos, ypos, 300, 200), serverMsg);
                    ypos += spacing;
                }
                if (manager.IsClientConnected())
                {
                        print("serveractive2 timer="+ timer);
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }else{
                    /*
                            print("timer:notconnected");
                            timer=0;
                    manager.StopHost();*/
                }
            }

            if (manager.IsClientConnected() && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (/*NetworkServer.active ||*/ manager.IsClientConnected())
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
            {
                ypos += 10;

                if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                    return;
                }

                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                            {
                                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                            manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                            ypos += spacing;

                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            for (int i = 0; i < manager.matches.Count; i++)
                            {
                                var match = manager.matches[i];
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Back to Match Menu"))
                            {
                                manager.matches = null;
                            }
                            ypos += spacing;
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                    {
                        m_ShowServer = !m_ShowServer;
                    }
                    if (m_ShowServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;
                }
            }
        }
    }
}
#endif //ENABLE_UNET