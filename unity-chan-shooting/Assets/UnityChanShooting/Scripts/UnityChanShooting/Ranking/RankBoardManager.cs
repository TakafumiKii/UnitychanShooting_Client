using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FakeServer.Network.Information;
namespace UnityChanShooting
{
    public class RankBoardManager : MonoBehaviour
    {

        [SerializeField] RankBoard TemplateObject = null;
        [SerializeField] RankBoard SelfRankBord = null;
        //    [SerializeField] int BoardNum = 10;
        [SerializeField] Vector3 StartPosition = new Vector3(768, 220, 0);
        [SerializeField] Vector3 EndPosition = new Vector3(0, 20, 0);
        [SerializeField] float Stride = 40;
        [SerializeField] float MoveTime = 0.5f;
        [SerializeField] float Deray = 0.2f;

        //    RankBoardBehaviour[] RankBoards;
        //    List<RankBoard> RankBoards = new List<RankBoard>();
        RankBoard[] RankBoards;



        public enum StateStatus
        {
            None,
            Ready,
            Action,
            Idle,
        }

        public StateStatus State { get; private set; } = StateStatus.None;

        private void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {
        }
        private void OnDestroy()
        {
            Clear();
        }
        //private void OnEnable()
        //{
        //    StartAction();
        //}

        public void Setup(UserRankInfo[] rankInfos, UserRankInfo userRank)
        {
            Clear();
            //        BoardNum = rankInfos.Length;
            bool isCheckSelect = true;
            RankBoards = new RankBoard[rankInfos.Length];
            for (int i = 0; i < RankBoards.Length; i++)
            {
                RankBoard board = Instantiate(TemplateObject, transform);
                RankBoards[i] = board;

                board.SetDisplayParam(rankInfos[i]);

                if (isCheckSelect && userRank.IsSame(rankInfos[i]))
                {
                    board.SetSelectMode(true);
                    isCheckSelect = false;
                }
            }
            SelfRankBord.SetDisplayParam(userRank);
            SelfRankBord.SetVisibleRank(false);
            SelfRankBord.gameObject.SetActive(true);
            State = StateStatus.Ready;
        }
        private void Clear()
        {
            if (RankBoards != null)
            {
                foreach (var obj in RankBoards)
                {
                    Destroy(obj);
                }
                RankBoards = null;
            }
        }

        public void StartAction()
        {
            Vector3 start, end;
            for (int i = 0; i < RankBoards.Length; i++)
            {
                var board = RankBoards[i];

                start = StartPosition;
                end = EndPosition;

                float stride = i * Stride;
                start.y += stride;
                end.y += stride;

                int no = (RankBoards.Length - (i + 1));
                board.SetMoveParam(start, end, MoveTime, no * Deray);
            }
            State = StateStatus.Action;
        }

        // Update is called once per frame
        void Update()
        {

            switch (State)
            {
            case StateStatus.Action:
                Debug.Assert(RankBoards.Length > 0);
                if (RankBoards[0].State == RankBoard.StateStatus.Idle)
                {
                    SelfRankBord.SetVisibleRank(true);
                    State = StateStatus.Idle;
                }
                break;
            }
        }
    }
}