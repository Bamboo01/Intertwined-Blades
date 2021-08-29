using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseGazeInterface
{
    abstract void OnGazeEnter(RaycastHit hit);
    abstract void OnGazeStay(RaycastHit hit);
    abstract void OnGazeExit(GameObject gazedObject);
}
