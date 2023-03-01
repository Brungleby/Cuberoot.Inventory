
/** IContainable.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.

*/

#region Includes

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#endregion

namespace Cuberoot
{
	/// <summary>
	/// Implements methods that allow this to be placed into a <see cref="Container"/>.
	///</summary>

	public interface IContainable
	{
		bool CanBeAddedTo(Container<IContainable> container);
		bool CanBeRemovedFrom(Container<IContainable> container);
	}
}
