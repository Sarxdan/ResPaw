﻿//Created By Mehmet
//Peer-Reviewed By Daniel
//Fixed by Daniel

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{   
    public TextMeshProUGUI playerlives1, playerlives2;   
    public GameObject lostPanel;
    GameManager manager;
    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        restart = SceneManager.GetActiveScene().buildIndex;
        lostPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        playerlives1.text = "" + manager.PlayerOneLife.ToString();
        playerlives2.text=  "" + manager.PlayerTwoLife.ToString();
        if (manager.PlayerOneLife == 0 || manager.PlayerTwoLife == 0)
        {           
            lostPanel.SetActive(true);
        }
    }
}
