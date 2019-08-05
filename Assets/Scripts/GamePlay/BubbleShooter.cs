using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShooter : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField]RectTransform ghostCircleTransform;
    public delegate void RetVoidArg3Int(int i1, int i2, int i3,int i4);
    public static event RetVoidArg3Int BubbleArrived;

    public delegate void RetvoidArgBool(bool b);
    public event RetvoidArgBool BubbleMoveDone;
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
    MoveBubble moveBubble;
    BubbleDataID thisBubbleID;
    bool doneMove = false;
    bool startMove;
    bool canShoot = false;
    Camera mainCamera;
    Vector2 lastMouse = new Vector2();

    private void Start()
    {
        mainCamera = Camera.main;
        InGameNotification.GameStartNotification += EnableShooting;
        InGameNotification.GamePausednotification += DisableShooting;
        BubbleGrid.DoneMove += EnableShooting;
        thisBubbleID = GetComponentInChildren<BubbleDataID>();
        lineRenderer = GetComponent<LineRenderer>();
        ghostCircleTransform.gameObject.SetActive(false);
        middlePosition = transform.position;
      //  moveBubble = FindObjectOfType<MoveBubble>();
        rect = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        target = new List<Vector3>();
    }


    void GetTargetCoordinate()
    {
        if (hitBall.collider != null)
        {
            List<Vector2> bubbleDataPack= hitBall.collider.GetComponentInParent<BubbleDataID>().GetNeighbor(hitBall.point, transform.position.z);
            collidingCol = (int)bubbleDataPack[0].x;
            collidingRow = (int)bubbleDataPack[0].y;
            ghostCircleTransform.anchoredPosition = new Vector2(collidingCol * GameConstants.circleRadius * 2f - bubbleDataPack[1].x * GameConstants.circleRadius, 
            -collidingRow * (GameConstants.circleRadius * 2f) - GameConstants.circleRadius + bubbleDataPack[1].y);

            Collider2D col = Physics2D.OverlapCircle(ghostCircleTransform.transform.position, 0.4f);

           
            if (col != null && col.gameObject.layer==11)
            {
                target = new List<Vector3>();
            }
            else if(target.Count<2)
            {
                target.Add(ghostCircleTransform.transform.position);
            }


            if (collidingCol < 0 || collidingCol > 5)
            {
                target = new List<Vector3>();
            }



        }
    }
    void RayCastFromBall()
    {
       

        Vector2 t = mainCamera.ScreenToWorldPoint(Input.mousePosition) - rect.position;
        if (t.y > GameConstants.yThreshold)
            return;
        if (Vector2.Distance(lastMouse, t) > 0.01f )
        {
            target = new List<Vector3>();
            ghostCircleTransform.gameObject.SetActive(false);
            lineRenderer.enabled = false;
        }
        else
            return;
        lastMouse = t;
        hitBall = Physics2D.Raycast(transform.position, t, 200, 1 << 9 | 1 << 11);

        if (hitBall.collider != null && hitBall.collider.gameObject.layer == 9)
        {
            hitWall = hitBall;// Physics2D.Raycast(transform.position, t,100, 1 << 9 );
            hitBall = Physics2D.Raycast(hitWall.point, new Vector2(-t.x, t.y + 1), 200, 1 << 11);
            // Debug.Log("wall intersect: " + hitWall.point+"   "+ new Vector2(-t.x, t.y));
            if (hitBall.collider != null)
            {
                target.Add(new Vector3(hitWall.point.x, hitWall.point.y, transform.position.z));

            }

        }
    }

    void DrawShootingPath()
    {
        if (target.Count > 0)
        {

            lineRenderer.startColor = Color.green;
            lineRenderer.startWidth = 0.2f;
            lineRenderer.positionCount = target.Count + 1;
            lineRenderer.SetPosition(0, this.transform.position);
            for (int i = 0; i < target.Count; i++)
            {
                lineRenderer.SetPosition(i+1,target[i]);
            }


            ghostCircleTransform.gameObject.SetActive(true);
            lineRenderer.enabled = true;
 
        }
    }

    void MoveToTarget()
    {
        if (startMove == true && target.Count > 0)
        {
      
            transform.position = Vector3.MoveTowards(transform.position, target[0], 5 * Time.deltaTime);
        }

        if (startMove == true && target.Count > 0 && transform.position == target[0])
        {
            target.RemoveAt(0);
            startMove = false;
            if (target.Count == 0)
            {
                BubbleArrived(collidingRow, collidingCol, thisBubbleID.bubbleNumber, 0);
                hitBall = new RaycastHit2D();
                hitWall = new RaycastHit2D();
                AssignNewBubble();
            }
            else
                startMove=true;


        }

    }
    private void Update()
    {
        MoveToTarget();
        if (canShoot == false)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            startMove= false;
            shoot = true;
        }
        if (shoot)
        {
            RayCastFromBall();
            GetTargetCoordinate();
            DrawShootingPath();

            if (Input.GetMouseButtonUp(0))
            {
                DisableShooting();

                ghostCircleTransform.gameObject.SetActive(false);
                lineRenderer.enabled = false;
                shoot = false;
            
                startMove = true;
       

            }

        }

       

        //rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + currentVelocity * Time.deltaTime);
    }

    void EnableShooting()
    {
        canShoot = true;
        shoot = false;
    }



    void DisableShooting()
    {
      
        canShoot = false;
    }

    void AssignNewBubble()
    {

        transform.position = middlePosition;
        BubbleMoveDone(true);

    }


    private void OnDestroy()
    {
        InGameNotification.GameStartNotification -= EnableShooting;
        InGameNotification.GamePausednotification -= DisableShooting;
        BubbleGrid.DoneMove -= EnableShooting;
    }


}
