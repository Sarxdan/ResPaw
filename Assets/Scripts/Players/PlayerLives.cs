//Created By Mehmet
//Peer-Reviewed By Daniel
//Fixed by Daniel

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerLives : MonoBehaviour
{   
    public TextMeshProUGUI playerlives1, playerlives2;   
    public GameObject lostPanel;

   [SerializeField] float volume;

    PlayerBase[] findPlayers;


    AudioSource loseSound;
    AudioSource[] otherSources;

    GameManager manager;
    // Start is called before the first frame update
    private void Start()
    {
        loseSound = GetComponent<AudioSource>();
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
            
            if (!loseSound.isPlaying)
            {
                LowerVolume();
                loseSound.volume = volume;
                loseSound.Play();
            }
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
    private void LowerVolume()
    {
        otherSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in otherSources)
        {
            audioS.volume = 0;
        }
    }
        
}
