using System;

namespace EditDistanceCalculating
{
	public interface IMutation<TItem>
		where TItem : IEquatable<TItem>
	{
		TResult Accept<TResult>(IMutationVisitor<TResult, TItem> visitor);
	}
}
