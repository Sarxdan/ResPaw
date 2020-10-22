using UnityEngine;

public class PlayerOne : PlayerBase
{
    public override string GetHorizontalAxies()
    {
        return "Horizontal";
    }

    public override string GetJumpButton()
    {
        return "Jump";
    }
    public override Vector3 GetPosition()
    {
        return transform.position;
    }

}
