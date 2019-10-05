using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LerpExampleWithAnimationCurve : MonoBehaviour
{
    public Slider slider;
    public float startX = -500;
    public float endY = 500;

    public AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var y = transform.localPosition.y;
        var value = curve.Evaluate(slider.value);
        transform.localPosition = Vector3.Lerp(new Vector3(startX, y, 0), new Vector3(endY, y, 0), value);
    }
}
