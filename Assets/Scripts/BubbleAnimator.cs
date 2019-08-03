using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAnimator : MonoBehaviour
{
    Animator animator;
    BubbleDataID bubbleData;
    // Start is called before the first frame update
    void Start()
    {
        bubbleData = GetComponent<BubbleDataID>();
        BubbleGrid.BubbleActivityEvent += CheckForAnimation;
        animator = GetComponent<Animator>();
    }


    void CheckForAnimation(int r, int c, int dummy, string animationTrigger)
    {
        if (bubbleData.row == r && bubbleData.column == c)
            animator.SetTrigger(animationTrigger);
    }



    private void OnDestroy()
    {
        BubbleGrid.BubbleActivityEvent -= CheckForAnimation;
    }
}
