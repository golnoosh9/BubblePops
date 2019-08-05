using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BubblePool : MonoBehaviour
{
    List<GameObject> bubbles = new List<GameObject>();
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] RectTransform bubbleParent;
    public static Dictionary<int, Sprite> bubblePowerSprites = new Dictionary<int, Sprite>();
    int poolSize = 0;

    int initNum = 65;

    private void Awake()
    {
        bubblePowerSprites.Add(1, Resources.Load<Sprite>("2"));
        bubblePowerSprites.Add(2, Resources.Load<Sprite>("4"));
        bubblePowerSprites.Add(3, Resources.Load<Sprite>("8"));
        bubblePowerSprites.Add(4, Resources.Load<Sprite>("16"));
        bubblePowerSprites.Add(5, Resources.Load<Sprite>("32"));
        bubblePowerSprites.Add(6, Resources.Load<Sprite>("64"));
        bubblePowerSprites.Add(7, Resources.Load<Sprite>("128"));
        bubblePowerSprites.Add(8, Resources.Load<Sprite>("256"));
        bubblePowerSprites.Add(9, Resources.Load<Sprite>("512"));
        bubblePowerSprites.Add(10, Resources.Load<Sprite>("1024"));
        InitialWarmUp();
    }

    void InitialWarmUp()
    {

        for (int i = 0; i < initNum; i++)
        {
            int index = Random.Range(1, GameConstants.maximumBubbleIndex);
            GameObject tempg = Instantiate(bubblePrefab, bubbleParent);
            tempg.name = "Bubble_" + i;
            tempg.GetComponent<BubbleDataID>().SetNum(index);
            tempg.GetComponentInChildren<Image>().sprite = bubblePowerSprites[index];
            tempg.SetActive(false);
            bubbles.Add(tempg);
        }
        poolSize += initNum-1;

    }

    public GameObject GetFromPool()
    {
       
            if (poolSize >= 0)
            {
                GameObject t = bubbles[poolSize];
                poolSize--;
                return t;
            }
            InitialWarmUp();
            return bubbles[poolSize];
       
    }

    public GameObject GetFromPool(int bubbleValue)
    {
 
        if (poolSize >= 0)
        {
            GameObject t = bubbles[poolSize];
            poolSize--;
            t.GetComponent<BubbleDataID>().SetNum(bubbleValue);
            t.GetComponentInChildren<Image>().sprite = bubblePowerSprites[bubbleValue];
            t.GetComponentInChildren<Image>().enabled = true;
            t.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
            return t;
        }
        InitialWarmUp();

        bubbles[poolSize].GetComponent<BubbleDataID>().SetNum(bubbleValue);
        bubbles[poolSize].GetComponentInChildren<Image>().sprite = bubblePowerSprites[bubbleValue];
        return bubbles[poolSize];
    }
    public void ReturnToPool(GameObject b)
    {
       
            poolSize++;
        bubbles.Insert(poolSize, b);
        StartCoroutine(DeleteWithDelay(b));

    }


    IEnumerator DeleteWithDelay(GameObject b)
    {
        yield return new WaitForSeconds(0.8f);
        b.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, -1000);
        b.GetComponent<BubbleAnimator>().ResetAnimation();
        b.SetActive(false);
    }



}
