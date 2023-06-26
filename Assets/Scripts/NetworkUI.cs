using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton; // Host = Client + server
    [SerializeField] private Button clientButton; // No server. Client only

    private void Awake()
    {
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }
}