using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextAnimationTimeline.Graphics;
using Vector3 = UnityEngine.Vector3;

public class TextMeshCutter : MonoBehaviour {

	public Material capMaterial;

	public Vector2 lineStart;
	public Vector2 lineEnd;

	private LineRenderer lineRenderer;

	public bool debugMode = false;
	private PrimitiveLine line;
	void Awake ()
	{
		gameObject.layer = 2;
		line = new GameObject("debug line").AddComponent<PrimitiveLine>();
		line.transform.SetParent(transform);

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.startWidth = 2;
		lineRenderer.endWidth = 2;
		
		lineRenderer.SetPosition(0,Vector3.zero);
		lineRenderer.SetPosition(1,Vector3.zero);
		
		lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));


	}


	public List<GameObject> CutLine(Vector3 lineStart,Vector3 lineEnd)
	{
		var dist = Vector3.Distance(lineStart, lineEnd);
		var step = 100;
		var count = 0;
		var pieces = new List<GameObject>();
		var direction = (lineEnd - lineStart).normalized;
		while (count < dist)
		{
			
			var origin = lineStart + direction * count;
			transform.position = origin;
			var dt = lineStart - origin;
			float rad = Mathf.Atan2 (dt.y, dt.x);
			float degree = rad * Mathf.Rad2Deg;

			degree += 90;
			transform.localEulerAngles = new Vector3(0f,0,degree);
			pieces =  Cut();
			
			count += step;
		}
		
		lineRenderer.SetPosition(0,lineStart);
		lineRenderer.SetPosition(1,lineEnd);

		return pieces;	
		
	}
	
	void Update(){

		if (Input.GetMouseButtonDown(0))
		{
			Cut();
		}
		
		CutLine(lineStart,lineEnd);
	}



	public List<GameObject> Cut()
	{
		RaycastHit hit;
		var pieces = new List<GameObject>();
		if(Physics.Raycast(transform.position, transform.forward, out hit)){

			Debug.Log(hit.collider.gameObject.name);
			var name = hit.collider.gameObject.name;
			GameObject victim = hit.collider.gameObject;

			if(name != "left side" && name != "right side")pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial).ToList();
			foreach (var p in pieces.ToList())
			{
				p.layer = 2;
			}
		}

		return pieces;
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		var planeWidth = 10;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1000.0f);
		Gizmos.DrawLine(transform.position + transform.up * planeWidth, transform.position + transform.up * planeWidth + transform.forward * 1000.0f);
		Gizmos.DrawLine(transform.position + -transform.up * planeWidth, transform.position + -transform.up * planeWidth + transform.forward * 1000.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * planeWidth);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * planeWidth);

	}

}
