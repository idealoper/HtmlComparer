using System;
using System.Linq;
using System.Collections.Generic;

namespace EditDistanceCalculating
{
	public class EditDistanceCalculator<TItem> : IEditDistanceCalculator<TItem>
		where TItem : IEquatable<TItem>
	{
		private const int MATCH_COST = 0;

		public static IEditDistanceCalculator<TItem> Default { get; internal set; }
			= new EditDistanceCalculator<TItem>();

		internal EditDistanceCalculator() { }

		public EditDistance<TItem> Get(TItem[] actualData, TItem[] desiredData, int insertCost = 1, int deleteCost = 1, int replaceCost = 1)
		{
			if (actualData == null)
				throw new ArgumentNullException(nameof(actualData));
			if (desiredData == null)
				throw new ArgumentNullException(nameof(desiredData));
			if (insertCost < 0)
				throw new ArgumentOutOfRangeException(nameof(insertCost));
			if (deleteCost < 0)
				throw new ArgumentOutOfRangeException(nameof(deleteCost));
			if (replaceCost < 0)
				throw new ArgumentOutOfRangeException(nameof(replaceCost));

			var cells = new EditDistanceCalculatorCell[actualData.Length + 1, desiredData.Length + 1];
			Init(actualData, desiredData, cells);

			var costs = new int[3];

			for (int actualDataDimIndex = 1; actualDataDimIndex < cells.GetLength(0); actualDataDimIndex++)
				for (int desiredDataDimIndex = 1; desiredDataDimIndex < cells.GetLength(1); desiredDataDimIndex++)
				{
					var desiredItem = actualData[actualDataDimIndex - 1];
					var actualItem = desiredData[desiredDataDimIndex - 1];

					costs[0] = cells[actualDataDimIndex - 1, desiredDataDimIndex - 1].Cost + (!desiredItem.Equals(actualItem) ? replaceCost : MATCH_COST);
					costs[1] = cells[actualDataDimIndex, desiredDataDimIndex - 1].Cost + insertCost;
					costs[2] = cells[actualDataDimIndex - 1, desiredDataDimIndex].Cost + deleteCost;

					GetMinCostInfo(costs, out var minCost, out var minCostIndex);

					var previousCell = cells[actualDataDimIndex - 1, desiredDataDimIndex - 1];
					Mutation<TItem> mutation = null;
					switch (minCostIndex)
					{
						case 0:
							{
								if (minCost == previousCell.Cost)
								{
									mutation = new LeaveAsIsMutation<TItem>(desiredItem);
								}
								else
								{
									mutation = new ReplaceMutation<TItem>(desiredItem, actualItem);
								}
							}
							break;
						case 1:
							mutation = new InsertMutation<TItem>(actualItem);
							break;
						case 2:
							mutation = new DeleteMutation<TItem>(desiredItem);
							break;
						default:
							break;
					}

					cells[actualDataDimIndex, desiredDataDimIndex] = new EditDistanceCalculatorCell(minCost, mutation);
				}

			return GetEditDistance(cells);
		}

		private static void GetMinCostInfo(int[] costs, out int minCost, out int minCostIndex)
		{
			minCost = int.MaxValue;
			minCostIndex = -1;

			for (int k = 0; k < costs.Length; k++)
			{
				if (costs[k] < minCost)
				{
					minCost = costs[k];
					minCostIndex = k;
				}
			}
		}

		private static EditDistance<TItem> GetEditDistance(EditDistanceCalculatorCell[,] cells)
		{
			var resultCell = cells[cells.GetLength(0) - 1, cells.GetLength(1) - 1];

			var editDistanceValue = resultCell.Cost;
			var mutations = GetMutations(cells).ToArray();

			return new EditDistance<TItem>(editDistanceValue, mutations);
		}

		private static IEnumerable<Mutation<TItem>> GetMutations(EditDistanceCalculatorCell[,] cells)
			=> GetBackwordMutations(cells).Reverse();

		private static IEnumerable<Mutation<TItem>> GetBackwordMutations(EditDistanceCalculatorCell[,] cells)
		{
			var currentCellX = cells.GetLength(0) - 1;
			var currentCellY = cells.GetLength(1) - 1;
			var currentCell = cells[currentCellX, currentCellY];
			while (currentCell.PreviousMutation != null)
			{
				yield return currentCell.PreviousMutation;

				if (currentCell.PreviousMutation is LeaveAsIsMutation<TItem> || currentCell.PreviousMutation is ReplaceMutation<TItem>)
				{
					currentCellX -= 1;
					currentCellY -= 1;
				}
				else if (currentCell.PreviousMutation is InsertMutation<TItem>)
				{
					currentCellY -= 1;
				}
				else if (currentCell.PreviousMutation is DeleteMutation<TItem>)
				{
					currentCellX -= 1;
				}
				currentCell = cells[currentCellX, currentCellY];
			}
		}

		private void Init(TItem[] actualData, TItem[] desiredData, EditDistanceCalculatorCell[,] cells)
		{
			InitDimensionOfActualData(actualData, cells);
			InitDimensionOfDesiredData(desiredData, cells);
		}

		private void InitDimensionOfActualData(TItem[] actualData, EditDistanceCalculatorCell[,] cells)
		{
			for (int i = 0; i < cells.GetLength(0); i++)
			{
				if (i > 0)
				{
					cells[i, 0] = new EditDistanceCalculatorCell(i, new DeleteMutation<TItem>(actualData[i - 1]));
				}
				else
				{
					cells[i, 0] = new EditDistanceCalculatorCell(i, null);
				}
			}
		}

		private void InitDimensionOfDesiredData(TItem[] desiredData, EditDistanceCalculatorCell[,] cells)
		{
			for (int i = 0; i < cells.GetLength(1); i++)
			{
				if (i > 0)
				{
					cells[0, i] = new EditDistanceCalculatorCell(i, new InsertMutation<TItem>(desiredData[i - 1]));
				}
				else
				{
					cells[0, i] = new EditDistanceCalculatorCell(i, null);
				}
			}
		}

		class EditDistanceCalculatorCell
		{
			public int Cost { get; }

			public Mutation<TItem> PreviousMutation { get; }

			public EditDistanceCalculatorCell(int cost, Mutation<TItem> previousMutation)
			{
				Cost = cost >= 0 ? cost : throw new ArgumentOutOfRangeException(nameof(cost));
				PreviousMutation = previousMutation;
			}
		}

	}
}
