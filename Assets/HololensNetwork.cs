using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.ComponentModel;

public class HololensNetwork : MonoBehaviour
{
    public TextAsset dictionaryTextFile;
    public NetworkManager manager;
    private string theWholeFileAsOneLongString;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        //manager.networkAddress = "10.89.176.149";
        //manager.networkAddress = "192.168.20.7";
        theWholeFileAsOneLongString = dictionaryTextFile.text;
        manager.networkAddress = theWholeFileAsOneLongString;
        manager.StartClient();
    }
}
