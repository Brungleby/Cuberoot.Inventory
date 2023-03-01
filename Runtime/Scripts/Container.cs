
/** ContainerBase.cs
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
using UnityEngine.Events;

#endregion

namespace Cuberoot
{
	/// <summary>
	/// A <see cref="Container"/> is a special data structure that allows an item to be added only if certain (overridable) conditions have been met. Additionally, the Container must have enough capacity for the item to be stored.
	///</summary>

	public abstract class Container<TItem> : MonoBehaviour, ICollection<TItem>
	where TItem : IContainable
	{
		#region Fields

		/// <summary>
		/// This event is called whenever any new items are added to this container. This event supplies the list of items which were successfully added.
		///</summary>
		[Tooltip("This event is called whenever any new items are added to this container.")]
		[SerializeField]

		public UnityEvent<TItem[]> OnContentsAdded;

		/// <summary>
		/// This event is called whenever any new items are removed from this container. This event supplies the list of items which were successfully removed.
		///</summary>
		[Tooltip("This event is called whenever any new items are removed from this container.")]
		[SerializeField]

		public UnityEvent<TItem[]> OnContentsRemoved;

		/// <summary>
		/// This event is called whenever any changes have been made to the contents of any item or subitem of this Container.
		///</summary>
		[Tooltip("This event is called whenever any changes have been made to the contents of any item or subitem of this Container.")]
		[SerializeField]

		public UnityEvent<TItem[]> OnContentsModified;

		#endregion
		#region Members

		#region Entries

		/// <summary>
		/// List of entries to be displayed for this container. This is NOT necessarily the comprehensive list of items within.
		///</summary>

		private List<TItem> _entries = new List<TItem>();

		/// <inheritdoc cref="_entries"/>

		public List<TItem> entries => _entries;

		#endregion

		#endregion

		#region Properties

		#region MaxEntries

		/// <summary>
		/// The maximum number of entries that this container can hold.
		///</summary>

		public abstract int maxItems { get; set; }

		#endregion
		#region IsInfinite

		/// <summary>
		/// This property will be TRUE if this container can hold an infinite number of items.
		///</summary>

		public virtual bool IsInfinite => maxItems < 0;

		#endregion
		#region IsFull

		/// <summary>
		/// This property will be TRUE if no further entries may be added to this container.
		///</summary>

		public virtual bool isFull => IsInfinite ? false : _entries.Count >= maxItems;

		#endregion
		#region IsEmpty

		/// <summary>
		/// This property will be TRUE if there are currently no items inside this container.
		///</summary>

		public bool isEmpty => _entries.Count == 0;

		#endregion

		#endregion
		#region Methods

		#region GetQuantityOf

		public int GetQuantityOf(TItem query)
		{
			var result = 0;
			foreach (var entry in _entries)
			{
				result += (entry.Equals(query) ? 1 : 0);
			}

			return result;
		}

		#endregion
		#region CanAddEntry

		public virtual bool CanAddEntry(TItem entry) => true;

		#endregion
		#region AddItem

		private bool _AddItem(TItem entry)
		{
			if (isFull && CanAddEntry(entry) && entry.CanBeAddedTo(this as Container<IContainable>))
			{
				Add(entry);
				return true;
			}

			return false;
		}

		public bool AddItem(TItem item)
		{
			if (_AddItem(item))
			{
				OnContentsAdded.Invoke(new TItem[1] { item });
				return true;
			}

			return false;
		}

		public int AddItem(TItem item, int count)
		{
			var __passed = new List<TItem>();
			for (var i = 0; i < count; i++)
			{
				if (_AddItem(item))
					__passed.Add(item);
				else
					break;
			}

			OnContentsAdded.Invoke(__passed.ToArray());

			return __passed.Count;
		}

		public (TItem[], TItem[]) AddItem(ICollection<TItem> items)
		{
			var __passed = new List<TItem>();
			var __failed = new List<TItem>();

			foreach (var item in items)
				(_AddItem(item) ? __passed : __failed).Add(item);

			var __passedArray = __passed.ToArray();
			var __failedArray = __failed.ToArray();

			OnContentsAdded.Invoke(__passedArray);

			return (__passedArray, __failedArray);
		}

		#endregion

		#region CanRemoveItem

		public virtual bool CanRemoveItem(TItem item) => true;

		#endregion
		#region RemoveItem

		private bool _RemoveItem(TItem item)
		{
			if (!isEmpty && CanRemoveItem(item) && item.CanBeRemovedFrom(this as Container<IContainable>))
				return Remove(item);
			return false;
		}

		public bool RemoveItem(TItem item)
		{
			if (_RemoveItem(item))
			{
				OnContentsRemoved.Invoke(new TItem[1] { item });
				return true;
			}

			return false;
		}

		public int RemoveItem(TItem item, int count)
		{
			var __passed = new List<TItem>();

			for (var i = 0; i < count; i++)
			{
				if (_RemoveItem(item))
					__passed.Add(item);
				else
					break;
			}

			OnContentsRemoved.Invoke(__passed.ToArray());

			return __passed.Count;
		}

		public (TItem[], TItem[]) RemoveItem(ICollection<TItem> items)
		{
			var __passed = new List<TItem>();
			var __failed = new List<TItem>();
			foreach (var item in items)
				(_RemoveItem(item) ? __passed : __failed).Add(item);

			var __passedArray = __passed.ToArray();
			var __failedArray = __failed.ToArray();

			OnContentsRemoved.Invoke(__passedArray);

			return (__passedArray, __failedArray);
		}

		#endregion

		#region Flush

		/// <summary>
		/// Clears the container, but safely retains any items that aren't allowed to be removed. To dispose of all items, use <see cref="Clear"/>.
		///</summary>

		public void Flush() =>
			RemoveItem(_entries);

		#endregion

		#endregion
		#region ICollection Overrides

		public int Count => _entries.Count;
		public bool IsReadOnly => false;

		public void Add(TItem item) => _entries.Add(item);

		public void Clear()
		{
			OnContentsModified.Invoke(_entries.ToArray());
			_entries.Clear();
		}

		public bool Contains(TItem item) => _entries.Contains(item);
		public void CopyTo(TItem[] array, int arrayIndex) => _entries.CopyTo(array, arrayIndex);
		public IEnumerator<TItem> GetEnumerator() => _entries.GetEnumerator();
		public bool Remove(TItem item) => _entries.Remove(item);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
}
