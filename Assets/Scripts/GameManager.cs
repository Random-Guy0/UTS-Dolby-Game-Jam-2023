using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject hunterPrefab;
    [SerializeField] private GameObject ghostPrefab;

    private NetworkVariable<int> survivorCount = new NetworkVariable<int>();
    private List<ulong> survivorIDs = new List<ulong>();

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        IReadOnlyList<ulong> clients = NetworkManager.Singleton.ConnectedClientsIds;

        survivorCount.Value = clients.Count - 1;
        UpdateSurvivorCounter();
        survivorIDs = clients.ToList();

        int hunterIDIndex = Random.Range(0, clients.Count);
        ulong hunterID = clients[hunterIDIndex];
        survivorIDs.Remove(hunterID);
        
        NetworkSpawnManager spawnManager = NetworkManager.Singleton.SpawnManager;
        spawnManager.GetPlayerNetworkObject(hunterID).Despawn();

        GameObject hunterGameObject = Instantiate(hunterPrefab, Vector3.zero, Quaternion.identity);
        hunterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(hunterID);
        DisableIPTextAndStartButton();
    }
    
    private void DisableIPTextAndStartButton()
    {
        foreach (ulong playerID in NetworkManager.ConnectedClientsIds)
        {
            NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerID);
            if (playerObject != null)
            {
                PlayerUI ui = playerObject.GetComponentInChildren<PlayerUI>(true);
                if (ui != null)
                {
                    ui.DisableStartAndIPUI();
                }
            }
        }
    }
    
    private void UpdateSurvivorCounter()
    {
        foreach (ulong playerID in NetworkManager.ConnectedClientsIds)
        {
            NetworkObject playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerID);
            if (playerObject != null)
            {
                PlayerUI ui = playerObject.GetComponentInChildren<PlayerUI>(true);
                if (ui != null)
                {
                    ui.UpdateSurvivorCounter(survivorCount.Value);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerDeathServerRpc(ulong clientID)
    {
        NetworkSpawnManager spawnManager = NetworkManager.Singleton.SpawnManager;
        NetworkObject currentPlayerObject = spawnManager.GetPlayerNetworkObject(clientID);
        Vector3 position = currentPlayerObject.transform.position;
        Quaternion rotation = currentPlayerObject.transform.rotation;
        currentPlayerObject.Despawn();

        GameObject ghostGameObject = Instantiate(ghostPrefab, position, rotation);
        ghostGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);

        survivorIDs.Remove(clientID);
        survivorCount.Value = survivorIDs.Count;
        UpdateSurvivorCounter();
    }
}
