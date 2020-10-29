using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLayout : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    float secondsWait;
    bool hasActivated = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !hasActivated)
        {
            StartCoroutine(showText());
        }
    }
    IEnumerator showText()
    {
        hasActivated = true;
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(secondsWait);
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(secondsWait);
        anim.SetTrigger("Reset");
        hasActivated = false;
    }
}
