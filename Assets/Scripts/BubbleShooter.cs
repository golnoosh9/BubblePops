using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public delegate void RetVoidArg3Int(int i1, int i2, int i3);
    public static event RetVoidArg3Int BubbleComingIn;
    Rigidbody2D rb;
    RectTransform rect;
    Vector2 currentVelocity;
    bool shoot = false;

    RaycastHit2D hitWall;
    RaycastHit2D hitBall;
    MoveBubble moveBubble;
    bool doneMove = false;

    private void Start()
    {
        moveBubble = FindObjectOfType<MoveBubble>();
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
            Debug.Log("draw ray  "+hitBall.collider.gameObject.name);
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

        if(hitWall.collider==null &&hitBall.collider!=null&&doneMove==false)
        {
            doneMove = true;
            // transform.position = Vector3.MoveTowards(transform.position,hitBall.transform.position, 2);
            moveBubble.StartMove(hitBall.transform.position);
            BubbleComingIn(hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().row, hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().column,
      moveBubble.GetComponent<BubbleDataID>().bubbleNumber);
        }
      
        //rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + currentVelocity * Time.deltaTime);
    }





    //private void OnTriggerEnter2D(Collider2D collision)
    //{
       
    //    if (collision.gameObject.tag == "Wall")
    //    {

    //        rb.velocity = new Vector2(-rb.velocity.x*1.1f, rb.velocity.y);
    //      //  Debug.Log("collide  "+cur);

    //    }
    //    else if (collision.gameObject.tag == "Bubble")
    //    {
    //        Debug.Log("trigger");
    //        rb.velocity = new Vector2(0, 0);
    //    }
    //}

    
}
