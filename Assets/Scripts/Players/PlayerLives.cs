using UnityEngine;
using TMPro;

public class PlayerLives : MonoBehaviour
{
    //[SerializeField] Text restartText;
    public TextMeshProUGUI retryText;
    GameManager manager;
    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        retryText.text = "P1: " + manager.PlayerOneLife.ToString() + " P2: " + manager.PlayerTwoLife.ToString();
        if(manager.PlayerOneLife == 0 && manager.PlayerTwoLife == 0)
        {
            RestartText();
        }
    }
    public void RestartText()
    {
        retryText.text = "PRESS R TO RESTART";
        retryText.rectTransform.localPosition = new Vector3(0, 0, 0);
    }
}
