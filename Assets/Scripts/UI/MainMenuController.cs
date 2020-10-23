using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public Button button2, button3, button4;
    public int levelpassed;
    void Start()
    {       
        levelpassed = PlayerPrefs.GetInt("LevelPassed");
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        switch (levelpassed)
        {
            case 1:
                button2.interactable = true;             
                break;
            case 2:
                button2.interactable = true;
                button3.interactable = true;
                break;
            case 3:
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;
                break;

        }
    }

    public void LevelToLoad(int level)
    {
       int test = level;

        SceneManager.LoadScene(level);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);//change 0 to whatever is mainmenu
    }   
    public void reset()
    {
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
        PlayerPrefs.DeleteAll();
    }
        

}
