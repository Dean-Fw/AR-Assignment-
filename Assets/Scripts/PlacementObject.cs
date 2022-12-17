using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementObject : MonoBehaviour
{
    [SerializeField]
    bool IsSelected;

    public bool Selected
    {
        get
        {
            return this.IsSelected;
        }
        set
        {
            IsSelected = value;

        }
    }


    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    private string OverlayDisplayText;

    public void SetOverlayText(string text)
    {
        if (OverlayText != null)
        {
            OverlayText.gameObject.SetActive(true);
            OverlayText.text = text;
        }
    }


    void Awake()
    {
        //gets overlay text
        OverlayText = GetComponentInChildren<TextMeshPro>();
        if (OverlayText != null)
        {
            Debug.Log("Text Found!");
            OverlayText.gameObject.SetActive(false);
        }

    }
    public void ToggleOverlay()
    {
        OverlayText.gameObject.SetActive(IsSelected);



    }

}    
