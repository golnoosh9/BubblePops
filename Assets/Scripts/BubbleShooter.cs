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

    List<Vector3> target;
    bool reachedTarget;
    MoveBubble moveBubble;
    int currentBubbleNumber=1;
    bool doneMove = false;
    bool startMove;

    private void Start()
    {
        middlePosition = transform.position;
      //  moveBubble = FindObjectOfType<MoveBubble>();
        rect = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        target = new List<Vector3>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            reachedTarget = true;
            shoot = true;
        }
        if (shoot)
        {
            target = new List<Vector3>();
            Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition)-rect.position;
            hitBall = Physics2D.Raycast(transform.position, t,100,1<<10);//check for empty ball
//            Debug.Log("draw ray  "+hitBall.collider.gameObject.name);
          //  Debug.DrawRay(transform.position, t, Color.red,100);
            if (hitBall.collider == null)
            {
                hitWall = Physics2D.Raycast(transform.position, t,100, 1 << 9);
                hitBall = Physics2D.Raycast(hitWall.point, new Vector2(-t.x, t.y),100, 1 << 10);
                Debug.Log("wall intersect: " + hitWall.point+"   "+ new Vector2(-t.x, t.y));
                if(hitBall.collider!=null)
                target.Add(new Vector3(hitWall.point.x,hitWall.point.y,transform.position.z));
            
            }
                if(hitBall.collider!=null)
                 target.Add(hitBall.collider.transform.position);



            if(Input.GetMouseButtonUp(0)&& target.Count>0)
            {
                reachedTarget = false;
                shoot = false;
                startMove = true;
                collidingRow = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().row;
                collidingCol = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().column;
                BubbleComingIn(collidingRow, collidingCol, currentBubbleNumber);
            }

        }
    


        if(reachedTarget==false && target.Count>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target[0], 2 * Time.deltaTime);
        }

        if (reachedTarget==false &&target.Count>0 && transform.position== target[0])
        {
            target.RemoveAt(0);
            reachedTarget = true;
            if (target.Count == 0)
            {
                BubbleArrived(collidingRow, collidingCol, currentBubbleNumber);
                hitBall = new RaycastHit2D();
                hitWall = new RaycastHit2D();
                AssignNewBubble();
            }
            else
                reachedTarget = false;


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
