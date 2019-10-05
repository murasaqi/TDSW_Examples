using System.Collections.Generic;
using UnityEngine;

namespace TextAnimationTimeline.Graphics
{
    public class PrimitiveQuad : MonoBehaviour
    {

        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public Mesh mesh;
        public float scale = 200;
        public List<Vector3> vertices;
        public MeshCollider meshCollider;
        public void Init()
        {
        
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Unlit/Transparent"));
            mesh = new Mesh();
        
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(-0.5f,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,-0.5f ,0));
            vertices.Add(new Vector3(-0.5f ,-0.5f ,0));


            var count = 0;
            for (int i = 0; i < vertices.Count; i++)
            {

                Matrix4x4 m = Matrix4x4.TRS(
                    Vector3.zero,
                    Quaternion.Euler(30, 45, 0),
                    new Vector3(scale,scale,scale)
                );
//
                vertices[i] = m.MultiplyPoint(vertices[i]);
                Debug.Log(vertices[count]);
            }

            var indices = new List<int>();
            indices.Add(0);
            indices.Add(1);
            indices.Add(3);
            indices.Add(3);
            indices.Add(1);
            indices.Add(2);
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(),MeshTopology.Triangles,0);

        
            var uvs = new List<Vector2>();
            uvs.Add(new  Vector2(0,1));
            uvs.Add(new  Vector2(1,1));
            uvs.Add(new  Vector2(1,0));
            uvs.Add(new  Vector2(0,0));
            mesh.uv = uvs.ToArray();
        
            mesh.RecalculateNormals();
        
            Debug.Log(mesh.normals.Length);
        
            meshFilter.mesh = mesh;
        
            meshCollider = gameObject.AddComponent<MeshCollider>();
       


        }
    }
}