using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDataID : MonoBehaviour
{
    public int bubbleNumber { get; private set; }
    public int row { get; private set; }
    public int column { get; private set; }
    float thisRadius=150f;
    float rowOffset = 0;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
       // thisRadius = GetComponentInChildren<CircleCollider2D>().radius;

        row = -1;
        column = -1;
    }

    public void SetNum(int num)
    {
        bubbleNumber = num;
    }

    public void SetPosition(int r, int c)
    {
        row = r;
        column = c;
        if (r % 2 == 1)
            rowOffset = 1;
        Debug.Log(thisRadius.ToString());
      //  rect.anchoredPosition = new Vector3(0, 0, 0);
        rect.anchoredPosition = new Vector3(c * thisRadius*2f-rowOffset*thisRadius, -r*(thisRadius*2f)-thisRadius,0 );
    }
}
