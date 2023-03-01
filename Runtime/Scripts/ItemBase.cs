
/** Item.cs
*
*	Created by LIAM WOFFORD of CUBEROOT SOFTWARE, LLC.
*
*	Free to use or modify, with or without creditation,
*	under the Creative Commons 0 License.
*/


#region Includes

using System.Collections.Generic;

using UnityEngine;

#endregion

namespace Cuberoot
{
	#region ItemBase

	/// <summary>
	/// The base class for all items. This class cannot be stored inside of a <see cref="Container"/>.
	///</summary>

	[CreateAssetMenu(fileName = "New ItemBase", menuName = "Cuberoot/Inventory/Basic Item", order = 50)]

	public class ItemBase : ScriptableObject, System.IComparable<ItemBase>, ITaggable
	{
		#region Fields

		[Header("Info")]

		#region ID

		/// <summary>
		/// The internal name of this item, used for comparing item equality (which is used when stacking item listings).
		///</summary>
		[Tooltip("The internal name of this item, used for comparing item equality.")]
		[SerializeField]

		private string _ID;

		/// <inheritdoc cref="_ID"/>

		public string ID => _ID;

		#endregion
		#region ListOrder

		/// <summary>
		/// Determines the default order in which this item should be listed.
		///</summary>
		[Tooltip("Determines the default order in which this item should be listed.")]
		[SerializeField]

		private int _ListOrder;

		/// <inheritdoc cref="_ListOrder"/>

		public int ListOrder
		{
			get => _ListOrder;
			set => _ListOrder = value;
		}

		#endregion

		#region DisplayName

		/// <summary>
		/// The name used to display in-game (for a singular quantity).
		///</summary>
		[Tooltip("The name used to display in-game (for a singular quantity).")]
		[SerializeField]

		private string _DisplayName;

		/// <inheritdoc cref="DisplayName"/>

		public string DisplayName
		{
			get => _DisplayName;
			set => _DisplayName = value;
		}

		#endregion
		#region PluralName

		/// <summary>
		/// Plural form of this item's name that can be used when describing multiple instances of this item.
		///</summary>
		[Tooltip("Plural form of this item's name that can be used when describing multiple instances of this item. If left unspecified, defaults to DisplayName.")]
		[SerializeField]

		private string _PluralName;

		/// <inheritdoc cref="PluralName"/>

		public string PluralName
		{
			get
			{
				if (_PluralName == string.Empty)
					return DisplayName;
				else
					return _PluralName;
			}
			set => _PluralName = value;
		}

		#endregion
		#region Description

		/// <summary>
		/// In-game, display description of this item.
		///</summary>
		[Tooltip("In-game, display description of this item.")]
		[TextArea(4, 10)]
		[SerializeField]

		private string _Description;

		/// <inheritdoc cref="Description"/>

		public string Description
		{
			get => _Description;
			set => _Description = value;
		}

		#endregion

		[Header("Attributes")]

		#region BaseValue

		/// <summary>
		/// Custom value that can be used for anything, but typically used to denote monetary value.
		///</summary>
		[Tooltip("Custom value that can be used for anything, but typically used to denote monetary value.")]

		public float BaseValue = 0f;

		#endregion
		#region IsUsable

		/// <summary>
		/// Whether or not players can use this item directly.
		///</summary>
		[Tooltip("Whether or not players can use this item directly.")]

		public bool IsUsable = false;

		#endregion
		#region IsConsumable

		/// <summary>
		/// Whether or not this item is consumed when it is used (either directly or indirectly).
		///</summary>
		[Tooltip("Whether or not this item is consumed when it is used (either directly or indirectly).")]

		public bool IsConsumable = false;

		#endregion
		#region IsDestructible

		/// <summary>
		/// Whether or not players can destroy (or sell, etc.) this item directly from their inventory. Key items should have this value set to false.
		///</summary>
		[Tooltip("Whether or not players can destroy (or sell, etc.) this item directly from their inventory. Key items should have this value set to false.")]

		public bool IsDestructible = false;

		#endregion

		[Header("Logistics")]

		#region Spawn Prefab

		/// <summary>
		/// This prefab will be Instantiated when we want this item to manifest itself in the world with all available functionality.
		///</summary>
		[Tooltip("This prefab will be Instantiated when we want this item to manifest itself in the world with all available functionality.")]
		[SerializeField]

		private GameObject _SpawnPrefab;

		/// <inheritdoc cref="_SpawnPrefab"/>

		public GameObject SpawnPrefab
		{
			get => _SpawnPrefab;
			set => _SpawnPrefab = value;
		}

		#endregion
		#region Preview Prefab

		/// <summary>
		/// This prefab will be Instantiated when we want to display this item without any actual functionality.
		///</summary>
		[Tooltip("This prefab will be Instantiated when we want to display this item without any actual functionality.")]
		[SerializeField]

