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
    private string OverlayTextName;


    void Awake()
    {
        //gets overlay text
        OverlayText = GetComponentInChildren<TextMeshPro>();
        if (OverlayText != null)
        {
            OverlayText.gameObject.SetActive(false);
        }
    }
    public void ToggleOverLay()
    {
        OverlayText.gameObject.SetActive(IsSelected);
    }
}
    
