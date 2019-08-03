﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShooter : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField]Transform ghostCircleTransform;
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

    public List<Vector3> target;
    bool reachedTarget;
    MoveBubble moveBubble;
    int currentBubbleNumber=1;
    bool doneMove = false;
    bool startMove;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
            lineRenderer.enabled = true;
            
            reachedTarget = true;
            shoot = true;
        }
        if (shoot)
        {
          //  ghostCircleTransform.gameObject.SetActive(false);
            target = new List<Vector3>();
            Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition)-rect.position;
            hitBall = Physics2D.Raycast(transform.position, t,100,1<<11 | 1<<9);

            if (hitBall.collider!=null && hitBall.collider.gameObject.layer == 9)
            {
                hitWall = hitBall;// Physics2D.Raycast(transform.position, t,100, 1 << 9 );
                hitBall = Physics2D.Raycast(hitWall.point, new Vector2(-t.x, t.y),100, 1 << 11);
                // Debug.Log("wall intersect: " + hitWall.point+"   "+ new Vector2(-t.x, t.y));
                if (hitBall.collider != null)
                {
                    target.Add(new Vector3(hitWall.point.x, hitWall.point.y, transform.position.z));
          
                }

            }
            if(hitBall.collider!=null)
            {
                target.Add(hitBall.collider.GetComponentInParent<BubbleDataID>().GetNeighbor(hitBall.point,transform.position.z));
       
            }

            if(target.Count>0)
            {
                ghostCircleTransform.gameObject.SetActive(false);
                lineRenderer.startColor = Color.green;
                lineRenderer.startWidth = 0.2f;
                lineRenderer.positionCount = target.Count + 1;
                lineRenderer.SetPosition(0, this.transform.position);
                for (int i = 0; i < target.Count; i++)
                {

                    lineRenderer.SetPosition(i+1, target[i]);
                 
                }
                ghostCircleTransform.gameObject.SetActive(true);
                ghostCircleTransform.position = target[target.Count - 1];
            }




            if (Input.GetMouseButtonUp(0)&& target.Count>0)
            {
                ghostCircleTransform.gameObject.SetActive(false);
                lineRenderer.enabled = false;
                reachedTarget = false;
                shoot = false;
                startMove = true;
                collidingRow = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().GetNeighborCoord().y;
                collidingCol = hitBall.collider.gameObject.GetComponentInParent<BubbleDataID>().GetNeighborCoord().x;
                BubbleComingIn(collidingRow, collidingCol, currentBubbleNumber);
            }

        }
    


        if(reachedTarget==false && target.Count>0)
        {
//            Debug.Log("moving:  " + rect.anchoredPosition + "    " + target[0]);
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
