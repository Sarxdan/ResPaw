using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLives : MonoBehaviour
{
    //[SerializeField] Text restartText;
    public TextMeshProUGUI retryText;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        retryText.text = "P1: " + GameManager.Instance.PlayerOneLife.ToString() + " P2: " + GameManager.Instance.PlayerTwoLife.ToString();
        if(GameManager.Instance.PlayerOneLife == 0 && GameManager.Instance.PlayerTwoLife == 0)
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
