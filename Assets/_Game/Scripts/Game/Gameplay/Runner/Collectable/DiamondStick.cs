using System;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner.Collectable
{
    public class DiamondStick : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private TextMeshProUGUI valueText;
        //[SerializeField] private ParticleSystem effect;
        [SerializeField] private Transform top;
        [SerializeField] private int value=5;

        private void Start()
        {
            valueText.text = value.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                myCollider.enabled = false;
                //effect.Play();
                top.gameObject.SetActive(false);
                GameManager.Instance.GainedDiamond(value);
                Destroy(gameObject);
            }
        }
    }
}
