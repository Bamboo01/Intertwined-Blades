using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBillboard : MonoBehaviour
{
    [HideInInspector] public Transform center;
    [HideInInspector] public Vector3 axis = Vector3.up;
    [HideInInspector] public Vector3 desiredPosition;
    [HideInInspector] public int terrainMask;

    void Awake()
    {
        terrainMask = LayerMask.GetMask("Terrain");
        center = Player.Instance.controller.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(center, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += 100.0f;
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, terrainMask))
        {
            transform.position = hit.point;
        }
    }
}
