using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public class TextFadeInOut : MonoBehaviour
    {
        private TextMeshPro text;
        public float delay;
        public float fadeInOutDuration;
        public float stayDuration;
        private AnimationCurve curveIn;
        private AnimationCurve curveOut;
        private Material material;
        // private float alpha = 0f;
        public void Init(AnimationCurve curveIn, AnimationCurve curveOut)
        {
            this.curveIn = curveIn;
            this.curveOut = curveOut;
            material = GetComponent<MeshRenderer>().sharedMaterial;
            material.SetFloat("_Alpha",0f);
        }

        public void OnProcess(float time)
        {
           
            var t =0f;
            if (time >= delay)
            {
                t = (time-delay)/(1f-delay);
                
                if (t >= 0 &&t <= fadeInOutDuration)
                {
//                Debug.Log(curve.Evaluate(t / fadeInOutDuration));
                    material.SetFloat("_Alpha",Mathf.Lerp(0f, 1f, curveIn.Evaluate(t / fadeInOutDuration)));
                }
               
                if (t >= fadeInOutDuration + stayDuration) 
                {
                    var fadeOutTimeline = Mathf.Clamp(t - (fadeInOutDuration + stayDuration), 0f, fadeInOutDuration) /
                                          fadeInOutDuration;
                    material.SetFloat("_Alpha",Mathf.Lerp(1f, 0f, curveOut.Evaluate(fadeOutTimeline)));
                }
            
            }
          
            

        }
    }
    

    
//    public class TextFadeOut : MonoBehaviour
//    {
//        private TextMeshPro text;
//        public float delay;
//        private AnimationCurve curve;
//        private Material material;
//        private float alpha;
//        public void Init(AnimationCurve curve)
//        {
//            this.curve = curve;
////            text = gameObject.GetComponent<TextMeshPro>();
//            material = GetComponent<MeshRenderer>().sharedMaterial;
////            material.SetFloat("_Alpha",1f);
////            alpha = 1f;
//        }
//
//        public void OnProcess(float time)
//        {
//            var t =0f;
//            if (time >= delay) t = (time-delay)/(1f-delay);
//
//            material.SetFloat("_Alpha",curve.Evaluate(t));
//
//        }
//    }
    public class SlideMove : MonoBehaviour
    {
        private MoveDirection moveDirection;
        public float movingDistance = 100f;
        public float delay = 0f;
        private Vector3 startPos;
        private Vector3 endPos;
        private AnimationCurve curve;
        private AnimationCurve curve_escape;

        public float fadeInOutDuration;
        public float stayDuration;
        
        public void Init(MoveDirection direction, AnimationCurve curve, AnimationCurve escapeCurve)
        {
            this.curve = curve;
            this.curve_escape = escapeCurve;
            this.moveDirection = direction;
            endPos = transform.localPosition;
//            Debug.Log(this.moveDirection);
            movingDistance = gameObject.transform.localScale.x * 0.3f;
            switch (this.moveDirection)
            {
                case MoveDirection.Right:
                    transform.localPosition += new Vector3(-movingDistance,0f,0f);
                    break;
                case MoveDirection.Left:
                    transform.localPosition += new Vector3(movingDistance,0f,0f);
                    break;
                case MoveDirection.Down:
                    transform.localPosition += new Vector3(0f, movingDistance, 0f);
                    break;
                case MoveDirection.Up:
                    transform.localPosition += new Vector3(0f, -movingDistance, 0f);
                    break;
                default:
                    break;
            }

            startPos = transform.localPosition;
            
//            fadeInOutDuration = 0.3f;
//            stayDuration = (1f - fadeInOutDuration * 2f) * Random.Range(1f, 0.6f);
        }
        public void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);

