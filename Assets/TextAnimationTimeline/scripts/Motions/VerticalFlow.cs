using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    public class TMProFadeInOut : MonoBehaviour
    {
        private TextMeshPro tmPro;
        public float duration;
        public float delay;
        public AnimationCurve curve;
        private float alpha;
        public void Init(float duration, float delay, AnimationCurve curve)
        {
            this.duration = duration;
            this.delay = delay;
            this.curve = curve;
            tmPro = GetComponent<TextMeshPro>();
            alpha = 0f;
            tmPro.alpha = alpha;
        }


        public void OnProcess(float time)
        {
            var t = 0f;
            
            if (time >= delay)
            {
                t = Mathf.Clamp(time-delay, 0f, duration) / duration;
                alpha = curve.Evaluate(t);
                tmPro.alpha = alpha;
            }
        }
    }
    
    
    public class TMProFlowMove : MonoBehaviour
    {
        private TextMeshPro tmPro;
        public float duration;
        public float delay;
        public AnimationCurve curve;
        public AnimationCurve fadeInOutCurve;
        public Vector3 startPos;
        public Vector3 endPos;
        public Vector3 startAngle;
        public Vector3 endAngle;
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
                transform.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(t));

                transform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, curve.Evaluate(t));

                tmPro.alpha = fadeInOutCurve.Evaluate(t);
            }
        }
    }
    public class VerticalFlow : MotionTextElement
    {

        public List<TMProFlowMove> flowMotions = new List<TMProFlowMove>();
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
           
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 1f;
            
            
            var pos = Vector3.zero;
            var totalHeight = 0f;
            foreach (var ch in TextMeshElement.Children)
            {
                pos += new Vector3(0f,-ch.preferredHeight / 2f, 0f);
                ch.transform.localPosition = pos;
                if (layer != -1) ch.gameObject.layer = layer;
                pos += new Vector3(0f, -ch.preferredHeight/2f, 0f );

                totalHeight += ch.preferredHeight;
            }

            var delayStep = 0.3f / (TextMeshElement.Children.Count - 1);
            
            foreach (var ch in TextMeshElement.Children)
            {
                var delay = Random.Range(0f, 0.3f);
                ch.transform.localPosition += new Vector3(0f, totalHeight/2f, 0f);
                var flowMo = ch.gameObject.AddComponent<TMProFlowMove>();
                flowMo.Init((1f-delay)*Random.Range(1f,0.8f), delay,animationCurveAsset.SlowMo2,animationCurveAsset.BasicInOut2);
                flowMo.startPos = ch.transform.localPosition;
                flowMo.startAngle = Vector3.zero;
                flowMo.endAngle = new Vector3(Random.Range(-30,30),Random.Range(-30,30),Random.Range(-30,30));

                float noiseScale = Random.Range(0.3f,0.5f);
                var diffx = Mathf.PerlinNoise(ch.transform.localPosition.x * noiseScale,
                            ch.transform.localPosition.y * noiseScale) * 500f;
                flowMo.endPos = ch.transform.localPosition += new Vector3(diffx, 0, 0);

                flowMotions.Add(flowMo);

//                delay += delayStep;

            }

            transform.localPosition = OffsetLocalPosition;
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);

            foreach (var f in flowMotions)
            {
                f.OnProcess((float)normalizedTime);
            }
        }

    }
}