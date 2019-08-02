using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGrid : MonoBehaviour
{
    Dictionary<Vector2, GameObject> bubbleCoordinateMap = new Dictionary<Vector2, GameObject>();
    int[,] bubbleGrids = new int[10,6];
    int rowNum = 10;
    int colNum = 6;
    BubblePool bubblePool;
    int initialBubbleNum = 30;
    // Start is called before the first frame update
    void Start()
    {
        BubbleShooter.BubbleComingIn += IncomingBubble;
        bubblePool = FindObjectOfType<BubblePool>();
        GenerateBubbles();
    }

    void GenerateBubbles()
    {
        int r = 0;
        int c = 0;
        for (int i = 0; i < initialBubbleNum; i++)
        {
            GameObject t = bubblePool.GetFromPool(false);
            t.GetComponent<BubbleDataID>().SetPosition(r, c);
            bubbleCoordinateMap.Add(new Vector2(r, c), t);
            bubbleGrids[r, c] = t.GetComponent<BubbleDataID>().bubbleNumber;
            t.SetActive(true);
            c = (c + 1) % colNum;
            if (c == 0)
                r++;
        }
        c = 0;

        for (int i = 1; i <= colNum-1; i++)
        {
            GameObject t = bubblePool.GetFromPool(true);
            bubbleCoordinateMap.Add(new Vector2(r, i), t);
            t.GetComponent<BubbleDataID>().SetPosition(r, i);
            bubbleGrids[r, i] = 0;
            t.SetActive(true);
            c++;
        }
    }


    void IncomingBubble(int r,int c,int score)
    {
        GameObject t = bubbleCoordinateMap[new Vector2(r, c)];
        bubblePool.ReturnToPool(t, true);
        bubbleGrids[r, c] = score;
    }


    private void OnDestroy()
    {
        BubbleShooter.BubbleComingIn -= IncomingBubble;
    }
}
