using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int health;

    [SerializeField] private float invincibleTime = 10f;
    private bool invincible = false;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage()
    {
        if (!invincible)
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(ActivateInvincibleTime());
            }
        }
    }

    private void Die()
    {
        
    }

    private IEnumerator ActivateInvincibleTime()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }
}
