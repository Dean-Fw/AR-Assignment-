using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private Button ModernDay;
    [SerializeField]
    private Button WW2;
    [SerializeField]
    private Button AncientGreece;
    [SerializeField]
    private GameObject OnboardPanel;
    [SerializeField]
    private Button CloseOnboard;
    
    private GameObject placedPrefab;

    private PlacementObject lastSelectedObject;

    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    private Vector2 touchPosistion = default;
 

    // Start is called before the first frame update
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        ChangePrefabTo("Person");
        // Assigns methods to run when a button is pressed 
        ModernDay.onClick.AddListener(() => ChangePrefabTo("Person"));
        WW2.onClick.AddListener(() => ChangePrefabTo("Wehrmacht_A_prefab"));
        AncientGreece.onClick.AddListener(() => ChangePrefabTo("Spartan_Warrior"));
        CloseOnboard.onClick.AddListener(Dismiss);
    }
    private void Dismiss() => OnboardPanel.SetActive(false);

    // Update is called once per frame
    void Update()
    {
        // If there is no set prefab for placement, as in the selected prefab can not be loaded, do nothing 
        if (OnboardPanel.activeSelf)
        {
            return;
        }
        if (placedPrefab == null)
        {
            return;
        }
        // if ther user touches the screen
        if (Input.touchCount > 0)
        {
            //create a an instance of a touch 
            Touch touch = Input.GetTouch(0);
            touchPosistion = touch.position; // take the touches position 
            if (touch.phase == TouchPhase.Began) //if the touch has just began 
            {
                bool isOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId); //find whether the touch is over the UI
                Debug.Log(isOverUI);
                // Stops touches that are over the UI from affecting main screen
                if (isOverUI) //if the touch is over the UI do nothing 
                {
                    Debug.Log(" blocked raycast");
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(touch.position); //touches not over the UI can cast a ray, create instance of a ray from the camera to the position the user touched
                RaycastHit hit; // create an instance of a raycast hit, this is where or what the raycast touches 
                //if a ray from a touch reaches model select it  
                
                if (Physics.Raycast(ray, out hit))
                {
                    lastSelectedObject = hit.transform.GetComponent<PlacementObject>();
                    if (lastSelectedObject != null)
                    {
                        PlacementObject[] allOtherObjects = FindObjectsOfType<PlacementObject>();
                        foreach (PlacementObject placementObject in allOtherObjects)
                        {
                            placementObject.Selected = placementObject == lastSelectedObject;
                        }
                    }
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    lastSelectedObject.Selected = false;
                }
                
            }
            // if instead a ray an AR plane, create a model  
            if (arRaycastManager.Raycast(touchPosistion, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
            {
               
                Pose hitpose = hits[0].pose;
                if(lastSelectedObject == null)
                {
                    lastSelectedObject = Instantiate(placedPrefab, hitpose.position, hitpose.rotation).GetComponent<PlacementObject>();
                }
                else
                {
                    lastSelectedObject.transform.position = hitpose.position;
                    lastSelectedObject.transform.rotation = hitpose.rotation;
                }
            }
        }   
    }
    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"prefabs/{prefabName}");
        if (placedPrefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs...");
        }
        Color ModernDayc = ModernDay.GetComponent<Image>().color;
        Color WW2c = WW2.GetComponent<Image>().color;
        Color AncientGreecec = AncientGreece.GetComponent<Image>().color;

        Debug.Log("is ModernDayPerson" + ModernDayc.a + " " + WW2c.a + " " + AncientGreecec.a);
        switch (prefabName)
        {
            case "Person":
                ModernDayc.a = 1f;
                WW2c.a = 0.5f;
                AncientGreecec.a = 0.5f;
                break;
            case "Wehrmacht_A_prefab":
                Debug.Log("is WW2 soldier ");
                ModernDayc.a = 0.5f;
                WW2c.a = 1f;
                AncientGreecec.a = 0.5f;
                break;

            case "Spartan_Warrior":
                Debug.Log("is Spartan");
                ModernDayc.a = 0.5f;
                WW2c.a = 0.5f;
                AncientGreecec.a = 1f;
                break;

        }
        ModernDay.GetComponent<Image>().color = ModernDayc;
        WW2.GetComponent<Image>().color = WW2c;
        AncientGreece.GetComponent<Image>().color = AncientGreecec;
    }
}
