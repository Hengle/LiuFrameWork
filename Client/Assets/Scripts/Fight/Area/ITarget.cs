using UnityEngine;
using System.Collections;

namespace Ranger
{
	public interface ITarget : IAgent 
	{
		void OnEnterArea(IArea area);
		void OnExitArea(IArea area);
		void OnStayInArea(IArea area);
    }
}
