using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class ManagerBaseMonoBehaviour : MonoBehaviour
    {
        Coroutine ActiveCoroutine = null;
        void Terminate()
        {
            StopAllCoroutines();
            ActiveCoroutine = null;
        }

        protected bool SetCoroutine(IEnumerator enumerator)
        {
            if (ActiveCoroutine == null)
            {
                ActiveCoroutine = StartCoroutine(ExecuteCoroutine(enumerator));
                return true;
            }
            return false;
        }

        protected IEnumerator ExecuteCoroutine(IEnumerator enumerator)
        {
            yield return enumerator;
            ActiveCoroutine = null;
        }

        private void OnDisable()
        {
            Terminate();
        }
    }

}
