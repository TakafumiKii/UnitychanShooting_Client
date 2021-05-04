using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanShooting
{
    public class ResultScoreUnit : MonoBehaviour
    {
        Text TextValue;
        Text TextUnitPrice;
        Text TextPoint;
        // Use this for initialization
        void Awake()
        {
            Text[] texts = GetComponentsInChildren<Text>();
            foreach(var text in texts)
            {
                switch(text.name)
                {
                case "Value":
                    TextValue = text;
                    break;
                case "UnitPrice":
                    TextUnitPrice = text;
                    break;
                case "Point":
                    TextPoint = text;
                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetParam(int value,int unit,int point)
        {
            if(TextValue != null) { TextValue.text = value.ToString(); }
            if (TextUnitPrice != null) { TextUnitPrice.text = unit.ToString(); }
            if (TextPoint != null) { TextPoint.text = point.ToString(); }
        }
    }
}