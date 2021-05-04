using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanShooting
{
    public class PlayerInput : MonoBehaviour {

        [SerializeField]
        Rect InputAria;
        Vector2 AreaMin;
        Vector2 AreaMax;

        PlayerController PlayerCtrl;

        void Start() {
            //            CharaCtrl = GetComponent<CharacterController>();
            PlayerCtrl = GetComponent<PlayerController>();

            Vector2 center = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
            center.x += InputAria.x;
            center.y += InputAria.y;

            Vector2 add = new Vector2(InputAria.width / 2.0f, InputAria.height / 2.0f);
            AreaMin = center - add;
            AreaMax = center + add;
        }

        // Update is called once per frame
        void Update() {
            //Touch[] touches = Input.touches;
            //if(touches.Length > 0)MoveCtrl
            //{
            //    Touch touch = touches[0];
            //    Debug.Log(touch);
            //}
             bool isPressShotButton = Input.GetMouseButton(0);
            Vector3 pos = Input.mousePosition;
            if (pos.x < AreaMin.x) { pos.x = AreaMin.x; }
            else if (pos.x > AreaMax.x) { pos.x = AreaMax.x; }

            if (pos.y < AreaMin.y) { pos.y = AreaMin.y; }
            else if (pos.y > AreaMax.y) { pos.y = AreaMax.y; }

            Ray ray = Camera.main.ScreenPointToRay(pos);

            Vector3 n = new Vector3(0, 1, 0);// 現在は固定
            Vector3 x = transform.position;//new Vector3(0, 0, 0);
            var x0 = ray.origin;
            var m = ray.direction;
            var h = Vector3.Dot(n, x);
            var intersectPoint = x0 + ((h - Vector3.Dot(n, x0)) / (Vector3.Dot(n, m))) * m;
            //                MoveCtrl.SetMove(move);
            //            transform.position = intersectPoint;
            //            transform.position = Vector3.Lerp(transform.position, intersectPoint, Time.deltaTime);

            if(isPressShotButton)
            {
                PlayerCtrl.MoveShooting(intersectPoint);
                PlayerCtrl.Shot();
            }
            else
            {
                PlayerCtrl.Move(intersectPoint);
            }
        }
    }
}