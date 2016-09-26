using UnityEngine;
using System.Collections;

using RenderHeads.Media.AVProVideo;

public class MediaPlayerController : MonoBehaviour {

    public MediaPlayer mp;


    float currentSceneTimeAvocado = 0.0f;
    float currentSceneTimeCarVisor = 0.0f;
    float currentSceneTimeNailPolish = 0.0f;
    float currentSceneTimeUniverseBluRay = 0.0f;
    float currentSceneTimeJetLagPill = 0.0f;
    float currentSceneTimeBalanceBar = 0.0f;
    float currentSceneTimeCandle = 0.0f;
    
    // Use this for initialization
    void Start ()
    {
        MediaPlayer mp = GetComponent<MediaPlayer>();
        mp.Events.AddListener(OnVideoEvent);

    }
/*
    public void LoadScene(string productType)
    {
        switch (productType)
        {
            case "AvocadoProduct":
                if (mp.Control.CanPlay() == true)
                {
                    mp.Control.Play();
                }
                break;
            case "OutOfStock":
                {
                    
                }

          
        }
    }

    public void UnloadScene()
    {
        mp.CloseVideo(
    }
*/

    public void PlayScene(string productType)
    {
    //    switch (productType)
    //    {
    //        case "AvocadoProduct":
                if (mp.Control.CanPlay() == true)
                {
                    mp.Control.Play();
                }
    //            break;
    //   }     
    }

    public void PauseScene(string productType)
    {
        switch (productType)
        {
            case "AvocadoProduct":
                if (mp.Control.IsPlaying() == true)
                {
                    mp.Control.Pause();
                    currentSceneTimeAvocado = mp.Control.GetCurrentTimeMs();
                    Debug.Log("currentSceneTimeAvocado" + "is set to" + currentSceneTimeAvocado.ToString());
                }
            break;
        }
    }


// Callback function to handle events
    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et,
ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                mp.Control.Play();
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                Debug.Log("First frame ready");
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                mp.Control.Rewind();
                break;
        }
        Debug.Log("Event: " + et.ToString());
    }


    public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                print("First Frame Is Ready");
                break;
            case MediaPlayerEvent.EventType.Started:
                print("Playback Started");
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                print("Finished Playing");
                break;
        }
    }


	// Update is called once per frame
	void Update () {
	


	}
}
