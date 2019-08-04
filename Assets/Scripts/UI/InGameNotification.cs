using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameNotification : MonoBehaviour
{
    [SerializeField] Text chainNumber;
    [SerializeField] Image barScore;
    [SerializeField] Text scoreText;

    int maxScore = 5000;
    int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        BubbleGrid.BubbleActivityEvent += ActivateChainNum;
    }



    void ActivateChainNum(int dummy1, int dummy2, int chainNum, string activityType)
    {
        if(activityType=="Enlarge")
        {
            Debug.Log("in UI: " + chainNum);
            if(chainNum>1)
            {
                chainNumber.text = "x" + chainNum;
                StartCoroutine( DisplayChainNum());
            }
        }

        if(activityType=="Score")
        {
            totalScore += chainNum;
            barScore.fillAmount = (float)totalScore / maxScore;
            scoreText.text = totalScore.ToString();
        }
    }

    IEnumerator DisplayChainNum()
    {
        chainNumber.enabled = true;
        yield return new WaitForSeconds(2);
        chainNumber.enabled = false;
    }


    private void OnDestroy()
    {
        BubbleGrid.BubbleActivityEvent -= ActivateChainNum;
    }
}
