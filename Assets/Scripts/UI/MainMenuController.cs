using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class MainMenuController : MonoBehaviour
{
    public Button button2, button3, button4;
    public int levelpassed;
    void Start()
    {       
        levelpassed = PlayerPrefs.GetInt("LevelPassed");
        //button2.interactable = false;
        //button3.interactable = false;
        //utton4.interactable = false;

        switch (levelpassed)
        {
            case 3:
                button2.interactable = true;             
                break;
            case 4:
                button2.interactable = true;
                button3.interactable = true;
                break;
            case 5:
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;
                break;

        }
    }

    public void LevelToLoad(int level)
    {
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
