using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class CheckpointBarricade : MonoBehaviour
    {
        [SerializeField] private Transform leftBarricade;
        [SerializeField] private Transform rightBarricade;
        [SerializeField] private float barricadeOpenTime=2f;

        public void OpenGates()
        {
            OpenBarricade();
        }

        private void OpenBarricade()
        {
            leftBarricade.DOLocalRotate(new Vector3(90, 0, 0), barricadeOpenTime);
            rightBarricade.DOLocalRotate(new Vector3(90, 0, 0), barricadeOpenTime);
        }
    }
}
