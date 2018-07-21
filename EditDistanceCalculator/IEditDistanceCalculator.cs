using System;

namespace EditDistanceCalculating
{
	public interface IEditDistanceCalculator<TItem> where TItem : IEquatable<TItem>
	{
		EditDistance<TItem> Get(TItem[] actualData, TItem[] desiredData, int insertCost = 1, int deleteCost = 1, int replaceCost = 1);
	}
}