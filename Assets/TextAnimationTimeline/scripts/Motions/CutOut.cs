using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    public class TextPopOutMotion : MonoBehaviour
    {
        public float delay;
        
        public Material material;
        public Vector3 endPos;
        public Vector3 startPos;
        public Vector3 startAngle;
        public Vector3 endAngle;

        public AnimationCurve curve;
        public AnimationCurve alphaCurve;
        
        public void Init()
        {
            material = GetComponent<MeshRenderer>().material; 
            material.SetFloat("_Alpha",0f);
            
        }

        public void OnProcess(float time)
        {
            var t = 0f;
            if (time >= delay)
            {
                t = (time - delay) / (1f - delay);
            }

            
            transform.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(t));
            transform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, curve.Evaluate(t));
            material.SetFloat("_Alpha",alphaCurve.Evaluate(t));
        }
        
    }

    public class CutOutLineMotion : MonoBehaviour
    {
        private Vector3 start;
        private Vector3 end;
        private LineRenderer line;
        private AnimationCurve curveIn;
        private AnimationCurve curveOut;
        public float delay = 0;
//        public float duration = 0;
        public void Init(AnimationCurve curveIn, AnimationCurve curveOut)
        {

            this.curveIn = curveIn;
            this.curveOut = curveOut;
            line = GetComponent<LineRenderer>();

            start = line.GetPosition(0);
            end = line.GetPosition(1);
            
            line.SetPosition(1,start);
        }

        public void OnProcess(float time)
        {
            var t = 0f;
            if (time >= delay)
            {
                t = (time - delay) / (1f - delay);
            }
            var pos01 = Vector3.Lerp(start, end, curveIn.Evaluate(t));
            var pos02 = Vector3.Lerp(start, end, curveOut.Evaluate(t));

            if (t >= 1)
            {
                line.sharedMaterial.SetFloat("_Alpha", 0f);
            }
            line.SetPosition(0,pos01);
            line.SetPosition(1,pos02);
        }
    }
    public class CutOutMotion : MonoBehaviour
    {
        List<LineRenderer> debugLines = new List<LineRenderer>();
        List<CutOutLineMotion> motions = new List<CutOutLineMotion>();
        private AnimationCurveAsset animationCurveAsset;
        public float delay;
        public float duration;
        public void Init(AnimationCurveAsset asset, Camera camera)
        {
            animationCurveAsset = asset;
            for (int i = 0; i < 4; i++)
            {
                var debugLine = new GameObject().AddComponent<LineRenderer>();
                debugLine.startWidth = 2;
                debugLine.endWidth = 2;
                debugLine.positionCount = 2;
            
                debugLine.transform.SetParent(transform);    
                debugLine.material = new Material(Shader.Find("Unlit/TextAnimationTransparent"));
                debugLine.gameObject.layer = gameObject.layer;
                debugLines.Add(debugLine);
            }
            
           
              
            var vertices = new List<Vector3>();
            vertices.Add(new Vector3(-0.5f,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,0.5f ,0));
            vertices.Add(new Vector3(0.5f ,-0.5f ,0));
            vertices.Add(new Vector3(-0.5f ,-0.5f ,0));


            for (int i = 0; i < vertices.Count; i++)
            {
                
                Matrix4x4 m = Matrix4x4.TRS(
                    transform.position,
                    Quaternion.Euler(transform.eulerAngles),
                    transform.localScale
                );
                vertices[i] = m.MultiplyPoint(vertices[i]);
                Debug.Log(vertices[i]);
                
            }

            var count = 0;
            foreach (var line in debugLines)
            {
                var linepos0 =camera.WorldToScreenPoint(vertices[count]) -
                              new Vector3(Screen.width / 2, Screen.height / 2, 0);
                linepos0.z = 0;
                
                var linepos1 = camera.WorldToScreenPoint(vertices[(count+1) % vertices.Count]) -
                               new Vector3(Screen.width / 2, Screen.height / 2, 0);
                linepos1.z = 0;

                var direction = (linepos1 - linepos0).normalized;
                
                line.SetPosition(0,linepos0 - direction * Random.Range(100,50));
                line.SetPosition(1,linepos1 + direction * Random.Range(100,50));

                var motion = line.gameObject.AddComponent<CutOutLineMotion>();
                motion.Init(animationCurveAsset.CutOutLineIn,animationCurveAsset.CutOutLineOut);
                motion.delay = Random.Range(0f, 0.3f);
                count++;
                
                motions.Add(motion);
            }


        }
        
        public void OnProcess(float time)
        {
            var t = 0f;
            if (time >= delay)
            {
                t = Mathf.Clamp(time - delay, 0f ,duration) /duration;
            }
            
            foreach (var m in motions)
            {
                m.OnProcess(t);
            }
        }
    }
    public class CutOut : MotionTextElement
    {
        List <GameObject> textureObjs = new List<GameObject>();
        List<CutOutMotion> motions  = new List<CutOutMotion>();
        List<TextPopOutMotion> textMotions = new List<TextPopOutMotion>();
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;


            transform.position = OffsetLocalPosition;

            var totalWidth = 0f;
            foreach (var c in TextMeshElement.Children)
            {
                var t = Graphics.WordToTextureGameObject(c, 2000, textAnimationManager.CaptureCamera);
                if(layer >= 0)t.layer = layer;
                textureObjs.Add(t);

                totalWidth += t.transform.localScale.x;
                t.transform.SetParent(transform);
            }
            transform.localPosition = OffsetLocalPosition;
            var characterWidth = textureObjs.First().transform.localScale.x;

            var textWidth = Random.Range(1000, 1200);
            var scale = Mathf.Clamp((textWidth / totalWidth) * characterWidth, 0, 250);

            var quadPos = Vector3.zero;
            var totalDelay = 0.2f;
            var totalDuration = 1f;
            var delaystep = totalDelay / (textureObjs.Count - 1);
            var lineDuration = totalDuration / (textureObjs.Count);
            var count = 0;
            foreach (var t in textureObjs)
            {
                var randomScale = Random.Range(1f, 0.5f);
                t.transform.localScale = new Vector3(scale,scale,scale) * randomScale;
                quadPos += new Vector3(scale/2f, 0f,0f);
                t.transform.localPosition = quadPos - new Vector3(scale * textureObjs.Count / 2, 0f, 0f);
                quadPos += new Vector3(scale/2f, 0f, 0f);
                t.transform.localEulerAngles = new Vector3(Random.Range(-30f,30f),Random.Range(-20f,20f),0);
                var mo = t.AddComponent<CutOutMotion>();
                if(layer >= 0) mo.gameObject.layer = layer;
                mo.transform.SetParent(transform);
                mo.Init(animationCurveAsset, textAnimationManager.mainCamera);
                mo.duration = lineDuration;
                mo.delay = delaystep * count;
                motions.Add(mo);


                var textMo = t.AddComponent<TextPopOutMotion>();
                textMo.startPos = t.transform.localPosition;
                textMo.endPos = t.transform.localPosition + t.transform.forward * -Random.Range(100, 50);

                textMo.startAngle = t.transform.localEulerAngles;
                textMo.endAngle = textMo.startAngle +
                                  new Vector3(Random.Range(-20, 20), Random.Range(-30, 30), 0);
                textMo.curve = animationCurveAsset.SteepIn;
                textMo.alphaCurve = animationCurveAsset.BasicInOut;
                
                textMo.Init();
                textMo.delay = delaystep * count + lineDuration * 0.3f;
                textMotions.Add(textMo);
                count++;

            }


           
            TextMeshElement.alpha = 0f;
            DestroyImmediate(TextMeshElement.gameObject);

        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);

            foreach (var m in motions)
            {
                m.OnProcess((float)normalizedTime);
            }

            foreach (var m in textMotions)
            {
                m.OnProcess((float)normalizedTime);
            }
        }

    }
}