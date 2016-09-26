using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{

    public AudioMixer masterMixer;
    public AudioMixerSnapshot[] snapshots;
    public float[] weights;
    public AudioSource productWorldAudioSource;
    Coroutine unpauseDelayTimed = null;
    bool isDelayed = true;
    //transitionType:
    // 0 = product world to retail 
    // 1 = retail to product world

    void Start()
    {

        productWorldAudioSource.Play();
        productWorldAudioSource.Pause();
        
    }

    public void audioTransition(int transitionType, float transitionTime)
    {
        Debug.Log("Audio transition type: " + transitionType);
        switch (transitionType) {
            case 0:
                snapshots[0].TransitionTo(transitionTime);
                break;
            case 1:
                snapshots[1].TransitionTo(transitionTime);
                break;
        }
    }

    public void playProductWorldAudio(string productType)
    {
        switch (productType)
        {
            case "AvocadoProduct":
                
                    if (productWorldAudioSource.tag == productType && isDelayed == true)
                    {
                    unpauseDelayTimed =  StartCoroutine(unpauseDelayed());
                    isDelayed = false;
                }
                break;
        }
    }

    public void pauseProductWorldAudio(string productType) {
        switch (productType)
        {
            case "AvocadoProduct":

                if (productWorldAudioSource.tag == productType)
                {
                    productWorldAudioSource.Pause();
                }
                break;
        }

    }
    IEnumerator unpauseDelayed()
    {
        yield return new WaitForSeconds(1f);
        productWorldAudioSource.UnPause();
        isDelayed = true;
    }
}


