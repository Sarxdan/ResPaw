using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Daniel
//peer reviewed by 



public class FinishLine : MonoBehaviour
{

    public List<GameObject> tb;
  
    private void Start()
    {
        tb = new List<GameObject>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {           
            if (!tb.Contains(other.gameObject))
            {
                tb.Add(other.gameObject);               
            }
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {         
            if (tb.Contains(other.gameObject))
            {
                tb.Remove(other.gameObject);
            }
        }
    }

    private void Update()
    {
        CheckAmount();
    }

    private void CheckAmount()
    {
        if(tb.Count == 2)
        {
            foreach (GameObject objects in tb)
            {
                objects.GetComponent<PlayerBase>().enabled = false;
                //TODO: UI
            }
        }        
    }
}
