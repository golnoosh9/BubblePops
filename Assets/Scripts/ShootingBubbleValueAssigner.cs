using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingBubbleValueAssigner : MonoBehaviour
{
    [SerializeField] Image upcomingBubble;
    Image thisImage;
    BubbleDataID thisData;
    int upcomingVal;
    Animator animator;
    Vector3 middleTransform;
    bool move = false;

    // Start is called before the first frame update
    void Start()
    {
        middleTransform = this.transform.position;
        upcomingVal = Random.Range(1,7);

        GetComponent<BubbleShooter>().BubbleMoveDone += StartNewShootingRound;
        thisData = GetComponentInChildren<BubbleDataID>();
        thisImage = GetComponentInChildren<Image>();
        StartNewShootingRound(false);
    }



    void StartNewShootingRound(bool moveBubble)
    {
      
        if (moveBubble == true)
        {
            transform.position = upcomingBubble.transform.position;
            move = true;
        }
        AssignValueToBubble(thisImage);
        thisData.SetNum(upcomingVal);
        upcomingVal = Random.Range(1, 7);
       // Debug.Log("inde: " + upcomingVal);
        AssignValueToBubble(upcomingBubble);
    }

    void AssignValueToBubble(Image b)
    {
        b.sprite = BubblePool.bubblePowerSprites[upcomingVal];

    }


    private void Update()
    {
        if(move)
        {
            transform.position = Vector3.MoveTowards(transform.position, middleTransform, 2 * Time.deltaTime);
            if (transform.position == middleTransform)
                move = false;
        }
    }


    private void OnDestroy()
    {
        GetComponent<BubbleShooter>().BubbleMoveDone -= StartNewShootingRound;
    }
}
