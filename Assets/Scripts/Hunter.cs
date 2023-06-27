using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Hunter : NetworkBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null)
        { 
            otherHealth.TakeDamage();
        }
    }
}
