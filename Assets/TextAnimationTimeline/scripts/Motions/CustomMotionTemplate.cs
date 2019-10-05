namespace TextAnimationTimeline.Motions
{
    public class CustomMotionTemplate : MotionTextElement
    {
        public override void Init(string word, double duration)
        {
            
           
            TextMeshElement.MotionTextAlignmentOptions = MotionTextAlignmentOptions.MiddleCenter;
//            TextMeshElement.Alpha = 0f;
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
//            TextMeshElement.Alpha = AnimationCurveAsset.BasicInOut.Evaluate((float) normalizedTime);
        }

    }
}