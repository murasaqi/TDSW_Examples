using System;
using System.Collections;
using System.Collections.Generic;
using TextAnimationTimeline.Motions;
//using NUnit.Framework;
using TMPro;
using UnityEngine;


namespace TextAnimationTimeline
{

    
    public class TextAnimationManager : MonoBehaviour
    {
        public AnimationCurveAsset AnimationCurves;
        public TMP_FontAsset BaseFont;
        public float BaseFontSize = 10;
        public GameObject ParentGameObject;
        public bool DebugMode = true;
        public Camera mainCamera;
        public Vector3 Resolution;
       
//        private TMProCapture tmProCapture;
        public Camera CaptureCamera;
       public TextAnimationGraphics graphics;
       
        void Start()
        {
            Init();
        
        }

        void Init()
        {

        }

        void Update()
        {
        }

        public void UpdateText(float curveValue)
        {
        }
        
        public MotionTextElement CreateMotionTextElement(string word, AnimationType animationType)
        {    
            var go = new GameObject(word);
            go.gameObject.name = "text: " + word;
            var parent = ParentGameObject != null ? ParentGameObject.transform : transform;
            go.transform.SetParent(parent);
            // go.transform.localPosition = Vector3.zero;
            // go.transform.localEulerAngles = Vector3.zero;
   
            var motion = SelectMotionType(animationType, go);
            return motion;   
        }

  
        

        private MotionTextElement SelectMotionType(AnimationType animationType, GameObject go)
        {
            MotionTextElement motion;
            switch (animationType)
            {
                    
                case AnimationType.BasicFadeInOut:
                    motion = go.AddComponent<BasicFadeInOut>();
                    break;
                
                case AnimationType.BasicGameTextBoxAnimation:
                    motion = go.AddComponent<BasicGameTextBoxAnimation>();
                    break;
                
                // case AnimationType.Slash:
                //     motion = go.AddComponent<BackgroundSlashs>();
                //     break;
                //
                // case AnimationType.CutSlash:
                //     motion = go.AddComponent<CutSlash>();
                //     break;
                //
                // case AnimationType.LaidWords:
                //     motion = go.AddComponent<LaidWords>();
                //     break;
                //
                // case AnimationType.LaidWords_FromTheLeft:
                //     motion = go.AddComponent<LaidWords_FromTheLeft>();
                //     break;
                //
                // case AnimationType.LineCircle:
                //     motion = go.AddComponent<LineCircleMotion>();
                //     break;
                
                case AnimationType.VerticalFlow:
                    motion = go.AddComponent<VerticalFlow>();
                    break;
                
                // case AnimationType.LineRect:
                //     motion = go.AddComponent<LineRectMotion>();
                //     break;
                //
                // case AnimationType.RectReactionText:
                //     motion = go.AddComponent<RectReactionText>();
                //     break;
                //
                case AnimationType.BasicFadeInOutVertical:
                    motion = go.AddComponent<BasicFadeInOutVertical>();
                    break;
                
                case AnimationType.HorizontalFlow:
                    motion = go.AddComponent<HorizontalFlow>();
                    break;
                
                case AnimationType.HorizontalLines:
                    motion = go.AddComponent<HorizontalLines>();
                    break;
                
                case AnimationType.FlowUp:
                    motion = go.AddComponent<FlowUp>();
                    break;
                
                // case AnimationType.VerticalLines:
                //     motion = go.AddComponent<VerticalLines>();
                //     break;
                
                case AnimationType.PopUpWords:
                    motion = go.AddComponent<PopUpWords>();
                    break;
                
                case AnimationType.SlideIn_LeftToRight:
                    motion = go.AddComponent<SlideIn_LeftToRight>();
                    break;
                
                case AnimationType.SlideIn_RightToLeft:
                    motion = go.AddComponent<SlideIn_RightToLeft>();
                    break;
                    
                default:
                    motion =go.AddComponent<BasicFadeInOut>();
                    break;
                    
            }

            
            motion.animationCurveAsset = AnimationCurves;
            motion.Graphics = graphics;
//            motion.TmProCapture = tmProCapture;
            
            
            
            return motion;
        }
    }
}