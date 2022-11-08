using UnityEngine;

namespace _Game.Scripts
{
    public class BallAdder : MonoBehaviour, ICollectable
    {
        enum AdderType
        {
            MultipleUpper,
            MultipleBack,
            MultipleRight,
            Adder
        }

        [SerializeField] private AdderType adderType;
        [SerializeField] private int ballCount;
        
        public void OnHit(BallManager ballManager)
        {
            // switch (adderType)
            // {
            //     case AdderType.Adder:
            //         ballManager.AddBalls(ballCount);
            //         break;
            //     case AdderType.MultipleBack:
            //         ballManager.MultiplyBack();
            //         break;
            //     case AdderType.MultipleRight:
            //         ballManager.MultiplyRight();
            //         break;
            //     case AdderType.MultipleUpper:
            //         ballManager.MultiplyUpper();
            //         break;
            // }
        }
    }
}
