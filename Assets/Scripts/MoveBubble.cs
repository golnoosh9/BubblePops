using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBubble : MonoBehaviour
{
    bool isMoving = false;
    Vector3 target;
    RectTransform rect;
    // Start is called before the first frame update
    public void StartMove(Vector3 t)
    {
        target = t;
        isMoving = true;
    }

    private void Update()
    {
        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 2*Time.deltaTime);
          
        }
    }


    //IEnumerator MoveTwords(Vector3 t)
    //{
    //    while(t!=transform.position)
    //    {
    //        transform.position = Vector3.Lerp(transform.position, t, 1f);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}
}
