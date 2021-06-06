using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextAnimationTimeline.Motions
{

    internal class CharacterAlphaInOut : MonoBehaviour
    {
        private float delay;
        private TextMeshPro text;
        private GameObject textureObj;
        private Material material;
        private AnimationCurve curve;
        public void Init(TextMeshPro text, AnimationCurve curve, float delay)
        {
            this.curve = curve;
            this.delay = delay;

            this.text = text;
        }
        
        public void Init(GameObject text, AnimationCurve curve, float delay)
        {
            this.curve = curve;
            this.delay = delay;

//            text = GetComponent<TextMeshPro>();
            this.textureObj = text;
            material = textureObj.GetComponent<MeshRenderer>().sharedMaterial;
            material.SetFloat("_Alpha", 0f);
        }
        

        public void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);
            if (text != null)
            {
                text.alpha = curve.Evaluate(t);
            }
            else
            {
                material.SetFloat("_Alpha",curve.Evaluate(t));
            }

        }
        
    }

    internal class CharacterPoursMotion : MonoBehaviour
    {
        private Vector3 endPosition;
        private Vector3 endRotate;
        
        private Vector3 startPosition;
        private Vector3 startRotate;

        public float delay;
        public void Init(Vector3 startPosition, Vector3 startRotate, Vector3 endPosition, Vector3 endRotate)
        {
            this.startPosition = startPosition;
            this.startRotate = startRotate;
            this.endPosition = endPosition;
            this.endRotate = endRotate;
        }


        public void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);
            transform.eulerAngles = Vector3.Lerp(startRotate, endRotate, t);
            transform.localPosition = Vector3.Lerp(startPosition,endPosition,t);
        }
    }

    internal class CharacterTextureInterpolation : MonoBehaviour
    {
        private float delay;
        private GameObject textureObj;
        private Material material;
        private AnimationCurve curve;

        public void Init(GameObject text, AnimationCurve curve, float delay)
        {
            this.curve = curve;
            this.delay = delay;
            
            this.textureObj = text;
            material = textureObj.GetComponent<MeshRenderer>().sharedMaterial;
        }


        public void OnProcess(float time)
        {
            var t = 0f;
            if (time >= delay) t = (time - delay) / (1f - delay);
            {
                material.SetFloat("_Interpolation", curve.Evaluate(t));
            }
        }
    }

    public class Kaf_CharacterPours : MotionTextElement
    {
        private List<CharacterPoursMotion> motions = new List<CharacterPoursMotion>();
        private List<CharacterAlphaInOut> alphaInOuts = new List<CharacterAlphaInOut>();
        private List<CharacterTextureInterpolation> texInterpolation = new List<CharacterTextureInterpolation>();
        private List<GameObject> textureObjs = new List<GameObject>();
        public override void Init(string word, double duration)
        {
            //ここで文字列からTMP群を子に持つTextMeshElementが生成される
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);

            foreach(var t in TextMeshElement.Children)
            {
                //一文字のテクスチャが貼られたオブジェクトが生成され参照をListに持つ
                var obj = Graphics.WordToTextureGameObject(t, 1500, textAnimationManager.CaptureCamera);
                obj.name = "QuadText:" + t.text;
                textureObjs.Add(obj);
            }

            GameObject textTexturesParent = new GameObject("textTextures:" + word);
            textTexturesParent.transform.SetParent(TextMeshElement.transform.parent);
            textTexturesParent.transform.localPosition = TextMeshElement.transform.localPosition;
            textTexturesParent.transform.localEulerAngles = TextMeshElement.transform.localEulerAngles;
            textTexturesParent.transform.localScale = TextMeshElement.transform.localScale;

            for (int i = 0; i < textureObjs.Count; i++) {
                //①TMPとほぼ同じ位置、向き、大きさになるようにQuadオブジェクトを調整
                var textmp = TextMeshElement.Children[i];
                var quadObj = textureObjs[i];
                var scaling = textmp.preferredWidth > textmp.preferredHeight ? textmp.preferredWidth : textmp.preferredHeight;
                quadObj.transform.SetParent(textTexturesParent.transform);
                quadObj.transform.localPosition = textmp.transform.localPosition;
                //Debug.Log("textmp.transform.localPosition" + textmp.transform.localPosition);
                //Debug.Log("textmp.rectTransform.localPosition" + textmp.rectTransform.localPosition);
                quadObj.transform.localEulerAngles = textmp.transform.localEulerAngles;
                quadObj.transform.localScale = new Vector3(scaling, scaling, scaling);
                //TMPの文字はレンダリングしない
                textmp.GetComponent<Renderer>().enabled = false;

                //②シェーダーを変える+いじれるようにする
                Renderer renderer = quadObj.GetComponent<Renderer>();
                Material currentMat = renderer.sharedMaterial;
                Material newMat = new Material(Shader.Find("TextureInterpolatrion/TextReflectionTexture"));
                newMat.SetTexture("_MaskTex", currentMat.GetTexture("_MainTex"));
                newMat.SetFloat("_Alpha", currentMat.GetFloat("_Alpha"));
                RenderTexture reflectiontex = Resources.Load("Textures/ReflectionTex") as RenderTexture;
                if (reflectiontex == null) Debug.Log("nullやで");
                newMat.SetTexture("_ReflectionTex", reflectiontex);
                renderer.sharedMaterial = newMat;
            }
            
            Init();
            transform.localPosition = OffsetLocalPosition;
        }

        public void Init()
        {
            var resolution = textAnimationManager.Resolution;
            var textLineDirection = new Vector3(resolution.x / 2, -resolution.y / 2,0f);
            textLineDirection = Vector3.Normalize(textLineDirection);

            var startpos = new Vector3(-resolution.x/2, resolution.y/2, 0f);
            var totalWidth = 0f;
            var totalHeight = 0f;
            
            // 文字の横幅を確認。
            foreach (var character in TextMeshElement.Children)
            {
                totalWidth += character.preferredWidth;
                totalHeight += character.preferredHeight;
            }
            
            var start = new Vector3(-resolution.x / 2f, resolution.y/3);
            var end = new Vector3(resolution.x/2, -resolution.y/3f);

            var maxWidth =  Vector2.Distance(start,end)*0.5f;

            var scaleDiff = maxWidth / totalHeight;

            var characterPos = startpos;
            var delay = 0f;
            var delayStep = 0.4f / (TextMeshElement.Children.Count-1);
            var wigglePos = Vector3.zero;


            //TMPの調整をQuadの調整に置き換える必要がある。
            //foreach (var character in TextMeshElement.Children)
            for(int i = 0; i < textureObjs.Count; i++)
            {
                var character = TextMeshElement.Children[i];
                var quadObj = textureObjs[i];
                //QuadをTMPのオブジェクトの子に入れてるのでTransformの変更はこのままOK
                //修正事項はアルファの反映
                var endRotate = new Vector3(Random.Range(-10f, 10f), Random.Range(-20f, 20f), Random.Range(-10f, 10f));
                character.color = Color.white;
                character.alpha = 1f;
                character.transform.localScale *= scaleDiff * Random.Range(1f, 0.4f);
                quadObj.transform.localScale   *= scaleDiff * Random.Range(1f, 0.4f);
                character.transform.localPosition = startpos;
                quadObj.transform.localPosition   = startpos;
                character.transform.eulerAngles = endRotate + new Vector3(Random.Range(-10f, 10f), 20f, 0f);
                quadObj.transform.eulerAngles   = endRotate + new Vector3(Random.Range(-10f, 10f), 20f, 0f);
                characterPos +=(textLineDirection * character.preferredWidth * scaleDiff);//ここの計算をどうするか
                character.alpha = 0f;

                //var mo = character.gameObject.AddComponent<CharacterPoursMotion>();
                var mo = quadObj.AddComponent<CharacterPoursMotion>();
                mo.Init(quadObj.transform.localPosition+wigglePos, quadObj.transform.eulerAngles, characterPos+wigglePos,endRotate);
                mo.delay = delay;
                motions.Add(mo);

                //var a = character.gameObject.AddComponent<CharacterAlphaInOut>();
                var a = quadObj.AddComponent<CharacterAlphaInOut>();
                //a.Init(character,AnimationCurveAsset.Kaf_CharacterPoursAlphaInOut,delay);
                a.Init(textureObjs[i],animationCurveAsset.Kaf_CharacterPoursAlphaInOut,delay);
                alphaInOuts.Add(a);

                var interpolation = quadObj.AddComponent<CharacterTextureInterpolation>();
                interpolation.Init(textureObjs[i], animationCurveAsset.Kaf_TextureInterpolation, delay);
                texInterpolation.Add(interpolation);

                delay += delayStep;
                wigglePos = new Vector3(0f, Random.Range(-60f,60f), 0f);

            }
            TextMeshElement.gameObject.SetActive(false);
        }
        public override void ProcessFrame(double normalizedTime, double seconds)
        {

            var t = animationCurveAsset.Kaf_CharacterPours.Evaluate((float) normalizedTime);
            foreach (var mo in motions)
            {
                mo.OnProcess(t);
            }

            foreach (var a in alphaInOuts)
            {
                a.OnProcess(t);
            }

            //反射表現のテクスチャ補間の値のアニメーション
            foreach (var inter in texInterpolation) {
                inter.OnProcess(t);
            }
        }

    }
}