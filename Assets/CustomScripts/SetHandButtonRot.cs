using UnityEngine;
using System.Collections;

public class SetHandButtonRot : MonoBehaviour {

    public Transform controllerAttachPoint;
    public Transform cameraHead;
    public float tilt = 2.0f;

    private GameObject lookAtAnchor;
    Vector3 newRot;
    
    void Start()
    {
        lookAtAnchor = new GameObject();
    }

    void LateUpdate()
    {
        lookAtAnchor.transform.position = controllerAttachPoint.position;
        lookAtAnchor.transform.LookAt(cameraHead);

        transform.eulerAngles = new Vector3(0f, lookAtAnchor.transform.eulerAngles.y, 0f);
    }
}