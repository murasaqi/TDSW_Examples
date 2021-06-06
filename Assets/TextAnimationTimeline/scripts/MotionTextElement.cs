using System;
using System.Collections.Generic;
using System.Linq;
using TextAnimationTimeline.Motions;
//using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

namespace TextAnimationTimeline
{
    
    
//    [MenuItem ("Assets/Change FrameRate")]

    public class MotionTextElement : MonoBehaviour
    {

        [ExecuteInEditMode]
        private TextMeshElement _textMeshElement;
        private TMP_FontAsset _font;
        private MotionTextAlignmentOptions _motionTextAlignmentOptions;
        private Vector3 _offsetEulerAngles;
        private Vector3 _offsetLocalScale;
        private Vector3 _offsetLocalPosition;
        private double _duration;
        private TextSegmentationOptions _textSegmentationOptions;
        private AnimationCurveAsset _animationCurvesAsset;
        private Transform _parent;
        private bool _debugMode;
        private TextAnimationManager _textAnimationManager;
        private TimelineClip _clip;
        private TextAnimationControlBehaviour _textAnimationControlBehaviour;
        private Transform _referenceTransform;
        private int _layer;
        private TextAnimationGraphics _graphics;
//        private TMProCapture _tmProCapture;
        public int ID;


        public TimelineClip clip
        {
            get => _clip;
            set => _clip = value;
        }

        public TextAnimationControlBehaviour textAnimationControlBehaviour
        {
            get => _textAnimationControlBehaviour;
            set => _textAnimationControlBehaviour = value;
        }

        //        private FontAsset fontsAsset;
        public float FontSize;

        public TMP_FontAsset Font
        {
            get => _font;
            set => _font = value;
        }

//        public TMProCapture TmProCapture
//        {
//            get => _tmProCapture;
//            set => _tmProCapture = value;
//        }

        public TextAnimationGraphics Graphics
        {
            get => _graphics;
            set => _graphics = value;
        }

        public int layer
        {
            get => _layer;
            set => _layer = value;
        }

        public TextAnimationManager textAnimationManager
        {
            get { return _textAnimationManager; }
            set { _textAnimationManager = value; }
        }
        

        public AnimationCurveAsset animationCurveAsset
        {
            set => _animationCurvesAsset = value;
            get => _animationCurvesAsset;
        }

        public Transform referenceTransform
        {
            get => _referenceTransform;
            set => _referenceTransform = value;
        }

        public double Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public Vector3 OffsetLocalPosition
        {
            get => _offsetLocalPosition;
            set => _offsetLocalPosition = value;
        }
        public Vector3 OffsetLocalScale
        {
            get => _offsetLocalScale;
            set => _offsetLocalScale = value;
        }
        
        public Vector3 OffsetEulerAngles
        {
            get => _offsetEulerAngles;
            set => _offsetEulerAngles = value;
        }

        public TextMeshElement TextMeshElement
        {
            get => _textMeshElement;
            set => _textMeshElement = value;
        }

        public MotionTextAlignmentOptions MotionTextAlignmentOptions
        {
            get => _motionTextAlignmentOptions;
            set => _motionTextAlignmentOptions = value;
        }

        public virtual void Init(string word, double duration)
        {
            
        }
       
        public virtual void ProcessFrame(double normalizedTime, double seconds)
        {
            
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public TextSegmentationOptions TextSegmentationOptions
        {
            get => _textSegmentationOptions;
            set => _textSegmentationOptions = value;
        }

        public TextMeshElement CreateTextMeshElement(string word, TMP_FontAsset font, float fontSize)
        {
            var textMeshElement = CreateTextMeshElement(word, font, fontSize, TextSegmentationOptions);
            return textMeshElement;   
        }
        
        public TextMeshElement CreateTextMeshElement(string word, TMP_FontAsset font, float fontSize, TextSegmentationOptions option)
        {
            var go = new GameObject("textMeshElement");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
//            go.gameObject.layer = 14;
            var textMeshElement = go.AddComponent<TextMeshElement>();
            textMeshElement.Init(word, font,fontSize,option);
            textMeshElement.gameObject.layer = 14;
            return textMeshElement;   
        }

        public bool DebugMode
        {
            get => _debugMode;
            set => _debugMode = value;
        }

        public virtual void Remove()
        {
            if (DebugMode)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
           
        }
        
    }

    
   
}