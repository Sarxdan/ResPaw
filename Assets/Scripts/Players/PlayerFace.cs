using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public event EventHandler<(bool, bool)> PlayerIsFacingSomething;
    int facingObjectsCount = 0;
    int facingPlayerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        facingObjectsCount++;

        var isFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);
        facingPlayerCount += isFacingPlayer ? 1 : 0;
        PlayerIsFacingSomething?.Invoke(this, (true, facingPlayerCount > 0));
    }

    private void OnTriggerExit(Collider other)
    {
        facingObjectsCount--;
        var isWasFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);
        facingPlayerCount -= isWasFacingPlayer ? 1 : 0;
        if (facingObjectsCount == 0)
        {
            PlayerIsFacingSomething?.Invoke(this, (false, facingPlayerCount > 0));
        }
    }
}