		private GameObject _PreviewPrefab;

		/// <inheritdoc cref="_PreviewPrefab"/>

		public GameObject PreviewPrefab
		{
			get => _PreviewPrefab;
			set => _PreviewPrefab = value;
		}

		#endregion

		#region Icon

		/// <summary>
		/// Sprite icon that represents this Item.
		///</summary>

		[Tooltip("Sprite icon that represents this Item.")]

		public Sprite Icon;

		#endregion

		#region Tags

		/// <inheritdoc cref="ITaggable.Tags"/>

		[Tooltip("A set of categorizational tags that apply to this item. In a container UI display, one can use these tags to order or filter items.")]
		[SerializeField]

		private string[] _Tags;

		/// <inheritdoc cref="ITaggable.Tags"/>
		/// <remarks>
		/// This HashSet is constructed from <see cref="_Tags"/>.
		///</remarks>

		private HashSet<string> _TagsHashSet;

		/// <inheritdoc cref="ITaggable.Tags"/>

		public HashSet<string> Tags => _TagsHashSet;

		#endregion

		#endregion

		#region Methods

		#region OnValidate

		protected virtual void OnValidate()
		{
			_TagsHashSet = new HashSet<string>();
			_TagsHashSet.AddAll(_Tags);
		}

		#endregion
		#region Awake

		protected virtual void Awake()
		{
			_PreviewPrefab ??= _SpawnPrefab;

			OnValidate();
		}

		#endregion

		#endregion

		#region (interface) CompareTo

		public int CompareTo(ItemBase other) => ListOrder.CompareTo(other.ListOrder);

		#endregion

		#region IsUsableWith

		/// <returns>
		/// TRUE if the item is currently usable for a specific <paramref name="user"/>. A specific <paramref name="user"/> is able to be provided but is not necessary for all Items.
		///</returns>

		public bool IsUsableWith(ItemUser<ItemBase> user = null) =>
			IsUsable && _IsUsableWith(user) && (user != null ? user.CanUse(this) : true);

		/// <inheritdoc cref="IsUsableWith"/>
		/// <remarks>
		/// This method can be overridden to create custom usability conditions.
		///</remarks>

		protected virtual bool _IsUsableWith(ItemUser<ItemBase> user) => true;

		#endregion
		#region TryUse

		/// <summary>
		/// Call this function to attempt to use this Item. A specific <paramref name="user"/> is able to be provided but is not necessary for all Items.
		///</summary>

		public void TryUse(ItemUser<ItemBase> user = null)
		{
			if (IsUsableWith(user))
			{
				UseWith(user);

				if (user != null) user.Use(this);
			}
			else
			{
				throw new ItemUseFailedException($"{DisplayName} couldn't be used with {user.ToString()}.");
			}
		}

		#endregion
		#region UseWith

		/// <summary>
		/// This function is called when the item is "officially" used, as it has been confirmed to be usable with the given <paramref name="user"/>.
		///</summary>

		protected virtual void UseWith(ItemUser<ItemBase> user) { }

		#endregion

		public virtual bool CanBeAddedTo(Container<IContainable> container) => true;
		public virtual bool CanBeRemovedFrom(Container<IContainable> container) => true;
	}

	#endregion
	#region (class) Item<TQuantity>

	/// <summary>
	/// The base class for items which can be stored inside containers and from there, filtered and sorted. Supports a few basic properties most games' items will have.
	///</summary>

	public abstract class Item<TQuantity> : ItemBase
	{
		#region Fields

		#region Capacity

		/// <inheritdoc cref="Capacity"/>
		[Tooltip("Quantity of this item can be stored into a single item listing or container. If left unspecified at 0, there will be no limit.")]
		[Min(0)]
		[SerializeField]

		private TQuantity _Capacity = default(TQuantity);

		public virtual TQuantity Capacity
		{
			get => _Capacity;
			set => _Capacity = value;
		}

		#endregion

		#endregion

		#region Properties

		public abstract bool IsCapacityInfinite { get; }

		#endregion
		#region Methods

		#endregion
	}

	#endregion

	#region (interface) ItemUser

	/// <summary>
	/// Implements methods that allow the use of items.
	///</summary>

	public interface ItemUser<TItem>
	{
		void Use(TItem item);
		virtual bool CanUse(TItem item) => true;
	}

	#endregion
	#region (exception) ItemUseFailedException

	[System.Serializable]

	public class ItemUseFailedException : System.Exception
	{
		public ItemUseFailedException() { }
		public ItemUseFailedException(string message) : base(message) { }
		public ItemUseFailedException(string message, System.Exception inner) : base(message, inner) { }
		protected ItemUseFailedException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

	#endregion
}
