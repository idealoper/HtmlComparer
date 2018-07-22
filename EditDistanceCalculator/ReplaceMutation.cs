using System;

namespace EditDistanceCalculating
{
	public class ReplaceMutation<TItem> : IMutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem ActualItem { get; }

		public TItem DesiredItem { get; }

		public ReplaceMutation(TItem actualItem, TItem desiredItem)
		{
			ActualItem = actualItem;
			DesiredItem = desiredItem;
		}

		public TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
