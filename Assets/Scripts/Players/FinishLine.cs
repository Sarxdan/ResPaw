using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

//created by Daniel
//peer reviewed by Sandra



public class FinishLine : MonoBehaviour
{
    [SerializeField]
    private int levelNumber;

    private List<GameObject> tb;
    private LevelControll lc;
    GameManager manager;
    public GameObject winPanel;
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        tb = new List<GameObject>();
        lc = GameObject.FindObjectOfType(typeof(LevelControll)) as LevelControll;
        winPanel.SetActive(false);
        //canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //saves playerobject for later use
        if (other.gameObject.tag == "Player" && !other.GetComponent<PlayerBase>().isDead)
        {
            if (!tb.Contains(other.gameObject))
            {
                tb.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.GetComponent<PlayerBase>().isDead)
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
        if (tb.Count == 2)
        {
            SaveScore();
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

    private int CalculateTotalDeaths()
    {
        int totalDeaths = 0;
        int totalLives = manager.playerLives * 2;
        totalDeaths = totalLives - manager.PlayerOneLife - manager.PlayerTwoLife;

        return totalDeaths;
    }

    private void SaveScore()
    {
        ScoreModel scoreModel = new ScoreModel()
        {
            DeathCount = CalculateTotalDeaths(),
            LevelNumber = levelNumber
        };

        ScoreManager.SavewNewScore(scoreModel);
    }



}
