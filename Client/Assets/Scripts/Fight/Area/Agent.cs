using UnityEngine;
using System.Collections;

namespace Ranger
{
	public class Node : MonoBehaviour, IAgent
	{
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
		private LayerMask detectMask = -1;
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

		void OnEnable()
		{
		}

		void OnDisable()
		{
		}
	}
}
