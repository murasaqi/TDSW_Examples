using UnityEngine;

namespace TextAnimationTimeline.scripts.Motions
{
    public class MotionSplash : MonoBehaviour
    {

        public Vector3 startPositon;
        public Vector3 endPosition;
        public AnimationCurve curve_x;
        public AnimationCurve curve_y;
        public Vector3 startAngle;
        public Vector3 endAngle;
        public float delay = 0;
        public void Init(AnimationCurve curve_x, AnimationCurve curve_y)
        {
//            this.start = start.position;
//            this.end = end.position;
//
            this.curve_x = curve_x;
            this.curve_y = curve_y;
//
//            this.startAngle = start.eulerAngles;
//            this.endAngle = end.eulerAngles;
            this.startPositon = transform.position;
            this.startAngle = transform.eulerAngles;
        }

        public void OnProcess(float t)
        {
            var time = 0f;
            if (t >= delay)
            {
                time = (t - delay) / (1f - delay);
            }
            var x = Mathf.Lerp(startPositon.x, endPosition.x, curve_x.Evaluate(time));
            var y = Mathf.Lerp(startPositon.y, endPosition.y, curve_y.Evaluate(time));
            var z = Mathf.Lerp(startPositon.z, endPosition.z, curve_x.Evaluate(time));
            var angle = Vector3.Lerp(startAngle, endAngle, curve_x.Evaluate(time));

            transform.eulerAngles = angle;
            transform.position = new Vector3(x,y,z);
        }

    }
}