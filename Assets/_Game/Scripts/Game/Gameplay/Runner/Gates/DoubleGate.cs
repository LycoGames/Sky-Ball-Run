using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Gates
{
    public class DoubleGate : MonoBehaviour
    {
        private List<Gate> gates = new List<Gate>();
        [SerializeField] private Collider myCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                myCollider.enabled = false;
                float distance = int.MaxValue;
                Gate selectedGate = gates[0];
                float newDistance = 0;
                foreach (var gate in gates)
                {
                    gate.DisableGate();
                    newDistance = Vector3.Distance(other.transform.position, gate.transform.position);
                    if (newDistance <= distance)
                    {
                        distance = newDistance;
                        selectedGate = gate;
                    }
                }

                selectedGate.EnterGate();
                gates.Remove(selectedGate);
                gates.First().gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            gates.AddRange(transform.GetComponentsInChildren<Gate>());
            foreach (var gate in gates) gate.SetDoubleGate(this);
        }
       
        public void SwapGate(Gate RemoveGate, Gate NewGate)
        {
            Debug.Log("swaped gate");
            gates.Remove(RemoveGate);
            gates.Add(NewGate);
            NewGate.SetDoubleGate(this);
        }
    }
}