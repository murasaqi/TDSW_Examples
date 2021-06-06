using System.Collections.Generic;
using System.Linq;
//using Kaf;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextAnimationTimeline.Motions
{
    public class StretchMoveWord : MotionTextElement
    {
        private List<TextMeshElement> textMeshElements = new List<TextMeshElement>();
        private List<GameObject> wordQuads = new List<GameObject>();
        public override void Init(string word, double duration)
        {
            
//              string[] words;
            var words = word.Split('/').ToList();
//            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);

            foreach (var w in words)
            {
                Debug.Log(w);
                textMeshElements.Add(CreateTextMeshElement(w,Font,FontSize,TextSegmentationOptions.Word));
            }

            foreach (var t in textMeshElements)
            {
                var texture =Graphics.WordToTextureGameObject(t.Children[0], (int)Random.Range(500,1000),textAnimationManager.CaptureCamera);
                texture.transform.SetParent(transform);
                wordQuads.Add(texture);
            }
            PackingWords();
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);
            
        }

        private void PackingWords()
        {
            var resolution = textAnimationManager.Resolution;
            var startPos = new Vector3(
                    -resolution.x/2f*Random.Range(0.8f,0.9f),
                    resolution.y/2f,
                    0f
                );
            
            var isVertical = (int) Random.Range(0, 2) == 1;
            var pastIsVertical = isVertical;
            var count = 0;
            var prevTransform = Vector3.zero;
            foreach (var word in wordQuads)
            {
                if (count > 0)
                    prevTransform = wordQuads[count - 1].transform.localScale;

                isVertical = !isVertical;
//                isVertical = (int) Random.Range(0, 2) == 1;
//                isVertical = true;
                if (isVertical)
                {
                    float x = word.transform.localScale.x / 2f;
                    float y = 0f;
                    if (count > 0 && !pastIsVertical)
                    {
                        y += word.transform.localScale.y / 2f;
                        x += wordQuads[count - 1].transform.localScale.y / 2f;
                    }
                    startPos += new Vector3(x, y,0f);    
                    word.transform.localPosition = startPos;
                    startPos += new Vector3(word.transform.localScale.x /2f, 0f, 0f);
                    
                }
                else
                {
                    float x = 0f;
                    if(pastIsVertical) x = -word.transform.localScale.y / 2f;
                    float y = -word.transform.localScale.x / 2f;
                    if (pastIsVertical) y -= prevTransform.y / 2f;
                    startPos += new Vector3(
                        x,
                        y,
                        0f);
                    word.transform.localPosition = startPos;
                    word.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    startPos += new Vector3(0f, -word.transform.localScale.x/2f, 0f);

                }

                count++;
                pastIsVertical = isVertical;
            }
            
           
            
            
            
        }

    }
}