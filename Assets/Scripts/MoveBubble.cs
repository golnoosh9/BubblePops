using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBubble : MonoBehaviour
{
    BubbleDataID bubbleDataID;
    bool isConnected = false;
    bool move;
    Vector3 target;

    private void Start()
    {

    }
    private void OnEnable()
    {
        bubbleDataID = GetComponent<BubbleDataID>();
        BubblesGridBasedMove.CheckForFalling += CheckForFalling;
        BubblesGridBasedMove.ConnectedBubble += SetToConnected;
        BubblesGridBasedMove.ScrollUp += MoveUp;
        BubblesGridBasedMove.ScrollDown += MoveDown;

    }

    void SetToConnected(int r, int c)
    {
        if (r == bubbleDataID.row && c ==bubbleDataID.column)
            isConnected = true;
    }

    void CheckForFalling(int dummy1, int dummy2)
    {
        if (isConnected == false)
        {
//            Debug.Log("is falling:  " + name);
            move = true;
            target = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
            //fall down
        }
        if (GetComponent<Animator>() != null)
            isConnected = false;
    }

    void MoveUp()
    {
      //  move = true;
        target = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        bubbleDataID.ModifyHeightOffset(-1);
        bubbleDataID.SetPosition(bubbleDataID.row - 1, bubbleDataID.column,true);

    }

    void MoveDown()
    {
        move = true;
        target = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
       // bubbleDataID.SetCoordinate(bubbleDataID.row + 1, bubbleDataID.column);
    }

    private void Update()
    {
        if (move == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target,1*Time.deltaTime);
            if (transform.position == target)
                move = false;
        }
    }


    private void OnDisable()
    {
        BubblesGridBasedMove.CheckForFalling -= CheckForFalling;
        BubblesGridBasedMove.ConnectedBubble -= SetToConnected;
        BubblesGridBasedMove.ScrollUp -= MoveUp;
        BubblesGridBasedMove.ScrollDown -= MoveDown;
    }
}
