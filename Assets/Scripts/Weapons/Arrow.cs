using System.Collections;
using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: Mehmet
//

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyBase baseScript = other.gameObject.GetComponent<EnemyBase>();
            if (baseScript)
            {
                other.gameObject.GetComponent<EnemyBase>().Death();
            }
            Destroy(gameObject);
        }
        else if(other.tag == "Player")
        {
            PlayerBase baseScript = other.gameObject.GetComponent<PlayerBase>();
            if (baseScript)
            {
                other.gameObject.GetComponent<PlayerBase>().OnDeath();
            }
            Destroy(gameObject);
        }
        else if (other.tag != "Spike" && other.tag != "AI Border" && other.tag != "Weapon")
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
