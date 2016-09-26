using UnityEngine;
using System.Collections;

public class FogFader : MonoBehaviour
{
    public float fadeOutStartDelayInSecs;
    public float fadeInStartDelayInSecs;

    private float onStartDensity;
    private float currentDensity;
    
    void Start()
    {
        onStartDensity = .05f;
        currentDensity = .05f;
    }
    
    void LateStart()
    {
        RenderSettings.fogDensity = .05f;
    }

    void Update()
    {
        //For testing
        /*
        if (Input.GetKeyUp(KeyCode.T))
        {
            FadeFogOut(2.5f);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            FadeFogIn(4.0f);
        }
        */
    }
    
    //Methods for fading out

    public void FadeFogOut(float fadeOutDurationInSecsArg)
    {
        //print("FadeFogOut() Duration:" + fadeDurationSecsArg.ToString());
        
        StopCoroutine("FadeIn");
        StopCoroutine("FadeOutDelayed");
        StopCoroutine("FadeInDelayed");
        StopCoroutine("FadeOut");

        StartCoroutine("FadeOut" ,fadeOutDurationInSecsArg);
    }
    IEnumerator FadeOut(float fadeOutDurationInSecsArg)
    {
        currentDensity = RenderSettings.fogDensity;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeOutDurationInSecsArg)
        {
            RenderSettings.fogDensity = Mathf.Lerp(onStartDensity, 0.0f, t);
            //Debug.Log("FadeOut RenderSettings.fogDensity:" + RenderSettings.fogDensity);
            yield return null;
        }
    }

    public void FadeFogOutDelayed(float fadeOutDurationOutSecsArg, float fadeOutStartDelayInSecsArg)
    {

        fadeOutStartDelayInSecs = fadeOutStartDelayInSecsArg;
        StopCoroutine("FadeIn");
        StopCoroutine("FadeOut");
        StartCoroutine("FadeIn", fadeOutStartDelayInSecs);
    }
    IEnumerator FadeOutDelayed(float fadeOutDurationInSecsArg)
    {
        yield return new WaitForSeconds(fadeOutStartDelayInSecs);
        currentDensity = RenderSettings.fogDensity;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeOutDurationInSecsArg)
        {
            RenderSettings.fogDensity = Mathf.Lerp(currentDensity, onStartDensity, t);
            //Debug.Log("FadeIn RenderSettings.fogDensity:" + RenderSettings.fogDensity);
            yield return null;

        }
    }

    //Methods for fading in


    public void FadeFogIn(float fadeInDurationInSecsArg)
    {
        StopCoroutine("FadeIn");
        StopCoroutine("FadeOut");
        StartCoroutine("FadeIn", fadeInDurationInSecsArg);
    }
    IEnumerator FadeIn(float fadeInDurationInSecsArg)
    {
        currentDensity = RenderSettings.fogDensity;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeInDurationInSecsArg)
        {
            RenderSettings.fogDensity = Mathf.Lerp(0.0f, onStartDensity, t);
            //Debug.Log("FadeIn RenderSettings.fogDensity:" + RenderSettings.fogDensity);
            yield return null;

        }
    }

    public void FadeFogInDelayed(float fadeInDurationInSecsArg, float fadeInStartDelayInSecsArg)
    {

        fadeInStartDelayInSecs = fadeInStartDelayInSecsArg;
        StopCoroutine("FadeIn");
        StopCoroutine("FadeOut");
        StartCoroutine("FadeIn", fadeInStartDelayInSecs);
    }

    IEnumerator FadeInDelayed(float fadeInDurationInSecsArg)
    {
        yield return new WaitForSeconds(fadeInStartDelayInSecs);
        currentDensity = RenderSettings.fogDensity;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / fadeInDurationInSecsArg)
        {
            RenderSettings.fogDensity = Mathf.Lerp(currentDensity, onStartDensity, t);
            //Debug.Log("FadeIn RenderSettings.fogDensity:" + RenderSettings.fogDensity);
            yield return null;
        }
    }
}