using UnityEngine;

class PlayerTwo : PlayerBase
{
    public override string GetActionButton()
    {
        return "Action2";
    }
    
    public override string GetDropButton()
    {
        return "Drop2";
    }

    public override string GetHorizontalAxies()
    {
        return "Horizontal2";
    }

    public override string GetJumpButton()
    {
        return "Jump2";
    }

    public override Vector3 GetPosition()
    {
        return transform.position;
    }
}
