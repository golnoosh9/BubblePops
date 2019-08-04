using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioSound
{
    Shrink,
    Shake,
    Create
}
public class AudioManager : MonoBehaviour
{
     AudioSource source;
     Dictionary<string, AudioClip> clips= new Dictionary<string, AudioClip>();


    private void Awake()
    {
        source = GetComponent<AudioSource>();
        clips.Add("Shrinking", Resources.Load<AudioClip>("Sounds/Shrink"));
        clips.Add("Shake", Resources.Load<AudioClip>("Sounds/Shake"));
        clips.Add("Enlarge", Resources.Load<AudioClip>("Sounds/Create"));
        BubbleGrid.BubbleActivityEvent += ParseBubbleActivity;
    }


    void ParseBubbleActivity(int dummy, int dummy2, int dummy3,string activityType)
    {
        if(activityType=="Shrinking"|| activityType=="Shake" || activityType=="Enlarge")
        source.PlayOneShot(clips[activityType]);
    }


    private void OnDestroy()
    {
        BubbleGrid.BubbleActivityEvent += ParseBubbleActivity;
    }



}
