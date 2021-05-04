using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FakeServer.Network.Information;
namespace UnityChanShooting
{
    public class RankBoard : MonoBehaviour
    {

        [SerializeField] Color NormalColor = new Color(1, 1, 1);
        [SerializeField] Color SelectColor = new Color(1, 1, 1);

        Vector3 StartPos;
        Vector3 EndPos;
        Vector3 Move;
        float MoveTimeMax;
        float MoveTime;
        float Delay;


        Text[] TextComponents;
        Text TextRank;

        public enum StateStatus
        {
            None,
            Ready,
            Delay,
            Move,
            Idle,
        }

        public StateStatus State { get; private set; } = StateStatus.None;

        public void SetDisplayParam(UserRankInfo info)
        {
            FindComponents();
            Debug.Assert(TextComponents != null);
            foreach (var obj in TextComponents)
            {
                switch (obj.name)
                {
                case "Rank":
                    obj.text = (info.Rank + 1).ToString();
                    TextRank = obj;
                    break;
                case "Point":
                    obj.text = info.Point.ToString();
                    break;
                case "Name":
                    obj.text = info.Name;
                    break;
                }
            }
            State = StateStatus.Ready;
        }
        public void SetSelectMode(bool isSelect)
        {
            FindComponents();
            Debug.Assert(TextComponents != null);
            Color color = (isSelect) ? SelectColor : NormalColor;
            foreach (var obj in TextComponents)
            {
                obj.color = color;
            }
        }
        public void SetVisibleRank(bool visibleRank)
        {
            if (TextRank == null)
            {
                Debug.Assert(TextComponents != null);
                foreach (var obj in TextComponents)
                {
                    if (obj.name == "Rank")
                    {
                        TextRank = obj;
                        break;
                    }
                }
            }
            Debug.Assert(TextRank != null);
            TextRank.gameObject.SetActive(visibleRank);
        }

        public void SetMoveParam(Vector3 start, Vector3 end, float time, float delay)
        {
            StartPos = start;
            EndPos = end;
            Move = end - start;

            Delay = delay;

            MoveTime = 0;
            MoveTimeMax = time;

            transform.localPosition = start;
            gameObject.SetActive(true);

            if (Delay > 0)
            {
                State = StateStatus.Delay;
            }
            else
            {
                State = StateStatus.Move;
            }
        }

        void FindComponents()
        {
            if (TextComponents == null)
            {
                TextComponents = GetComponentsInChildren<Text>(true);
            }
        }

        private void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch (State)
            {
            case StateStatus.Delay:
                Delay -= Time.deltaTime;
                if (Delay < 0)
                {
                    if (MoveTimeMax > 0)
                    {
                        State = StateStatus.Move;
                    }
                    else
                    {
                        State = StateStatus.Idle;
                    }
                }
                break;
            case StateStatus.Move:
                MoveTime += Time.deltaTime;

                if (MoveTime < MoveTimeMax)
                {
                    float delta = MoveTime / MoveTimeMax;
                    Vector3 pos = StartPos + Move * delta;
                    transform.localPosition = pos;
                }
                else
                {
                    transform.localPosition = EndPos;
                    State = StateStatus.Idle;
                }
                break;
            case StateStatus.Idle:
                break;
            default:
                break;
            }

        }
    }
}