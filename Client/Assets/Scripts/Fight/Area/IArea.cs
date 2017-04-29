using UnityEngine;
using System.Collections;

namespace Ranger
{
	public interface IArea : IAgent
	{
		void DetectTarget(ITarget target);
		void RemoveTarget(ITarget target);
	}
}
