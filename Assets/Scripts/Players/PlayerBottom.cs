using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class playerBottom : MonoBehaviour
{
    public event EventHandler<(Rigidbody, bool)> PlayerIsAbovePlayer;
    public event EventHandler<bool> PlayerIsAboveGround;

    int playerBelowMeCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)LayerEnum.Player)
        {
            playerBelowMeCount++;
            var rb = other.GetComponent<Rigidbody>();
            PlayerIsAbovePlayer?.Invoke(this, (rb, true));
        }
        if (other.gameObject.layer == (int)LayerEnum.Ground)
        {
            PlayerIsAboveGround?.Invoke(this, true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayerEnum.Player)
        {
            playerBelowMeCount--;
            if (playerBelowMeCount == 0)
            {
                PlayerIsAbovePlayer?.Invoke(this, (null, false));
            }
        }
        if (other.gameObject.layer == (int)LayerEnum.Ground)
        {
            PlayerIsAboveGround?.Invoke(this, false);
        }
    }
}
