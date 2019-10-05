using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace TextAnimationTimeline.Motions
{
  
    public class TextAnimationGraphics : MonoBehaviour
    {
        Dictionary<string,Texture2D> texturePool = new Dictionary<string, Texture2D>();
//        private 

        private void Awake()
        {
            texturePool.Clear();
        }
        
         public Texture2D TMProToTex2D(TextMeshPro text, int fontSize,Camera camera)
        {
            if (texturePool.ContainsKey(text.text+":"+fontSize))
            {
                var poolObject = texturePool[text.text + ":" + fontSize];
                if (poolObject != null)
                {
                    return poolObject;   
                }
                
            }
            
            
            text.fontSize = fontSize;
            text.transform.localPosition = new Vector3(-9999,-9999,-9999);
            camera.transform.localPosition = new Vector3(-9999,-9999,-9999-10);

            var cellSize = text.preferredWidth > text.preferredHeight ? text.preferredWidth : text.preferredHeight;
            var height = cellSize;
            var width =cellSize;
            
           
            Debug.Log("width: " + width + "," + "height: " + height);
            text.transform.localPosition = new Vector3(camera.transform.localPosition.x,camera.transform.localPosition.y,camera.transform.localPosition.z+1);
            var Screen = new RenderTexture((int)width,(int)height,8,RenderTextureFormat.ARGB32);
            
            camera.targetTexture = Screen;

            
            float aspect = (float)Screen.height / (float)Screen.width;
            float bgAcpect = height / width;

            // カメラコンポーネントを取得します
            camera.orthographicSize = (height / 2f );
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0,0,0,0);
            

            if (bgAcpect > aspect) {
                // 倍率
                float bgScale = height / Screen.height;
                // viewport rectの幅
                float camWidth = width / (Screen.width * bgScale);
                // viewportRectを設定
                camera.rect = new Rect ((1f - camWidth) / 2f, 0f, camWidth, 1f);
            } else {
                // 倍率
                float bgScale = width / Screen.width;
                // viewport rectの幅
                float camHeight = height / (Screen.height * bgScale);
                // viewportRectを設定
                camera.rect = new Rect (0f, (1f - camHeight) / 2f, 1f, camHeight);
            }

            camera.Render();


            var image = RTImage(camera);
            text.alpha = 0f;

            
            if (!texturePool.ContainsKey(text.text+":"+fontSize))
            {
                texturePool.Add(text.text+":"+fontSize,image);   
            }
            camera.gameObject.SetActive(false);
//            DestroyImmediate(camera);
            return image;
        }

        public GameObject WordToTextureGameObject(TextMeshPro text, int fontSize,Camera camera)
        {
            if (texturePool.ContainsKey(text.text+":"+fontSize))
            {
                var poolObject = texturePool[text.text + ":" + fontSize];
                if (poolObject != null)
                {
                    Debug.Log("has texture");
                    var clone = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    var clone_material = new Material(Shader.Find("Unlit/TextAnimationTransparent"));
                    clone_material.SetTexture("_MainTex",poolObject);
                    
                    clone.GetComponent<MeshRenderer>().material = clone_material;
                    clone.name = text.text;
                    clone.transform.localScale = new Vector3(poolObject.width,poolObject.height,1);
//                    clone.transform.localPosition = Vector3.zero;
                    return clone;   
                }
                
            }
            
//            Debug.Log("create text texture");
            
            text.fontSize = fontSize;
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = text.text;
            text.transform.position = new Vector3(-9999,-9999,-9999);
            go.transform.position = new Vector3(0,0,0);
            camera.transform.position = new Vector3(-9999,-9999,-9999-10);
            
            var material = new Material(Shader.Find("Unlit/TextAnimationTransparent"));
            go.GetComponent<MeshRenderer>().material = material;
            
            
//            textMesh.outlineWidth;pubpupripp pripppppraprivate

            var cellSize = text.preferredWidth > text.preferredHeight ? text.preferredWidth : text.preferredHeight;
            var height = cellSize;
            var width =cellSize;
            
            
           
            Debug.Log("width: " + width + "," + "height: " + height);
//            Debug.Log(textMesh.preferredHeight);
            text.transform.position = new Vector3(camera.transform.position.x,camera.transform.position.y,camera.transform.position.z+1);
            var Screen = new RenderTexture((int)width,(int)height,8,RenderTextureFormat.ARGB32);
            
            camera.targetTexture = Screen;

            
            float aspect = (float)Screen.height / (float)Screen.width;
            float bgAcpect = height / width;

            // カメラコンポーネントを取得します
            camera.orthographicSize = (height / 2f );
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0,0,0,0);
            
//            captureCamera.clearFlags = captureCameraClearFlags.Nothing;


            if (bgAcpect > aspect) {
                // 倍率
                float bgScale = height / Screen.height;
                // viewport rectの幅
                float camWidth = width / (Screen.width * bgScale);
                // viewportRectを設定
                camera.rect = new Rect ((1f - camWidth) / 2f, 0f, camWidth, 1f);
            } else {
                // 倍率
                float bgScale = width / Screen.width;
                // viewport rectの幅
                float camHeight = height / (Screen.height * bgScale);
                // viewportRectを設定
                camera.rect = new Rect (0f, (1f - camHeight) / 2f, 1f, camHeight);
            }

            camera.Render();


            var image = RTImage(camera);
            material.SetTexture("_MainTex", image);
            
//            text.transform.localPosition = new Vector3(9999,9999,9999);
            text.alpha = 0f;

            
            go.transform.localScale = new Vector3(width,height,width);
            go.transform.position = Vector3.zero;
            
            
            if (!texturePool.ContainsKey(text.text+":"+fontSize))
            {
                texturePool.Add(text.text+":"+fontSize,image);   
            }
            camera.gameObject.SetActive(false);
//            DestroyImmediate(camera);
            go.layer = 14;
            return go;
        }
        
        public static Texture2D RTImage(Camera cam) {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;
            cam.Render();
            Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = currentRT;
            return image;
        }
        
        public static float GetCameraDistanceWithWidthSize(Camera camera, float width) {
            if (camera == null || width <= 0.0f) {
                return 0.0f;
            }

            float frustumHeight = width / camera.aspect;

            return frustumHeight * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }
        
    }
}