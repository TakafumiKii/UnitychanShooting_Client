using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityChanShooting
{
    public class MainGameManager : Utility.SingletonMonoBehaviour<MainGameManager> // MonoBehaviour //  : Utility.ManagerBaseMonoBehaviour
    {
        [SerializeField] float GameTime = 120.0f;
        [SerializeField] ResultGUIManager ResultGUI;
        [SerializeField] IconManager ScratchIconManager;
        [SerializeField] IconManager HitIconManager;
        [SerializeField] IconManager DamageIconManager;



        float StepTime;

        enum GameState
        {
            Initialize,
            Tutorial,
            Ready,
            Main,
            Result,
            Ranking,
            End,
        }
        GameState State;

        MainGameGUIManager GUIManager;
        MainGameScore GameScore;
        PlayerInput PlayerInput;
        PlayerController PlayerCtrl;
        EnemyController EnemyCtrl;

        bool IsGameClear;
        bool IsGameOver;
        float IntervalTime;

        protected override void Awake()
        {
            base.Awake();
            GUIManager = GetComponent<MainGameGUIManager>();
            GameScore = GetComponent<MainGameScore>();

//            ResultGUI = FindObjectOfType<ResultGUIManager>();
            Debug.Assert(ResultGUI != null);
            ResultGUI.SetVisible(true);

            PlayerCtrl = FindObjectOfType<PlayerController>();
            EnemyCtrl = FindObjectOfType<EnemyController>();

            Debug.Assert(PlayerCtrl != null);
            Debug.Assert(EnemyCtrl != null);
        }

        // Use this for initialization
        void Start()
        {
            ResultGUI.SetVisible(false);

            State = GameState.Ready;
            PlayerInput = FindObjectOfType<PlayerInput>();
            PlayerInput.enabled = false;

            IntervalTime = GameTime;
            IsGameClear = false;
            IsGameOver = false;

            GUIManager.InitializeTimer((int)GameTime);
        }

        // Update is called once per frame
        void Update()
        {
            switch (State)
            {
            case GameState.Initialize:

                State = GameState.Ready;
                break;
            case GameState.Tutorial:
                State = GameState.Main;
                break;
            case GameState.Ready:
                PlayerInput.enabled = true;
                State = GameState.Main;
                break;
            case GameState.Main:


                IntervalTime -= Time.deltaTime;
                GUIManager.UpdateGameTimer((int)Math.Ceiling(IntervalTime));
                if (IntervalTime < 0)
                {
//                    State = GameState.Result;
                    TimeOver();
                }
                else if(IsGameClear)
                {
                    GameClear();
                }
                else if (IsGameOver)
                {
                    GameOver();
                }
                break;
            case GameState.Result:
                IntervalTime -= Time.deltaTime;
                if (IntervalTime < 0)
                {
                    GUIManager.SetVisibleTimeUp(false);
                    GUIManager.SetVisibleGameOver(false);
                    ResultGUI.SetVisible(false);
                    GameMaster.Instance.AddScene(GameMaster.AddSceneList.Ranking);
                    State = GameState.Ranking;
                }
                break;
            case GameState.Ranking:
                break;
            case GameState.End:
            default:
                break;
            }
        }

        void TimeOver()
        {
            GUIManager.SetVisibleTimeUp(true);

            //            PlayerInput.enabled = false;
            PlayerCtrl.Deactivate();
            EnemyCtrl.Deactivate();
            IntervalTime = 3.0f;
            State = GameState.Result;
        }
        void GameOver()
        {
            GUIManager.SetVisibleGameOver(true);

//            PlayerInput.enabled = false;
            PlayerCtrl.Deactivate();
            EnemyCtrl.Deactivate();
            IntervalTime = 3.0f;
            State = GameState.Result;
        }
        void GameClear()
        {
            GameScore.AddCrearPoint(IntervalTime);
            ResultGUI.SetParam(GameScore, IntervalTime);
            ResultGUI.SetVisible(true);

            PlayerCtrl.Deactivate();
            EnemyCtrl.Deactivate();

            //            PlayerInput.enabled = false;
            IntervalTime = 4.0f;
            State = GameState.Result;
        }


        public void OnEnemyDamage(EnemyController enemyCtrl)//, Vector3 position)
        {
            GUIManager.UpdateEnemyHP(enemyCtrl);
            GameScore.AddHitEnemy();

            //Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
            //HitIconManager.AppearIcon(screenPos);
            
            if (enemyCtrl.HitPoint <= 0)
            {
                IsGameClear = true;
            }
        }
        public void OnPlayerDamage(PlayerController playerCtrl, Vector3 position)
        {
            GUIManager.UpdatePlayerHP(playerCtrl);
            GameScore.ClearChain();

            Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
            DamageIconManager.AppearIcon(screenPos);

            if (playerCtrl.HitPoint <= 0)
            {
                IsGameOver = true;
            }
        }
        public void OnPlayerScratch(PlayerController playerCtrl,Vector3 position)
        {
            GameScore.AddScratch();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
            ScratchIconManager.AppearIcon(screenPos);
        }
    }
}

