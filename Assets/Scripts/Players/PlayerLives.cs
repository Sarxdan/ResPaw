using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    //[SerializeField] Text restartText;
    public TextMeshProUGUI retryText;
    public GameObject lostPanel;
    private int restart;
    GameManager manager;
    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        restart = SceneManager.GetActiveScene().buildIndex;
        //lostCanvas.enabled = false;
        lostPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        retryText.text = "P1: " + manager.PlayerOneLife.ToString() + " P2: " + manager.PlayerTwoLife.ToString();
        if(manager.PlayerOneLife == 0 || manager.PlayerTwoLife == 0)
        {
            //RestartText();
            //lostCanvas.enabled = true;
            lostPanel.SetActive(true);
        }
    }
    public void RestartText()
    {
        retryText.text = "PRESS R TO RESTART";
        retryText.rectTransform.localPosition = new Vector3(0, 0, 0);       
    }

    public void ToLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void Restart()
    {
        SceneManager.LoadScene(restart);
    }
}
