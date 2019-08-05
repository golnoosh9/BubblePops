using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleDataID : MonoBehaviour
{
    public static int offsetSwitch=1;
    public delegate void RetVoidArgInt(int i);
    public int bubbleNumber { get; private set; }
    public int row { get; private set; }
    public int column { get; private set; }
    public int offset;
    int rowOffset = 0;
  
    RectTransform rect;
    float heightOffset;
    Vector2 target;
    bool move;
 


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        heightOffset = 1 *( 2 * GameConstants.circleRadius);
        row = -1;
        column = -1;
    }

    private void Update()
    {
        if(move==true)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, target, 1000 * Time.deltaTime);
            if (rect.anchoredPosition == target)
                move = false;
        }
    }

    public void SetNum(int num)
    {
        bubbleNumber = num;
    }

    public static void SwitchOffset()
    {
        offsetSwitch = Mathf.Abs(1 - offsetSwitch);
    }

    public void SetPosition(int r, int c, bool smooth)
    {
        rowOffset = 0;
        row = r;
        column = c;

        if (row % 2 == offsetSwitch )
            rowOffset = 1;
        offset = rowOffset;
        target = new Vector2(column * GameConstants.circleRadius * 2f - rowOffset * GameConstants.circleRadius, 
        -row * (GameConstants.circleRadius * 2f) - GameConstants.circleRadius + heightOffset);
        if (smooth)
        {
            move = true;

        }
        else
            rect.anchoredPosition = target;
    }

    public List<Vector2> GetNeighbor(Vector2 collisionPoint,float z)
    {
        List<Vector2> returningVals = new List<Vector2>();
        Vector2 emptyNeighborPosition = transform.position;
        int lastNeighborR = row;
        int lastNeighborC = column;
        int thisOffset=rowOffset;
        Vector2 centeredCollisionPoint = collisionPoint - new Vector2(transform.position.x,transform.position.y);
        if(centeredCollisionPoint.y<0)
        {
            thisOffset = Mathf.Abs(rowOffset - 1);
            lastNeighborR = row + 1;
        }

        if(centeredCollisionPoint.x>0)
        {

            lastNeighborC= column + 1 - rowOffset;
        }
        if (centeredCollisionPoint.x < 0)
        {
            lastNeighborC= column  - rowOffset;
        }
        returningVals.Add(new Vector2(lastNeighborC, lastNeighborR));
        returningVals.Add(new Vector2(thisOffset, heightOffset));
        return returningVals;
    }

}
