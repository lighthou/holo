using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.ComponentModel;

public class MobileNetwork : MonoBehaviour
{

    public NetworkManager manager;

    public string x;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        manager.StartHost();
    }


}
