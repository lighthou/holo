using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Networking;
using System.Diagnostics;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
/// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
/// and overlays some information as well as the source Texture2D on top of the
/// detected image.
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The camera to set on the world space UI canvas for each instantiated image info.")]
    Camera m_WorldSpaceCanvasCamera;

    /// <summary>
    /// The prefab has a world space UI canvas,
    /// which requires a camera to function properly.
    /// </summary>
    public Camera worldSpaceCanvasCamera
    {
        get { return m_WorldSpaceCanvasCamera; }
        set { m_WorldSpaceCanvasCamera = value; }
    }

    ARTrackedImageManager m_TrackedImageManager;

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {

        // Disable the visual plane if it is not being tracked
        if (trackedImage.trackingState != TrackingState.None)
        {
            SharedVariables clientShared = null;
            SharedVariables serverShared = null;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Respawn")) {
                SharedVariables sharedVariables = obj.GetComponent<SharedVariables>();
                if (sharedVariables.isLocalPlayer) {
                    if (sharedVariables.isServer) serverShared = sharedVariables;
                    if (!sharedVariables.isServer) clientShared = sharedVariables;
                } else {
                    if (sharedVariables.isServer) clientShared = sharedVariables;
                    if (!sharedVariables.isServer) serverShared = sharedVariables;
                }
            }


            //UnityEngine.Debug.Log("Server scale : " + sharedVariables.getScale().x);
            //trackedImage.transform.localScale = new Vector3(clientShared.getScale().x, clientShared.getScale().y, clientShared.getScale().z);
            trackedImage.transform.localScale = new Vector3(serverShared.getScale().x, serverShared.getScale().y, serverShared.getScale().z);

            trackedImage.transform.rotation = serverShared.getRotation();
            //trackedImage.transform.rotation = clientShared.getRotation();
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Give the initial image a reasonable default scale
            trackedImage.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        foreach (var trackedImage in eventArgs.updated)
            UpdateInfo(trackedImage);
    }
}
