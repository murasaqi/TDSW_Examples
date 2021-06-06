using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
    public class BasicFadeInOut : MotionTextElement
    {
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
           
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 0f;
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            TextMeshElement.alpha = animationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);
        }

    }
    
    
    public class FadeInOut : MonoBehaviour
    {
        public AnimationCurve Curve;
        public float Duration;
        private TextMeshPro _text;
        private float delay;
        public void Init(AnimationCurve curve,float delay)
        {

            Curve = curve;
            this.delay = delay;
            _text = GetComponent<TextMeshPro>();

        }

        public  void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);
            _text.alpha = Mathf.Lerp(0f,1f, Curve.Evaluate(t));
            
        }
    }
}