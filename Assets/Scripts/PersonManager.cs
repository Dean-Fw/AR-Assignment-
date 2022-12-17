using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PersonManager : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject personPrefab;

    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

    // Update is called once per frame
    void Update()
    {
       if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if(Input.touchCount == 1)
                {
                    if(arRaycastManager.Raycast(touch.position, arRaycastHits))
                    {
                        var pose = arRaycastHits[0].pose;
                        CreatePerson(pose.position);
                        return;
                    }
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if(hit.collider.tag == "ModernPerson")
                        {
                            DeletePerson(hit.collider.gameObject);
                        }
                    }
                }
            }
        } 
    }

    private void CreatePerson(Vector3 position)
    {
        Instantiate(personPrefab, position, Quaternion.identity);
    }
    private void DeletePerson(GameObject personObject)
    {
        Destroy(personObject);
    }
}
