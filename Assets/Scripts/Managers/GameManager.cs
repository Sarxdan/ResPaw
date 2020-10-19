//Created by Fares && Mehmet
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int PlayerOneLife { get; set; }
    public int PlayerTwoLife { get; set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool CanSpawn(PlayerBase player)
    {
        if (player is PlayerOne)
        {
            return PlayerOneLife != 3;
        }
        else
        {
            return PlayerTwoLife != 3;
        }
    }

    public void RemoveLife(PlayerBase player)
    {
        if (player is PlayerOne)
        {
            PlayerOneLife++;
        }
        else
        {
            PlayerTwoLife++;
        }
    }
}
