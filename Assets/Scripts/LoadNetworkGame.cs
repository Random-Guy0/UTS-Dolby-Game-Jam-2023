using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class LoadNetworkGame : MonoBehaviour
{
    private PlayerUI playerUI;
    
    private void Start()
    {
        ConnectionInfo connection = FindObjectOfType<ConnectionInfo>();
        if (connection != null)
        {
            if (connection.IsHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                StartClient(connection.IPAddress);
            }
        }

        StartCoroutine(WaitForPlayerObject(connection));
    }

    private String GetIPAddress()
    {
        String ipAddress = "";
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                break;
            }
        }

        return ipAddress;
    }

    private void StartClient(String ipAddress)
    {
        SetIpAddress(ipAddress);
        NetworkManager.Singleton.StartClient();
    }

    private void SetIpAddress(string ipAddress) {
        if (ipAddress.Equals(String.Empty))
        {
            ipAddress = "127.0.0.1";
        }
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }

    private IEnumerator WaitForPlayerObject(ConnectionInfo connection)
    {
        bool success = false;

        while (!success)
        {
            NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            if (playerObject != null)
            {
                playerUI = playerObject.GetComponentInChildren<PlayerUI>(true);
                if (playerUI != null)
                {
                    if (connection.IsHost)
                    {
                        playerUI.SetupStartAndIPUI(GetIPAddress());
                    }
                    else
                    {
                        playerUI.SetupStartAndIPUI(connection.IPAddress);
                    }

                    success = true;
                    break;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

        Destroy(connection);
    }
}
