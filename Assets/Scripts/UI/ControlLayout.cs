using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLayout : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    float secondsWait;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(showText());
        }
    }
    IEnumerator showText()
    {
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(secondsWait);
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(secondsWait);
        anim.SetTrigger("Reset");
    }
}
