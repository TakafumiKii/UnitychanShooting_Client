using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace UnityChanShooting
{
    public class RatioBar : MonoBehaviour
    {
        [SerializeField] Image Bar;
        [SerializeField] Text Percent;


        float _Ratio = 1.0f;
        public float Rate
        {
            get { return _Ratio; }
            set
            {
                float next = (value < 0) ? 0 : (value > 1.0f) ? 1.0f : value;
                if (next != _Ratio) { _Ratio = next; UpdateRatio(); }
            }
        }

        // Use this for initialization
        void Start()
        {
            UpdateRatio();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void UpdateRatio()
        {
            if (Bar != null)
            {
                Bar.fillAmount = Rate;
            }
            if (Percent != null)
            {
                int ratio = (int)Math.Ceiling(Rate * 100.0f);
                Percent.text = $"{ratio}%";
            }
        }
    }
}
