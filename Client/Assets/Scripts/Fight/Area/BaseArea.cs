using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ranger
{
	public abstract class BaseArea : MonoBehaviour, IArea
	{
		[Serializable]
		public class AgentActionEvent : UnityEvent<IAgent> {}

        // 当检测目标从区域范围外进入范围内时触发
        // 可通过AddListener将具体实现添加到调用列表中
        // e.g. 比如当人物玩家进入到NPC发现范围时触发NPC追击
		[SerializeField]
		public AgentActionEvent OnAgentEnter = new AgentActionEvent();
        // 当检测目标从区域范围内离开到范围外时触发
		[SerializeField]
		public AgentActionEvent OnAgentExit = new AgentActionEvent();
        // 当检测目标停留在区域内时触发
		[SerializeField]
		public AgentActionEvent OnAgentStay = new AgentActionEvent();

		public delegate bool CustomPostCheckMethod(ITarget target);

        // 自定义后续检测步骤
        // 当检测目标通过单纯的数学计算发现在范围内时，如果此delegate被设置则调用此方法
        // e.g. 比如当玩家进入NPC发现范围时，后续步骤可以是通过Physics.Raycast检测是否被墙挡住
		public CustomPostCheckMethod postCheckMethod = null;

		protected List<ITarget> mCaptureTarget = new List<ITarget> ();
		/// 获得范围内所有目标//
		public List<ITarget> CaptureTargets{
			get
			{
				return mCaptureTarget;
			}
		}

		public int layer
		{
			get
			{
				return transform.gameObject.layer;
			}
		}

//		public new Transform transform
//		{
//			get
//			{
//				return base.transform;
//			}
//		}
			
		[SerializeField]
		private LayerMask detectMask = -1; // 规则参考Camera的cullingMask

		public LayerMask DetectMask
		{
			get
			{
				return detectMask;
			}
			set
			{
				detectMask = value;
			}
		}

		public virtual void OnEnable()
		{
			AgentManager.Instance.RegisterArea (this);
		}

		public virtual void OnDisable()
		{
			foreach (ITarget target in mCaptureTarget)
			{
				target.OnExitArea (this);
				OnAgentExit.Invoke (target);
			}
			mCaptureTarget.Clear ();
			AgentManager.Instance.UnRegisterArea (this);
		}

		public virtual void Update()
		{
			for (int i = 0; i < mCaptureTarget.Count; ++i)
			{
				mCaptureTarget [i].OnStayInArea (this);
				if (OnAgentStay != null)
				{
					OnAgentStay.Invoke (mCaptureTarget [i]);
				}
			}
		}

		public void DetectTarget(ITarget target)
		{
			bool _in = CheckTarget (target);

			if (_in && postCheckMethod != null)
			{
				_in = postCheckMethod (target);
			}

			if (_in && (!mCaptureTarget.Contains (target)))
			{
				OnTargetEnter (target);
			}
			else if (!_in && (mCaptureTarget.Contains (target)))
			{
				OnTargetExit (target);
			}
		}

		protected virtual bool CheckTarget(ITarget target)
		{
			return CheckLayer (target.layer);
		}

		protected bool CheckLayer(int layer)
		{
			return (DetectMask.value & (1 << layer)) != 0;
		}

		private void OnTargetEnter(ITarget target)
		{
			mCaptureTarget.Add (target);
			target.OnEnterArea (this);
			OnAgentEnter.Invoke (target);
		}

		private void OnTargetExit(ITarget target)
		{
			
			mCaptureTarget.Remove (target);
			

			target.OnExitArea (this);
			OnAgentExit.Invoke (target);
		}

		public void RemoveTarget(ITarget target)
		{
			if(mCaptureTarget.Contains(target))
			{
				OnTargetExit (target);
			}
			
		}
	}
}
