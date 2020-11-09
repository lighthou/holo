using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

public class SharedVariables : NetworkBehaviour
{

    [SyncVar]
    private Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);

    [SyncVar]
    private Quaternion rotation = Quaternion.Euler(0,0,0);

    private Vector3 oldScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Quaternion oldRotation = Quaternion.Euler(0, 0, 0);

    // Update is called once per frame
    private void Update()
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

        if (isServer) {
            if (isLocalPlayer) {
                // server object on server
                if (oldScale != clientShared.getScale()) {
                    scale = clientShared.getScale();
                    oldScale = scale;
                }

                if (oldRotation != clientShared.getRotation()) {
                    rotation = clientShared.getRotation();
                    oldRotation = rotation;
                    Debug.Log("Server object on server rotation: " + rotation);
                }
            } else {
                // client object on server
                if (oldScale != clientShared.getScale()) {
                    scale = clientShared.getScale();
                    oldScale = scale;
                }

                if (oldRotation != clientShared.getRotation()) {
                    rotation = clientShared.getRotation();
                    oldRotation = rotation;
                    Debug.Log("Server object on server rotation: " + rotation);
                }
            }
        } else {
            if (isLocalPlayer) {
                // client object on client
                if (oldScale != serverShared.getScale()) {
                    scale = serverShared.getScale();
                    oldScale = scale;
                }

                if (oldRotation != serverShared.getRotation()) {
                    rotation = serverShared.getRotation();
                    oldRotation = rotation;
                    Debug.Log("Server object on server rotation: " + rotation);
                }

            } else {
                // server object on client
                if (oldScale != serverShared.getScale()) {
                    scale = serverShared.getScale();
                    oldScale = scale;
                }

                if (oldRotation != serverShared.getRotation()) {
                    rotation = serverShared.getRotation();
                    oldRotation = rotation;
                    Debug.Log("Server object on server rotation: " + rotation);
                }
            }
        }
    }

    public Quaternion getRotation()
    {
        return rotation;
    }

    public Vector3 getScale()
    {
        return scale;
    }

    public void SetScale(Vector3 newScale)
    {
        if (isServer)
        {
            scale = newScale;
        }
    }

    public void SetRotation(Quaternion newRotation)
    {
        if (isServer)
        {
            rotation = newRotation;
        }
    }

    [Command]
    public void CmdUpdateScale(Vector3 newScale)
    {
        if (!isServer) return;
        if (scale == newScale) return;
        scale = newScale;
    }

    [Command]
    public void CmdUpdateRotation(Quaternion newRotation)
    {
        if (!isServer) return;
        if (rotation == newRotation) return;
        rotation = newRotation;
    }

}
