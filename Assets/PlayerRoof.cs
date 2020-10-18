using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class PlayerRoof : MonoBehaviour
{
    public event EventHandler<(Rigidbody, bool)> PlayerIsCarryingAnotherPlayer;

    int holdingObjectsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)LayerEnum.Player)
        {
            holdingObjectsCount++;
            var rb = other.GetComponent<Rigidbody>();
            PlayerIsCarryingAnotherPlayer?.Invoke(this, (rb, true));
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayerEnum.Player)
        {
            holdingObjectsCount--;
            if (holdingObjectsCount == 0)
            {
                PlayerIsCarryingAnotherPlayer?.Invoke(this, (null, false));
            }
        }
    }
}
