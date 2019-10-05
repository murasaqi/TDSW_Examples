using System.Collections;
using System.Linq;
using TextAnimationTimeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TextAnimationTimeline
{
	[TrackClipType(typeof(TextAnimationControlClip))]
//	[TrackBindingType(typeof(Light))]
	[TrackBindingType(typeof(TextAnimationManager))]
	

	public class TextAnimationControlTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			var mixer = ScriptPlayable<TextAnimationControlMixerBehaviour>.Create(graph, inputCount);
//				Debug.Log(GetClips().ToList());
			mixer.GetBehaviour().clips = GetClips().ToList();
			mixer.GetBehaviour().m_PlayableDirector = go.GetComponent<PlayableDirector>();
			return mixer;
		}
	}
}