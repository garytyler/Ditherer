using UnityEngine;
using System.Collections;

public class SetHandButtonPos : MonoBehaviour {

    public Transform controllerAttachPoint;

    void Update()
    {   
        transform.position = controllerAttachPoint.position;
    }
}