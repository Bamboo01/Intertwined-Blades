using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCameraRaycaster : MonoBehaviour
{
    float GazeDistance = 30.0f;
    GameObject gazedObject = null;
    [SerializeField] List<string> GazeableLayers = new List<string>();
    [SerializeField] List<GameObject> listeners = new List<GameObject>();

    int masks;

    void Start()
    {
        masks = LayerMask.GetMask(GazeableLayers.ToArray());
    }

    void Update()
    {
        GameObject newGazedObject;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, GazeDistance, masks))
        {
            newGazedObject = hit.collider.gameObject;
            if (gazedObject == newGazedObject)
            {
                // Gaze stay
                foreach(var a in listeners)
                {
                    if (!a.activeSelf)
                    {
                        continue;
                    }
                    a.SendMessage("OnGazeStay", hit, SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                // Gaze exit
                if (gazedObject != null)
                {
                    foreach (var a in listeners)
                    {
                        if (!a.activeSelf)
                        {
                            continue;
                        }
                        a.SendMessage("OnGazeExit", gazedObject, SendMessageOptions.DontRequireReceiver);
                    }
                }

                // New gaze enter
                foreach (var a in listeners)
                {
                    if (!a.activeSelf)
                    {
                        continue;
                    }
                    a.SendMessage("OnGazeEnter", hit, SendMessageOptions.DontRequireReceiver);
                }

                gazedObject = newGazedObject;
            }
        }
        else
        {
            // Gaze exit
            if (gazedObject != null)
            {
                foreach (var a in listeners)
                {
                    if (!a.activeSelf)
                    {
                        continue;
                    }
                    a.SendMessage("OnGazeExit", gazedObject, SendMessageOptions.DontRequireReceiver);
                }
            }
            gazedObject = null;
        }
    }

    public void AddGazeListener(GameObject gameObject)
    {
        listeners.Add(gameObject);
    }

    public void RemoveGazeListener(GameObject gameObject)
    {
        listeners.Remove(gameObject);
    }
}
