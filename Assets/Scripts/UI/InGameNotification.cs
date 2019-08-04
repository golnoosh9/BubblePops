using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameNotification : MonoBehaviour
{
    public delegate void RetvoidArgVoid();
    public static event RetvoidArgVoid SoundToggleEvent;
    public static event RetvoidArgVoid GameStartNotification;
    [SerializeField] Text chainNumber;
    [SerializeField] Image barScore;
    [SerializeField] Text scoreText;
    [SerializeField] Text soundText;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject retryMenu;

    int maxScore = 5000;
    int totalScore = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        BubbleGrid.BubbleActivityEvent += ActivateChainNum;
    }



    void ActivateChainNum(int dummy1, int dummy2, int chainNum, string activityType)
    {
        if(activityType=="Finish")
        {
            Time.timeScale = 0;
            retryMenu.SetActive(true);
        }
        if (activityType=="Enlarge")
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

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        GameStartNotification();
        mainMenu.SetActive(false);

    }

    public void SoundToggle()
    {
        SoundToggleEvent();
        if (soundText.text == "Mute")
            soundText.text = "Unmute";
        else
            soundText.text = "Mute";
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
