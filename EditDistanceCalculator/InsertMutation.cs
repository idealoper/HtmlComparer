using System;

namespace EditDistanceCalculating
{
	public class InsertMutation<TItem> : IMutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem DesiredItem { get; }

		public InsertMutation(TItem desiredItem)
			=> DesiredItem = desiredItem;

		public TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
