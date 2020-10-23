using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void Tolevel()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitLevel()
    {
        Application.Quit();
    }

}
