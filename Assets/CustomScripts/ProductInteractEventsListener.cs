using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenderHeads.Media.AVProVideo;

namespace VRTK
{
    public class ProductInteractEventsListener : MonoBehaviour
    {
        // Variables for Transition Elements and/or Components
        MediaPlayerController mediaPlayerController;
        MediaPlayer mp; //Use only for closing/exiting states
        //MediaPlayer.FileLocation fl;
        FogFader fogFader;
        RendererEnabler retailPeripheryEnabler;
        ProductInfo productInfo;
        AudioMixerController audioMixerController;

        bool holdingThis = false;
        Renderer thisRenderer;

        //   List<Renderer> thisRenderers;

        // Use this for initialization
        void Start()
        {

            thisRenderer = this.GetComponent<Renderer>();
            
            //fl = new MediaPlayer.FileLocation();
            productInfo = GetComponent<ProductInfo>();

            // Initialization of Variables for Transition Elements and/or Components
            mediaPlayerController = GameObject.Find("AVPro Video Media Player").GetComponent<MediaPlayerController>();
            mp = GameObject.Find("AVPro Video Media Player").GetComponent<MediaPlayer>();
            fogFader = mediaPlayerController.gameObject.GetComponent<FogFader>();
            retailPeripheryEnabler = GameObject.FindGameObjectWithTag("RetailPeriphery").GetComponent<RendererEnabler>();
            audioMixerController = GameObject.Find("Audio").GetComponent<AudioMixerController>();
            //Setup controller event listeners
            GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += DoInteractableObjectTouched;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += DoInteractableObjectUntouched;
            GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += DoInteractableObjectGrabbed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += DoInteractableObjectUngrabbed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += DoInteractableObjectUsed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += DoInteractableObjectUnused;
        }

        void LateUpdate()
        {
            if (holdingThis == true)
            {
                thisRenderer.enabled = true;
            }


        }

        // Product interaction events
        void DoInteractableObjectTouched(object sender, InteractableObjectEventArgs e)
        {
            holdingThis = true;
            //print(e.interactingObject + "TOUCHED");
            switch (productInfo.type)
            {
                case ProductInfo.Type.AvocadoProduct:
                    break;
                case ProductInfo.Type.CarVisorProduct:
                    break;
                case ProductInfo.Type.NailPolishProduct:
                    break;
                case ProductInfo.Type.UniverseBluRayProduct:
                    break;
                case ProductInfo.Type.JetLagPillProduct:
                    break;
                case ProductInfo.Type.BalanceBarProduct:
                    break;
                case ProductInfo.Type.CandleProduct:
                    break;
            }
        }

        void DoInteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
        {
            holdingThis = false;
            //print(e.interactingObject + "UNTOUCHED");
            switch (productInfo.type)
            {
                case ProductInfo.Type.AvocadoProduct:
                    break;
                case ProductInfo.Type.CarVisorProduct:
                    break;
                case ProductInfo.Type.NailPolishProduct:
                    break;
                case ProductInfo.Type.UniverseBluRayProduct:
                    break;
                case ProductInfo.Type.JetLagPillProduct:
                    break;
                case ProductInfo.Type.BalanceBarProduct:
                    break;
                case ProductInfo.Type.CandleProduct:
                    break;
            }
        }

        //Grabbed
        void DoInteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
        {
            Debug.Log(productInfo.ToString() + "Grabbed By" + e.interactingObject.ToString());

            if (e.interactingObject != null)
            {
                holdingThis = true;
                switch (productInfo.type)
                {
                    case ProductInfo.Type.AvocadoProduct:
                        mp.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToProjectFolder, "Assets/StreamingAssets/SceneAvocado-Hap.mov", true);
                        retailPeripheryEnabler.IntervalDisableTargets();
                        Invoke("FadeFogOut", 1.5f);
                        //transition between mixer groups
                        audioMixerController.audioTransition(1, 1.0f);
                        audioMixerController.playProductWorldAudio("AvocadoProduct");
                        break;
                    case ProductInfo.Type.CarVisorProduct:
                        //mediaPlayerController.PlayScene("AvocadoProduct");
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.NailPolishProduct:
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.UniverseBluRayProduct:
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.JetLagPillProduct:
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.BalanceBarProduct:
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.CandleProduct:
                        retailPeripheryEnabler.IntervalDisableTargets();
                        break;
                    case ProductInfo.Type.OutOfStock:
                        mp.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToProjectFolder, "Assets/StreamingAssets/OutOfStock-Hap.mov", true);
                        mp.Control.SetLooping(true);
                        retailPeripheryEnabler.IntervalDisableTargets();
                        Invoke("FadeFogOut", 1.5f);
                        break;
                }
            }
        }

   //     IEnumerator PlayWhenReady()
   //     {
   //         yield return new WaitUntil()
   //     }

        //Invoked methods for Grabbed event
        void FadeFogOut()
        {
            fogFader.FadeFogOut(1.5f);
        }

        //Ungrabbed
        void DoInteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
        {
            holdingThis = false;
            print(productInfo.ToString() + "Ungrabbed By" + e.interactingObject.ToString());
            switch (productInfo.type)
            {
                case ProductInfo.Type.AvocadoProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    fogFader.FadeFogIn(1.5f);
                    mediaPlayerController.PauseScene("AvocadoProduct");
                    mp.Control.CloseVideo();
                    audioMixerController.audioTransition(0, 1.0f);
                    audioMixerController.pauseProductWorldAudio("AvocadoProduct");
                    break;
                case ProductInfo.Type.CarVisorProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.NailPolishProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.UniverseBluRayProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.JetLagPillProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.BalanceBarProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.CandleProduct:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    break;
                case ProductInfo.Type.OutOfStock:
                    retailPeripheryEnabler.IntervalEnableTargets();
                    fogFader.FadeFogIn(1.5f);
                    mp.Pause();
                    mp.CloseVideo();
                    break;
            }
        }

        void DoInteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {

        }

        void DoInteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        {

        }
    }

}