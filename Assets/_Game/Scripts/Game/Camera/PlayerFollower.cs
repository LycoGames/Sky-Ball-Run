using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Camera
{
    public class PlayerFollower : MonoBehaviour
    {
        private Transform Player;
        private Vector3 offset;
        void Start()
        {
            Player = GameManager.gameManager.GetPlayerController().transform;
            offset = transform.position-Player.position;
        }

    
        void Update()
        {
            transform.position = offset + new Vector3(Player.position.x,0,Player.position.z);
        }
    }
}
