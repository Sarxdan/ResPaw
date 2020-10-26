using System.Collections.Generic;
using UnityEngine;
using TMPro;

//created by Daniel
//peer reviewed by Sandra



public class FinishLine : MonoBehaviour
{ 
    private List<GameObject> tb;
    private LevelControll lc;

    public GameObject winPanel;    
    private void Start()
    {
        tb = new List<GameObject>();
        lc = GameObject.FindObjectOfType(typeof(LevelControll)) as LevelControll;
        winPanel.SetActive(false);
        //canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        //saves playerobject for later use
        if(other.gameObject.tag == "Player" && !other.GetComponent<PlayerBase>().isDead)
        {           
            if (!tb.Contains(other.gameObject))
            {
                tb.Add(other.gameObject);               
            }
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" && !other.GetComponent<PlayerBase>().isDead)
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
            //canvas.enabled = true;
            winPanel.SetActive(true);
            lc.YouWin();
            
            foreach (GameObject objects in tb)
            {                     
                objects.GetComponent<PlayerBase>().enabled = false;
                ParticleSystem confetti = transform.Find("Winning Confetti").gameObject.GetComponent<ParticleSystem>();
                confetti.Play();
                enabled = false;
                //TODO: make the final UI
                //TODO: make winning animation
            }
        }        
    }

 
    
}
