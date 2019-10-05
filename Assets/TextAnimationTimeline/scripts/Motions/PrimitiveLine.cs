using System.Collections.Generic;
using UnityEngine;

namespace TextAnimationTimeline.Graphics
{
    public class PrimitiveLine : MonoBehaviour
    {
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public Mesh mesh;
        public float scale = 200;
        public List<Vector3> vertices;

        public void Init(Vector3 start, Vector3 end)
        {
            if (vertices != null)
            {
                UpdateLine(start,end);
            }
            else
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material(Shader.Find("Unlit/Transparent"));
                mesh = new Mesh();
        
                vertices = new List<Vector3>();
                vertices.Add(start);
                vertices.Add(end);
     

        
                var indices = new List<int>();
        
                indices.Add(0);
                indices.Add(1);
        
                mesh.SetVertices(vertices);
                mesh.SetIndices(indices.ToArray(),MeshTopology.Lines,0);

                meshFilter.mesh = mesh;      
            }
          
        }

        public void UpdateLine(Vector3 start, Vector3 end)
        {
            vertices.Clear();
            vertices.Add(start);
            vertices.Add(end);
     

        
            var indices = new List<int>();
        
            indices.Add(0);
            indices.Add(1);

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices.ToArray(),MeshTopology.Lines,0);

            meshFilter.mesh = mesh;      
        }
    }
}