using UnityEngine;

namespace TextAnimationTimeline.Motions
{
    public class BasicGameTextBoxAnimation : MotionTextElement
    {
        public override void Init(string word, double duration)
        {
            TextMeshElement = CreateTextMeshElement(word, Font, FontSize);
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
            TextMeshElement.alpha = 0f;
        }

        public override void ProcessFrame(double normalizedTime,  double seconds)
        {
//            TextMeshElement.Alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);

            var childDuration = 1.0f / TextMeshElement.Children.Count;
            var fadeOutDuration = 0.1f;
            var fadeoutDelay = 1.0 - fadeOutDuration;
            var currentTime = normalizedTime / fadeoutDelay;
            var delay = 0f;
            var count = 0;
            foreach (var character in TextMeshElement.Children)
            {
              
                
                if (currentTime > delay)
                {
                    var time = Mathf.Clamp((float)(currentTime - delay)/childDuration, 0f,1f);
                    character.alpha = animationCurveAsset.BasicIn.Evaluate(time);
                }
                if (currentTime > delay + childDuration)
                {
                    character.alpha = 1f;
                }

                if (normalizedTime > fadeoutDelay)
                {
                    var t = Mathf.Clamp( (float)(normalizedTime - fadeoutDelay) / fadeOutDuration, 0f, 1f);
                    TextMeshElement.alpha = animationCurveAsset.BasicOut.Evaluate(t);
                }
                delay += childDuration;
                count++;
            }
        }
    }
}