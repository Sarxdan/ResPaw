using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by:
//

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBase>().Death();
            Destroy(gameObject);
        }
        else if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBase>().OnDeath();
            Destroy(gameObject);
        }
        else if (other.tag != "Spike" && other.tag != "AI Border")
        {
            Destroy(gameObject);
        }
    }

    // Destroy after timer runs out
    IEnumerator DestroyWhenFinished(int lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
