using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


namespace TextAnimationTimeline
{
   
    public static class TextGenerater
    {

        
  
        public static List<TextMeshPro> CreateTextMeshList(string word, TMP_FontAsset font, float fontSize)
        {

            List<TextMeshPro> texts = new List<TextMeshPro>();
            Vector3 position = new Vector3(0,0,0);
            var totalWidth = 0f;
            var height = 0f;
            foreach (var ch in word)
            {
                if(texts.Count > 0 )position += new Vector3(texts.Last().preferredWidth / 1.8f, 0f, 0f);
                var textMesh = CreateTextMesh(ch.ToString(),font, fontSize);
                textMesh.name = "text: " + ch;
                textMesh.fontSize = fontSize;
                textMesh.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                position += new Vector3(textMesh.preferredWidth / 1.8f, 0f, 0f);
                textMesh.transform.localPosition = position;
                var renderer = textMesh.GetComponent<MeshRenderer>();
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.allowOcclusionWhenDynamic = false;
                totalWidth += textMesh.preferredWidth;
                if (height < textMesh.preferredHeight)
                {
                    height = textMesh.preferredHeight;
                }
                texts.Add(textMesh);
            }

            var diff = new Vector3(totalWidth/2f, -height, 0f);
            
            foreach (var t in texts)
            {
                t.transform.localPosition -= diff;
            }

            return texts;
        }
        

        public static TextMeshPro CreateTextMesh(string character, TMP_FontAsset font, float fontSize)
        {
            var tmPro = new GameObject().AddComponent<TextMeshPro>();
            tmPro.font = font;
            tmPro.UpdateFontAsset();
            tmPro.text = character;
            tmPro.alignment = TextAlignmentOptions.CenterGeoAligned;
            tmPro.fontSize = fontSize;
            tmPro.enableWordWrapping = false;
            tmPro.alignment = TextAlignmentOptions.Center;
            tmPro.name = character;
            tmPro.transform.localEulerAngles = Vector3.zero;
            tmPro.transform.localPosition = Vector3.zero;
            tmPro.transform.localScale = Vector3.one;

            return tmPro;
        }
    }
}