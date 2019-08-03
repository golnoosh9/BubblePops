using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGrid : MonoBehaviour
{
    Dictionary<Vector2Int, GameObject> bubbleCoordinateMap = new Dictionary<Vector2Int, GameObject>();
    int[,] bubbleGrids = new int[10,6];
    int rowNum = 10;
    int colNum = 6;
    BubblePool bubblePool;
    int initialBubbleNum = 30;
    // Start is called before the first frame update
    void Start()
    {
        BubbleShooter.BubbleComingIn += IncomingBubble;
        BubbleShooter.BubbleArrived += CreateNewBubble;
        bubblePool = FindObjectOfType<BubblePool>();
        GenerateBubbles();
    }

    void GenerateBubbles()
    {
        int r = 0;
        int c = 0;
        for (int i = 0; i < initialBubbleNum; i++)
        {
            AddBubbleOfTypeAt(r, c, Random.Range(1, 9), false);
            c = (c + 1) % colNum;
            if (c == 0)
                r++;
        }
        c = 0;

        for (int i = 1; i <= colNum-1; i++)
        {
            AddBubbleOfTypeAt(r, i, -1, true);
            c++;
        }
    }


    void IncomingBubble(int r,int c,int score)
    {

        DeleteBubbleAt(r, c, true);
    }


    void CreateNewBubble(int r, int c, int score)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
     //   Debug.Log("score: " + score);
        AddBubbleOfTypeAt(r, c, score,false);
 
        neighbors= NeighborUtility.GetAllNeighbors(bubbleGrids, r, c, score, rowNum, colNum);

        if (neighbors.Count > 0)
        {
            neighbors.Add(new Vector2Int(r, c));
            MergeBubbles(neighbors, score);
        }
        else
        {
           neighbors= NeighborUtility.GetAllNeighbors(bubbleGrids, r, c, 0, rowNum, colNum);
            for (int i = 0; i < neighbors.Count; i++)
            {
                AddBubbleOfTypeAt(neighbors[i].x, neighbors[i].y, -1, true);
            }
        }
    }
    

    void MergeBubbles(List<Vector2Int> bubbles,int score)
    {
 
        for (int i = 0; i < bubbles.Count; i++)
        {
            DeleteBubbleAt(bubbles[i].x, bubbles[i].y,false);

        }
    }


void DeleteBubbleAt(int r, int c, bool isEmpty)
    {
        GameObject t = bubbleCoordinateMap[new Vector2Int(r, c)];
        bubblePool.ReturnToPool(t, isEmpty);
        bubbleCoordinateMap.Remove(new Vector2Int(r, c));
        if(isEmpty==false)//add empty bubble
        {
            AddBubbleOfTypeAt(r, c, -1, true);
        }
    }

    void AddBubbleOfTypeAt(int r, int c, int score,bool isEmpty)
    {
        GameObject t;
        if (isEmpty == false)
        {
            t = bubblePool.GetFromPool(score);
        }
        else
        {
            t = bubblePool.GetFromPool(true);
        }
        bubbleCoordinateMap.Add(new Vector2Int(r, c), t);
            t.GetComponent<BubbleDataID>().SetPosition(r, c);
            t.SetActive(true);
            bubbleGrids[r, c] = score;

    }


    private void OnDestroy()
    {
        BubbleShooter.BubbleComingIn -= IncomingBubble;
        BubbleShooter.BubbleArrived -= CreateNewBubble;
    }
}
