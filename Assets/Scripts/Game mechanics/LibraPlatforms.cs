using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraPlatforms : MonoBehaviour
{
    GameObject Platform1, Platform2;
    // Start is called before the first frame update
    void Start()
    {
        Platform1 = GameObject.Find("PlatForm1");
        Platform2 = GameObject.Find("Platform2");

        Collider collider1 = Platform1.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           Debug.Log("Hello");
        }

    }
}
