using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//created by Daniel
//peer reviewed by Mehmet

public class MainMenuController : MonoBehaviour
{
    public Button button2, button3, button4;

    //This to be changes to A star?
    public Text level1Deaths, level2Deaths, level3Deaths;
    public int levelpassed;


    void Start()
    {
        levelpassed = PlayerPrefs.GetInt("LevelPassed");
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

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
        GetAllLevelsScores();
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

    private void GetAllLevelsScores()
    {
        var allScores = ScoreManager.GetScores();

        foreach (var score in allScores.Scores)
        {
            switch (score.LevelNumber)
            {
                case 1:
                    level1Deaths.text = "Deaths =" + score.DeathCount;
                    break;
                case 2:
                    level2Deaths.text = "Deaths =" + score.DeathCount;
                    break;
                case 3:
                    level3Deaths.text = "Deaths =" + score.DeathCount;
                    break;
                default:
                    break;
            }
        }
    }


}
