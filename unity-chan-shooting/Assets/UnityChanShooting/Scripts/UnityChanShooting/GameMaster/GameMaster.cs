using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using FakeServer.Unity;
namespace UnityChanShooting
{
    public class GameMaster : Utility.SingletonMonoBehaviour<GameMaster>
    {
        public enum LoadSceneList
        {
            Title,
            MainGame,
        }
        public enum AddSceneList
        {
            Ranking,
        }

        LogTextWriter LogWriter = new LogTextWriter();

        protected override void Awake()
        {
            if(CheckInstance())
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        private void OnEnable()
        {
            LogWriter.SetEnable(true);

        }
        private void OnDisable()
        {
            LogWriter.SetEnable(false);

        }
        // Use this for initialization
        //void Start()
        //{
        //    DontDestroyOnLoad(this);
        //}

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
        }

        public void LoadScene(LoadSceneList scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
        public AsyncOperation LoadSceneAsync(LoadSceneList scene)
        {
            return SceneManager.LoadSceneAsync(scene.ToString());
        }
        public void AddScene(AddSceneList scene)
        {
            SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
        }
        public AsyncOperation AddSceneAsync(AddSceneList scene)
        {
            return SceneManager.LoadSceneAsync(scene.ToString(),LoadSceneMode.Additive);
        }

    }
}