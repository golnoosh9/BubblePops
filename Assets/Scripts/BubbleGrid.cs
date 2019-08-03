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
            AddBubbleOfTypeAt(r, c, Random.Range(1, 9));
            c = (c + 1) % colNum;
            if (c == 0)
                r++;
        }
        //c = 0;

        //for (int i = 1; i <= colNum-1; i++)
        //{
        //    AddBubbleOfTypeAt(r, i, -1, true);
        //    c++;
        //}
    }


    void IncomingBubble(int r,int c,int score)
    {

      //  DeleteBubbleAt(r, c);
    }


    void CreateNewBubble(int r, int c, int score)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
     //   Debug.Log("score: " + score);
        AddBubbleOfTypeAt(r, c, score);
 
        neighbors= NeighborUtility.GetAllNeighbors(bubbleGrids, r, c, score, rowNum, colNum);

        if (neighbors.Count > 0)
        {
            neighbors.Add(new Vector2Int(c, r));
            int newScore = score;
            for (int i = 0; i < neighbors.Count-1; i++)
            {
                newScore++;
            }
            Vector2Int newBubblePosition = NeighborUtility.SearchInNodeNeighbors(neighbors, bubbleGrids, rowNum, colNum, newScore);
            MergeBubbles(neighbors, score);
          //  DeleteBubbleAt(newBubblePosition.y, newBubblePosition.x);
            CreateNewBubble(newBubblePosition.y, newBubblePosition.x, newScore);
        }

    }




    void MergeBubbles(List<Vector2Int> bubbles,int score)
    {
 
        for (int i = 0; i < bubbles.Count; i++)
        {
            DeleteBubbleAt(bubbles[i].y, bubbles[i].x);

        }
    }


void DeleteBubbleAt(int r, int c)
    {
        GameObject t = bubbleCoordinateMap[new Vector2Int(c, r)];
        bubblePool.ReturnToPool(t);
        bubbleCoordinateMap.Remove(new Vector2Int(c, r));
       
    }

    void AddBubbleOfTypeAt(int r, int c, int score)
    {
        bubbleCoordinateMap.Remove(new Vector2Int(c, r));
        GameObject t;
   
        t = bubblePool.GetFromPool(score);
       
        bubbleCoordinateMap.Add(new Vector2Int(c, r), t);
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
