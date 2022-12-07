using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class Line : MonoBehaviour
    {
        private Action<int> onLinePassed;
        private int index;

        public void InitializeLine(Action<int> _onLinePassed,int _index)
        {
            index = _index;
            onLinePassed += _onLinePassed;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onLinePassed?.Invoke(index);
            }
        }
    }
}