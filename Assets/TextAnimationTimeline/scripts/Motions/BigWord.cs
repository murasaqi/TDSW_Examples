using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    public class FlowUpMotion : MonoBehaviour
    {
        public Vector3 startScale;
        public Vector3 endScale;
        public Vector3 startAngle;
        public Vector3 endAngle;
        public Vector3 endPos;
        public Vector3 startPos;
        public AnimationCurve curve_move;
        public AnimationCurve curve_scale;
        public AnimationCurve curve_alpha;
        public float delay = 0;
        public float duration = 0;
        private TextMeshPro text;

        public void Init()
        {
            text = GetComponent<TextMeshPro>();
        }

        public void OnProcess(float time)
        {

            var t = 0f;
            if (time >= delay)
            {
                t = Mathf.Clamp(time - delay, 0f, duration) / duration;

                var pos = Vector3.Lerp(startPos, endPos, curve_move.Evaluate(t));
                var scale = Vector3.Lerp(startScale, endScale, curve_scale.Evaluate(t));
                var alpha = curve_alpha.Evaluate(t);
                var angle = Vector3.Lerp(startAngle, endAngle, curve_move.Evaluate(t));
                transform.localPosition = pos;
                transform.localScale = scale;
                transform.localEulerAngles = angle;
                text.alpha = alpha;
            }

            
        }
        
    }
    
    public class FlowUp : MotionTextElement
    {
        List<FlowUpMotion> motions = new List<FlowUpMotion>();
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
           
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 0f;

            var width = TextMeshElement.Children.First().preferredWidth * TextMeshElement.Children.Count;
           
            Vector3 pos = Vector3.zero;
            
            foreach (var ch in TextMeshElement.Children)
            {
                pos += new Vector3(ch.preferredWidth/2f, 0f,0f);
                ch.transform.localPosition = pos - new Vector3(width/2f, 0f, 0f);
                ch.gameObject.layer = layer;
                pos += new Vector3(ch.preferredWidth/2f, 0f,0f);
                ch.fontSize *= Random.Range(1f, 0.6f);

                var mo = ch.gameObject.AddComponent<FlowUpMotion>();
                mo.startPos = ch.gameObject.transform.localPosition + new Vector3(0f, -100f, 0f);
                mo.endPos = ch.gameObject.transform.localPosition + new Vector3(0f, 400f, 0f);

                mo.startScale = new Vector3(0,0,0);
                mo.endScale = new Vector3(1,1,1);
                
                mo.startAngle = Vector3.zero;
                mo.endAngle = new Vector3(Random.Range(-30,30),Random.Range(-60,60),Random.Range(-60,60));

                mo.curve_move = animationCurveAsset.BigWordMove;
                mo.curve_alpha = animationCurveAsset.BigWordInOut;
                mo.curve_scale = animationCurveAsset.SteepIn;
                motions.Add(mo);
                

                mo.delay = Random.Range(0f, 0.3f);
                mo.duration = (1f - mo.delay) * Random.Range(1f, 0.8f);
                mo.Init();
            }

            transform.localPosition = OffsetLocalPosition;
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {

            foreach (var m in motions)
            {
                m.OnProcess((float)normalizedTime);
            }
        }

    }
}