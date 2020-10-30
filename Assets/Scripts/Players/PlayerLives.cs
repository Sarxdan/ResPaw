//Created By Mehmet
//Peer-Reviewed By Daniel
//Fixed by Daniel

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.TerrainAPI;
using System.Collections.Generic;

public class PlayerLives : MonoBehaviour
{   
    public TextMeshProUGUI playerlives1, playerlives2;   
    public GameObject lostPanel;
    PlayerBase[] findPlayers;

    GameManager manager;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("Test", 0.02f);
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        lostPanel.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        playerlives1.text = "" + manager.PlayerOneLife.ToString();
        playerlives2.text=  "" + manager.PlayerTwoLife.ToString();
        if (manager.PlayerOneLife == 0 || manager.PlayerTwoLife == 0)
        {
            foreach(PlayerBase objects in findPlayers)
            {
                objects.GetComponent<PlayerBase>().enabled = false;
            }
            lostPanel.SetActive(true);
        }
    }
    private void Test()
    {
        findPlayers = FindObjectsOfType<PlayerBase>();
        
    }
        
}
