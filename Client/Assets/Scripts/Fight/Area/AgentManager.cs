using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ranger
{
	public class AgentManager : MonoBehaviour, IFightTick
	{
        #region IFightTick
        public void FightTick (float nDeltaTime) {
            for (int i = 0; i < mTargetList.Count; ++i) {
                DetectTargetAgent( mTargetList[i] );
            }
        }
        #endregion

        private static AgentManager _instance = null;

		public static AgentManager Instance
		{
			get
			{
				return _instance;
			}
		}

        // key 是IAgent的detectMask
		private Dictionary<int, List<IArea>> mAreaDics = null;

		private List<ITarget> mTargetList = null;

		void Awake()
		{
			_instance = this;
			Init ();
		}

		private void Init()
		{
			mAreaDics = new Dictionary<int, List<IArea>> ();
			mTargetList = new List<ITarget> ();
		}

		public void Clear()
		{
			mAreaDics.Clear ();
			mTargetList.Clear ();
		}

        // 注册监测区域
		public void RegisterArea(IArea area)
		{
			for (int i = 0; i < 32; ++i)
			{
				if ((area.DetectMask & (1 << i)) != 0)
				{
					if (!mAreaDics.ContainsKey (i))
					{
						mAreaDics.Add (i, new List<IArea> ());
					}

					if (mAreaDics [i].Contains (area))
					{
						Debug.Log ("Duplicate area");
					}
					else
					{
						mAreaDics [i].Add (area);
					}
				}
			}
		}

        // 注销监测区域
		public void UnRegisterArea(IArea area)
		{
			for (int i = 0; i < 32; ++i)
			{
				if ((area.DetectMask & (1 << i)) != 0)
				{
					if (mAreaDics.ContainsKey (i) && mAreaDics [i].Contains (area))
					{
						mAreaDics [i].Remove (area);
					}
//					else
//					{
//						Debug.LogFormat ("Layer {0} does not contain this area", i);
//					}
				}
			}
		}

		public void AddTarget(ITarget target)
		{
			if (!mTargetList.Contains (target))
			{
				mTargetList.Add (target);
			}
			else
			{
				Debug.Log ("Duplicate target");
			}
		}

		public void RemoveTarget(ITarget target)
		{
			if (mTargetList.Contains (target))
			{
				mTargetList.Remove (target);
				var areaList = new List<List<IArea>> (mAreaDics.Values);
				for (int i = 0; i < areaList.Count; ++i) {
					var list = areaList [i];
					for (int j = 0; j < list.Count; ++j) {
						list [j].RemoveTarget (target);
					}
				}
			}
			else
			{
				Debug.Log ("No such target");
			}
		}

        

  //      void Update()
		//{
		//	for (int i = 0; i < mTargetList.Count; ++i)
		//	{
		//		DetectTargetAgent (mTargetList [i]);
		//	}
		//}

		private void DetectTargetAgent(ITarget target)
		{
			if (mAreaDics.ContainsKey (target.layer))
			{
				for (int i = 0; i < mAreaDics [target.layer].Count; ++i)
				{
					mAreaDics [target.layer] [i].DetectTarget (target);
				}
			}
		}
		/// <summary>
		/// 主动监测,用来做某些需要立即计算的范围,适用于需要不等Update马上获得结果的情况.
		/// 不要在单帧调用多次.
		/// </summary>
		public void DetectTargetWithArea(IArea area)
		{
			for (int i=0; i<mTargetList.Count; i++)
			{
				area.DetectTarget(mTargetList[i]);
			}
		}
	}
}
