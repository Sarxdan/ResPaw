using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] Text P1, P2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        P1.text = "P1: " + GameManager.Instance.PlayerOneLife.ToString();
        P2.text = "P2: " + GameManager.Instance.PlayerTwoLife.ToString();
    }
}
