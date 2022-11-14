using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Ball
{
    public class Trail : MonoBehaviour
    {

        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed = 1f;
        [SerializeField] private float rotateStartAngle = 20f;
        [SerializeField] private TrailManager trailManager; 
        
        private bool firstRun=true; //TODO kötü duruyor.
        private int activeBallColumnCount = 0;

        public void InitiliazeTrail(TrailManager _trailManager)
        {
            trailManager = _trailManager;
        }
        public void SetPosition(float posX)
        {
            StopAllCoroutines();
            StartCoroutine(RotateToDestination(posX));
            StartCoroutine(MoveToDestination(posX));
            //StartCoroutine()
        }

        public void IncreaseActiveBallColumnCount()
        {
            activeBallColumnCount++;
            
        }

        public void DecreaseActiveBallColumnCount()
        {
            activeBallColumnCount--;
            if (activeBallColumnCount <= 0)
            {
                trailManager.DeactivetingTrail(this);
                firstRun = true;
            }
            
        }

        private IEnumerator RotateToDestination(float newX)
        {
            bool isPositive=(transform.localPosition.x - newX)>=0;
            float yRotation = rotateStartAngle * (isPositive ? -1 : 1);
            transform.localRotation=Quaternion.Euler(0,yRotation,0);
            while (transform.eulerAngles.y!=0)
            {
                Debug.Log("Girdi");
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), rotateSpeed*Time.deltaTime);
                yield return null;
            }
            transform.localRotation=Quaternion.Euler(0,0,0);
            yield return null;
        }
        private IEnumerator MoveToDestination(float newX)
        {
            Vector3 newPos = Vector3.zero;
            newPos.x = newX;
            while (Vector3.Distance(transform.localPosition, newPos) >= 0&&!firstRun)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            firstRun = false;
            transform.localPosition = newPos;
            yield return null;
        }
    }
}
