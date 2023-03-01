
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
	/// This is your typical item class. Countable items are measured in integer quantities.
	///</summary>

	[CreateAssetMenu(fileName = "New Countable Item", menuName = "Cuberoot/Inventory/Countable Item", order = 50)]

	public class ItemCountable : Item<int>
	{
		public sealed override int Capacity
		{
			get => base.Capacity;
			set => base.Capacity = System.Math.Max(value, -1);
		}

		public sealed override bool IsCapacityInfinite => Capacity < 0;
	}
}
