using UnityEngine;
using System.Collections;

namespace Ranger
{
	public interface IAgent
	{
		int layer { get; }
		Transform transform { get; }
		LayerMask DetectMask { get; set; } // 规则参考Camera的cullingMask
	}
}
