﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGrid : MonoBehaviour
{
    public delegate void RetVoidArg3Int1String(int r, int c, int chainNum, string animTrigger);
    public static event RetVoidArg3Int1String BubbleActivityEvent;
    Dictionary<Vector2Int, GameObject> bubbleCoordinateMap = new Dictionary<Vector2Int, GameObject>();
    int[,] bubbleGrids = new int[10,6];
    int rowNum = 10;
    int colNum = 6;
    bool smooth;
    BubblePool bubblePool;
    int initialBubbleNum = 6*7;
    bool mergeDone = false;
    // Start is called before the first frame update
    void Start()
    {
        MoveBubble.BubbleHitGround += DeleteBubbleAt;
        BubbleShooter.BubbleArrived += CreateNewBubble;
        BubblesGridBasedMove.ScrollUp += AddRowBelow;
        BubblesGridBasedMove.ScrollDown += AddRowAbove;
        bubblePool = FindObjectOfType<BubblePool>();
        GenerateBubbles();
    }

    void GenerateBubbles()
    {
        int r = 0;
        int c = 0;
        for (int i = 0; i < initialBubbleNum; i++)
        {
            smooth = true;
            AddBubbleOfTypeAt(r, c, Random.Range(1, 9));
            c = (c + 1) % colNum;
            if (c == 0)
                r++;
        }

    }



    void CreateNewBubble(int r, int c, int score)
    {

        List<Vector2Int> neighbors = new List<Vector2Int>();
     
        smooth = false;
        AddBubbleOfTypeAt(r, c, score);
 

        neighbors = NeighborUtility.GetAllNeighbors(bubbleGrids, r, c, score, rowNum, colNum, false);
        for (int i = 0; i < neighbors.Count; i++)
        {
            BubbleActivityEvent(neighbors[i].y, neighbors[i].x, NeighborUtility.chainRounds, "Shake");
        }
        neighbors = NeighborUtility.GetAllNeighbors(bubbleGrids, r, c, score, rowNum, colNum,true);

        if (neighbors.Count > 0)
        {
            StartCoroutine(ProcessBubbleEffectInTime(r, c, score,neighbors));
        }
        else
        {

            BubblesGridBasedMove.CheckScroll(bubbleGrids, rowNum, colNum);
            Debug.Log("before neighbor call");
            Debug.Log(ToString());
            
            BubblesGridBasedMove.CheckForFallingBubbles(bubbleGrids, rowNum, colNum);
            Debug.Log("after neighbor call");
            Debug.Log(ToString());
            Debug.Break();

        }
        //  Debug.Log(ToString());

    }

    void AddRowBelow()
    {
       
        BubbleDataID.SwitchOffset();
        int[,] temp = new int[10,6];

        for (int j = 0; j < colNum; j++)
            {
           DeleteBubbleAt(0, j);
            }

        for (int i = 1; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                if (bubbleGrids[i, j] == 0)
                    continue;
              //  Debug.Log("key: " + i + "   " + j);
                GameObject t = bubbleCoordinateMap[new Vector2Int(j, i)];
                bubbleCoordinateMap.Remove(new Vector2Int(j, i));
                bubbleCoordinateMap.Add(new Vector2Int(j , i-1),t);
            }
        }
        for (int i = 0; i < rowNum-1; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                temp[i , j] = bubbleGrids[i+1, j];
            }
        }
        bubbleGrids = temp;
       
    }

    void AddRowAbove()
    {
        BubbleDataID.SwitchOffset();
        int[,] temp = new int[10, 6];
        for (int j = 0; j < colNum; j++)
        {
            AddBubbleOfTypeAt(0, j, Random.Range(1, 9));
        }
        for (int i = 1; i < rowNum ; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                temp[i -1, j] = bubbleGrids[i, j];
            }
        }
        bubbleGrids = temp;

    }

    IEnumerator ProcessBubbleEffectInTime(int r, int c, int score, List<Vector2Int> neighbors)
    {
        neighbors.Add(new Vector2Int(c, r));
        int newScore = score;
        for (int i = 0; i < neighbors.Count - 1; i++)
        {
            newScore++;
        }
        Vector2Int newBubblePosition = NeighborUtility.SearchInNodeNeighbors(neighbors, bubbleGrids, rowNum, colNum, newScore);
        for (int i = 0; i < neighbors.Count; i++)
        {
            DeleteBubbleAt(neighbors[i].y, neighbors[i].x);

        }
        yield return new WaitForSeconds(0.9f);
   
        CreateNewBubble(newBubblePosition.y, newBubblePosition.x, newScore);


        BubbleActivityEvent(newBubblePosition.y, newBubblePosition.x, NeighborUtility.chainRounds, "Enlarge");
        Debug.Log(ToString());
    }


    void DeleteBubbleAt(int r, int c)
    {
    
        GameObject t = bubbleCoordinateMap[new Vector2Int(c, r)];
        BubbleActivityEvent(r, c, NeighborUtility.chainRounds, "Shrink");
        bubbleCoordinateMap.Remove(new Vector2Int(c, r));
        bubblePool.ReturnToPool(t);
        bubbleGrids[r, c] = 0;

    }

    void AddBubbleOfTypeAt(int r, int c, int score)
    {
        bubbleCoordinateMap.Remove(new Vector2Int(c, r));
        GameObject t;
   
        t = bubblePool.GetFromPool(score);
        bubbleGrids[r, c] = score;
        bubbleCoordinateMap.Add(new Vector2Int(c, r), t);
            t.GetComponent<BubbleDataID>().SetPosition(r, c,smooth);
            
      //  t.transform.position = new Vector3(0, 0, 0);
            t.SetActive(true);
           

    }


    private void OnDestroy()
    {
        BubbleShooter.BubbleArrived -= CreateNewBubble;
        BubblesGridBasedMove.ScrollUp -= AddRowBelow;
        BubblesGridBasedMove.ScrollDown -= AddRowAbove;
        MoveBubble.BubbleHitGround -= DeleteBubbleAt;
    }


    public override string ToString()
    {
        string res = "";
        for (int j = 0; j < 10; j++)
        {


            for (int i = 0; i < 6; i++)
            {
                res = res + bubbleGrids[j, i] + " , ";
            }
            res = res + "       ";
        }
        return res;
    }

}
