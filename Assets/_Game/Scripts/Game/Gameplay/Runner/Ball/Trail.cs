using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Gameplay.Runner.Ball;
using UnityEngine;

public class Trail : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    public void SetPosition(float posX)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToDestination(posX));
    }
    private IEnumerator MoveToDestination(float newX)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = newX;
        while (Vector3.Distance(transform.localPosition, newPos) >= 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = newPos;
        yield return null;
    }
}
