using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TextAnimationTimeline
{
	[Serializable]
	public class TextAnimationControlClip : PlayableAsset, ITimelineClipAsset
	{
		public TextAnimationControlBehaviour template = new TextAnimationControlBehaviour();
		
		public ExposedReference<GameObject> overrideParent;
		// public ExposedReference<GameObject> referenceTransform;
//		public ExposedReference<List<GameObject>> referenceGameObjects;
		public ClipCaps clipCaps
		{
			get { return ClipCaps.None; }
		}
		public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<TextAnimationControlBehaviour>.Create(graph, template);
			var clone = playable.GetBehaviour();
			clone.overrideParent = overrideParent.Resolve(graph.GetResolver());
			// clone.referenceTransform = referenceTransform.Resolve(graph.GetResolver());
			return playable;
		}
		
	}
}