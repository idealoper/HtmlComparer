using System;

namespace EditDistanceCalculating
{
	public class EditDistance<TItem>
		where TItem : IEquatable<TItem>
	{
		public int Value { get; }

		public IMutation<TItem>[] Mutations { get; }

		public EditDistance(int value, IMutation<TItem>[] mutations)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value));
			Value = value;
			Mutations = mutations ?? throw new ArgumentNullException(nameof(mutations));
		}
	}
}
