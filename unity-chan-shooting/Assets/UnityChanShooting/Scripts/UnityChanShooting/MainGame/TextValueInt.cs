using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityChanShooting
{
    public class TextValueInt : MonoBehaviour
    {

        //    [SerializeField] Text ValueText;
        [SerializeField] int Min = 0;
        [SerializeField] int Max = 999;

        Text ValueText;
        int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set { int next = (value < Min) ? Min : (value > Max) ? Max : value; if (_Value != next) { _Value = next; UpdateText(); } }
        }
        void Awake()
        {
            ValueText = GetComponent<Text>();
            Debug.Assert(ValueText != null);
        }
        // Use this for initialization
        void Start()
        {
            UpdateText();
        }

        // Update is called once per frame
        void Update()
        {

        }
        void UpdateText()
        {
            ValueText.text = Value.ToString();
        }
    }
}
