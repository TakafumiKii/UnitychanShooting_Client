using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using FakeServer.Unity;

namespace UnityChanShooting
{
    public class TitleManager : Utility.ManagerBaseMonoBehaviour// MonoBehaviour
    {
//        Coroutine ActiveCoroutine = null;
        DialogBoxManager DialogMan;
        //        GameMaster _GameMaster;
        UserDataManager UserDataMan;
        NetworkClient NetClient;
        InputField InputUserName;

        private void Awake()
        {
//            DialogMan = GetComponentInChildren<DialogBoxManager>();
            DialogMan = GameMaster.Instance.GetComponent<DialogBoxManager>();
            UserDataMan = GameMaster.Instance.GetComponent<UserDataManager>();
            NetClient = GameMaster.Instance.GetComponent<NetworkClient>();
        }

        // Use this for initialization
        void Start()
        {
            InputUserName = GetComponentInChildren<InputField>(true);
            Debug.Assert(InputUserName != null && InputUserName.name == "UserName");

            InputUserName.text = UserDataMan.UserInfo.Name;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnClick_GameStart()
        {
            SetCoroutine(GameStart());
        }

        //public void OnClick_Rename()
        //{
        //    SetCoroutine(Rename());
        //}
        public void OnClick_ClearData()
        {
            SetCoroutine(ClearData());
        }

        public void OnValueChange_UserName(string name)
        {
//            Debug.Log(name);
        }
        public void OnEndEdit_UserName(string name)
        {
//            Debug.Log(name);
            UserDataMan.UserInfo.Name = name;
        }




        IEnumerator GameStart()
        {
            if (InputUserName.text.Length == 0)
            {// 名前がない
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "ユーザー名", "ユーザー名を入力してください");
                yield break;
            }
            // ログイン
            yield return NetClient.Login();

            if (!NetClient.IsConnected)
            {// 未接続
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "サーバー接続失敗", "サーバーに接続できませんでした");
                yield break;
            }


            if (!NetClient.IsLogin)
            {// ログイン失敗
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "ログイン失敗", "ログイン処理が正しく行われませんでした");
                yield break;
            }

            // チュートリアル未実行
            //  チュートリアルへシーン遷移

            //  ゲーム本編へシーン遷移
            GameMaster.Instance.LoadScene(GameMaster.LoadSceneList.MainGame);
        }

        //IEnumerator Rename()
        //{
        //    //  入力ダイアログを表示
        //    DialogMan.InputText = UserDataMan.UserInfo.Name;
        //    yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Input, "名前入力", "ユーザー名を入力してください",false);

        //    if (DialogMan.Result == DialogBox.ResultStatus.YesOk)
        //    {
        //        //  削除する
        //        UserDataMan.ResetUserInfo();
        //        yield return DialogMan.CloseDialog(true);
        //        //  削除しました
        //        yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "初期化完了", "初期化を行いました");
        //    }
        //    yield return DialogMan.CloseDialog(false);
        //}

        IEnumerator ClearData()
        {
            //  確認ダイアログを表示
            yield return DialogMan.OpenDialog(DialogBoxManager.Kind.YesNo, "ユーザーデータ初期化", "ユーザーデータを初期化します\nよろしいですか？",false);

            if (DialogMan.Result == DialogBox.ResultStatus.YesOk)
            {
                //  削除する
                UserDataMan.ResetUserInfo();
                yield return DialogMan.CloseDialog(true);
                InputUserName.text = UserDataMan.UserInfo.Name;
                //  削除しました
                yield return DialogMan.OpenDialog(DialogBoxManager.Kind.Ok, "初期化完了", "初期化を行いました");
            }
            else
            {
                yield return DialogMan.CloseDialog(false);
            }
        }
    }
}
