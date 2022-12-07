using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        protected DoubleGate myDoubleGate;
        protected Action OnEnterGate;

        private void OnTriggerEnter(Collider other)
        {
            if (!myCollider.enabled || !other.CompareTag("Ball")) return;
            OnEnterGate?.Invoke();
            DisableMyCollider();
        }

        public void EnterGate() => OnEnterGate?.Invoke();
        private void DisableMyCollider() => myCollider.enabled = false;

        public void SetDoubleGate(DoubleGate doubleGate)
        {
            DisableMyCollider();
            myDoubleGate = doubleGate;
        }
    }
}