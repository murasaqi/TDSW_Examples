using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public class SineWave : MotionTextElement
    {
        private Material material;
        // private float time = 0f;
        public override void Init(string word, double duration)
        {
            name = "sinWave";
            var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
            q.transform.SetParent(transform);
            q.transform.localScale  = new Vector3(1920,1080);
            q.transform.localPosition = Vector3.zero;
            material = new Material(Shader.Find("Unlit/SineWave"));
            q.GetComponent<MeshRenderer>().sharedMaterial = material;
            q.gameObject.layer = 12;
            material.SetFloat("_Threshold", -12);
            transform.localPosition = Vector3.zero;
        }

        void Update()
        {
        
            
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            material.SetFloat("_Timer", (float)normalizedTime * 3.4f);
            var with = material.GetFloat("_LineWidth");
            material.SetFloat("_Threshold", Mathf.Lerp(-0.5f, (float)normalizedTime + with* 1.4f, (float)normalizedTime));
        }

    }
}