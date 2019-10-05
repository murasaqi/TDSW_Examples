using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineCircle : MonoBehaviour
{
    private const int CircleSegmentCount = 64;
    private const int CircleVertexCount = CircleSegmentCount + 2;
    private const int CircleIndexCount = CircleSegmentCount * 3;

       
//        private MeshFilter _meshFilter;
//        private MeshRenderer _meshRenderer;
    private LineRenderer lineRenderer;
    public float Radius = 10f;
    public float lineWidth = 12;
    public float alpha = 1f;
    private Material material;
    public void Init()

    {

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
//            _meshFilter = gameObject.AddComponent<MeshFilter>();
//            _meshRenderer = gameObject.AddComponent<MeshRenderer> ();
        material = new Material(Shader.Find("Unlit/TextAnimationTransparent"));
//            material.SetColor("_Color", Color.white);
        lineRenderer.sharedMaterial = material;
        


    }

    public void UpdateCircle()
    {
//            _mesh.vertices.Clone()
            
        var vertices = new List<Vector3>(CircleVertexCount);
//            var indices = new int[CircleIndexCount];
        var segmentWidth = Mathf.PI * 2f / CircleSegmentCount;
        var angle = 0f;
        for (int i = 0; i < CircleVertexCount; i++)
        {
            vertices.Add(new Vector3(Mathf.Cos(angle)*Radius, Mathf.Sin(angle)*Radius, 0f)+transform.position);
            angle -= segmentWidth;
                
        }

        lineRenderer.positionCount = vertices.Count;
        lineRenderer.SetPositions(vertices.ToArray());
        material.SetFloat("_Alpha",alpha);

    }

    private void Update()
    {
        UpdateCircle();
    }
}
