using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUpdate : MonoBehaviour
{

    private Quaternion savedRotation;
    private Vector2 startPos;
    private float rotateSpeedModifier = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    // If we have moved we want to rotate

                    SharedVariables sharedVariables = GameObject.FindWithTag("Respawn").GetComponent<SharedVariables>();
                    Quaternion rotation = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeedModifier, 0f);

                    Quaternion newRotation = serverShared.getRotation() * rotation;
                    //Quaternion newRotation = clientShared.getRotation() * rotation;
                    UnityEngine.Debug.Log("mobile rotation update " + newRotation.y);

                    //clientShared.CmdUpdateRotation(newRotation);
                    serverShared.SetRotation(newRotation);

                    break;

                case TouchPhase.Ended:
                    // If when we ended the finger hadn't moved, it's a tap
                    if (touch.position == startPos)
                    {
                        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit raycastHit;
                        if (Physics.Raycast(raycast, out raycastHit))
                        {
                            if (raycastHit.collider != null)
                            {
                                UnityEngine.Debug.Log("Tapped " + raycastHit.transform.gameObject.name);
                            }
                        }
                    }
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            // Store both of the touches on screen.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = (prevTouchDeltaMag - touchDeltaMag) * -0.0001f;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                if (obj.GetComponent<SharedVariables>().isServer)
                //if (!obj.GetComponent<SharedVariables>().isServer)
                {
                    SharedVariables sharedVariables = obj.GetComponent<SharedVariables>();
                    Vector3 savedScale = serverShared.getScale();
                    //Vector3 savedScale = clientShared.getScale();

                    var newX = Mathf.Clamp(savedScale.x + deltaMagnitudeDiff, 0.001f, 10);
                    var newY = Mathf.Clamp(savedScale.y + deltaMagnitudeDiff, 0.001f, 10);
                    var newZ = Mathf.Clamp(savedScale.z + deltaMagnitudeDiff, 0.001f, 10);

                    Vector3 newScale = new Vector3(newX, newY, newZ);


                    serverShared.SetScale(newScale);
                    //clientShared.CmdUpdateScale(newScale);
                }
            }
            
        }   else
        {

        }
    }
}
