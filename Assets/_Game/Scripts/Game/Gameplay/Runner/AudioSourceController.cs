using System;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.Game.Gameplay.Runner
{
    public class AudioSourceController : MonoBehaviour
    {
        public static AudioSourceController Instance;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip ballExplode;
        [SerializeField] private AudioClip ballAdd;
        [SerializeField] private AudioClip passCheckpoint;
        [SerializeField] private AudioClip winLevel;
        [SerializeField] private AudioClip diamondCollected;
        [SerializeField] private AudioClip shapeChange;

        private void OnEnable()
        {
            Instance = this;
        }

        public void PlaySoundType(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.BallExplode:
                    audioSource.PlayOneShot(ballExplode);
                    break;
                case SoundType.BallAdd:
                    audioSource.PlayOneShot(ballAdd);
                    break;
                case SoundType.PassCheckpoint:
                    audioSource.PlayOneShot(passCheckpoint);
                    break;
                case SoundType.WinLevel:
                    audioSource.PlayOneShot(winLevel);
                    break;
                case SoundType.DiamondCollected:
                    audioSource.PlayOneShot(diamondCollected);
                    break;
                case SoundType.ShapeChange:
                    audioSource.PlayOneShot(shapeChange);
                    break;
            }
        }
    }
}

public enum SoundType
{
    BallExplode,
    BallAdd,
    PassCheckpoint,
    WinLevel,
    DiamondCollected,
    ShapeChange
}