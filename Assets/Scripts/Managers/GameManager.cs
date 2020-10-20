//Created by Fares && Mehmet
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    public int PlayerOneLife { get; set; }
    public int PlayerTwoLife { get; set; }

    public int playerLives = 3;

    private void Awake()
    {
        PlayerOneLife = playerLives;
        PlayerTwoLife = playerLives;

    }

    public bool CanSpawn(PlayerBase player)
    {
        if (player is PlayerOne)
        {            
            return PlayerOneLife != 1;
        }
        else
        {
            return PlayerTwoLife != 1;
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
