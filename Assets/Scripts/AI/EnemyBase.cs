using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;

public class EnemyBase : MonoBehaviour
{
    public void Death()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/GreyScale");
        GetComponent<Animator>().enabled = false;
        enabled = false;
    }
}
