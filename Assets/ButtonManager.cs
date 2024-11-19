using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Examples.Pong;
using UnityEngine.UI;

namespace Mirror.Discovery
{
    public class ButtonManager : MonoBehaviour
    {
        public static ButtonManager _instance = null;
        
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        public string gameId;
        [SerializeField]
        private InputField inputField;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ButtonManager>();
            }
            if(this == _instance)
                DontDestroyOnLoad(_instance);
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void StringFromNetworkResponse(ServerResponse info)
        {
            gameId = info.EndPoint.Address.ToString();
            inputField.text = gameId;
            Foosball_NetworkManager.Instance.networkAddress = gameId;
        }



        public void ReadStringInput(string s)
        {
            gameId = s;
            Foosball_NetworkManager.Instance.networkAddress = gameId;
        }



        public void PongManagerSetNewType(string newType)
        {
            Foosball_NetworkManager.Instance.SetNetworkType(newType);
        }
    }
}