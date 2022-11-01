using UnityEngine;

namespace Base.UserInterface
{
    [RequireComponent(typeof(Canvas))]
    public abstract class BaseCanvas : MonoBehaviour
    {
        private Canvas canvas;

        public void Initialize()
        {
            canvas = GetComponent<Canvas>();
            Debug.Log("<color=green>" + GetType().Name + " initialized!</color>");
        }

        public void Activate()
        {
            canvas.enabled = true;
        }

        public void Deactivate()
        {
            canvas.enabled = false;
        }
    }
}