//            var p = transform.localPosition;
            if (t >= 0 &&t <= fadeInOutDuration)
            {
//                Debug.Log(curve.Evaluate(t / fadeInOutDuration));
                transform.localPosition = Vector3.Lerp(startPos, endPos, this.curve.Evaluate(t / fadeInOutDuration));
            }
            else
            {
                if (t >= fadeInOutDuration + stayDuration) 
                {
                    var fadeOutTimeline = Mathf.Clamp(t - (fadeInOutDuration + stayDuration), 0f, fadeInOutDuration) /
                                          fadeInOutDuration;
                    var diretion = (endPos - startPos).normalized;
                    var dist = Vector3.Distance(endPos, startPos);
                    var escapePos = endPos + diretion * dist;
                    transform.localPosition = Vector3.Lerp(endPos, escapePos, this.curve_escape.Evaluate(fadeOutTimeline));
                }    
            }

            
            
        }
        
    }
    public class BasicSlideWord : MotionTextElement
    {

        private List<SlideMove> slideMoves = new List<SlideMove>(); 
        private List<TextFadeInOut> textFadeIns = new List<TextFadeInOut>();
//        private List<TextFadeOut> textFadeOut = new List<TextFadeOut>();
//        private List<SlideMove> slideOutMoves = new List<SlideMove>();
        List<GameObject> textTextures = new List<GameObject>();
        public float delay = 0f;
        public float mainDuration = 1f;
        private List<Material> materials = new List<Material>();
        private float glitchDelay = 0f;
        private float glitchDurtaion = 0f;
        public float isBackGround;
        public override void Init(string word, double duration)
        {


            isBackGround = Random.Range(0f,1.0f) < 0.2f ? 1 : 0;
            var shadername = "";
            int i = Random.Range(0, 3);

            switch (i)
            {
                case 0:
                    shadername = "Unlit/Glitch01";
                    break;
//                case 1:
//                    shadername = "Unlit/Glitch02";
//                    break;
                case 1:
                    shadername = "Unlit/Glitch03";
                    break;
                case 2:
                    shadername = "Unlit/Glitch04";
                    break;
               default:
                   break;
            }
            
            foreach (var character in word)
            {
                var tmpro = CreateTextMeshElement(character.ToString(), Font, FontSize);
                var t = Graphics.TMProToTex2D(tmpro.Children.First(), (int)FontSize, textAnimationManager.CaptureCamera);

                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.transform.SetParent(transform);
                go.transform.localScale = new Vector3(t.width,t.width,t.width);
                
                var material = new Material(Shader.Find(shadername));
                material.SetTexture("_MainTex",t);
                if (shadername == "Unlit/Glitch04")
                {
                    material.SetTexture("_NoiseTex", Resources.Load("Materials/ColorNoise") as Texture2D);
                }

                if (shadername == "Unlit/Glitch03")
                {
                    material.SetFloat("_NoiseScale", 80f);
                }
                
                
                material.SetFloat("_IsGlitch", 0);
                materials.Add(material);
                go.GetComponent<MeshRenderer>().material = material;
//                t.transform.SetParent(transform);
                textTextures.Add(go);
                tmpro.alpha = 0f;
            }
            
            var delay = 0f;
            var delaystep = 0f;


            var totalWidth = 0f;
//            var totalHeight = 0f;
            foreach (var text in textTextures)
            {
                totalWidth += text.transform.localScale.x;
            }

            transform.position = referenceTransform.position;
            var isVertical= referenceTransform.localScale.x < referenceTransform.localScale.y;
            var baseScale = 0f;
            
            if (!isVertical)
            {
                baseScale = referenceTransform.localScale.x / totalWidth;
            }
            else
            {
                baseScale = referenceTransform.localScale.y / totalWidth;

            }
            
            var textPos = isVertical ? new Vector3(0f, referenceTransform.localScale.y/2f, 0f) : new Vector3(-referenceTransform.localScale.x/2f, 0f, 0f) ;
            foreach (var text in textTextures)
            {
                text.transform.localScale *= baseScale;
                if (isVertical)
                {
                    
                    textPos += new Vector3(0f, -text.transform.localScale.x/2f, 0f);
                    text.transform.localPosition = textPos;
                    textPos += new Vector3(0f, -text.transform.localScale.x/2f, 0f);
                    text.transform.eulerAngles = new Vector3(0f,0,-90f);
                }
                else
                {
                    textPos += new Vector3(text.transform.localScale.x/2f, 0f, 0f);
                    text.transform.localPosition = textPos;
                    textPos += new Vector3(text.transform.localScale.x/2f, 0f, 0f);
                }
                
            }
            
            
            
            if (textTextures.Count > 1) delaystep = 0.3f / (textTextures.Count - 1);

            
            foreach (var text in textTextures)
            {
                var m = text.gameObject.AddComponent<SlideMove>();
                var d = MoveDirection.Up;
                if (isVertical)
                {
                    d = Random.Range(0, 2) == 1 ? MoveDirection.Left : MoveDirection.Right;
                }
                else
                {
                    d = Random.Range(0, 2) == 1 ? MoveDirection.Up : MoveDirection.Down;
                }
                
                m.Init(d,animationCurveAsset.BasicIn,animationCurveAsset.CutSlashOut);
                m.delay = delay;
                m.fadeInOutDuration = 0.3f;
                m.stayDuration = (mainDuration - m.fadeInOutDuration * 2) * Random.Range(1f, 0.6f);
                var a = text.gameObject.AddComponent<TextFadeInOut>();
                a.delay = delay;
                a.fadeInOutDuration = m.fadeInOutDuration;
                a.stayDuration = m.stayDuration;
                a.Init(animationCurveAsset.SlideWordsIn,animationCurveAsset.SlideWordsOut);
               
                    
                textFadeIns.Add(a);
                slideMoves.Add(m);
                
                delay += delaystep;
            }

            transform.localPosition = referenceTransform.localPosition;


            var noiseScale = 0.5f;
            glitchDurtaion = 0.3f;
            glitchDelay = 0.6f * Mathf.PerlinNoise(transform.localPosition.x*noiseScale, 0.1f);
            


        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);


            var bgDuration = Mathf.Clamp((float)normalizedTime, 0f, 0.1f)/0.1f;
            if (isBackGround > 0)
            {
                foreach (var m in materials)
                {
                    m.SetFloat("_BGSlider", animationCurveAsset.BasicIn.Evaluate(bgDuration));
                }    
            }
            
            var t = 0f;
            if ((float) normalizedTime > delay)
            {
                t = ((float) normalizedTime - delay) / (1f - delay);
                
                foreach (var text in slideMoves)
                {
                    text.OnProcess(t);
               
                }
            
                foreach (var text in textFadeIns)
                {
                    text.OnProcess(t);
               
                }
            }

            var isGlitch = 0f;
            
            if ((float) normalizedTime >= glitchDelay && (float) normalizedTime <= glitchDelay + glitchDurtaion)
            {
                isGlitch = 1f;
            }

            foreach (var material in materials)
            {
                material.SetFloat("_IsGlitch", isGlitch);
     
            }
           


        }

    }


    public class LaidWords : MotionTextElement
    {
        List<MotionTextElement> motions = new List<MotionTextElement>();
        public override void Init(string word, double duration)
        {
            var splitWords = word.Split('/').ToList();
            foreach (Transform child in referenceTransform)
            {

                var isCreate = false;
                if (ID != 0)
                {
                    var th = Mathf.PerlinNoise(child.transform.localPosition.x * 0.01f, ID * 0.01f);

                    if (th > 0.5)
                    {
                        isCreate = true;
                    }
                }
                else
                {
                    isCreate = true;
                }


                if (isCreate)
                {
                    var randomDelay = Random.Range(0f, 0.3f);
                    var pickWord = splitWords[Random.Range(0, splitWords.Count)];
                    
                    var go = new GameObject(word);
                    go.gameObject.name = "text: " + word;
                    var parent = transform;
                    go.transform.SetParent(parent);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localEulerAngles = Vector3.zero;
   
                    MotionTextElement motion;
                    motion = go.AddComponent<BasicSlideWord>();
                    motion.animationCurveAsset = animationCurveAsset;
                    motion.Graphics = Graphics;
                    
                    motion.textAnimationManager = textAnimationManager;
                    motion.DebugMode = DebugMode;
                    motion.Font = Font;
                    motion.FontSize = FontSize;
                    motion.transform.SetParent(transform);
                    motion.TextSegmentationOptions = TextSegmentationOptions;
                    motion.referenceTransform = child;
                    motion.GetComponent<BasicSlideWord>().delay = randomDelay;
                    motion.Init(pickWord, Duration);
                    
                    motions.Add(motion);
                }
              
            }

        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            foreach (var motion in motions)
            {
                motion.ProcessFrame(normalizedTime,seconds);
            }
        }

    }
    
    
    public class LaidWords_FromTheLeft : MotionTextElement
    {
        List<BasicSlideWord> motions = new List<BasicSlideWord>();
        
        List<float> delays = new List<float>();
        public override void Init(string word, double duration)
        {
            var maxDistance =Vector3.Distance( new Vector3(Screen.width/2f, 0f, 0f), new Vector3(-Screen.width/2f,0f,0f)) ;
            var splitWords = word.Split('/').ToList();
            var totalDelay = 0.4f;
            foreach (Transform child in referenceTransform)
            {

                var isCreate = false;
                if (ID != 0)
                {
                    var th = Mathf.PerlinNoise(child.transform.localPosition.x * 0.01f, ID * 0.01f);

                    if (th > 0.5)
                    {
                        isCreate = true;
                    }
                }
                else
                {
                    isCreate = true;
                }


                if (isCreate)
                {
                    
                    var d = Vector3.Distance(new Vector3(Screen.width/2f, child.transform.position.y,0) , child.transform.position);
                    var th = d / maxDistance;
                    
                    var pickWord = splitWords[Random.Range(0, splitWords.Count)];
                    
                    
                    
//                    var motion = TextAnimationManager.CreateMotionTextElement(pickWord, AnimationType.BasicSlideWord);
                    var go = new GameObject(word);
                    go.gameObject.name = "text: " + word;
                    var parent = transform;
                    go.transform.SetParent(parent);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localEulerAngles = Vector3.zero;
   
                    MotionTextElement motion;
                    motion = go.AddComponent<BasicSlideWord>();
                    motion.animationCurveAsset = animationCurveAsset;
                    motion.Graphics = Graphics;
                    
                    motion.textAnimationManager = textAnimationManager;
                    motion.DebugMode = DebugMode;
                    motion.Font = Font;
                    motion.FontSize = FontSize;
                    motion.transform.SetParent(transform);
                    motion.TextSegmentationOptions = TextSegmentationOptions;
                    motion.referenceTransform = child;
                    motion.Init(pickWord, Duration);
//                    motion.GetComponent<BasicSlideWord>().delay = Mathf.Lerp(0f,0, th);
//                    motion.GetComponent<BasicSlideWord>().mainDuration = Mathf.Clamp(1f - totalDelay, 0f, 0.2f);
                    delays.Add( Mathf.Lerp(totalDelay,0, th));
                    
                    motions.Add( motion.GetComponent<BasicSlideWord>());
                }
              
            }

        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            var time = (float) normalizedTime;
            var count = 0;
            var duration = 0.4f;
            foreach (var motion in motions)
            {
                var th = 0f;
                if (time >= delays[count])
                {
                    th = Mathf.Clamp((time - delays[count]), 0f, duration) / duration;
                   
                }

                if (th > 0)
                {
                    motion.ProcessFrame(th,seconds);     
                }
               
                count++;
            }
        }

    }
    
  
}