using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public CanvasOptions optionsMenu;

    private void Start()
    {
        if (!optionsMenu)
        {
            Debug.LogError("Unable to find options menu component.");
        }
    }

    // this method is required in order to receive PointerUp events
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    // play test sound when slider control is released
    public void OnPointerUp(PointerEventData eventData)
    {
        if (optionsMenu)
        {
            optionsMenu.PlayTestSound();
        }
    }
}
