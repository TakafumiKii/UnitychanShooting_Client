using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanShooting
{
    public class ResultGUIManager : MonoBehaviour
    {
        ResultScoreUnit UnitDestroy;
        ResultScoreUnit UnitTime;
        ResultScoreUnit UnitChain;
        private void Awake()
        {
            ResultScoreUnit[] units = GetComponentsInChildren<ResultScoreUnit>(true);
            foreach (var unit in units)
            {
                switch (unit.name)
                {
                case "Destroy":
                    UnitDestroy = unit;
                    break;
                case "Time":
                    UnitTime = unit;
                    break;
                case "Chain":
                    UnitChain = unit;
                    break;
                }
            }
        }
        // Use this for initialization
        void Start()
        {
//            SetVisible(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetParam(MainGameScore score,float time)
        {
            UnitDestroy.SetParam(1, 0, (int)score.DestroyEnemyPoint);
            int timeValue = (int)Math.Ceiling(time);
            UnitTime.SetParam(timeValue, (int)score.TimeBonusRatio, (int)(timeValue * score.TimeBonusRatio));
            UnitChain.SetParam(score.ChainNumMax,(int)score.ChainBonusRatio, (int)(score.ChainNumMax * score.ChainBonusRatio));
        }
        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}