using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Remain Bugs: Can only create square mesh
 */

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProcedureMesh : MonoBehaviour {

    [SerializeField]
    private float _width = 1f;
    [SerializeField]
    private float _height = 1f;
    [SerializeField]
    private int _widthDevide = 1; 
    [SerializeField]
    private int _heightDevide = 1;

    private Mesh _mesh;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;

	void Awake () {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateMesh();
        }
    }
	
    // Bug Reamins
	private void GenerateMesh () {
        Vector3[] vertices = new Vector3[(_widthDevide + 1) * (_heightDevide + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
        for(int w = 0, i = 0; w <= _widthDevide; ++w)
        {
            for(int h = 0; h <= _heightDevide; ++h, ++i)
            {
                vertices[i] = new Vector3(w, 0, h);
				uv [i] = new Vector2 ((float)w / _widthDevide, (float)h / _heightDevide);
            }
        }

        int[] triangles = new int[_widthDevide * _heightDevide * 6];
        for(int i =0, mesh = 0; i < triangles.Length; i += 6, ++mesh)
        {
            int index = mesh + mesh / _widthDevide;
            triangles[i] = index;
            triangles[i + 1] = index + 1;
            triangles[i + 2] = index + _widthDevide + 1;

            triangles[i + 3] = index + 1;
            triangles[i + 4] = index + _widthDevide + 2;
            triangles[i + 5] = index + _widthDevide + 1;
        }


        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
		_mesh.uv = uv;
		_mesh.RecalculateNormals ();
		GetComponent<MeshCollider> ().sharedMesh = _mesh;
	}
}
