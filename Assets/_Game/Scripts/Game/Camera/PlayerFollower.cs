using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

namespace _Game.Scripts.Game.Camera
{
    public class PlayerFollower : MonoBehaviour
    {
        private Transform Player;
        [SerializeField] private Vector3 offset;
        void Start()
        {
            Player = GameManager.Instance.GetPlayerController().transform;
        }

    
        void Update()
        {
            transform.position = offset + new Vector3(Player.position.x,0,Player.position.z);
        }
    }
}
