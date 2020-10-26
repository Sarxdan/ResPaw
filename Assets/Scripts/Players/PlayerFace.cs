using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public event EventHandler<(Rigidbody, bool, bool)> PlayerIsFacingSomething;
    int facingObjectsCount = 0;
    int facingPlayerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            return;
        }
        facingObjectsCount++;

        var isFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);
        facingPlayerCount += isFacingPlayer ? 1 : 0;
        var theOtherPlayerRb = facingPlayerCount > 0 && isFacingPlayer ? other.GetComponent<Rigidbody>() == null ? other.GetComponentInParent<Rigidbody>() : other.GetComponent<Rigidbody>() : null;

        PlayerIsFacingSomething?.Invoke(this, (theOtherPlayerRb, true, facingPlayerCount > 0));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            return;
        }
        facingObjectsCount--;
        var isWasFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);
        facingPlayerCount -= isWasFacingPlayer ? 1 : 0;
        var theOtherPlayerRb = facingPlayerCount > 0 && isWasFacingPlayer ? other.GetComponent<Rigidbody>() : null;
        if (facingObjectsCount == 0)
        {
            PlayerIsFacingSomething?.Invoke(this, (theOtherPlayerRb, false, facingPlayerCount > 0));
        }
    }
}
