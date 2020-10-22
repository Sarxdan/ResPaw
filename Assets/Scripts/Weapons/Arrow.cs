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
    // Destroy after timer runs out
    IEnumerator DestroyWhenFinished(int lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
