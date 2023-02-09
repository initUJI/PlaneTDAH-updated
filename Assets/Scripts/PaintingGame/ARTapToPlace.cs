using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    MeshRenderer quadInScreen;

    public Painting_GameManager minigame_manager;

    public bool hasBeenPlaced;

    private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        hasBeenPlaced = false;

#if UNITY_ANDROID
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
#endif

        quadInScreen = placementIndicator.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (!hasBeenPlaced)
        {
            Instantiate(objectToPlace, PlacementPose.position, PlacementPose.rotation);

            hasBeenPlaced = true;
            minigame_manager.StartTheGame();
            quadInScreen.material.color = new Color(1, 1, 1, 0f);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }



    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // var screenCenter = new Vector3(0.5f, 0.5f);
        var hits = new List<ARRaycastHit>();

#if UNITY_IOS
		arOrigin.GetComponent<ARRaycastManager>().Raycast(screenCenter, hits, TrackableType.Planes);
#endif

#if UNITY_ANDROID
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
#endif

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}