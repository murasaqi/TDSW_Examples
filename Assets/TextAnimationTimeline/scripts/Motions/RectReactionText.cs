using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    
    public class ScaleInOut : MonoBehaviour
    {
        private TextMeshPro tmPro;
        public float duration;
        public float delay;
        public AnimationCurve curve;
        public AnimationCurve fadeInOutCurve;
        
        public Vector3 endScale;
        public void Init(float duration, float delay, AnimationCurve curve, AnimationCurve fadeInOutCurve)
        {
            this.duration = duration;
            this.delay = delay;
            this.curve = curve;
            this.fadeInOutCurve = fadeInOutCurve;
            tmPro = GetComponent<TextMeshPro>();
            tmPro.alpha = 0f;

        }
        public void OnProcess(float time)
        {
            var t = 0f;
            
            if (time >= delay)
            {
                t = Mathf.Clamp(time-delay, 0f, duration) / duration;
                transform.localScale = Vector3.Lerp(Vector3.zero, endScale, curve.Evaluate(t));

                tmPro.alpha = fadeInOutCurve.Evaluate(t);
            }
        }
    }
  
    public class RectReactionText : MotionTextElement
    {

        public List<ScaleInOut> scaleInouts = new List<ScaleInOut>();
        public List<LineRect> lineRects = new List<LineRect>();
        public List<float> delays = new List<float>();
        public List<float> durations = new List<float>();
        private float radius = 0f;
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
           
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 1f;
            transform.localPosition = OffsetLocalPosition;
            
            var pos = Vector3.zero;
            var totalHeight = 0f;
            foreach (var ch in TextMeshElement.Children)
            {
                pos += new Vector3(0f,-ch.preferredHeight / 2f, 0f);
                ch.transform.localPosition = pos;
                
                pos += new Vector3(0f, -ch.preferredHeight/2f, 0f );

                totalHeight += ch.preferredHeight;
            }

            var totalDelay = 0.3f;
            var delayStep =totalDelay / (TextMeshElement.Children.Count - 1);
            var delay = 0f;
            foreach (var ch in TextMeshElement.Children)
            {
                ch.transform.localPosition += new Vector3(0f, totalHeight/2f, 0f);
                ch.gameObject.layer = 14;
                var mo = ch.gameObject.AddComponent<ScaleInOut>();
                mo.Init(1f-totalDelay, delay, animationCurveAsset.SteepIn,
                    animationCurveAsset.BasicInOut);
                mo.endScale = new Vector3(transform.localScale.x,transform.localScale.x, transform.localScale.x);
                scaleInouts.Add(mo);

                var rect = new GameObject("rect").AddComponent<LineRect>();
                rect.transform.SetParent(ch.transform);
                rect.transform.localPosition = Vector3.zero;
                
                rect.radius = 0f;
                rect.Init();
                rect.gameObject.layer = 12;
                lineRects.Add(rect);
                
                delays.Add(delay);
                durations.Add(1f-totalDelay);
//                lineRectMotions.Add(rect);

                radius = ch.preferredWidth*2f;
                ch.fontSize *= 0.6f;
                
                delay += delayStep;


            }
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);

            float time = (float) normalizedTime;
            var count = 0;
            foreach (var f in scaleInouts)
            {
                f.OnProcess((float)normalizedTime);
                
            }

            foreach (var rect in lineRects)
            {
                float t = 0f;
                if (time >= delays[count])
                {
                    t = Mathf.Clamp(time - delays[count], 0f, durations[count]) / durations[count];
                   
                }
                
                rect.alpha = animationCurveAsset.BasicInOut.Evaluate(t);
                rect.radius = animationCurveAsset.SlowMo.Evaluate(t) * radius;
                rect.UpdateVertices();

                count++;
            }
            
            
        }

    }
}