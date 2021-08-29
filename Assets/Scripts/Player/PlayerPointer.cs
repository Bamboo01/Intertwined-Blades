using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointer : MonoBehaviour, BaseGazeInterface
{
    [SerializeField] LineRenderer rend;
    [SerializeField] Color gazeColor;
    [SerializeField] Color noGazeColor;
    Vector3[] points = new Vector3[2];
    bool isGazing;

    public void _Start()
    {
        isGazing = false;
        rend.startColor = noGazeColor;
        rend.endColor = noGazeColor;
    }

    void LateUpdate()
    {
        points[0] = transform.position;
        points[1] = transform.position + (transform.forward * 30.0f);
        rend.SetPositions(points);
    }

    void OnEnable()
    {
        rend.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        rend.gameObject.SetActive(false);
    }

    public void OnGazeEnter(RaycastHit hit)
    {
        isGazing = true;
        rend.startColor = gazeColor;
        rend.endColor = gazeColor;
    }

    public void OnGazeStay(RaycastHit hit)
    {
        isGazing = true;
        rend.startColor = gazeColor;
        rend.endColor = gazeColor;
    }

    public void OnGazeExit(GameObject gazedObject)
    {
        isGazing = false;
        rend.startColor = noGazeColor;
        rend.endColor = noGazeColor;
    }
}
