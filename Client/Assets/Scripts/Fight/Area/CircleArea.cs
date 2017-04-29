using UnityEngine;
using System.Collections;

namespace Ranger
{
	public class CircleArea : BaseArea
	{
		[SerializeField]
		public float radius = 1;

		protected override bool CheckTarget (ITarget target)
		{
			if (!base.CheckTarget (target))
			{
				return false;
			}

			Vector3 dis = new Vector3 (
							  target.transform.position.x - transform.position.x,
				              0,
							  target.transform.position.z - transform.position.z);
			if (dis.magnitude < radius)
			{
				return true;
			}

			return false;
		}
	}
}
