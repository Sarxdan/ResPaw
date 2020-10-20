//Created by Fares && Mehmet
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int PlayerOneLife { get; set; }
    public int PlayerTwoLife { get; set; }

    public int playerLives = 3;

    private void Awake()
    {
        PlayerOneLife = playerLives;
        PlayerTwoLife = playerLives;
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
            Debug.Log(PlayerOneLife);
            return PlayerOneLife != 0;
        }
        else
        {
            Debug.Log(PlayerTwoLife);
            return PlayerTwoLife != 0;
        }
    }

    public void RemoveLife(PlayerBase player)
    {
        if (player is PlayerOne)
        {
            PlayerOneLife--;
            
        }
        else
        {
            PlayerTwoLife--;
            
        }
    }
}
