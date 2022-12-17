using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    public Button ModernDay;
    [SerializeField]
    public Button WW2;
    [SerializeField]
    private Button AncientGreece;

    private GameObject placedPrefab;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        ChangePrefabTo("Person");
        //assumes your prefabs are named Wayne and Patrick (Could be white_knight)
        ModernDay.onClick.AddListener(() => ChangePrefabTo("Person"));
        WW2.onClick.AddListener(() => ChangePrefabTo("Wehrmacht_A_prefab"));
        AncientGreece.onClick.AddListener(() => ChangePrefabTo("Spartan_Warrior"));
    }

    // Update is called once per frame
    void Update()
    {
            if (placedPrefab == null)
            {
                return;
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    var touchPosition = touch.position;
                    bool isOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                    Debug.Log(isOverUI);
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        Debug.Log(" blocked raycast");
                        return;
                    }
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit) && (hit.transform.tag == "ModernPerson" || hit.transform.tag == "SpartanWarrior" || hit.transform.tag == "ww2Soldier"))
                    {
                        Debug.Log(" raycast");
                        if (Input.GetTouch(0).deltaTime > 0.2f)
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else if (!isOverUI && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                    {
                        Debug.Log(" arraycast");
                        var hitPose = hits[0].pose;
                        Instantiate(placedPrefab, hitPose.position, hitPose.rotation);


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
