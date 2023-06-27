using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton; // Host = Client + server
    [SerializeField] private Button clientButton; // No server. Client only
    [SerializeField] private TMP_InputField ipField;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => StartHost());
        clientButton.onClick.AddListener(() => StartClient());
    }

    private void StartHost()
    {
        GameObject connectionInfoObject = new GameObject("ConnectionInfo");
        ConnectionInfo connectionInfo = connectionInfoObject.AddComponent<ConnectionInfo>();
        connectionInfo.SetConnectionInfo("", true);
        SceneManager.LoadScene("PlayerScene");
    }

    private void StartClient()
    {
        GameObject connectionInfoObject = new GameObject("ConnectionInfo");
        ConnectionInfo connectionInfo = connectionInfoObject.AddComponent<ConnectionInfo>();
        connectionInfo.SetConnectionInfo(ipField.text, false);
        SceneManager.LoadScene("PlayerScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}