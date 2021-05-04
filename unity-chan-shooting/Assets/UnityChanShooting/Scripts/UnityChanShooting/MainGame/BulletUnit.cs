using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    [System.Serializable]
    public class BulletUnit
    {
        [SerializeField] int BulletNum;
        [SerializeField] BulletController TemplateBullet;

        BulletController[] Bullets;
        Stack<BulletController> inActiveStack = new Stack<BulletController>();

        public void Initialize(BulletManager manager)
        {
            Debug.Assert(TemplateBullet != null);
            Bullets = new BulletController[BulletNum];
            for (int i = 0; i < Bullets.Length; i++)
            {
                var bullet = Object.Instantiate(TemplateBullet, manager.transform);
                bullet.Unit = this;
                bullet.gameObject.SetActive(true);
                Bullets[i] = bullet;
            }
        }

        public void Terminate()
        {
            inActiveStack.Clear();
            if (Bullets != null)
            {
                for (int i = 0; i < Bullets.Length; i++)
                {
                    Object.Destroy(Bullets[i]);
                }
                Bullets = null;
            }
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
            inActiveStack.Clear();
            foreach (var bullet in Bullets)
            {
                bullet.gameObject.SetActive(false);
                inActiveStack.Push(bullet);
            }
        }
        public bool ShotBullet(Vector3 position, Quaternion rotation, Vector3 move)
        {
            if(inActiveStack.Count == 0)
            {
                Debug.Log("empty bullet");
                return false;
            }
            BulletController bullet = inActiveStack.Pop();
            Debug.Assert(bullet != null);

            bullet.Shot(position, rotation, move);
            return true;
        }

        internal void PushBullet(BulletController bullet)
        {
            bullet.gameObject.SetActive(false);
            inActiveStack.Push(bullet);
        }
    }
}