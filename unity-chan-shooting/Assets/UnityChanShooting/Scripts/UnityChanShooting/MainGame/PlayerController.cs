using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float MoveSpeed = 1.5f;
        [SerializeField] float MoveSpeedShooting = 0.5f;

        [SerializeField] BulletManager BulletManager;

        [SerializeField] float ShotInterval = 0.15f;
        [SerializeField] Vector3 ShotPosition = new Vector3(0, 0.5f, 0.5f);


        [System.Serializable]
        class Status
        {
            [SerializeField] internal float HitPointMax = 10.0f;
        }
        enum ShotKind
        {
            Normal,
        }

        [SerializeField] Status InitialStatus;
        public float HitPoint { get; private set; }
        public float HitPointMax { get { return InitialStatus.HitPointMax; } }
        public float HitPointRatio { get { return (HitPointMax > 0) ? (HitPoint / HitPointMax) : 0; } }

        public Vector3 ShotTargetPosition { get; private set; }

        EnemyController EnemyCtrl;
        float _ShotInterval = 0;


        Collider ScratchTarget;

        Vector3 ShotMove;
        // Use this for initialization

        private void Awake()
        {
            ShotTargetPosition = transform.position;// + ShotPosition;
            HitPoint = HitPointMax;
        }

        void Start()
        {
            EnemyCtrl = FindObjectOfType<EnemyController>();
            Debug.Assert(EnemyCtrl != null);

            ShotMove = EnemyCtrl.ShotTargetPosition - ShotTargetPosition;
            ShotMove.x = 0; // 横成分は不要
            ShotMove.y = 0; // 高さは不要
        }

        // Update is called once per frame
        void Update()
        {
            _ShotInterval -= Time.deltaTime;
        }

        public void Move(Vector3 target)
        {
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        public void MoveShooting(Vector3 target)
        {
            float step = MoveSpeedShooting * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        public void Shot()
        {
            if (_ShotInterval <= 0)
            {
                BulletManager.ShotBullet((int)ShotKind.Normal,transform.position + ShotPosition, transform.rotation, ShotMove);
                _ShotInterval = ShotInterval;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
//            Debug.Log(other.gameObject.name + "Trigger Enter");
            ScratchTarget = other;
        }
        private void OnTriggerExit(Collider other)
        {
            if(ScratchTarget != null)
            {
                Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
                
                //  掠り報告
                // ゲームマネージャーに報告
                MainGameManager.Instance.OnPlayerScratch(this, hitPos); 
            }
        }
        // 衝突の瞬間判定
        void OnCollisionEnter(Collision other)
        {
//            Debug.Log(other.gameObject.name + "Enter");

            if(ScratchTarget == other.collider)
            {
                ScratchTarget = null;
            }
            BulletController bulletController = other.gameObject.GetComponent<BulletController>();
            if (bulletController != null && bulletController.LastHitObject == null)
            {
                bulletController.LastHitObject = gameObject;
                HitPoint -= bulletController.DamageValue;
                if (HitPoint < 0)
                {
                    HitPoint = 0;
                }
                Vector3 pos = Vector3.zero;
                foreach(var cp in other.contacts)
                {
                    pos = cp.point;
                }
                // ゲームマネージャーに報告
                MainGameManager.Instance.OnPlayerDamage(this, pos);
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
            var input = GetComponent<PlayerInput>();
            input.enabled = false;
        }
    }
}