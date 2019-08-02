using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShooter : MonoBehaviour
{
    public delegate void RetVoidArg3Int(int i1, int i2, int i3);
    public static event RetVoidArg3Int BubbleComingIn;
    public static event RetVoidArg3Int BubbleArrived;
    Rigidbody2D rb;
    RectTransform rect;
    Vector2 currentVelocity;
    bool shoot = false;
    Vector3 middlePosition;
    int collidingRow;
    int collidingCol;

    RaycastHit2D hitWall;
    RaycastHit2D hitBall;
    MoveBubble moveBubble;
    int currentBubbleNumber=1;
    bool doneMove = false;

    private void Start()
    {
        middlePosition = transform.position;
      //  moveBubble = FindObjectOfType<MoveBubble>();
        rect = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shoot = true;
        }
        if (shoot)
        {
            //  Touch touch = Input.GetTouch(0);
            Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition)-rect.position;
            hitBall = Physics2D.Raycast(transform.position, t,100,1<<10);//check for empty ball
//            Debug.Log("draw ray  "+hitBall.collider.gameObject.name);
          //  Debug.DrawRay(transform.position, t, Color.red,100);
            if (hitBall.collider == null)
            {
                hitWall = Physics2D.Raycast(transform.position, t,100, 1 << 9);
                hitBall = Physics2D.Raycast(hitWall.transform.position, new Vector2(-t.x, t.y),100, 1 << 10);
            }
            //currentVelocity = t;
            //rb.velocity = t;
            shoot = false;
        }
    

        if (hitWall.collider==null &&hitBall.collider!=null)
        {
            if(doneMove==false)
            {
                collidingRow = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().row;
                collidingCol = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().column;
                BubbleComingIn(collidingRow,collidingCol,currentBubbleNumber);
            }

            doneMove = true;
            transform.position = Vector3.MoveTowards(transform.position, hitBall.transform.position, 2 * Time.deltaTime);
            // transform.position = Vector3.MoveTowards(transform.position,hitBall.transform.position, 2);
          //  moveBubble.StartMove(hitBall.transform.position);
           
        }
        if(hitBall.collider!=null && transform.position== hitBall.transform.position)
        {
            BubbleArrived(collidingRow, collidingCol, currentBubbleNumber);
            hitBall = new RaycastHit2D();
            hitWall = new RaycastHit2D();
            doneMove = false;
            AssignNewBubble();
        }

        //rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + currentVelocity * Time.deltaTime);
    }

    void AssignNewBubble()
    {

        transform.position = middlePosition;
        currentBubbleNumber = Random.Range(1, 8);
        GetComponentInChildren<Image>().sprite = BubblePool.bubblePowerSprites[currentBubbleNumber];

    }


}
