using System.Collections;
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
    BubblePool bubblePool;
    int initialBubbleNum = 6*7;
    bool mergeDone = false;
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

    }


    void IncomingBubble(int r,int c,int score)
    {


    }


    void CreateNewBubble(int r, int c, int score)
    {

        List<Vector2Int> neighbors = new List<Vector2Int>();
     //   Debug.Log("score: " + score);
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
      //  Debug.Log(ToString());

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
            StartCoroutine(DeleteBubbleAt(neighbors[i].y, neighbors[i].x));

        }
        yield return new WaitForSeconds(1.5f);
        CreateNewBubble(newBubblePosition.y, newBubblePosition.x, newScore);
        Debug.Log(ToString());
        BubblesGridBasedMove.CheckForFallingBubbles(bubbleGrids, rowNum, colNum);
        BubbleActivityEvent(newBubblePosition.y, newBubblePosition.x, NeighborUtility.chainRounds, "Enlarge");

    }






    IEnumerator DeleteBubbleAt(int r, int c)
    {
        GameObject t = bubbleCoordinateMap[new Vector2Int(c, r)];
        BubbleActivityEvent(r, c, NeighborUtility.chainRounds, "Shrink");
        yield return new WaitForSeconds(1.01f);
        bubblePool.ReturnToPool(t);
        bubbleCoordinateMap.Remove(new Vector2Int(c, r));
        bubbleGrids[r, c] = 0;

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
