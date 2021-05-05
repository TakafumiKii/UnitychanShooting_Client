using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FakeServer.Netcode.Scheme;
using FakeServer.Unity;

namespace UnityChanShooting
{
    public class RankingManager : MonoBehaviour
    {
        [SerializeField] Camera TargetCamera;
        enum GameState
        {
            None,
            Initialize,
            Retry,
            Animate,
            Idle,
            End
        }
        GameState State = GameState.None;
        NetworkClient Client;
        DialogBoxManager DialogMan;

        // 表示用にデータ参照を残しておく
        UserRankInfo SelfRankInfo { get; set; } 
        UserRankInfo[] RankInfos { get; set; }

        RankBoardManager RankBoardMan;

        int Point;
        private void Awake()
        {
            var camera = GameObject.FindGameObjectWithTag("MainCamera");
            if(camera != null && TargetCamera != null)
            {
                TargetCamera.gameObject.SetActive(false);
            }
            RankBoardMan = GetComponentInChildren<RankBoardManager>(true);
            Debug.Assert(RankBoardMan != null);
        }

        // Use this for initialization
        void Start()
        {
            Client = GameMaster.Instance.GetComponent<NetworkClient>();
            DialogMan = GameMaster.Instance.GetComponent<DialogBoxManager>();
            Debug.Assert(Client != null);
            Debug.Assert(DialogMan != null);

            var score = FindObjectOfType<MainGameScore>();
            Point = (score != null) ? (int)score.Point: Random.Range(3000, 15000);

            State = GameState.Initialize;
            StartCoroutine(RankingTask());
        }

        // Update is called once per frame
        void Update()
        {
            switch(State)
            {
            case GameState.Initialize:
                //            StartCoroutine(GameMaster.Instance.GetComponent<DialogBoxManager>().OpenDialog(DialogBoxManager.Kind.Ok, "サーバー接続失敗", "サーバーに接続する事が出来ませんでした"));
                break;
            case GameState.Retry:
                State = GameState.Initialize;
                StartCoroutine(RankingTask());
                break;
            case GameState.Animate:
                if (RankBoardMan.State == RankBoardManager.StateStatus.Idle)
                {
                    State = GameState.Idle;
                }
                break;
            case GameState.Idle:
                if(Input.anyKeyDown)
                {
                    GameMaster.Instance.LoadScene(GameMaster.LoadSceneList.Title);
                    State = GameState.End;
                }
                break;
            //case GameState.End:
            default:
                break;
            }
        }
        IEnumerator RankingTask()
        {
            yield return UploadScore(Point);
            if(SelfRankInfo == null)
            {
                State = GameState.Retry;
                yield break;
            }

            yield return RequestRanking();
            if (RankInfos == null)
            {
                State = GameState.Retry;
                yield break;
            }

            RankBoardMan.Setup(RankInfos, SelfRankInfo);
            RankBoardMan.StartAction();

            State = GameState.Animate;
        }


        IEnumerator UploadScore(int point)
        {
            SelfRankInfo = null;
            if (!Client.IsConnected)
            {
                yield return Client.Login();
                if(!Client.IsConnected)
                {
                    yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "サーバー接続失敗", "サーバーに接続する事が出来ませんでした");
                    yield break;
                }
            }
            yield return Client.UploadUserScore(point);
            if(!Client.IsCompleteSendMessage)
            {
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "アップロード失敗", "スコアのアップロードに失敗しました");
                yield break;
            }
            SelfRankInfo = Client.SelfRankInfo;
        }
        IEnumerator RequestRanking()
        {
            RankInfos = null;
            if (!Client.IsConnected)
            {
                yield return Client.Login();
                if (!Client.IsConnected)
                {
                    yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "サーバー接続失敗", "サーバーに接続する事が出来ませんでした");
                    yield break;
                }
            }
            // ランキングの取得リクエスト
            yield return Client.RequestRanking(0, 10);
            if (!Client.IsCompleteSendMessage)
            {
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "リクエスト失敗", "ランキングの取得に失敗しました");
                yield break;
            }
            RankInfos = Client.RankInfos;
        }
    }
}
