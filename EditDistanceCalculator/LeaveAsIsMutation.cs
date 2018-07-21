using System;

namespace EditDistanceCalculating
{
	public class LeaveAsIsMutation<TItem> : Mutation<TItem>
		where TItem : IEquatable<TItem>
	{
		public LeaveAsIsMutation(TItem item) : base(item) { }

		public override TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor)
			=> visitor.Visit(this);
	}
}
