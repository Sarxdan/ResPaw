using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class PlayerDrag : MonoBehaviour
{
    public event EventHandler<(Rigidbody, bool)> PlayerIsFacingPlayer;
    private void OnTriggerEnter(Collider other)
    {


        var isFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);
        var theOtherPlayerRb = isFacingPlayer ? other.GetComponent<Rigidbody>() == null ? other.GetComponentInParent<Rigidbody>() : other.GetComponent<Rigidbody>() : null;
        if (isFacingPlayer)
            PlayerIsFacingPlayer?.Invoke(this, (theOtherPlayerRb, isFacingPlayer));

    }

    private void OnTriggerExit(Collider other)
    {
        var wasFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);

        if (wasFacingPlayer)
            PlayerIsFacingPlayer?.Invoke(this, (null, false));

    }
}
