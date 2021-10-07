using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Spawner : MonoBehaviour
{

    public ARRaycastManager arRaycastManager;
    public GameObject cubePrefab;
    private GameObject SpawnedObject;
    private bool _placed = false;
    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
    public ARSessionOrigin MyArSessionOrigin;

    public void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount>0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public void Start()
    {
        //cubePrefab.SetActive(false);
    }
    void Update()
    {

        //if (Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        if (Input.touchCount == 1)
        //        {
        //            //Rraycast Planes
        //            if (!_placed && arRaycastManager.Raycast(touch.position, arRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        //            {
        //                var pose = arRaycastHits[0].pose;
        //                CreateCube(pose.position);
        //                return;
        //            }


        //        }
        //    }
        //}
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if(arRaycastManager.Raycast(touchPosition,arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = arRaycastHits[0].pose;
            if(SpawnedObject ==null)
            {
                SpawnedObject = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
            }
            else
            {

            }
        }
    }

    private void CreateCube(Vector3 position)
    {
        if (!_placed)
        {
            Vector3 pos = position;
            Instantiate(cubePrefab, pos, new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w)); _placed = true;
            //MyArSessionOrigin.MakeContentAppearAt(cubePrefab.transform, position,new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w));
            //cubePrefab.SetActive(true);
            //gameObject.SetActive(false);
        }
    }

    
}
