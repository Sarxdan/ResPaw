using UnityEngine;

//
// Created by: Sandra Andersson
//
// Peer-reviewed by: Mehmet
//

public class EnemyBase : MonoBehaviour
{
    public void Death()
    {
        if (enabled)
        {
            GetComponent<Rigidbody>().mass = 1.0f;
            gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/GreyScale");
            GetComponent<Animator>().enabled = false;
            enabled = false;
            Destroy(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject);    // Destroy sword/bow
        }
    }
}
