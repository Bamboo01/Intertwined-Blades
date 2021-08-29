using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour, BaseGazeInterface
{
    public void OnGazeEnter(RaycastHit hit)
    {
        InteractableBase interactable;
        if (hit.collider.gameObject.TryGetComponent(out interactable))
        {
            if (interactable.hasClickInteraction)
            {
                if (Player.Instance._isScreenTouched && !interactable.isClicked)
                {
                    interactable.OnClickStart();
                }
                else if (Player.Instance._isScreenTouched && interactable.isClicked)
                {
                    interactable.OnClickHold();
                }
                else if (interactable.isClicked)
                {
                    interactable.OnClickRelease();
                }
            }
            interactable.OnGazeEnter(gameObject);
        }
    }

    public void OnGazeExit(GameObject gazedObject)
    {
        InteractableBase interactable;
        if (gazedObject.TryGetComponent(out interactable))
        {
            interactable.OnGazeExit(gameObject);
        }
    }

    public void OnGazeStay(RaycastHit hit)
    {
        InteractableBase interactable;
        if (hit.collider.gameObject.TryGetComponent(out interactable))
        {
            if (interactable.hasClickInteraction)
            {
                if (Player.Instance._isScreenTouched && !interactable.isClicked)
                {
                    interactable.OnClickStart();
                }
                else if (Player.Instance._isScreenTouched && interactable.isClicked)
                {
                    interactable.OnClickHold();
                }
                else if (interactable.isClicked)
                {
                    interactable.OnClickRelease();
                }
            }
            interactable.OnGazeStay(gameObject);
        }
    }
}
