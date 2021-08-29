using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] Camera camera;
    
    void Update()
    {
        float xRotation = camera.transform.rotation.eulerAngles.x;
        float yRotation = camera.transform.rotation.eulerAngles.y;
        if (Input.GetKey(KeyCode.W))
        {
            xRotation -= Time.deltaTime * 180.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            xRotation += Time.deltaTime * 180.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            yRotation -= Time.deltaTime * 180.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            yRotation += Time.deltaTime * 180.0f;
        }

        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

#endif  // UNITY_EDITOR
}
