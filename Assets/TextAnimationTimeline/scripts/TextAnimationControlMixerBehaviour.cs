using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TextAnimationTimeline;
//using TextAnimationGenerater.Motions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Debug = UnityEngine.Debug;

//using UnityEditor;
namespace TextAnimationTimeline
{



	public class TextAnimationControlMixerBehaviour : PlayableBehaviour
	{

		public List<TimelineClip> clips;
		internal PlayableDirector m_PlayableDirector;
		private List<PlayableBehaviour> inputs = new List<PlayableBehaviour>();

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			TextAnimationManager trackBinding = playerData as TextAnimationManager;

			if (!trackBinding)
				return;

			double time = m_PlayableDirector.time;
			var counter = 0;
			foreach (var clip in clips)
			{
				counter++;
				
				var inputPlayable = (ScriptPlayable<TextAnimationControlBehaviour>) playable.GetInput(counter - 1);
				var input = inputPlayable.GetBehaviour();
				if (clip.end < time)
				{
					if (input.motionTextElement)
					{
//						Debug.Log(clip.displayName+":" + 1);
						input.motionTextElement.ProcessFrame(1, time-clip.start);
						
						if(input.DestroyTextOnEnd){input.motionTextElement.Remove();}
					}
				}

				if (clip.start <= time && clip.end >= time)
				{
					if (!input.motionTextElement)
					{
						var motion = trackBinding.CreateMotionTextElement(clip.displayName, input.animationType);
						motion.TextAnimationManager = trackBinding;
						motion.DebugMode = trackBinding.DebugMode;
						motion.Font = input.overrideFont ? input.overrideFont : trackBinding.BaseFont;
						motion.FontSize = input.fontSize >= 0 ? input.fontSize : trackBinding.BaseFontSize;
						motion.Parent = trackBinding.ParentGameObject != null ? trackBinding.ParentGameObject.transform : trackBinding.transform;
						if (input.overrideParent != null) motion.Parent = input.overrideParent.transform;
						motion.transform.SetParent(motion.Parent);
						motion.OffsetLocalScale = input.offsetLocalScale;
						motion.OffsetEulerAngles = input.offsetEulerAngles;
						motion.OffsetLocalPosition = input.offsetLocalPosition;
						if (input.layer >= 0) motion.layer = input.layer;
						motion.ID = input.id;
						motion.TextSegmentationOptions = input.textSegmentationOptions;
						if (input.referenceTransform != null)
							motion.ReferenceTransform = input.referenceTransform.transform;
						
						input.motionTextElement = motion;
						input.isCreate = true;
						motion.Init(clip.displayName, clip.duration);
						break;
					}
					else
					{
						var normalizedTime = Mathf.Clamp((float)((time-clip.start)/clip.duration),0f,1f);
//						Debug.Log(clip.displayName+":" + normalizedTime);
						input.motionTextElement.ProcessFrame(normalizedTime, time-clip.start);
					}
				}

				if (clip.end < time && input.isCreate)
				{
					input.isCreate = false;
				}



				if (input.motionTextElement && time < clip.start)
				{
					input.motionTextElement.Remove();
					input.motionTextElement = null;
				}
			}
		}
	}
}
