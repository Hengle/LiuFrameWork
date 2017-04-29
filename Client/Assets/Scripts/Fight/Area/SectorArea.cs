using UnityEngine;
using System.Collections;

namespace Ranger
{
	public class SectorArea : BaseArea 
	{
		[SerializeField]
		public float radius = 1;
		[SerializeField]
		public float angle = 30;

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

			if (dis.magnitude > radius)
			{
				return false;
			}

			dis = Quaternion.Euler (0, -transform.eulerAngles.y, 0) * dis;
			dis.Normalize ();

			float _ang = Vector3.Angle (dis, Vector3.forward);

			if (_ang < angle / 2.0f)
			{
				return true;
			}

			return false;
		}
	}
}
