using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
  
    public class VerticalLines : MotionTextElement
    {
        private List<BasicLine> lines = new List<BasicLine>();
        private int size = 5;

        public override void Init(string word, double duration)
        {
            transform.localPosition = Vector3.zero;

            var width = 600f;
            for (int i = 0; i < size; i++)
            {
                var l = new GameObject().AddComponent<BasicLine>();
                l.transform.SetParent(transform);

//                l.gameObject.layer = 14;

                l.delay = Random.Range(0f, 0.5f);
                l.duration =(1f-l.delay) * Random.Range(1f, 0.7f);

                var y = Random.Range(-400, -550);
                var x = -width + (width*2f / (size - 1) * i) + Mathf.PerlinNoise(y*0.1f, i*0.1f) * 400 - 200f;
                var endy = Random.Range(400, 600f);
                l.start = new Vector3(x, y, 0) + transform.position;
                l.end = new Vector3(x, endy, 0) + transform.position;
                
                l.curveIn = animationCurveAsset.SteepIn;
                l.curveOut = animationCurveAsset.HorizontalLineOut;
//                l.gameObject.layer = 15;
                lines.Add(l);
                l.Init();
                l.SetLineWidth(Random.Range(2,10));
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