using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanShooting
{
    public class MainGameGUIManager : MonoBehaviour
    {
        [SerializeField] RatioTimer GameTimer;
        [SerializeField] Text TimeUp;
        [SerializeField] Text GameOver;
        
        [SerializeField] RatioBar PlayerHP;
        [SerializeField] RatioBar EnemyHP;

        [SerializeField] Text Point;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdatePlayerHP(PlayerController player)
        {
            PlayerHP.Rate = player.HitPointRatio;
        }
        public void UpdateEnemyHP(EnemyController enemy)
        {
            EnemyHP.Rate = enemy.HitPointRate;
        }
        public void InitializeTimer(int time)
        {
            GameTimer.ValueMax = time;
            GameTimer.Value = time;
        }
        public void UpdateGameTimer(int time)
        {
            if(GameTimer.Value != time)
            {
                GameTimer.Value = time;
            }
        }

        public void SetVisibleTimeUp(bool isVisible)
        {
            TimeUp.gameObject.SetActive(isVisible);
        }
        public void SetVisibleGameOver(bool isVisible)
        {
            GameOver.gameObject.SetActive(isVisible);
        }

        public void SetPlayerScore(int point)
        {
            Point.text = point.ToString();
        }
    }
}