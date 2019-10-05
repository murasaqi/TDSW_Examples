using UnityEngine;
using UnityEditor;
using TMPro;
namespace TextAnimationTimeline
{
   
    [CreateAssetMenu(menuName = "TextAnimationTimeline/Create AnimationCurveAssets Instance")]
    public class AnimationCurveAsset:ScriptableObject
    {
        public AnimationCurve BasicIn;
        public AnimationCurve BasicOut;
        public AnimationCurve BasicInOut;
        public AnimationCurve SteepIn;
        public AnimationCurve SteepOut;
        public AnimationCurve SteepInOut;
        public AnimationCurve Flow;
        public AnimationCurve FlowInOut;
        public AnimationCurve SlowMo;
        public AnimationCurve TimeRemapScattering;
        public AnimationCurve Kaf_CharacterPours;
        public AnimationCurve Kaf_CharacterPoursAlphaInOut;
        public AnimationCurve Kaf_TextureInterpolation;
        public AnimationCurve Kaf_SlashOut;
        public AnimationCurve Bounce;
        public AnimationCurve CutSlashIn;
        public AnimationCurve CutSlashOut;
        public AnimationCurve CutOutLineIn;
        public AnimationCurve CutOutLineOut;
        public AnimationCurve SlideWordsIn;
        public AnimationCurve SlideWordsOut;
        public AnimationCurve BasicInOut2;
        public AnimationCurve SlowMo2;
        public AnimationCurve HorizontalLineOut;
        public AnimationCurve BigWordMove;
        public AnimationCurve BigWordInOut;

    }
}