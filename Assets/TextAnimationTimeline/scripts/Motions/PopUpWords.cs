using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
    public class BasicBounceUpMove:MonoBehaviour
    {
        public AnimationCurve Curve;
        public float Duration;
        public Vector3 start;
        public Vector3 end;
        
        private float delay;
        public void Init(AnimationCurve curve,float delay)
        {

            Curve = curve;
            this.delay = delay;

        }

        public  void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);
              transform.localPosition = Vector3.Lerp(start,end, Curve.Evaluate(t));
            
        }
    }
    
    
    public class BasicPopup:MonoBehaviour
    {
        public AnimationCurve Curve;
        public float delay;
        public void Init(AnimationCurve curve, float delay)
        {

            Curve = curve;
            this.delay = delay;

        }

        public void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);
            var s = Curve.Evaluate(t);
            transform.localScale = new Vector3(s,s,s);
            
            
        }
    }
    
    
     public class PopUpWords:MotionTextElement
    {
        private List<BasicPopup> _basicPopups = new List<BasicPopup>();
        private List<BasicBounceUpMove> _basicBounceUpMove = new List<BasicBounceUpMove>();
        private List<float> delays = new List<float>();
    
        private GameObject wrapper;
//        private BasicMove wrapperBasicMove;
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
//            wrapper = new GameObject("texts");
//            wrapper.transform.SetParent(transform);
//            wrapper.transform.localPosition = Vector3.zero;
//            wrapper.transform.localEulerAngles = Vector3.zero;


            var delay = 0f;
            var delayStep = (0.5f / (TextMeshElement.Children.Count - 1));
            // var fadeinDuration = (0.5f / (TextMeshElement.Children.Count - 1));
            // var fadeoutDuration = 0.1f;
            foreach (var t in TextMeshElement.Children)
            {
                t.transform.localScale = Vector3.zero;
                // Debug.Log(t.transform.localPosition);
                t.transform.SetParent(transform,false);
                var m = t.gameObject.AddComponent<BasicPopup>();
                m.Init(animationCurveAsset.SteepInOut,delay);
                var b = t.gameObject.AddComponent<BasicBounceUpMove>();
                b.Init(animationCurveAsset.SteepIn,delay);
                
                b.start = t.transform.localPosition + new Vector3(0,-150f, 0);
                b.end =t.transform.localPosition ;
                _basicPopups.Add(m);
                _basicBounceUpMove.Add(b);
                delays.Add(delay);
                delay += delayStep;
            }



            transform.localPosition = OffsetLocalPosition;
        }

        public override void ProcessFrame(double normalizedTime, double seconds)
        {
           
            
            
            // var count = 0;
            for (int i = 0; i < TextMeshElement.Children.Count; i++)
            {
                _basicPopups[i].OnProcess((float)normalizedTime);
                _basicBounceUpMove[i].OnProcess((float)normalizedTime);
            }
        }
    }

}