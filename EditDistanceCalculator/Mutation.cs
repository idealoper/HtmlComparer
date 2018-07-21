using System;

namespace EditDistanceCalculating
{
	public abstract class Mutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem Item { get; }

		public Mutation(TItem item)
			=> Item = item;

		public abstract TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor);
	}
}
