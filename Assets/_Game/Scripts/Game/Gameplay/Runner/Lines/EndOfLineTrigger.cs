using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Lines
{
    public class EndOfLineTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                GameManager.Instance.ArriveEndOfLine();
        }
    
    }
}
