using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAnimator : MonoBehaviour
{
    public delegate void RetVoidArgInt2(int i, int j);
    public static event RetVoidArgInt2 DoneShrinking;
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

    public void FinishShrinking()
    {
        DoneShrinking(bubbleData.row,bubbleData.column);
    }
}
