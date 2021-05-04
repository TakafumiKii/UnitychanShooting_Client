using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UnityChanShooting
{
    // TODO:実装方法が微妙なので一旦保留
    public class DialogBoxInput : DialogBox
    {
        public InputField Input { get; private set; }
        private void Awake()
        {
            Input = GetComponentInChildren<InputField>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}