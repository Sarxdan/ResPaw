using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public void Death()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/GreyScale");
        GetComponent<Animator>().enabled = false;
        enabled = false;
    }
}
