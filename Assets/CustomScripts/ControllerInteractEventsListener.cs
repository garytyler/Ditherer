
//Used for hand button display and interaction while holding products

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRTK;

public class ControllerInteractEventsListener : MonoBehaviour
{
    public enum ControllerHand {left, right};
    public GameObject buttonRig;
    public GameObject buttonAddToCart;
//    public GameObject buttonCheckout;

    public GameObject otherController;
    public Collider otherPressCollider;


    GameObject currentHeldProd;
    ProductInfo currentHeldProdInfo;
    ShoppingCartComponent shoppingCartComponent;

    InteractableObjectEventArgs e;



    private void Start()
    {
        GetComponent<VRTK_InteractTouch>().ControllerTouchInteractableObject += new ObjectInteractEventHandler(DoInteractTouch);
        GetComponent<VRTK_InteractTouch>().ControllerUntouchInteractableObject += new ObjectInteractEventHandler(DoInteractUntouch);
        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += new ObjectInteractEventHandler(DoInteractGrab);
        GetComponent<VRTK_InteractGrab>().ControllerUngrabInteractableObject += new ObjectInteractEventHandler(DoInteractUngrab);

        currentHeldProd = new GameObject();
        currentHeldProdInfo = new ProductInfo();
        
        shoppingCartComponent = buttonAddToCart.GetComponent<ShoppingCartComponent>();
        buttonAddToCart.GetComponent<VRTK_Button>().events.OnPush.AddListener(ButtonAddToCartPush);

//        buttonCheckout.GetComponent<VRTK_Button>().events.OnPush.AddListener(ButtonCheckoutPush);

        StartCoroutine(LateStart(1.0f));
    }

    IEnumerator LateStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        buttonRig.SetActive(false);
    }        
    
    void ButtonAddToCartPush()
    {
        Debug.Log("AddToCart Button Pushed: " + currentHeldProdInfo.type);
        shoppingCartComponent.IncreaseCartQuantity(currentHeldProdInfo);
    }

  //  void ButtonCheckoutPush()
 //   {
        
//    }

    private void DoInteractTouch(object sender, ObjectInteractEventArgs e)
    {
    }

    private void DoInteractUntouch(object sender, ObjectInteractEventArgs e)
    {
    }

    private void DoInteractGrab(object sender, ObjectInteractEventArgs e)
    {
        if (e.target == null)
        {
            Debug.Log("DoInteractGrab() e.target is null");
        }
        else
        {
            currentHeldProd = e.target;
            currentHeldProdInfo = e.target.GetComponent<ProductInfo>();
        }

        if (currentHeldProdInfo.type != ProductInfo.Type.OutOfStock)
        {
            StartCoroutine(DelayButtonsActive(2.0f));
        }
        
        //Debug.Log(currentHeldProdInfo.type + "Grabbed By: " + VRTK_DeviceFinder.ControllerByIndex(e.controllerIndex));

        currentHeldProd = e.target;
        otherPressCollider.enabled = true;
    }

    private void DoInteractUngrab(object sender, ObjectInteractEventArgs e)
    {
            otherPressCollider.enabled = false;
        if(buttonRig)
        {
            buttonRig.SetActive(false);
        }
            
    }

    IEnumerator DelayButtonsActive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        buttonRig.SetActive(true);
    }



}

