using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSize : MonoBehaviour
{
    public Camera CameraClass;
    private Builds BuildsClass;
    public float sizeGrid = 1000f;
    void Start()
    {
        BuildsClass = CameraClass.GetComponent<Builds>();
        float round = BuildsClass.roundCoordinatesConst;
        MeshRenderer MeshRendererClass = gameObject.GetComponent<MeshRenderer>();
        MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(sizeGrid * round, sizeGrid * round));
    }

    void Update()
    {
        
    }
}

