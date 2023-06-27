using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfo : MonoBehaviour
{
    public string IPAddress { get; private set; }
    
    public bool IsHost { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetConnectionInfo(string ipAddress, bool isHost)
    {
        IPAddress = ipAddress;
        IsHost = isHost;
    }
}
