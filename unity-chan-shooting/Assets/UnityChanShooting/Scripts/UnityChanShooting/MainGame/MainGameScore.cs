using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityChanShooting
{
    public class MainGameScore : MonoBehaviour
    {
        [SerializeField] float HitEnemyPoint = 100.0f;
        [SerializeField] float HitEnemyChainRatio = 0.1f;

        [SerializeField] float ScratchPoint = 1000.0f;
        [SerializeField] float ScratchChainRatio = 0.5f;


        [SerializeField] public float DestroyEnemyPoint = 100000.0f;
        [SerializeField] public float TimeBonusRatio = 1000.0f;
        [SerializeField] public float ChainBonusRatio = 500.0f;

        [SerializeField] float AddChainHitEnemy = 10.0f;
        [SerializeField] float AddChainScratch = 100.0f;
        [SerializeField] float SubChainPerSecond = 50.0f;
        [SerializeField] float ChainValueMax = 100.0f;

        [SerializeField] ChainGUIManager ChainGUIManager;

        //        [SerializeField] UserGUIManager UserGUIManager;


        MainGameGUIManager GUIManager;

        float _Point;
        public float Point { get { return _Point; } private set { _Point = value; if (GUIManager != null) { GUIManager.SetPlayerScore((int)value); } } }

        int _HitEnemyChain;
        public int HitEnemyChain { get { return _HitEnemyChain; } private set { _HitEnemyChain = value; UpdateHitEnemyNum(); } }
        public int HitEnemyChainMax { get; private set; }

        int _ScratchChain;
        public int ScratchChain { get { return _ScratchChain; } private set { _ScratchChain = value; UpdateScratchNum(); } }
        public int ScratchChainMax { get; private set; }

        int _ChainNum;
        public int ChainNum { get { return _ChainNum; } private set { if (_ChainNum != value) { _ChainNum = value; if (ChainNumMax < ChainNum) { ChainNumMax = ChainNum; } UpdateChainNum(); } } }
        public int ChainNumMax { get; private set; }

        float _LeftChainValue;
        public float LeftChainValue
        {
            get { return _LeftChainValue; }
            private set
            {
                float next = (value < 0) ? 0 : (value > ChainValueMax) ? ChainValueMax : value;
                if (_LeftChainValue != next)
                {
                    _LeftChainValue = next;
                    UpdateChainRatio();
                }
            }
        }

        //        float LeftTime;

        private void Awake()
        {
            GUIManager = GetComponent<MainGameGUIManager>();
            Debug.Assert(ChainGUIManager != null);
        }
        void Start()
        {
            Point = 0;
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            if (LeftChainValue > 0)
            {
                LeftChainValue -= SubChainPerSecond * delta;
                if (LeftChainValue <= 0)
                {
                    HitEnemyChain = 0;
                    ScratchChain = 0;
                }
            }
        }

        public void Clear()
        {
            Point = 0;

            HitEnemyChain = 0;
            HitEnemyChainMax = 0;

            ScratchChain = 0;
            ScratchChainMax = 0;

            ChainNum = 0;
            ChainNumMax = 0;
        }
        public void ClearChain()
        {
            LeftChainValue = 0;
            HitEnemyChain = 0;
            ScratchChain = 0;
        }

        public void AddScratch()
        {
            if(++ScratchChain > ScratchChainMax)
            {
                ScratchChainMax = ScratchChain;
            }

            Point += ScratchPoint * (1 + (ScratchChain - 1) * ScratchChainRatio);
            LeftChainValue += AddChainScratch;
        }

        public void AddHitEnemy()
        {
            if (++HitEnemyChain > HitEnemyChainMax)
            {
                HitEnemyChainMax = HitEnemyChain;
            }

            Point += HitEnemyPoint * ( 1 + (HitEnemyChain - 1) * HitEnemyChainRatio);
            LeftChainValue += AddChainHitEnemy;
        }
        public void AddCrearPoint(float time)
        {
            float addPoint = DestroyEnemyPoint + (int)Math.Ceiling(time) * TimeBonusRatio + ChainNumMax * ChainBonusRatio;
            Point += addPoint;
        }

        void UpdateChainRatio()
        {
            ChainGUIManager.SetChainRatio(LeftChainValue / ChainValueMax);
        }
        void UpdateHitEnemyNum()
        {
            ChainGUIManager.HitEnemyNum = HitEnemyChain;
            ChainNum = HitEnemyChain + ScratchChain;
        }
        void UpdateScratchNum()
        {
            ChainGUIManager.ScratchNum = ScratchChain;
            ChainNum = HitEnemyChain + ScratchChain;
        }
        void UpdateChainNum()
        {
            ChainGUIManager.ChainNum = ChainNum;
        }
    }

}

