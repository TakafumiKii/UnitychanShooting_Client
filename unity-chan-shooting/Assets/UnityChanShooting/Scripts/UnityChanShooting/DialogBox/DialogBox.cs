using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityChanShooting
{
    public class DialogBox : MonoBehaviour
    {

        public enum ResultStatus
        {
            None,
            Cancel,
            No,
            YesOk,
        }
        public enum KindStatus
        {
            Unknown,
            Ok,
            YesNo,
            Input,
        }

        public ResultStatus Result { get; private set; } = ResultStatus.No;
        [SerializeField]
        KindStatus _Kind = KindStatus.Unknown;
        public KindStatus Kind { get { return _Kind; } }

        Text TextTitle;
        Text TextExplanation;

        //public string Title { get { return (TextTitle != null) ? TextTitle.text : ""; } set { if(TextTitle != null) TextTitle.text = value; } }
        //public string Explanation { get { return (TextExplanation != null) ? TextExplanation.text : ""; } set { if (TextExplanation != null) TextExplanation.text = value; } }

        private void Awake()
        {
            Text[] texts = GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                switch (text.name)
                {
                case "Title":
                    TextTitle = text;
                    break;
                case "Explanation":
                    TextExplanation = text;
                    break;
                default:
                    Debug.Log("Unknown Text name:" + text.name);
                    break;
                }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator OpenDialog(string title, string explanation)
        {
            Result = ResultStatus.None;

            TextTitle.text = title;
            TextExplanation.text = explanation;

            gameObject.SetActive(true);

            while (Result == ResultStatus.None)
            {
                yield return null;
            }

            gameObject.SetActive(false);
        }

        public void OnClick_Ok()
        {
            Result = ResultStatus.YesOk;
        }
        public void OnClick_Yes()
        {
            Result = ResultStatus.YesOk;
        }
        public void OnClick_No()
        {
            Result = ResultStatus.No;
        }
    }
}