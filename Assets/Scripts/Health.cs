using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private NetworkVariable<int> health = new NetworkVariable<int>();

    [SerializeField] private float invincibleTime = 10f;
    private NetworkVariable<bool> invincible = new NetworkVariable<bool>();

    [SerializeField] private GameObject[] heartIndicators;

    private void Start()
    {
        if (IsServer)
        {
            health.Value = maxHealth;
            invincible.Value = false;
        }
    }

    public void TakeDamage()
    {
        if (!invincible.Value)
        {
            health.Value--;
            UpdateHeartDisplayClientRpc();
            if (health.Value <= 0)
            {
                health.Value = 0;
                Die();
            }
            else
            {
                if (IsServer)
                {
                    StartCoroutine(ActivateInvincibleTime());
                }
            }
        }
    }

    private void Die()
    {
        DieClientRpc();
    }
    
    [ClientRpc]
    private void DieClientRpc()
    {
        if (IsOwner)
        {
            ulong clientID = NetworkManager.Singleton.LocalClientId;
            GameManager.Instance.PlayerDeathServerRpc(clientID);
        }
    }

    [ClientRpc]
    private void UpdateHeartDisplayClientRpc()
    {
        if (IsOwner)
        {
            heartIndicators[health.Value].SetActive(false);
        }
    }

    private IEnumerator ActivateInvincibleTime()
    {
        if (!invincible.Value)
        {
            invincible.Value = true;
            yield return new WaitForSeconds(invincibleTime);
            invincible.Value = false;
        }
    }
}