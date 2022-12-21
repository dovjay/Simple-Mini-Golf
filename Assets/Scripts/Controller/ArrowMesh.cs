using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowMesh : MonoBehaviour
{
    [SerializeField]
    private Sprite arrowSprite;
    
    private MeshFilter meshFilter;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mesh.vertices = Array.ConvertAll(arrowSprite.vertices, i => (Vector3)i);
        mesh.uv = arrowSprite.uv;
        mesh.triangles = Array.ConvertAll(arrowSprite.triangles, i => (int)i);

        meshFilter.mesh = mesh;
    }
}
