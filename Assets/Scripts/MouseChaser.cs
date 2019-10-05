using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseChaser : MonoBehaviour
{
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log(Input.mousePosition);
        transform.localPosition += ( Input.mousePosition  - transform.position) * speed;
    }
}
