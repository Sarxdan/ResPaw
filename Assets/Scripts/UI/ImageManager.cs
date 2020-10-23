using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    public GameObject preview;
    public GameObject arrow;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            preview.SetActive(false);
            arrow.SetActive(false);
        }
        else
        {
            preview.SetActive(true);
            arrow.SetActive(true);
        }
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            preview.SetActive(false);
            arrow.SetActive(false);
        }
           
    }
}
