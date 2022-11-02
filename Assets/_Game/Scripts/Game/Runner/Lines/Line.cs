using System;
using UnityEngine;

namespace _Game.Scripts.Lines
{
    public class Line : MonoBehaviour
    {
        public Action OnLinePassed;
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) OnLinePassed.Invoke();
        }
    }
}
