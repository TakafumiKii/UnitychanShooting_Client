using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace UnityChanShooting
{
    public class EnemyController : MonoBehaviour
    {

        [System.Serializable]
        class AttackParamNoraml
        {
            [SerializeField] internal float ShotInterval = 0.15f;
            [SerializeField] internal float RandomHalfWidth = 0.8f;
        }
        [System.Serializable]
        class AttackParamHeavy
        {
            [SerializeField] internal float ShotInterval = 0.15f;
            [SerializeField] internal float HalfWidth = 1.2f;
            [SerializeField] internal int ShotNum = 5;
            [SerializeField] internal int StepNum = 6;
            [SerializeField] internal float AttackInterval = 2.0f;
        }
        [System.Serializable]
        class AttackParamFast
        {
            [SerializeField] internal float ShotInterval = 0.15f;
            [SerializeField] internal float HalfWidth = 0.8f;
            [SerializeField] internal int ShotNum = 10;
            [SerializeField] internal int StepNum = 6;
            [SerializeField] internal float AttackInterval = 2.0f;
        }
        //[System.Serializable]
        //class AttackParamHeavyRush
        //{
        //    [SerializeField] internal float RandomWide = 10.0f;
        //}

        [System.Serializable]
        class AttackSequence
        {
            [SerializeField] internal float StartPercent = 100.0f;
            [SerializeField] internal float IntervalTime = 2.0f;
            [SerializeField] internal AttackKind Kind = AttackKind.Normal;
        }

        [SerializeField] BulletManager BulletManager;
        [SerializeField] Vector3 ShotPosition = new Vector3(0,0.4f,0.5f);

        [SerializeField] float HitRandomAddForce = 20.0f;

        [SerializeField] AttackSequence[] AtkSequences;
        int AtkSequenceIndex;

        [SerializeField] AttackParamNoraml AtkParamNormal;
        [SerializeField] AttackParamHeavy AtkParamHeavy;
        [SerializeField] AttackParamFast AtkParamFast;
//        [SerializeField] AttackParamHeavyRush AtkParamHeavyRush;

        [System.Serializable]
        class Status
        {
            [SerializeField] internal float HitPointMax = 300.0f;
        }

        [SerializeField] Status InitStatus;
        public float HitPoint { get; private set; }
        public float HitPointMax { get { return InitStatus.HitPointMax; } }
        public float HitPointRate { get { return (HitPointMax > 0)?(HitPoint / HitPointMax) : 0; } }

        public Vector3 ShotTargetPosition { get; private set; }

        enum AttackKind
        {
            Normal,
            Heavy,
            Fast,
            HevyRush,
        }

        float AtkInterval;
        AttackKind AtkKind;
        float NextHPRate;


        PlayerController PlayerCtrl;
        Vector3 ShotMove;

        class ShotParam
        {
            public float Interval;
            public int Step;
            public int Num;
            public void Clear()
            {
                Interval = 0;
                Step = 0;
                Num = 0;
            }
        }
        enum ShotKind
        {
            Normal,
            Heavy,
            Fast,
        }
        ShotParam[] ShotParams;

        enum StateStatus
        {
            Idle,
            Interval,
            Attack,
            End,
        }
        StateStatus State;




        private void Awake()
        {
            ShotTargetPosition = transform.position;// + ShotPosition;
            HitPoint = HitPointMax;

            ShotParams = new ShotParam[Enum.GetNames(typeof(ShotKind)).Length];
            for(int i = 0;i < ShotParams.Length;i++)
            {
                ShotParams[i] = new ShotParam();
            }
        }
        // Use this for initialization
        void Start()
        {
            PlayerCtrl = FindObjectOfType<PlayerController>();
            Debug.Assert(PlayerCtrl != null);

            ShotMove = PlayerCtrl.ShotTargetPosition - ShotTargetPosition;
            ShotMove.x = 0; // 横成分は不要
            ShotMove.y = 0; // 高さは不要

            AtkSequenceIndex = 0;
            SetAttackSequence(AtkSequenceIndex);
        }

        // Update is called once per frame
        void Update()
        {
            switch (State)
            {
            case StateStatus.Idle:
                break;
            case StateStatus.Interval:
                AtkInterval -= Time.deltaTime;
                if (AtkInterval < 0)
                {
                    State = StateStatus.Attack;
                }
                break;
            case StateStatus.Attack:
                float rate = HitPointRate;

                if (rate <= NextHPRate)
                {
                    if(++AtkSequenceIndex < AtkSequences.Length)
                    {
                        SetAttackSequence(AtkSequenceIndex);
                    }
                    else
                    {
                        State = StateStatus.Idle;
                    }                        
                }
                else
                {
                    switch (AtkKind)
                    {
                    case AttackKind.Normal:
                        AttackNormal();
                        break;
                    case AttackKind.Heavy:
                        AttackHevy();
                        break;
                    case AttackKind.Fast:
                        AttackFast();
                        break;
                    case AttackKind.HevyRush:
                        AttackHevyRush();
                        break;
                    default:
                        Debug.Log("No Support AttackKind");
                        break;
                    }
                }
                break;
            case StateStatus.End:
                break;
            }

        }

        // 衝突の瞬間判定
        void OnCollisionEnter(Collision other)
        {
//            Debug.Log(other.gameObject.name + "Enter");
            BulletController bulletController = other.gameObject.GetComponent<BulletController>();
            if(bulletController != null && bulletController.LastHitObject == null)
            {
                Rigidbody body = bulletController.gameObject.GetComponent<Rigidbody>();
                body.AddRelativeForce(new Vector3(Random.Range(-HitRandomAddForce, HitRandomAddForce), Random.Range(-HitRandomAddForce, HitRandomAddForce), 0),ForceMode.Force);
                bulletController.LastHitObject = gameObject;

                HitPoint -= bulletController.DamageValue;
                if(HitPoint < 0)
                {
                    HitPoint = 0;
                }
                //Vector3 pos = Vector3.zero;
                //foreach (var cp in other.contacts)
                //{
                //    pos = cp.point;
                //}
                // ゲームマネージャーに報告
                //MainGameManager.Instance.OnEnemyDamage(this, pos);
                MainGameManager.Instance.OnEnemyDamage(this);
            }
        }

        //// 衝突中の判定
        //void OnCollisionStay(Collision other)
        //{
        //    Debug.Log(other.gameObject.name + "Stay");
        //}

        //// 衝突離脱の判定
        //void OnCollisionExit(Collision other)
        //{
        //    Debug.Log(other.gameObject.name + "Exit");
        //}

        public void Deactivate()
        {
            BulletManager.Deactivate();
            State = StateStatus.End;
        }

        void SetAttackSequence(int index)
        {
            Debug.Assert(0 <= index && index < AtkSequences.Length);

//            for(int i = 0;i < ShotParams.Length;i++)
            foreach(var param in ShotParams)
            {
                param.Clear();
            }

            var seq = AtkSequences[index];
            AtkKind = seq.Kind;
            AtkInterval = seq.IntervalTime;

            int next = index + 1;
            NextHPRate = (next < AtkSequences.Length) ? AtkSequences[next].StartPercent / 100.0f : 0;

            State = (AtkInterval > 0)? StateStatus.Interval : StateStatus.Attack;
        }
        void AttackNormal()
        {
            var param = ShotParams[(int)ShotKind.Normal];

            param.Interval -= Time.deltaTime;
            if (param.Interval < 0)
            {
                ShotNormal();
                param.Num++;
                param.Interval = AtkParamNormal.ShotInterval;
            }
        }
        void ShotNormal()
        {
            Vector3 move = ShotMove;
            move.x += Random.Range(-AtkParamNormal.RandomHalfWidth, AtkParamNormal.RandomHalfWidth);
            BulletManager.ShotBullet((int)ShotKind.Normal, transform.position + transform.rotation * ShotPosition, transform.rotation, move);
        }
        void AttackHevy()
        {
            var param = ShotParams[(int)ShotKind.Heavy];

            param.Interval -= Time.deltaTime;
            if (param.Interval < 0)
            {
                ShotHeavy(param);
                if(param.Step >= AtkParamHeavy.StepNum)
                {
//                    AtkInterval = AtkParamHeavy.AttackInterval;
//                    State = StateStatus.Interval;
                    param.Clear();
                    param.Interval = AtkParamHeavy.AttackInterval;
                }
                else
                {
                    param.Interval = AtkParamHeavy.ShotInterval;
                }
            }
        }
        void ShotHeavy(ShotParam param)
        {
            float width = AtkParamHeavy.HalfWidth * 2.0f;
            Vector3 move = ShotMove;

            int num = AtkParamHeavy.ShotNum;
            Debug.Assert(num >= 3);

            move.x -= AtkParamHeavy.HalfWidth;
            float stride = width / (num - 1);

            if(param.Step % 2 == 1)
            {// 弾を偶数個にする回
                num -= 1;
                move.x += stride / 2.0f;
            }

            for (int i = 0; i < num;i++)
            {
                BulletManager.ShotBullet((int)ShotKind.Heavy, transform.position + transform.rotation * ShotPosition, transform.rotation, move);
                param.Num++;
                move.x += stride;
            }
            param.Step++;
            //            move.x += Random.Range(-AtkParamHeavy.HalfWidth, AtkParamHeavy.HalfWidth);
        }

        void AttackFast()
        {
            var param = ShotParams[(int)ShotKind.Fast];

            param.Interval -= Time.deltaTime;
            if (param.Interval < 0)
            {
                ShotFast(param);
                if (param.Step >= AtkParamFast.StepNum)
                {
                    //AtkInterval = AtkParamFast.AttackInterval;
                    //State = StateStatus.Interval;
                    param.Clear();
                    param.Interval = AtkParamHeavy.AttackInterval;
                }
                else
                {
                    param.Interval = AtkParamFast.ShotInterval;
                }
            }
        }
        void ShotFast(ShotParam param)
        {
            float width = AtkParamHeavy.HalfWidth * 2.0f;
            Vector3 move = ShotMove;

            int num = AtkParamFast.ShotNum;
            Debug.Assert(num >= 3);

            float stride = width / (num - 1);

            if(param.Step % 2 == 0)
            {// 左から開始
                move.x -= AtkParamFast.HalfWidth;
                move.x += stride * param.Num;
            }
            else
            {// 右から開始
                move.x += AtkParamFast.HalfWidth;
                move.x -= stride * param.Num;
            }

            BulletManager.ShotBullet((int)ShotKind.Fast, transform.position + transform.rotation * ShotPosition, transform.rotation, move);
            param.Num++;

            if(param.Num >= AtkParamFast.ShotNum)
            {
                param.Num = 0;
                param.Step++;
            }
        }
        void AttackHevyRush()
        {
            AttackHevy();
            AttackNormal();
//            AttackFast();
        }

        //AtackParamBase GetAttackParam(AttackKind kind)
        //{
        //    switch (kind)
        //    {
        //    case AttackKind.Normal:
        //        return AtkParamNormal;
        //    case AttackKind.Heavy:
        //        return AtkParamHeavy;
        //    case AttackKind.Fast:
        //        return AtkParamFast;
        //    case AttackKind.HevyRush:
        //        return AtkParamHevyRush;
        //    default:
        //        Debug.Log("default error! unknown kind:" + kind);
        //        return default(AtackParamBase);
        //    }
        //}
    }
}