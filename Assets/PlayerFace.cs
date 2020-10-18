using System;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public event EventHandler<bool> PlayerIsFacingSomthing;
    int facingObjectsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        facingObjectsCount++;
        PlayerIsFacingSomthing?.Invoke(this, true);
    }

    private void OnTriggerExit(Collider other)
    {
        facingObjectsCount--;
        if (facingObjectsCount == 0)
        {
            PlayerIsFacingSomthing?.Invoke(this, false);
        }
    }
}
