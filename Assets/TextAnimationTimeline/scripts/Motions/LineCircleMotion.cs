using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public class LineCircleMotion : MotionTextElement
    {
        private LineCircle lineCircle;
        private float radius;
        public override void Init(string word, double duration)
        {
            if(Parent != null)transform.SetParent(Parent);
            transform.localPosition = Vector3.zero;
            lineCircle = gameObject.AddComponent<LineCircle>();
            radius = FontSize > 0 ? FontSize : Random.Range(100, 400);
            lineCircle.transform.localPosition = OffsetLocalPosition;

            gameObject.layer = 12;
//            lineCircle.lineWidth = 4;
            lineCircle.Init();
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            lineCircle.alpha = animationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);
            lineCircle.Radius = animationCurveAsset.SteepIn.Evaluate((float) normalizedTime) * radius;
            lineCircle.UpdateCircle();
        }

    }
}