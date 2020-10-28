using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    public event EventHandler<bool> PlayerIsFacingObject;
    public event EventHandler<(Rigidbody, bool)> PlayerIsFacingPlayer;
    int facingObjectsCount = 0;
    int facingPlayerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            return;
        }





        var isFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);


        if (isFacingPlayer)
        {
            facingPlayerCount++;
            var theOtherPlayerRb = facingPlayerCount > 0 && isFacingPlayer ? other.GetComponent<Rigidbody>() == null ? other.GetComponentInParent<Rigidbody>() : other.GetComponent<Rigidbody>() : null;

            PlayerIsFacingPlayer?.Invoke(this, (theOtherPlayerRb, facingPlayerCount > 0));
        }

        facingObjectsCount++;
        PlayerIsFacingObject?.Invoke(this, facingObjectsCount > 0);




    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            return;
        }


        var isWasFacingPlayer = (other.gameObject.layer == (int)LayerEnum.Player) || (other.gameObject.layer == (int)LayerEnum.Body);

        if (isWasFacingPlayer)
        {
            facingPlayerCount--;
            var theOtherPlayerRb = facingPlayerCount > 0 && isWasFacingPlayer ? other.GetComponent<Rigidbody>() : null;
            PlayerIsFacingPlayer?.Invoke(this, (theOtherPlayerRb, facingPlayerCount > 0));
        }

        facingObjectsCount--;
        PlayerIsFacingObject?.Invoke(this, facingObjectsCount > 0);


    }
}
