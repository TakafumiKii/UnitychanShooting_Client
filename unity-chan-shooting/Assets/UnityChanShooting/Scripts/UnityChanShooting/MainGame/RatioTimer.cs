using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityChanShooting
{
    public class RatioTimer : RatioBar
    {
        [SerializeField] Text ValueText;

        int _ValueMax = 0;
        public int ValueMax
        {
            get { return _ValueMax; }
            set { if (_ValueMax != value) { _ValueMax = value; CalcRatio(); } }
        }

        int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set { if (_Value != value) { _Value = value; CalcRatio(); UpdateText(); } }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void CalcRatio()
        {
            Rate = (_ValueMax > 0) ? (float)_Value / (float)ValueMax : 0;
        }

        void UpdateText()
        {
            if (ValueText != null)
            {
                ValueText.text = Value.ToString();
            }
        }

    }
}