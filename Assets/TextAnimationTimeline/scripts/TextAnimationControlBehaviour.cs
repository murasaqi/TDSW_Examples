using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace TextAnimationTimeline
{
	[Serializable]
	public class TextAnimationControlBehaviour : PlayableBehaviour
	{
		public int id;
		public int layer = -1;
		[HideInInspector]
		public bool isCreate = false;
		[HideInInspector]
		public MotionTextElement motionTextElement;
		[HideInInspector]
		public GameObject overrideParent = null;
		// public bool DestroyTextOnEnd = false;
		public AnimationType animationType;
		public TextSegmentationOptions textSegmentationOptions;
		public float fontSize = -1;
		public TMP_FontAsset overrideFont;
		public Vector3 offsetLocalPosition;
		public Vector3 offsetEulerAngles;
		public Vector3 offsetLocalScale = Vector3.one;
		[HideInInspector]
		public GameObject referenceTransform;
//		public Vector3 StartPosition;
	}
	
	
	
}