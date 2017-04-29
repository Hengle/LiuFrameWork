using UnityEngine;
using System.Collections;

namespace Ranger
{
	public class RectArea : BaseArea
	{
		[SerializeField]
		public float width = 1;
		[SerializeField]
		public float height = 1;

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

			dis = Quaternion.Euler (0, -transform.eulerAngles.y, 0) * dis;

			if (dis.x > -width/2.0f && dis.x < width/2.0f && dis.z > -height/2.0f && dis.z < height/2.0f)
			{
				return true;
			}

			return false;
		}
	}
}
