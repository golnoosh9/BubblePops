using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborUtility : MonoBehaviour
{
    public static List<Vector2Int> neighbors;
    public static void ClearNeighbor()
    {
        neighbors = new List<Vector2Int>();
    }
    public static List<Vector2Int> GetAllNeighbors(int[,] bubbleGrids, int r, int c, int value, int rowNum, int colNum)
    {
        ClearNeighbor();
        if(bubbleGrids[r,c]==value)
          neighbors.Add(new Vector2Int(c, r));
        GetNeighborsWithValue(bubbleGrids, r, c, value, rowNum, colNum);
        if (bubbleGrids[r, c] == value)
            neighbors.Remove(new Vector2Int(c, r));
        return neighbors;
    }
    public static  void GetNeighborsWithValue( int[,]bubbleGrids,int r, int c, int value,int rowNum, int colNum)
    {
       
        //List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2[] directions;


        if (r % 2 == 1)
        {
            Vector2[] d = { new Vector2(-1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(1, 0) };
            directions = d;
        }
        else
        {
            Vector2[] d = { new Vector2(-1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
            directions = d;
        }

        for (int i = 0; i < directions.Length; i++)
        {
            int nr = r + (int)directions[i].y;
            int nc = c + (int)directions[i].x;
            if (nr < 0 || nc < 0 || nr >= rowNum || nc >= colNum)
                continue;
            if (bubbleGrids[nr, nc] == value && neighbors.Contains(new Vector2Int(nc, nr))==false)
            {
                neighbors.Add(new Vector2Int(nc, nr));
                if (value > 0)
                {
                     GetNeighborsWithValue(bubbleGrids, nr, nc, value, rowNum, colNum);
                }
            }
        }

        return;



       // return neighbors;
    }


    public static Vector2Int SearchInNodeNeighbors( List<Vector2Int> nodes,int[,]bubbleGrids,int rowNum, int colNum, int value)
    {

        int r;
        int c;
        for (int i = 0; i < nodes.Count; i++)
        {
            r = nodes[i].y;
            c = nodes[i].x;
            Vector2[] directions;

            if (r % 2 == 1)
            {
                Vector2[] d = { new Vector2(-1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(0, 1) };
                directions = d;
            }
            else
            {
                Vector2[] d = { new Vector2(-1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
                directions = d;
            }
            for (int j = 0; j < directions.Length; j++)
            {
                int nr = r + (int)directions[j].y;
                int nc = c + (int)directions[j].x;
                if (nr < 0 || nc < 0 || nr >= rowNum || nc >= colNum)
                    continue;
                if (bubbleGrids[nr, nc] == value )
                {
                    return new Vector2Int(c, r);

                }
            }

        }
        return nodes[0];
    }
}
