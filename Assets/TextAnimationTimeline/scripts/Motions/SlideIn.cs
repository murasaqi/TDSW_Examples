using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
    public class SlideIn_LeftToRight:MotionTextElement
    {
        private List<BasicBounceUpMove> moves = new List<BasicBounceUpMove>();
//        private List<BasicFadeOut> fadeouts = new List<BasicFadeOut>();
        private List<FadeInOut> fadeins = new List<FadeInOut>();
        private GameObject _textsWrapper;
        
        public override void Init(string word, double duration)
        {
            
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            Duration = duration;
             Vector3 position = new Vector3(0,0,0);
            var maxHeight = 0f;
            var texts = new List<TextMeshPro>();
            foreach (var textMesh in TextMeshElement.Children)
            {
                if(texts.Count > 0 )position += new Vector3(texts.Last().preferredWidth / 1.8f, 0f, 0f);
//                textMesh.name = "text: " + ch;
                textMesh.transform.SetParent(transform);
//                textMesh.fontSize = Random.Range(10, 20);
                textMesh.transform.SetParent(transform);
                textMesh.transform.localPosition = Vector3.zero;
                
//                textMesh.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-30f,30f));
                position += new Vector3(textMesh.preferredWidth / 1.8f, 0f, 0f);
                textMesh.transform.localPosition = position;
                if (maxHeight < textMesh.preferredHeight) maxHeight = textMesh.preferredWidth;
                texts.Add(textMesh);
            }
            
            transform.localPosition = OffsetLocalPosition;

            var delayCount = 0.3f / (TextMeshElement.Children.Count - 1);
            var delay = 0f;
            var count = 0;
            foreach (var t in texts)
            {
                
                    delay = 0.3f - count * delayCount;  
                
                
                t.transform.localPosition -= new Vector3(position.x/2f, 0f,0f);
                t.alpha = 0f;
                moves.Add(t.gameObject.AddComponent<BasicBounceUpMove>());
                moves.Last().Init(animationCurveAsset.SteepIn,delay);
                moves.Last().end = new Vector3(t.transform.localPosition.x,0f,t.transform.localPosition.z);
                var startPos = t.transform.localPosition;
                startPos += new Vector3(-300f, 0f, 0f);
                moves.Last().start = startPos;
                
                fadeins.Add(t.gameObject.AddComponent<FadeInOut>());
                fadeins.Last().Init(animationCurveAsset.SteepInOut,delay );

                count++;
            }

            transform.localEulerAngles = OffsetEulerAngles;

        }


        public override void ProcessFrame(double normalizedTime, double seconds)
        {
           
            
            
            // var count = 0;
            for (int i = 0; i < TextMeshElement.Children.Count; i++)
            {
                moves[i].OnProcess((float)normalizedTime);
                fadeins[i].OnProcess((float)normalizedTime);
//                _basicBounceUpMove[i].OnProcess((float)normalizedTime);
            }
        }
    }
    
    
     public class SlideIn_RightToLeft:MotionTextElement
    {
        private List<BasicBounceUpMove> moves = new List<BasicBounceUpMove>();
//        private List<BasicFadeOut> fadeouts = new List<BasicFadeOut>();
        private List<FadeInOut> fadeins = new List<FadeInOut>();
        private GameObject _textsWrapper;
        
        public override void Init(string word, double duration)
        {
            
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            Duration = duration;
             Vector3 position = new Vector3(0,0,0);
            var maxHeight = 0f;
            var texts = new List<TextMeshPro>();
            foreach (var textMesh in TextMeshElement.Children)
            {
                if(texts.Count > 0 )position += new Vector3(texts.Last().preferredWidth / 1.8f, 0f, 0f);
//                textMesh.name = "text: " + ch;
                textMesh.transform.SetParent(transform);
//                textMesh.fontSize = Random.Range(10, 20);
                textMesh.transform.SetParent(transform);
                textMesh.transform.localPosition = Vector3.zero;
                
//                textMesh.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-30f,30f));
                position += new Vector3(textMesh.preferredWidth / 1.8f, 0f, 0f);
                textMesh.transform.localPosition = position;
                if (maxHeight < textMesh.preferredHeight) maxHeight = textMesh.preferredWidth;
                texts.Add(textMesh);
            }
            
            transform.localPosition = OffsetLocalPosition;

            var delayCount = 0.3f / (TextMeshElement.Children.Count - 1);
            var delay = 0f;
            var count = 0;
            foreach (var t in texts)
            {
                
                
                delay = count * delayCount;
                
                
                t.transform.localPosition -= new Vector3(position.x/2f, 0f,0f);
                t.alpha = 0f;
                moves.Add(t.gameObject.AddComponent<BasicBounceUpMove>());
                moves.Last().Init(animationCurveAsset.SteepIn,delay);
                moves.Last().end = new Vector3(t.transform.localPosition.x,0f,t.transform.localPosition.z);
                var startPos = t.transform.localPosition;
                startPos += new Vector3(300f, 0f, 0f);
                moves.Last().start = startPos;
                
                fadeins.Add(t.gameObject.AddComponent<FadeInOut>());
                fadeins.Last().Init(animationCurveAsset.SteepInOut,delay );

                count++;
            }

            transform.localEulerAngles = OffsetEulerAngles;

        }


        public override void ProcessFrame(double normalizedTime, double seconds)
        {
           
            
            
            // var count = 0;
            for (int i = 0; i < TextMeshElement.Children.Count; i++)
            {
                moves[i].OnProcess((float)normalizedTime);
                fadeins[i].OnProcess((float)normalizedTime);
//                _basicBounceUpMove[i].OnProcess((float)normalizedTime);
            }
        }
    }

}