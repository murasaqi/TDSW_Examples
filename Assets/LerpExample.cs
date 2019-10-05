using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LerpExample : MonoBehaviour
{
    public Slider slider;

    public float startX = -500;

    public float endY = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var y = transform.localPosition.y;
    
        transform.localPosition = Vector3.Lerp(new Vector3(startX, y, 0), new Vector3(endY, y, 0), slider.value);
    }
}
