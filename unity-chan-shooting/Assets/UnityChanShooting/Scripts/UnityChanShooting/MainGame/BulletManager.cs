using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] BulletUnit[] BulletUnits;


        private void Awake()
        {
            foreach(var unit in BulletUnits)
            {
                unit.Initialize(this);
            }
        }

        private void OnDestroy()
        {
            foreach (var unit in BulletUnits)
            {
                unit.Terminate();
            }
        }

        // Use this for initialization
        void Start()
        {
            Deactivate();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //public bool ShotBullet(Transform transform)
        //{
        //    BulletController bullet = inActiveStack.Pop();

        //    if (bullet != null)
        //    {
        //        bullet.Shot(transform.position, transform.rotation,Vector3.zero);
        //        return true;
        //    }
        //    return false;
        //}
        public void Deactivate()
        {
            foreach (var unit in BulletUnits)
            {
                unit.Deactivate();
            }
        }
        public bool ShotBullet(int index, Vector3 position, Quaternion rotation,Vector3 move)
        {
            Debug.Assert(index < BulletUnits.Length);
            return BulletUnits[index].ShotBullet(position, rotation, move);
        }
    }
}