using UnityEngine;
using System.Collections;

namespace Ranger
{
	public class TwoSectorArea : BaseArea 
	{
		[SerializeField]
		public float radiusFront = 1;
		[SerializeField][Range(1,180)]
		public int angleFront = 180;
		[SerializeField]
		public float radiusBack = 1;
		[SerializeField][Range(1,180)]
		public int angleBack = 180;

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
			float length = dis.magnitude;
			if (length > radiusFront && length > radiusBack)
			{
				return false;
			}

			dis = Quaternion.Euler (0, -transform.eulerAngles.y, 0) * dis;
			dis.Normalize ();

			float _angf = Vector3.Angle (dis, Vector3.forward);
			float _angb = Vector3.Angle (dis, Vector3.back);

			if (length <= radiusFront && _angf < angleFront / 2.0f)
			{
				return true;
			}
			if (length <= radiusBack && _angb < angleBack / 2.0f)
			{
				return true;
			}
			return false;
		}
	}
}
