using System;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public abstract class Gate : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] protected Animator myAnimator;
        protected DoubleGate myDoubleGate;
        protected Action OnEnterGate;
        protected Action OnDisableGate;
        protected bool canCheckSize = true;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!myCollider.enabled || !other.CompareTag("Ball")) return;
            OnEnterGate?.Invoke();
            DisableMyCollider();
            canCheckSize = false;
        }

        public void EnableMyAnimator() => myAnimator.enabled = true;

        public void DisableGate() => OnDisableGate?.Invoke();
        public void EnterGate() => OnEnterGate?.Invoke();
        private void DisableMyCollider() => myCollider.enabled = false;

        public void SetDoubleGate(DoubleGate doubleGate)
        {
            DisableMyCollider();
            myDoubleGate = doubleGate;
        }
    }
}