using System;

namespace EditDistanceCalculating
{
	public interface IMutationVisitor<TResult, TItem>
		where TItem : IEquatable<TItem>
	{
		TResult Visit(InsertMutation<TItem> mutation);
		TResult Visit(ReplaceMutation<TItem> mutation);
		TResult Visit(DeleteMutation<TItem> mutation);
		TResult Visit(LeaveAsIsMutation<TItem> mutation);
	}
}
