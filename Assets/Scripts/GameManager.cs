using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject hunterPrefab;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
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

    [ServerRpc]
    public void StartGameServerRpc()
    {
        IReadOnlyList<ulong> clients = NetworkManager.Singleton.ConnectedClientsIds;

        int hunterIDIndex = Random.Range(0, clients.Count);
        ulong hunterID = clients[hunterIDIndex];
        
        NetworkSpawnManager spawnManager = NetworkManager.Singleton.SpawnManager;
        spawnManager.GetPlayerNetworkObject(hunterID).Despawn();

        GameObject hunterGameObject = Instantiate(hunterPrefab, Vector3.zero, Quaternion.identity);
        hunterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(hunterID, true);
    }
}
