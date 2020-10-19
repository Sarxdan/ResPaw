using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public List<GameObject> Tb;
    
    

    private void Start()
    {
        Tb = new List<GameObject>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {           
            if (!Tb.Contains(other.gameObject))
            {
                Tb.Add(other.gameObject);
                Debug.Log("added");
            }
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {         
            if (Tb.Contains(other.gameObject))
            {
                Tb.Remove(other.gameObject);
                Debug.Log("remove");
            }
        }

    }

    private void Update()
    {
        checkAmount();
    }

    private void checkAmount()
    {
        if(Tb.Count >= 2)
        {
            
          
        }
    }





}
