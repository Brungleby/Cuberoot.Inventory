
/** CuberootTemplate.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.
*/

#region Includes

using UnityEngine;

#endregion

namespace Cuberoot
{
	/// <summary>
	/// This is an atypical item class. Fluid items are measured in float quantities.
	///</summary>

	[CreateAssetMenu(fileName = "New Fluid Item", menuName = "Cuberoot/Inventory/Fluid Item", order = 50)]

	public class ItemFluid : Item<float>
	{
		public sealed override float Capacity
		{
			get => base.Capacity;
			set => base.Capacity = System.Math.Max(value, -1f);
		}

		public sealed override bool IsCapacityInfinite => Mathf.FloorToInt(Capacity) < 0;
	}
}
