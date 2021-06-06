using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
    public class BasicLine : MonoBehaviour
    {
        private LineRenderer line;
        public Vector3 start;
        public Vector3 end;
        public AnimationCurve curveIn;
        public AnimationCurve curveOut;
        public Material material;
        public float delay;
        public float duration;
        public void Init()
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            var width = Random.Range(3, 6);
            line.startWidth = width;
            line.startWidth = width;
            material = new Material(Shader.Find("Unlit/FadeLine"));
            line.sharedMaterial = material;
            material.SetFloat("_Alpha",0f);
        }

        public void SetLineWidth(float width)
        {
            line.startWidth = width;
            line.endWidth = width;
        }
        public void OnProcess(float time)
        {
            if (time > delay)
            {
                material.SetFloat("_Alpha",1f);
                var t = Mathf.Clamp(time - delay, 0f, duration) / duration;
                var startPos = Vector3.Lerp(start, end, curveIn.Evaluate(t));
                var endPos = Vector3.Lerp(start,end, curveOut.Evaluate(t));
                
                line.SetPosition(0,startPos);
                line.SetPosition(1,endPos);
            }
        }
    }

    public class HorizontalLines : MotionTextElement
    {
        private List<BasicLine> lines = new List<BasicLine>();
        private int size = 4;

        public override void Init(string word, double duration)
        {
            transform.localPosition = Vector3.zero;

            for (int i = 0; i < size; i++)
            {
                var l = new GameObject().AddComponent<BasicLine>();
                l.transform.SetParent(transform);

//                l.gameObject.layer = 14;

                l.delay = Random.Range(0f, 0.3f);
                l.duration = Random.Range(0.5f, 0.7f);

                var y = Random.Range(400, -400);
                var x = Random.Range(-1200f, -800f);
                var endx = Random.Range(600, 1200f);
                l.start = new Vector3(x, y, 0) + transform.position;
                l.end = new Vector3(endx, y, 0) + transform.position;
                l.curveIn = animationCurveAsset.SteepIn;
                l.curveOut = animationCurveAsset.HorizontalLineOut;
//                l.gameObject.layer = 15;
                lines.Add(l);
                l.Init();

//                l.gameObject.layer = 0;
            }
            
            
        }

        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            foreach (var l in lines)
            {
                l.OnProcess((float) normalizedTime);
            }
        }
    }

}