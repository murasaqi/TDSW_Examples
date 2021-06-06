using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    
    
    public class HorizontalFlow : MotionTextElement
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
                pos += new Vector3(ch.preferredHeight / 2f, 0f, 0f);
                ch.transform.localPosition = pos;
                
                pos += new Vector3(ch.preferredHeight/2f, 0f, 0f );

                totalHeight += ch.preferredHeight;
            }

            var delayStep = 0.3f / (TextMeshElement.Children.Count - 1);
            
            foreach (var ch in TextMeshElement.Children)
            {
                
                var delay = Random.Range(0f, 0.3f);
                ch.transform.localPosition -= new Vector3(totalHeight/2f, 0f, 0f);
                var flowMo = ch.gameObject.AddComponent<TMProFlowMove>();
                flowMo.Init((1f-delay)*Random.Range(1f,0.8f), delay,animationCurveAsset.SlowMo2,animationCurveAsset.BasicInOut2);
                flowMo.startPos = ch.transform.localPosition;
                flowMo.startAngle = Vector3.zero;
                flowMo.endAngle = new Vector3(Random.Range(-50,50),Random.Range(-70,70),Random.Range(-30,30));
                if(layer >= 0) ch.gameObject.layer = layer;
                float noiseScale = Random.Range(0.3f,0.5f);
                var diffx = Mathf.PerlinNoise(ch.transform.localPosition.x * noiseScale,
                            ch.transform.localPosition.y * noiseScale) * 80 + 500f;
                flowMo.endPos = ch.transform.localPosition += new Vector3(diffx, 0, 0);

                flowMotions.Add(flowMo);
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