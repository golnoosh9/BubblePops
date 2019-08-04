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
        InGameNotification.SoundToggleEvent += ToggleMute;
        InGameNotification.GameStartNotification += StartGame;
        source = GetComponent<AudioSource>();
        clips.Add("Shrinking", Resources.Load<AudioClip>("Sounds/Shrink"));
        clips.Add("Shake", Resources.Load<AudioClip>("Sounds/Shake"));
        clips.Add("Create", Resources.Load<AudioClip>("Sounds/Create"));
        BubbleGrid.BubbleActivityEvent += ParseBubbleActivity;
    }


    void ParseBubbleActivity(int dummy, int dummy2, int dummy3,string activityType)
    {
        if(activityType=="Shrinking"|| activityType=="Shake" || activityType=="Create")
        source.PlayOneShot(clips[activityType]);
    }

    void ToggleMute()
    {
        source.enabled = !source.isActiveAndEnabled;
    }

    void StartGame()
    {
        source.PlayOneShot(clips["Create"]);
    }



    private void OnDestroy()
    {
        BubbleGrid.BubbleActivityEvent -= ParseBubbleActivity;
        InGameNotification.SoundToggleEvent -= ToggleMute;
        InGameNotification.GameStartNotification -= StartGame;
    }



}
