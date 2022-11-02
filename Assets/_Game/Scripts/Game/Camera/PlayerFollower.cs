using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform Player;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position-Player.position;
    }

    
    void Update()
    {
        transform.position = offset + new Vector3(Player.position.x,0,Player.position.z);
    }
}
