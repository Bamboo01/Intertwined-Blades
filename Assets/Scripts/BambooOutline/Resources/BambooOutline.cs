using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BambooOutline : MonoBehaviour
{
    [SerializeField, Range(0.001f, 2.0f)]
    private float outlineWidth = 0.001f;
    [SerializeField]
    private Color outlineColor = Color.white;

    private Renderer[] renderers;
    private MeshFilter[] meshfilters;
    private Material outlineMaterial = null;
    private GameObject[] outlineGameObjects;

   public List<string> OutlineLayers = new List<string>();

    private bool isInitialized = false;

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            if (outlineMaterial != null)
            {
                outlineColor = value;
                outlineMaterial.SetColor("_OutlineColor", outlineColor);
            }
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            if (outlineMaterial != null)
            {
                outlineWidth = value;
                outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
            }
        }
    }

    [ContextMenu("Update Editor Outline")]
    void UpdateOutlines()
    {
        OutlineColor = outlineColor;
        OutlineWidth = outlineWidth;
    }

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        meshfilters = GetComponentsInChildren<MeshFilter>();
        outlineGameObjects = new GameObject[renderers.Length];
        outlineMaterial = Instantiate(Resources.Load<Material>(@"OutlineMaterial"));
        outlineMaterial.name = "OutlineMaterial (Instance)";

        for (int i = 0; i < renderers.Length; i++)
        {
            if (OutlineLayers.Count != 0)
            {
                if (OutlineLayers.Find((a) => { return a == LayerMask.LayerToName(renderers[i].gameObject.layer); }) == null)
                {
                    continue;
                }
            }

            GameObject clone = new GameObject(renderers[i].gameObject.name + " Outline");
            Instantiate(clone);
            clone.transform.parent = renderers[i].transform;
            outlineGameObjects[i] = clone;
            var renderer = outlineGameObjects[i].AddComponent<MeshRenderer>();
            var mesh = outlineGameObjects[i].AddComponent<MeshFilter>();

            Material[] mat = new Material[1]{ outlineMaterial };
            renderer.materials = mat;

            mesh.mesh = meshfilters[i].mesh;

            outlineGameObjects[i].transform.position = renderers[i].transform.position;
            outlineGameObjects[i].transform.localRotation = Quaternion.identity;
            outlineGameObjects[i].transform.localScale = new Vector3(1, 1, 1);

            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        }
        isInitialized = true;
    }

    void OnEnable()
    {
        if (isInitialized)
        {
            foreach (GameObject go in outlineGameObjects)
            {
                go.SetActive(true);
            }
        }
    }

    void OnDisable()
    {
        if (isInitialized)
        {
            foreach (GameObject go in outlineGameObjects)
            {
                go.SetActive(false);
            }
        }
    }
}
