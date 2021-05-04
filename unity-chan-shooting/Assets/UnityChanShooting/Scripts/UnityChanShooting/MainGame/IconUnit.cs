using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    public class IconUnit : MonoBehaviour
    {
        [SerializeField] float LifeTime = 1;

        internal IconManager Manager { get;  set; }

        float _LifeTime;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            _LifeTime -= delta;
            if (_LifeTime <= 0)
            {
                Manager.Push(this);
            }
            //if(IsMove)
            //      {

            //      }
            //            transform.Translate(Speed * delta);
        }

        internal void Appear(Vector3 position)
        {
            transform.position = position;
            _LifeTime = LifeTime;
            gameObject.SetActive(true);
        }

    }
}