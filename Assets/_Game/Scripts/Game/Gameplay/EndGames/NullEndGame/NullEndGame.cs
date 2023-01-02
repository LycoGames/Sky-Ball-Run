using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.EndGames.NullEndGame
{
    public class NullEndGame : EndGameController
    {
        [SerializeField] private Vector3 confettiEffectOffset;
        public override void LaunchEndGame()
        {
            Invoke("EndGame",.3f);
        }

        private void EndGame()
        {
            SetConfettiPos(Camera.main.transform.position+confettiEffectOffset);
            EndGameEnded?.Invoke();
        }
    }
}
