using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanShooting
{
    public class IconManager : MonoBehaviour
    {
        [SerializeField]
        int IconNum;
        [SerializeField]
        IconUnit TemplateIcon;

        IconUnit[] Icons;
        Stack<IconUnit> inActiveStack = new Stack<IconUnit>();

        private void Awake()
        {
            Debug.Assert(TemplateIcon != null);
            Icons = new IconUnit[IconNum];
            for (int i = 0; i < Icons.Length; i++)
            {
                var icon = Instantiate(TemplateIcon,transform);
                icon.Manager = this;
                icon.gameObject.SetActive(true);
                Icons[i] = icon;
            }
        }

        private void OnDestroy()
        {
            inActiveStack.Clear();
            if(Icons != null)
            {
                for (int i = 0; i < Icons.Length; i++)
                {
                    Destroy(Icons[i]);
                }
                Icons = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            foreach(var bullet in Icons)
            {
                bullet.gameObject.SetActive(false);
                inActiveStack.Push(bullet);
            }
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

        public bool AppearIcon(Vector3 position)
        {
            IconUnit icon = inActiveStack.Pop();

            if (icon != null)
            {
                icon.Appear(position);
                return true;
            }
            return false;
        }

        internal void Push(IconUnit icon)
        {
            icon.gameObject.SetActive(false);
            inActiveStack.Push(icon);
        }
    }
}