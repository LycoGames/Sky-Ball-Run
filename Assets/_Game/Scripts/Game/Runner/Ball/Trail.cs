using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Game.Runner.Ball;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] private BallColumn ballColumn;
    [SerializeField] private int maxColumn = 20;
    [SerializeField] private int maxColumnSize = 5;
    [SerializeField] private Ball ball;
    [SerializeField] private float distance = 0.5f;
    private readonly List<BallColumn> ballColumnList = new List<BallColumn>();

    void Start()
    {
        for (int i = 0; i < maxColumn; i++)
        {
            ballColumnList.Add(Instantiate(ballColumn));
            if (i == 0) ballColumnList[0].InitializeBallColumn(transform, this, distance);
            else ballColumnList[i].InitializeBallColumn(ballColumnList[i - 1].transform, this, distance);
            for (int j = 0; j < 5; j++) ballColumnList[i].AddBall(Instantiate(ball));
        }
    }

    public void StartRemoveProcess()
    {
        foreach (BallColumn ballColumn in ballColumnList)
        {
            ballColumn.StartRemoveProcess();
        }
        Invoke("RepositioningToForward",0.1f);
    }

    public void RepositioningToForward()
    {
        
        for (int i = 0; i < maxColumn-1; i++)
        {
            BallColumn ballColumn = ballColumnList[i];
            for (int j = i+1; j < maxColumn; j++)
            {
                if (ballColumn.ReturnBallCount() >= maxColumnSize) break;
                if (ballColumnList[j].ReturnBallCount() >ballColumn.ReturnBallCount())
                {
                    ballColumn.AddBall(ballColumnList[j].GetBall(ballColumn.ReturnBallCount()));
                    j--;
                }
            }
        }
    }

    // public BallColumn GetBackColumn(BallColumn ballColumn)
    // {
    //     int index = ballColumnList.IndexOf(ballColumn);
    //     if (index >= maxColumn-1) return null;
    //     return ballColumnList[index + 1];
    // }
    // public bool CheckForLongerColumn(BallColumn ballColumn)
    // {
    //     for (int i = ballColumnList.IndexOf(ballColumn)+1; i < maxColumn - 1; i++)
    //     {
    //         if (ballColumn.BallCount < ballColumnList[i].BallCount) return true;
    //     }
    //     return false;
    // }
}