using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDataID : MonoBehaviour
{
    public int bubbleNumber { get; private set; }
    public int row { get; private set; }
    public int column { get; private set; }
    int thisRadius=150;
    int rowOffset = 0;
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
        rowOffset = 0;
        row = r;
        column = c;
        if (row % 2 == 1)
            rowOffset = 1;

      //  rect.anchoredPosition = new Vector3(0, 0, 0);
        rect.anchoredPosition = new Vector3(column * thisRadius*2f-rowOffset*thisRadius, -row*(thisRadius*2f)-thisRadius,0 );
    }
}
