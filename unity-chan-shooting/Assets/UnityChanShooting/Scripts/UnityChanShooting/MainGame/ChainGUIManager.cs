using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityChanShooting
{
    public class ChainGUIManager : MonoBehaviour
    {
        [SerializeField] RatioBar ChainBar;
        [SerializeField] TextValueInt TextHitEnemyNum;
        [SerializeField] TextValueInt TextScratchNum;
        [SerializeField] TextValueInt TextChainNum;

        public int HitEnemyNum { get { return TextHitEnemyNum.Value; } set { TextHitEnemyNum.Value = value; } }
        public int ScratchNum { get { return TextScratchNum.Value; } set { TextScratchNum.Value = value; } }
        public int ChainNum { get { return TextChainNum.Value; } set { TextChainNum.Value = value; } }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetChainRatio(float ratio)
        {
            ChainBar.Rate = ratio;
        }
    }
}