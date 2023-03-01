
/** Pickup.cs
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

// namespace Cuberoot.Interaction
// {
// 	/// <summary>
// 	/// An interactable component that stores Item data and adds it to a specified Container when interacted with.
// 	///</summary>

// 	public class Pickup : Interactable
// 	{
// 		#region Fields

// 		#region (field) ItemData

// 		/// <summary>
// 		/// The Item that will be placed into the container when interacted with.
// 		///</summary>

// 		[Tooltip("The Item that will be placed into the container when interacted with.")]
// 		[SerializeField]

// 		public ItemCountable ItemData;

// 		#endregion
// 		#region (field) EnableInteractOnTouch

// 		/// <summary>
// 		/// If enabled, this pickup will be placed into an available container the moment a valid InteractableSensor touches it.
// 		///</summary>

// 		[Tooltip("If enabled, this pickup will be placed into an available container the moment a valid InteractableSensor touches it.")]
// 		[SerializeField]

// 		public bool EnableInteractOnTouch = true;

// 		#endregion

// 		#endregion
// 		#region Members



// 		#endregion
// 		#region Properties



// 		#endregion
// 		#region Functions

// 		protected override Interaction CreateInteractionWith(Interactor user)
// 		{
// 			var interaction = base.CreateInteractionWith(user);

// 			// Container container = user.GetComponentInParent<Container>();

// 			// if (containter == null) return;

// 			// if (container.Add(ItemData));
// 			// 	Destroy(gameObject);

// 			return interaction;
// 		}

// 		#endregion
// 	}
// }
