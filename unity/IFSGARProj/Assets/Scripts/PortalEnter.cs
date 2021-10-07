using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalEnter : MonoBehaviour
{
    public Transform device;

    public GameObject p1;
    void Update()
    {
        WhileCameraColliding();
    }

    bool isColliding;
    bool inOtherWorld;
    bool wasInFront;
    void Start()
    {
        device = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        SetMaterials(false);
    }
    bool GetIsInFront()
    {
        Vector3 worldPos = device.position + device.forward * Camera.main.nearClipPlane;

        Vector3 pos = transform.InverseTransformPoint(worldPos);
        return pos.z >= 0 ? true : false;
    }
    void SetMaterials(bool fullRender)
    {
        if (fullRender)
        {
            Show();
            
            SetLayerRecursively(p1, LayerMask.NameToLayer("Mask"));
        }
        else
        {
            Hide();
            
            SetLayerRecursively(p1, LayerMask.NameToLayer("InsidePortal"));
        }

    }
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
    private void Show()
    {
        Debug.Log("Showww");
       // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Underworld");
    }

    private void Hide()
    {
        Debug.Log("Hideee");
       // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Underworld"));
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform != device)
            return;
        //Important to do this for if the user re-enters the portal from the same side
        wasInFront = GetIsInFront();
        Debug.Log("wasifFront" + wasInFront);
        isColliding = true;
        //if (other.tag=="MainCamera")
        //{
        //    Debug.Log("enter"); Show();
        GetComponent<MeshRenderer>().enabled = false;
        //}
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform != device)
            return;
        isColliding = false;
        //if (other.tag == "MainCamera")
        //{
        //    Debug.Log("Exit"); Hide();
        GetComponent<MeshRenderer>().enabled = true;
        //}
    }
    void WhileCameraColliding()
    {
        if (!isColliding)
            return;
        bool isInFront = GetIsInFront();
        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);
        }

        wasInFront = isInFront;
    }
}
