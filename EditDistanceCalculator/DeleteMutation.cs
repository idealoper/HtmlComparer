using System;

namespace EditDistanceCalculating
{
	public class DeleteMutation<TItem> : IMutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem ActualItem { get; }

		public DeleteMutation(TItem actualItem)
			=> ActualItem = actualItem;

		public TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
