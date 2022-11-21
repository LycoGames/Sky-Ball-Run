using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class EndOfLineTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                GameManager.Instance.OnEndOfLine();
        }
    
    }
}
