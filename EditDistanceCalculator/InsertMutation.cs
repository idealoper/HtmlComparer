using System;

namespace EditDistanceCalculating
{
	public class InsertMutation<TItem> : Mutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public InsertMutation(TItem item) : base(item) { }

		public override TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
