using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//created by Daniel
//peer reviewed by 



public class FinishLine : MonoBehaviour
{
    
    private List<GameObject> tb;
    [SerializeField]private Canvas canvas;

    private void Start()
    {
        tb = new List<GameObject>();
        canvas = canvas.GetComponent<Canvas>();      
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
            canvas.enabled = true;
            
            foreach (GameObject objects in tb)
            {
                objects.GetComponent<Animator>().enabled = false;
                objects.GetComponent<PlayerBase>().enabled = false;               
                //TODO: make the final UI
            }
        }        
    }
    
}
