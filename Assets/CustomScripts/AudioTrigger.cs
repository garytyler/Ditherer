using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
{
    AudioSource audio;
    Renderer cubeRenderer;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        cubeRenderer = GetComponentInChildren<Renderer>();
    }

    void Update() {
        //if (audio.isPlaying == true)
        //{
            //cubeRenderer.material.color = Color.green;
        //}
        //else {
            //cubeRenderer.material.color = Color.gray;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (audio.isPlaying == false)
        {
            if (other.tag == "MainCamera")
            {
                audio.Play();
            }
        }
    }

  /*  void OnTriggerExit(Collider other)
    {
            if (other.tag == "MainCamera")
            {
             //   audio.Stop();
            }
    }*/
    
}