using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Game.Gameplay.Runner;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private float sense = 5f;
    private bool isStillTouch=false;
    private Vector3 firstTouch;
    private PlayerController playerController;
    private float slipOnX=0;

    void Start()
    {
        firstTouch = Vector3.zero;
        playerController = GameManager.gameManager.GetPlayerController();
    }

    void Update()
    {
        TouchHandler();
        MovePlayer();
    }

    void TouchHandler()
    {
        if (Input.GetMouseButton(0))
        {
            if (isStillTouch)
            {
                slipOnX = CalculateSliping().x;
                firstTouch = Input.mousePosition;
            }
            else
            {
                firstTouch = Input.mousePosition;
                isStillTouch = true;
            }

            return;
        }
        isStillTouch = false;
        firstTouch = Vector3.zero;
        slipOnX = 0;
    }

    private Vector3 CalculateSliping()
    {
        return new Vector3(firstTouch.x, firstTouch.y, 0)
               - Input.mousePosition;
    }

    private void MovePlayer()
    {
        playerController.HorizontalInput = -slipOnX * sense;
    }
}