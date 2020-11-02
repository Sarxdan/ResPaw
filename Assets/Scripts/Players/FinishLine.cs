using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//created by Daniel
//peer reviewed by Sandra



public class FinishLine : MonoBehaviour
{
    [SerializeField]
    private int levelNumber;

    [SerializeField] private List<GameObject> tb;
    private LevelControll lc;
    GameManager manager;
    AudioSource victory;
    [SerializeField] AudioSource[] otherSources;
    [SerializeField] float soundVolume;


    public GameObject winPanel;


    private bool win = false;

    [SerializeField]
    TextMeshProUGUI levelTime;
    private void Awake()
    {
        victory = GetComponentInChildren<AudioSource>();
    }
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
        if (other.gameObject.layer == 9)
        {
            if (!tb.Contains(other.gameObject))
            {
                tb.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
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

        DisplayTime();
    }

    private void DisplayTime()
    {
        if (!win)
        {
            TimeSpan result = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);

            var textTime = result.ToString("mm':'ss':'ff");
            var segments = textTime.Split(':');
            segments[0] = result.TotalMinutes < 1 ? "00" : (result.TotalMinutes).ToString("00");
            var finalText = segments[0] + ":" + segments[1] + ":" + segments[2];

            levelTime.text = finalText;
        }
    }

    private void CheckAmount()
    {
        if (tb.Count == 2)
        {
            if (!win)
            {
                win = true;
                SaveScore();
                //canvas.enabled = true;
                winPanel.SetActive(true);
                lc.YouWin();

                foreach (GameObject objects in tb)
                {
                    objects.GetComponent<PlayerBase>().enabled = false;
                    ParticleSystem confetti = transform.Find("Winning Confetti").gameObject.GetComponent<ParticleSystem>();
                    StopAllAudio();
                    victory.volume = soundVolume;
                    victory.Play();
                    confetti.Play();
                    enabled = false;
                    //TODO: make the final UI
                    //TODO: make winning animation
                }
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
            LevelNumber = levelNumber,
            Time = Time.timeSinceLevelLoad
        };

        var toLevelSelevt = FindObjectOfType<ToLevelSelect>();
        toLevelSelevt.SetScore(scoreModel);
    }

    void StopAllAudio()
    {
        otherSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in otherSources)
        {
            audioS.volume = 0;
        }
    }
}
