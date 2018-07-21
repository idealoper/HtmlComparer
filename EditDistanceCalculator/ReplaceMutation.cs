using System;

namespace EditDistanceCalculating
{
	public class ReplaceMutation<TItem> : Mutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public TItem NewItem { get; }

		public ReplaceMutation(TItem oldItem, TItem newItem)
			: base(oldItem)
				=> NewItem = newItem;

		public override TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
