using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SlickControls;

public class SlickImageBackgroundControlCollection : IList<SlickImageBackgroundControl>, ICollection<SlickImageBackgroundControl>, IEnumerable
{
	private readonly List<SlickImageBackgroundControl> controls = [];
	private readonly object lockObj = new();

	public int Count
	{
		get
		{
			lock (lockObj)
			{
				return controls.Count;
			}
		}
	}

	public bool IsReadOnly { get; }

	public SlickImageBackgroundPanel Owner { get; }

	public SlickImageBackgroundControl this[int index]
	{
		get
		{
			lock (lockObj)
			{
				return controls[index];
			}
		}
		set
		{
			lock (lockObj)
			{
				controls[index] = value;
			}
		}
	}

	public SlickImageBackgroundControlCollection(SlickImageBackgroundPanel owner)
	{
		Owner = owner;
	}

	public void Add(SlickImageBackgroundControl value)
	{
		lock (lockObj)
		{
			value.Parent = Owner;
			value.Container = Owner.Container;

			if (!controls.Contains(value))
			{
				controls.Add(value);
			}

			if (value is SlickImageBackgroundPanel panel)
			{
				panel.Controls.AddRange(panel.Controls.ToArray());
			}
		}
	}

	public void AddRange(SlickImageBackgroundControl[] SlickImageBackgroundControls)
	{
		lock (lockObj)
		{
			foreach (var value in SlickImageBackgroundControls)
			{
				Add(value);
			}
		}
	}

	public void Clear()
	{
		lock (lockObj)
		{
			controls.Clear();
		}
	}

	public bool Contains(SlickImageBackgroundControl SlickImageBackgroundControl)
	{
		lock (lockObj)
		{
			return controls.Contains(SlickImageBackgroundControl);
		}
	}

	public void CopyTo(SlickImageBackgroundControl[] array, int arrayIndex)
	{
		lock (lockObj)
		{
			controls.CopyTo(array, arrayIndex);
		}
	}

	public int GetChildIndex(SlickImageBackgroundControl child)
	{
		lock (lockObj)
		{
			return controls.IndexOf(child);
		}
	}

	public IEnumerator<SlickImageBackgroundControl> GetEnumerator()
	{
		lock (lockObj)
		{
			foreach (var item in controls)
			{
				yield return item;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return controls.GetEnumerator();
	}

	public int IndexOf(SlickImageBackgroundControl SlickImageBackgroundControl)
	{
		lock (lockObj)
		{
			return controls.IndexOf(SlickImageBackgroundControl);
		}
	}

	public void Insert(int index, SlickImageBackgroundControl item)
	{
		lock (lockObj)
		{
			controls.Insert(index, item);
		}
	}

	public void Remove(SlickImageBackgroundControl value)
	{
		lock (lockObj)
		{
			controls.Remove(value);
		}
	}

	bool ICollection<SlickImageBackgroundControl>.Remove(SlickImageBackgroundControl item)
	{
		lock (lockObj)
		{
			return controls.Remove(item);
		}
	}

	public void RemoveAt(int index)
	{
		lock (lockObj)
		{
			controls.RemoveAt(index);
		}
	}

	public void SetChildIndex(SlickImageBackgroundControl child, int newIndex)
	{
		lock (lockObj)
		{
			controls.Insert(newIndex, child);
		}
	}

	public void Add(object p)
	{
		throw new NotImplementedException();
	}
}