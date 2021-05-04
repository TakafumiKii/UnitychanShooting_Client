using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanShooting
{
    public class TitleBG : MonoBehaviour
    {
        [SerializeField] Vector2 MoveSpeed = new Vector2(100,100);
        [SerializeField] Vector2 RepeatDistance = new Vector2(100, 100);

        Vector2 MovePos;
        Vector2 OriginPos;
        Image ImageBG;

        CanvasScaler Scaler;

        private void Awake()
        {
            ImageBG = GetComponent<Image>();
            OriginPos = transform.localPosition;
            Scaler = GetComponent<CanvasScaler>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 move = MoveSpeed * Time.deltaTime;
            MovePos += move;

            if (MovePos.x > RepeatDistance.x)
            {
                MovePos.x -= RepeatDistance.x;
            }
            if (MovePos.y > RepeatDistance.y)
            {
                MovePos.y -= RepeatDistance.y;
            }
            transform.localPosition = OriginPos - MovePos;
        }

        public void OnClick_GameStart()
        {
        }
    }
}
