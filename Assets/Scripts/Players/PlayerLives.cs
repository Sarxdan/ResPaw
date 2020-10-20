using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] Text restartText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        restartText.text = "P1: " + GameManager.Instance.PlayerOneLife.ToString() + " P2: " + GameManager.Instance.PlayerTwoLife.ToString();
        if(GameManager.Instance.PlayerOneLife == 0 && GameManager.Instance.PlayerTwoLife == 0)
        {
            RestartText();
        }
    }
    public void RestartText()
    {
        restartText.text = "PRESS R TO RESTART";
        restartText.rectTransform.localPosition = new Vector3(0, 0, 0);
    }
}
