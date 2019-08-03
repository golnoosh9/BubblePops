using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleDataID : MonoBehaviour
{
    public int bubbleNumber { get; private set; }
    public int row { get; private set; }
    public int column { get; private set; }
    int thisRadius=150;
    int rowOffset = 0;
    int lastNeighborR;
    int lastNeighborC;
    RectTransform rect;
    float heightOffset;
    bool fallDown;
    bool isConnected = false;

    private void Awake()
    {
        if (GetComponent < Animator >()== null)
            isConnected = true;
        rect = GetComponent<RectTransform>();
        // thisRadius = GetComponentInChildren<CircleCollider2D>().radius;
        heightOffset = 1 *( 2 * thisRadius);
        row = -1;
        column = -1;
    }

    private void OnEnable()
    {
        BubblesGridBasedMove.CheckForFalling += CheckForFalling;
        BubblesGridBasedMove.ConnectedBubble += SetToConnected;
    }

    void SetToConnected(int r, int c)
    {
        if(r==row && c==column)
          isConnected = true;
    }

    void CheckForFalling(int dummy1, int dummy2)
    {
        if(isConnected==false)
        {
            Debug.Log("is falling:  " + name);
            fallDown = true;
            //fall down
        }
        if(GetComponent<Animator>() != null)
        isConnected = false;
    }

    private void Update()
    {
        if(fallDown==true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), 1);
        }
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
        rect.anchoredPosition = new Vector3(column * thisRadius*2f-rowOffset*thisRadius, -row*(thisRadius*2f)-thisRadius+heightOffset,0 );
    }

    public Vector3 GetNeighbor(Vector2 collisionPoint,float z)
    {
     //   GetComponentInChildren<Image>().enabled = false;
        Vector3 emptyNeighborPosition = transform.position;
        emptyNeighborPosition.z = z;
        lastNeighborR = row;
        lastNeighborC = column;
        int thisOffset=rowOffset;
        Vector2 centeredCollisionPoint = collisionPoint - new Vector2(transform.position.x,transform.position.y);
        if(centeredCollisionPoint.y<0)
        {
            thisOffset = Mathf.Abs(rowOffset - 1);
            lastNeighborR = row + 1;
            emptyNeighborPosition.y -= 1;
        }

        if(centeredCollisionPoint.x>0)
        {

            lastNeighborC= column + 1 - rowOffset;
            emptyNeighborPosition.x += 0.5f;
        }
        if (centeredCollisionPoint.x < 0)
        {
            lastNeighborC= column  - rowOffset;
            emptyNeighborPosition.x-=0.5f;// - thisRadius * 2;
        }


        return emptyNeighborPosition;
    }

    public Vector2Int GetNeighborCoord()
    {
        return new Vector2Int(lastNeighborC, lastNeighborR);
    }

    private void OnDisable()
    {
        BubblesGridBasedMove.CheckForFalling -= CheckForFalling;
        BubblesGridBasedMove.ConnectedBubble -= SetToConnected;
    }
}
