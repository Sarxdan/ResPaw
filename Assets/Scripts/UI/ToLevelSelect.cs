using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void Tolevel()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitLevel()
    {
        Application.Quit();
    }

}
