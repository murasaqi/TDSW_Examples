using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

  
    
   
    public class BasicFadeInOutVertical : MotionTextElement
    {

        public List<TMProFadeInOut> fadeInOuts = new List<TMProFadeInOut>();
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
                var mo = ch.gameObject.AddComponent<TMProFadeInOut>();
                mo.Init((float)duration,0,animationCurveAsset.BasicInOut2);
                // flowMo.Init((1f-delay)*Random.Range(1f,0.8f), delay,animationCurveAsset.SlowMo2,animationCurveAsset.BasicInOut2);
                // flowMo.startPos = ch.transform.localPosition;
                // flowMo.startAngle = Vector3.zero;
                // flowMo.endAngle = new Vector3(Random.Range(-30,30),Random.Range(-30,30),Random.Range(-30,30));

                // float noiseScale = Random.Range(0.3f,0.5f);
                // var diffx = Mathf.PerlinNoise(ch.transform.localPosition.x * noiseScale,
                //             ch.transform.localPosition.y * noiseScale) * 500f;
                // flowMo.endPos = ch.transform.localPosition += new Vector3(diffx, 0, 0);

                fadeInOuts.Add(mo);

//                delay += delayStep;

            }

            transform.localPosition = OffsetLocalPosition;
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);

            foreach (var f in fadeInOuts)
            {
                f.OnProcess((float)normalizedTime);
            }
        }

    }
}