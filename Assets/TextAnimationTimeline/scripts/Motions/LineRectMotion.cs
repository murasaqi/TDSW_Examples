using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    public class LineRect : MonoBehaviour
    {
        public float lineWidth = 10;
        public float radius = 0f;
        public LineRenderer linreRenderer;
        public Material material;
        public float alpha;
        public void Init()
        {
            gameObject.layer = 12;

            linreRenderer = gameObject.AddComponent<LineRenderer>();
            linreRenderer.positionCount = 4;
            linreRenderer.loop = true;
            linreRenderer.startWidth = lineWidth;
            linreRenderer.endWidth = lineWidth;
            
            material = new Material(Shader.Find("Unlit/TextAnimationTransparent")); 
           
           

            linreRenderer.sharedMaterial = material;
//            material.SetFloat("_Alpha", 0f);
        }

        public void UpdateVertices()
        {
            var vertices = new List<Vector3>();
            vertices.Add(new Vector3(-0.5f,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,-0.5f ,0));
            vertices.Add(new Vector3(-0.5f ,-0.5f ,0));

           

            for (int i = 0; i < vertices.Count; i++)
            {

                Matrix4x4 m = Matrix4x4.TRS(
                    transform.position,
                    Quaternion.Euler(new Vector3(0f, 0f, 45f)),
                    new Vector3(radius, radius, radius)
                );
//
                vertices[i] = m.MultiplyPoint(vertices[i]);
                
                linreRenderer.SetPosition(i,vertices[i]);
            }
            
            material.SetFloat("_Alpha", alpha);
            
        }

        public void OnProcess(float time)
        {
            
        }
    }
    public class LineRectMotion : MotionTextElement
    {
        private LineRect rect;
        public float radius = 0;
        public override void Init(string word, double duration)
        {
            if(Parent != null)transform.SetParent(Parent);
            transform.localPosition = Vector3.zero;
            rect = gameObject.AddComponent<LineRect>();
            radius = FontSize > 0 ? FontSize : Random.Range(100, 400);
            Debug.Log(radius);
            if(OffsetLocalPosition != null)rect.transform.localPosition =OffsetLocalPosition;
            
//            lineCircle.lineWidth = 4;
            rect.Init();
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            rect.alpha = animationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);
            rect.radius = animationCurveAsset.SteepIn.Evaluate((float) normalizedTime) * radius;
            rect.UpdateVertices();
        }

    }
}