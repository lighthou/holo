using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HololensUpdate : MonoBehaviour
{

    private Vector3 savedScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Quaternion savedRotation = Quaternion.Euler(0, 0, 0);

    private void Update()
    {

        //UnityEngine.Debug.Log(transform.localScale);
        //UnityEngine.Debug.Log(savedScale);

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


        if (clientShared != null && serverShared != null) {
            if (transform.localScale.x != savedScale.x || transform.rotation.y != savedRotation.y) {
                if (transform.localScale.x != savedScale.x) {
                    clientShared.CmdUpdateScale(transform.localScale);
                    //serverShared.SetScale(transform.localScale);
                } else {
                    // transform.localScale = clientShared.getScale();
                    transform.localScale = serverShared.getScale();
                }
                savedScale = transform.localScale;

                if (transform.rotation.y != savedRotation.y) {
                    clientShared.CmdUpdateRotation(transform.rotation);
                    //serverShared.SetRotation(transform.rotation);
                } else {
                    //transform.rotation = clientShared.getRotation();
                    transform.rotation = serverShared.getRotation();
                }
                savedRotation = transform.rotation;
            } else
            {

                transform.localScale = new Vector3(serverShared.getScale().x * 1.2f, serverShared.getScale().y * 1.2f, serverShared.getScale().z * 1.2f) ;
                //transform.localScale = serverShared.getScale();
                savedScale = transform.localScale;
                transform.rotation = serverShared.getRotation();
                //transform.rotation = serverShared.getRotation();
                savedRotation = transform.rotation;
            }
        } 
    }
}
