using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
namespace UnityChanShooting
{
    public class DialogBoxManager : MonoBehaviour
    {

        public enum Kind
        {
            Ok,
            YesNo,
            //        Input,
        }
        DialogBox[] DialogBoxes = new DialogBox[Enum.GetNames(typeof(Kind)).Length];
        DialogBox Dialog;

        Canvas Canvas;

        public DialogBox.ResultStatus Result { get { return (Dialog != null) ? Dialog.Result : DialogBox.ResultStatus.None; } }
        //    DialogBoxInput DBInput { get { return (Dialog is DialogBoxInput) ? (DialogBoxInput)Dialog : null; } }
        public string InputText { get; set; }

        struct DialogParam
        {
            Kind kind;
            string title;
            string explanation;
        }

        private void Awake()
        {
            Canvas = GetComponentInChildren<Canvas>(true);
            Debug.Assert(Canvas != null);
            Canvas.gameObject.SetActive(true);

            var boxes = GetComponentsInChildren<DialogBox>(true);
            foreach (var box in boxes)
            {
                Kind kind = Kind.Ok;
                switch (box.Kind)
                {
                case DialogBox.KindStatus.Ok:
                    kind = Kind.Ok;
                    break;
                case DialogBox.KindStatus.YesNo:
                    kind = Kind.YesNo;
                    break;
                //case DialogBox.KindStatus.Input:
                //    kind = Kind.Input;
                //    break;
                default:
                    Debug.Assert(false, "Unknown DialogBox.Kind + " + box.Kind);
                    return;
                }
                Debug.Assert(DialogBoxes[(int)kind] == null);     //  重複チェック
                DialogBoxes[(int)kind] = box;
            }

            //  設定ミスチェック
            foreach (var box in DialogBoxes)
            {
                Debug.Assert(box != null);
                box.gameObject.SetActive(false);
            }
        }

        public IEnumerator OpenDialog(Kind kind, string title, string explanation, bool autoClose = true)
        {
            while (Dialog != null)
            {// 何か実行中
                yield return null;
            }
            Dialog = DialogBoxes[(int)kind];
            Debug.Assert(Dialog != null);

            //DialogBoxInput input = Dialog as DialogBoxInput;
            //if (input != null)
            //{
            //    input.Input.text = InputText;
            //}

            Canvas.gameObject.SetActive(true);
            yield return Dialog.OpenDialog(title, explanation);

            //if(input != null)
            //{
            //    InputText = input.Input.text;
            //}
            if (autoClose)
            {
                yield return CloseDialog(false);
            }
        }
        public IEnumerator CloseDialog(bool isActive = false)
        {
            Dialog = null;
            Canvas.gameObject.SetActive(isActive);
            yield break;
        }
        // Use this for initialization
        void Start()
        {
            Canvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}