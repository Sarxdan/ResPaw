using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    public GameObject image;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            image.SetActive(false);
        }
        else
        {
            image.SetActive(true);
        }
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            image.SetActive(false);
        }
           
    }
}
