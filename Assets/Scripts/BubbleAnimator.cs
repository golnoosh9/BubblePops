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
        //bubbleData = GetComponent<BubbleDataID>();
        //animator = GetComponent<Animator>();
        //BubbleGrid.BubbleActivityEvent += CheckForAnimation;
        // animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {

        BubbleGrid.BubbleActivityEvent += CheckForAnimation;
        bubbleData = GetComponent<BubbleDataID>();
        animator = GetComponent<Animator>();
        //  animator.SetTrigger("Shake");
    }


    void CheckForAnimation(int r, int c, int dummy, string animationTrigger)
    {

        if (bubbleData.row == r && bubbleData.column == c)
        {

            animator.SetTrigger(animationTrigger);
        }
    }


    public void ResetAnimation()
    {
        animator.SetTrigger("Idle");
    }
    private void OnDisable()
    {
        BubbleGrid.BubbleActivityEvent -= CheckForAnimation;
    }

    private void OnDestroy()
    {
        BubbleGrid.BubbleActivityEvent -= CheckForAnimation;
    }
}
