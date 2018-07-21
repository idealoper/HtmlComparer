using System;

namespace EditDistanceCalculating
{
	public class DeleteMutation<TItem> : Mutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public DeleteMutation(TItem item) : base(item) { }

		public override TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
