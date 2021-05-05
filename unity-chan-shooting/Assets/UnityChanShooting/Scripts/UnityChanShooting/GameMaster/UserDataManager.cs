using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FakeServer.Unity;
using FakeServer.Netcode.Scheme;
namespace UnityChanShooting
{
    public class UserDataManager : MonoBehaviour
    {

        NetworkClient Client;

        public UserInfo UserInfo { get { return (Client != null) ? Client.UserInfo : null; } }


        public class GameScore
        {
        }
        private void Awake()
        {
            Client = GetComponent<NetworkClient>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetUserInfo()
        {
            Client.ResetUserInfo();
        }
    }
}