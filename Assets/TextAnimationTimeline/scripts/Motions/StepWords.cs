using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public class HorizontalRandomSizeWriting : MonoBehaviour
    {
        private List<GameObject> words;
        public float fontSizeMin = 50f;
        public float fontSizeMax = 100f;
        public void Init(List<GameObject> words)
        {
            this.words = words;
            var x = 0f;
            var count = 0;
            foreach (var w in this.words)
            {
                w.transform.SetParent(transform);
                w.transform.localPosition = Vector3.zero;
                var s = Random.Range(fontSizeMin, fontSizeMax);
                w.transform.localScale = new Vector3(s,s,s);

                x += s / 2f;
                w.transform.localPosition = new Vector3(x,0f,0f);
                x += s / 2f;
                count++;
            }


            foreach (var w in this.words)
            {
                w.transform.localPosition -= new Vector3(x/2f, 0f,0f);
            }
        }
        
    }
    
    public class VerticalRandomSizeWriting : MonoBehaviour
    {
        private List<GameObject> words;
        public float fontSizeMin = 50f;
        public float fontSizeMax = 100f;
        public void Init(List<GameObject> words)
        {
            this.words = words;
            var y = 0f;
            var count = 0;
            foreach (var w in this.words)
            {
                w.transform.SetParent(transform);
                w.transform.localPosition = Vector3.zero;
                var s = Random.Range(fontSizeMin, fontSizeMax);
                w.transform.localScale = new Vector3(s,s,s);

                y += s / 2f;
                w.transform.localPosition = new Vector3(0,y,0f);
                y += s / 2f;
                count++;
            }


            foreach (var w in this.words)
            {
                w.transform.localPosition -= new Vector3(0, y/2f,0f);
            }
        }
        
    }
   
    public class StepWords : MotionTextElement
    {
        private List<GameObject> words = new List<GameObject>();
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 0f;
            

            // var delay = 0f;
            var delayStep = 0.3f / (TextMeshElement.Children.Count - 1);
            
            foreach (var character in TextMeshElement.Children)
            {
                character.color = Color.white;
                character.alpha = 1f;
                var t = Graphics.WordToTextureGameObject(character,100, textAnimationManager.CaptureCamera);
                words.Add(t);
            }
            
            var horizontal = new GameObject().AddComponent<VerticalRandomSizeWriting>();
            horizontal.name = "VerticalRandomSizeWriting";
            horizontal.transform.SetParent(transform);
            horizontal.Init(words);
            
            
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {

        }

    }
}