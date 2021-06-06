using System.Collections.Generic;
using UnityEngine;


namespace TextAnimationTimeline.Motions
{
    public class Slash : MonoBehaviour
    {
        private LineRenderer line;
        public Vector3 start;
        public Vector3 end;
        private AnimationCurve curveIn;
        private AnimationCurve curveOut;
        public float delay;
        public List<Vector3> positions =new List<Vector3>();
        public void Init(Vector3 startPos, Vector3 endPos, AnimationCurve curveIn, AnimationCurve curveOut)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Unlit/Color"));

            this.start = startPos;
            this.end = endPos;
            this.curveIn = curveIn;
            this.curveOut = curveOut;
            
            positions.Add(startPos);
            positions.Add(startPos);
//            line.SetPosition(0, startPos);
//            line.SetPosition(1,startPos);

        }

        public void OnProcess(float time)
        {
            var t =0f;
            if (time >= delay) t = (time-delay)/(1f-delay);

            if (t < 0.5)
            {
                positions[0] = Vector3.Lerp(start,end,curveIn.Evaluate(t/0.5f));    
            }
            else
            {
                positions[1] = Vector3.Lerp(start,end,curveOut.Evaluate((t-0.5f)/0.5f));  
            }

//            Debug.Log(positions[0]);


            line.SetPositions(positions.ToArray());
            
        }
    }
    public class BackgroundSlashs : MotionTextElement
    {

        public int size = 3;
        public List <Slash> slashs = new List<Slash>();
        public override void Init(string word, double duration)
        {
            var resolution = textAnimationManager.Resolution;
            
            for (int i = 0; i < size; i++)
            {
                var randomize = new Vector3(Random.Range(resolution.x * 0.35f, -resolution.x * 0.35f), Random.Range(0f,resolution.y * 0.5f), 0f);
                Debug.Log(randomize.x);
                var start = new Vector3(-resolution.x/2, resolution.y/2, 0f) + randomize;
                var min = new Vector3(resolution.x / 2f, -resolution.y / 2f, 0);
                var max = new Vector3(-resolution.x/2f,resolution.y/2f, 0f);

                var direction = min - max;
                var distance = Vector3.Distance(max, min);
                var end = start + direction.normalized * distance * Random.Range(0.5f, 1f);
                var go = new GameObject("line");
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                var l = go.AddComponent<Slash>();
                l.delay = Random.Range(0.5f, 0f);
                l.Init(start, end,animationCurveAsset.BasicIn,animationCurveAsset.Kaf_SlashOut);
                
                slashs.Add(l);     
            }
           
        }
        
        public override void ProcessFrame(double normalizedTime, double seconds)
        {
            foreach (var s in slashs)
            {
                s.OnProcess((float)normalizedTime);
            }
        }

    }
}