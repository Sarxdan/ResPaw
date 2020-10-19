using System;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public event EventHandler<bool> PlayerIsFacingSomething;
    int facingObjectsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        facingObjectsCount++;
        PlayerIsFacingSomething?.Invoke(this, true);
    }

    private void OnTriggerExit(Collider other)
    {
        facingObjectsCount--;
        if (facingObjectsCount == 0)
        {
            PlayerIsFacingSomething?.Invoke(this, false);
        }
    }
}
