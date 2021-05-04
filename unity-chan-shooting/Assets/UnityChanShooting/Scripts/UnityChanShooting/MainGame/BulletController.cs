using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    public class BulletController : MonoBehaviour {

        [SerializeField] float ArrivalTime = 3;
        [SerializeField] float LifeTime = 5;
        [SerializeField] float Damage = 1;


        internal BulletUnit Unit { get; set; }

        public float DamageValue { get { return Damage; } }

        public GameObject LastHitObject { get; set; }

        Vector3 Speed;
        //Vector3 Target;
        //bool IsMove = false;
        float _LifeTime;

        Rigidbody _Rigidbody;

        private void Awake()
        {
            Debug.Assert(Unit != null);
            Debug.Assert(ArrivalTime > 0);
            _Rigidbody = GetComponent<Rigidbody>();
        }

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            float delta = Time.deltaTime;
            _LifeTime -= delta;
            if (_LifeTime <= 0)
            {
                Unit.PushBullet(this);
            }
            //if(IsMove)
            //      {

            //      }
//            transform.Translate(Speed * delta);
        }

        //public void SetTarget(Vector3 target)
        //{
        //    Target = target;
        //    IsMove = true;
        //}
        //internal void Shot(Transform trans)
        //{
        //    Shot(trans.position,trans.rotation, Vector3.zero);
        //}
        internal void Shot(Vector3 position,Quaternion rotation, Vector3 move)
        {
            transform.position = position;
            transform.rotation = rotation;

            LastHitObject = null;
            _LifeTime = LifeTime;

            //            _Rigidbody.velocity = Vector3.zero;
            _Rigidbody.velocity = move / ArrivalTime;
            _Rigidbody.angularVelocity = Vector3.zero;
            _Rigidbody.ResetInertiaTensor();
            //            _Rigidbody.AddForce(Speed);
            gameObject.SetActive(true);
        }



    }
}
