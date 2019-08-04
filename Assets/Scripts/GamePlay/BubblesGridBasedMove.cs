using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesGridBasedMove : MonoBehaviour
{
    public delegate void RetVoidArg2Int(int r, int c);
    public static event RetVoidArg2Int ConnectedBubble;
    public static event RetVoidArg2Int CheckForFalling;
    public delegate void RetVoidArgVoid();
    public static event RetVoidArgVoid startingNighborCalc;
    public static event RetVoidArgVoid ScrollDown;
    public static event RetVoidArgVoid ScrollUp;

    static List<Vector2Int> connectedOnes = new List<Vector2Int>();

    //public static void OverallGridCheck(int[,]bubbleGrid, int rowNum, int colNum)
    //{
    //    CheckForFallingBubbles(bubbleGrid, rowNum, colNum);

    //}

    public static void CheckForFallingBubbles(int[,]bubbleGrid, int rowNum, int colNum)
    {
        startingNighborCalc();
        connectedOnes = new List<Vector2Int>();
        List<Vector2Int> neighbors;
        for (int i = 0; i < colNum; i++)
        {

            ConnectedBubble(0, colNum);
            connectedOnes.Add(new Vector2Int(colNum, 0));
            neighbors = NeighborUtility.GetAllNeighbors(bubbleGrid, 0, i, 0, rowNum, colNum, false);
            CallNeighbors(bubbleGrid, rowNum, colNum, neighbors);
            
        }
        CheckForFalling(0, 0);
      //  CheckScroll(bubbleGrid, rowNum, colNum);
    }


    static void CallNeighbors(int[,]bubbleGrid, int rowNum, int colNum, List<Vector2Int> neighbors)
    {
        List<Vector2Int> neighborsNew;
        for (int i = 0; i <neighbors.Count; i++)
        {
            if (connectedOnes.Contains(neighbors[i]) == false)
            {
                ConnectedBubble(neighbors[i].y, neighbors[i].x);
                connectedOnes.Add(neighbors[i]);
                neighborsNew = NeighborUtility.GetAllNeighbors(bubbleGrid, neighbors[i].y, neighbors[i].x, 0, rowNum, colNum, false);
                CallNeighbors(bubbleGrid, rowNum, colNum, neighborsNew);
            }

        }

        return;
    }


    public static void CheckScroll(int[,] bubbleGrid, int rowNum, int colNum)
    {
        bool rowEmpty = true;
        int fistFullRow=0;
        for (int i = rowNum-1; i >= 0; i--)
        {
            for (int j = 0; j < colNum; j++)
            {
               if(bubbleGrid[i,j]>0)
                {
                    rowEmpty = false;
                    fistFullRow = i;
                    break;
                }
            }
            if (rowEmpty == false)
                break;
        }
        Debug.Log("first empty:  " + fistFullRow);
        if(fistFullRow>rowNum-3)
        {
            ScrollUp();
         }

        //else if(fistFullRow>rowNum-3)
        //{
        //    ScrollUp();
        //}

    }






}
