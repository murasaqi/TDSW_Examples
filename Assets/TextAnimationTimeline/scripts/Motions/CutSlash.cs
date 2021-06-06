using System.Collections.Generic;
using System.Linq;
using TextAnimationTimeline.scripts.Motions;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public class CutSlash : MotionTextElement
    {
        private Vector3 lineStart;
        private Vector3 lineEnd;
        private TextMeshCutter meshCutter;
        private List<CharacterPoursMotion> motions =new List<CharacterPoursMotion>();
        private List<GameObject> textureObjs = new List<GameObject>();
        private List<MotionSplash> motionSplashes = new List<MotionSplash>();
        private List<CharacterAlphaInOut> alphaInOuts = new List<CharacterAlphaInOut>();
        
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, 2000);
            transform.localPosition = OffsetLocalPosition;
            
           

            foreach (var c in TextMeshElement.Children)
            {
                var t = Graphics.WordToTextureGameObject(c,1600,textAnimationManager.CaptureCamera);
                t.transform.SetParent(transform);
                t.layer = 2;
                textureObjs.Add(t);
            }
            Init();

           

            TextMeshElement.gameObject.layer = 2;
            foreach (var t in TextMeshElement.Children)
            {
                t.gameObject.layer = 2;
            }
            
            
            meshCutter = new GameObject("mesh cutter").AddComponent<TextMeshCutter>();
            meshCutter.transform.SetParent(transform);
            meshCutter.capMaterial = new Material(Shader.Find("Unlit/Transparent"));
            meshCutter.enabled = false;

            TextMeshElement.alpha = 0f;
        }

        public void Init()
        {
            
            var resolution = textAnimationManager.Resolution;
            var textLineDirection = new Vector3(resolution.x / 2, -resolution.y / 2,0f);
            textLineDirection = Vector3.Normalize(textLineDirection);

            var startpos = new Vector3(-resolution.x/2, resolution.y/2, 0f);
            lineStart = startpos;
            startpos += new Vector3(0,0,50);
            
            var totalWidth = 0f;
            var totalHeight = 0f;
            
            // 文字の横幅を確認。
            foreach (var character in textureObjs)
            {
                totalWidth += character.transform.localScale.x;
                totalHeight += character.transform.localScale.y;
            }
            
            var start = new Vector3(-resolution.x / 2f, resolution.y/2);
            var end = new Vector3(resolution.x/2, -resolution.y/2f );
            var maxWidth =  Vector2.Distance(start,end)* 0.8f;

            var scaleDiff = maxWidth / totalHeight;
            Debug.Log(scaleDiff);
            var characterPos = startpos;
            var delay = 0f;
            var delayStep = 0.4f / (textureObjs.Count-1);
            var wigglePos = Vector3.zero;
            
            foreach (var character in textureObjs)
            {
                Debug.Log("init");
                var endRotate = new Vector3(Random.Range(-10f, 10f), Random.Range(-30f, 30f), Random.Range(-10f, 10f));
//                character.color = Color.white;
//                character.alpha = 1f;
                var scale = character.transform.localScale.x *  scaleDiff * Random.Range(1f, 0.4f);
                Debug.Log(scale);
                character.transform.localScale = new Vector3(scale,scale,scale); 
                character.transform.localPosition = startpos;
                character.transform.eulerAngles = endRotate + new Vector3(Random.Range(-10f, 10f), 80f, 0f);
                characterPos +=(textLineDirection * character.transform.localScale.y);
//                character.alpha = 0f;

                var mo = character.gameObject.AddComponent<CharacterPoursMotion>();
                
                mo.Init(character.transform.localPosition+wigglePos,character.transform.eulerAngles, characterPos+wigglePos,endRotate);
                mo.delay = delay;
                motions.Add(mo);

                var a = character.gameObject.AddComponent<CharacterAlphaInOut>();
                a.Init(character,animationCurveAsset.Kaf_CharacterPoursAlphaInOut,delay);
                alphaInOuts.Add(a);
                
                delay += delayStep;
                wigglePos = new Vector3(0f, Random.Range(-30f,30f), 0f);
                lineEnd = characterPos;
            }
        }
        public override void ProcessFrame(double normalizedTime, double seconds)
        {

            var cutDelay = 0.2f;
//            var t = Mathf.Clamp((float)normalizedTime, 0f, 1f-cutDuration) /(1f-cutDuration);
            var t = animationCurveAsset.Kaf_CharacterPours.Evaluate((float)normalizedTime);
            foreach (var mo in motions)
            {
                mo.OnProcess(t);
            }

            foreach (var a in alphaInOuts)
            {
                a.OnProcess(t);
            }
            
            

//            if (t >= 1)
//            {
//                foreach (var tx in textureObjs)
//                {
//                    if (tx.GetComponent<MotionSplash>() != null)
//                    {
//                        var m = tx.AddComponent<MotionSplash>();
//                        m.Init(AnimationCurveAsset.SteepIn, AnimationCurveAsset.BasicIn);
//                        m.endPosition = m.transform.localPosition + new Vector3(0f, Random.Range(-150, 100), 0);
//                        m.endAngle = m.transform.eulerAngles =new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 10));
//                    
//                        motionSplashes.Add(m);    
//                    }
//                    
//                }
//                

            if ((float) normalizedTime >= cutDelay)
            {


                foreach (var tx in textureObjs)
                {
                    tx.layer = 0;
                }
                var d = ((float) normalizedTime-cutDelay)/(1f-cutDelay);
                meshCutter.enabled = true;
//                
                var offset = new Vector3(0f,0f,0);
                var start =lineStart;
                var end = lineEnd;
                var pieces = meshCutter.CutLine(
                    Vector3.Lerp(start, end, animationCurveAsset.CutSlashOut.Evaluate(d)) + transform.localPosition, 
                    Vector3.Lerp(start, end, animationCurveAsset.CutSlashIn.Evaluate(d)) + transform.localPosition );
                meshCutter.enabled = false;
                //
//                
//                if (pieces.Count >=2)
//                {
//                    var center = (pieces[0].transform.position + pieces[1].transform.position)/2;
//
//                    var v01 = pieces.First().transform.position - center;
//                    var v02 = pieces.Last().transform.position - center;
//
//                    var p01 = pieces.First().transform.position + v01.normalized * Random.Range(50, 80) + new Vector3(0f, Random.Range(-150,-100));
//                    var p02 = pieces.Last().transform.position + v02.normalized * Random.Range(50, 80)+ new Vector3(0f, Random.Range(-150,-100));
//
//                    var a01 = pieces.First().transform.eulerAngles +
//                              new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 10));
//                    var a02 = pieces.Last().transform.eulerAngles +
//                              new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 10));

//                    if (pieces.First().GetComponent<MotionSplash>() != null)
//                    {
//                        var mo01 = pieces.First().AddComponent<MotionSplash>();
//                        mo01.Init(AnimationCurveAsset.SteepIn, AnimationCurveAsset.BasicIn);
//                        mo01.endPosition = p01;
//                        mo01.endAngle = a01;
//                        mo01.delay = netTime;
//                        motionSplashes.Add(mo01);     
//                    }
//
//                    if (pieces.Last().GetComponent<MotionSplash>() != null)
//                    {
//                        var mo02 = pieces.Last().AddComponent<MotionSplash>();
//                        mo02.Init(AnimationCurveAsset.SteepIn, AnimationCurveAsset.BasicIn);
//                        mo02.endPosition = p02;
//                        mo02.endAngle = a02;
//                        mo02.delay = netTime;
//                        motionSplashes.Add(mo02);    
//                    }
                    
//                }
//
//                foreach (var m in motionSplashes)
//                {
//                    if(m !=null)m.OnProcess(netTime);
//                }
//            }

            }

        }

    }
}