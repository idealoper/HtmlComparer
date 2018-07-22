using System;

namespace EditDistanceCalculating
{
	public class LeaveAsIsMutation<TItem> : IMutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem ActualItem { get; }

		public TItem DesiredItem { get; }

		public LeaveAsIsMutation(TItem actualItem, TItem desiredItem)
		{
			ActualItem = actualItem;
			DesiredItem = desiredItem;
		}

		public TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
