using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TextAnimationTimeline
{
    public class TextMeshElement:MonoBehaviour
    {
        public List<TextMeshPro> Children { get; set; }
        private float _alpha;
        private MotionTextAlignmentOptions _motionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
        private TextSegmentationOptions _textSegmentationOptions = TextSegmentationOptions.Word; 

        public void Init(string words, TMP_FontAsset font, float fontSize, TextSegmentationOptions textSegmentationOptions)
        {
            Children = new List<TextMeshPro>();
            if (textSegmentationOptions == TextSegmentationOptions.Character)
            {
                foreach (var w in words)
                {
                    var text = TextGenerater.CreateTextMesh(w.ToString(), font, fontSize);
                    text.transform.SetParent(transform,false);
                    text.gameObject.layer = 14;
                    // text.transform.localPosition = Vector3.zero;
                    // text.transform.localEulerAngles = Vector3.zero;
                    // text.transform.localScale = Vector3.one;
                    text.alignment = TextAlignmentOptions.CenterGeoAligned;
                    Children.Add(text);
                }
            }
            else
            {
                var text = TextGenerater.CreateTextMesh(words, font, fontSize);
                text.alignment = TextAlignmentOptions.CenterGeoAligned;
                text.transform.SetParent(transform,false);
                Children.Add(text);
            }
            
            
        }

        public void AlignCenter()
        {
            
            
            var x = 0f;
            var totalWidth = 0f;
            var count = 0;
            foreach (var t in Children)
            {
                if(count > 0 )x += Children.Last().preferredWidth / 2f;
                x += t.preferredWidth /2f;
//                Debug.Log(x);
                t.transform.localPosition = new Vector3(x,t.transform.localPosition.x,t.transform.localPosition.z);
                totalWidth += t.preferredWidth;
                count++;
            }
                        
            var diff = new Vector3(totalWidth/2f, 0f, 0f);
            
            foreach (var t in Children)
            {
                t.transform.localPosition -= diff;
            }

        }
        
        public void AlignMiddle()
        {

            foreach (var textMesh in Children)
            {
                textMesh.transform.localPosition = new Vector3(
                    textMesh.transform.localPosition.x,
                    0f,
                    textMesh.transform.localPosition.z
                    );
            }

        }

        public TextSegmentationOptions TextSegmentationOptions
        {
            get => _textSegmentationOptions;
            set => _textSegmentationOptions = value;
        }

        public float alpha
        {
            get => _alpha;
            set
            {
                _alpha = value;
                foreach (var text in Children)
                {
                    text.alpha = _alpha;
                }
            }
        }


        public MotionTextAlignmentOptions MotionTextAlignmentOptions
        {
            
            set
            {
                _motionTextAlignmentOptions = value;
                if(Children.Count == 0 && Children == null) return;
                switch (value)
                {
                    case MotionTextAlignmentOptions.MiddleCenter:
                        AlignCenter();
                        AlignMiddle();
                        break;
                    default:
                        break;
                        
                }
            }

            get { return _motionTextAlignmentOptions; }
        }
        
        
    }
}