using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesGridBasedMove : MonoBehaviour
{
    public delegate void RetVoidArg2Int(int r, int c);
    public static event RetVoidArg2Int ConnectedBubble;
    public static event RetVoidArg2Int CheckForFalling;
    static List<Vector2Int> connectedOnes = new List<Vector2Int>();

    public static void CheckForFallingBubbles(int[,]bubbleGrid, int rowNum, int colNum)
    {

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


}
