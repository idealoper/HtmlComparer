using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistanceCalculating.Tests
{
	[TestFixture]
	public class EditDistanceCalculatorTests
	{
		[Test]
		public void It_has_default_implementaion_created()
		{
			Assert.That(EditDistanceCalculator<char>.Default, Is.Not.Null);
		}

		[Test]
		[TestCase("", "", ExpectedResult = 0)]
		[TestCase("A", "", ExpectedResult = 1)]
		[TestCase("", "A", ExpectedResult = 1)]
		[TestCase("A", "A", ExpectedResult = 0)]
		[TestCase("Ab", "A", ExpectedResult = 1)]
		[TestCase("A", "Ab", ExpectedResult = 1)]
		[TestCase("bA", "A", ExpectedResult = 1)]
		[TestCase("A", "bA", ExpectedResult = 1)]
		[TestCase("Asd", "fgh", ExpectedResult = 3)]
		[TestCase("AsA", "AgA", ExpectedResult = 1)]
		[TestCase("AsA", "AgA~", ExpectedResult = 2)]
		public int It_correctly_calculate_edit_distance(string actualString, string desiredString)
		{
			var editDistance = EditDistanceCalculator<char>.Default.Get(actualString.ToArray(), desiredString.ToArray());
			Assert.That(editDistance, Is.Not.Null);
			Assert.That(editDistance.Mutations, Is.Not.Null);
			return editDistance.Value;
		}

		[Test]
		[TestCase("", "", ExpectedResult = "")]
		[TestCase("A", "", ExpectedResult = "[d:A]")]
		[TestCase("", "A", ExpectedResult = "[i:A]")]
		[TestCase("A", "A", ExpectedResult = "[l:A]")]
		[TestCase("Ab", "A", ExpectedResult = "[l:A][d:b]")]
		[TestCase("A", "Ab", ExpectedResult = "[l:A][i:b]")]
		[TestCase("bA", "A", ExpectedResult = "[d:b][l:A]")]
		[TestCase("A", "bA", ExpectedResult = "[i:b][l:A]")]
		[TestCase("Asd", "fgh", ExpectedResult = "[r:Af][r:sg][r:dh]")]
		[TestCase("AsA", "AgA", ExpectedResult = "[l:A][r:sg][l:A]")]
		[TestCase("AsA", "AgA~", ExpectedResult = "[l:A][r:sg][l:A][i:~]")]
		public string It_correctly_calculate_mutations(string actualString, string desiredString)
		{
			var editDistance = EditDistanceCalculator<char>.Default.Get(actualString.ToArray(), desiredString.ToArray());
			Assert.That(editDistance, Is.Not.Null);
			Assert.That(editDistance.Mutations, Is.Not.Null);
			return string.Join("", editDistance.Mutations.Select(x => MutationEncoder.Encode(x)));
		}

		class MutationEncoder : IMutationVisitor<string, char>
		{
			private MutationEncoder() { }

			public static string Encode(Mutation<char> mutation)
				=> mutation.Accept(new MutationEncoder());

			public string Visit(InsertMutation<char> mutation)
				=> $"[i:{mutation.Item}]";

			public string Visit(ReplaceMutation<char> mutation)
				=> $"[r:{mutation.Item}{mutation.NewItem}]";

			public string Visit(DeleteMutation<char> mutation)
				=> $"[d:{mutation.Item}]";

			public string Visit(LeaveAsIsMutation<char> mutation)
				=> $"[l:{mutation.Item}]";
		}

		[Test]
		public void It_failed_when_actual_data_null()
		{
			Assert.Throws<ArgumentNullException>(() => 
				EditDistanceCalculator<char>.Default.Get(null, "Test".ToArray()));
		}

		[Test]
		public void It_failed_when_desired_data_null()
		{
			Assert.Throws<ArgumentNullException>(() => 
				EditDistanceCalculator<char>.Default.Get("Test".ToArray(), null));
		}

		[Test]
		public void It_failed_when_insert_cost_less_than_0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => 
				EditDistanceCalculator<char>.Default.Get("Test".ToArray(), "Test".ToArray(), insertCost: -1));
		}

		[Test]
		public void It_failed_when_delete_cost_less_than_0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				EditDistanceCalculator<char>.Default.Get("Test".ToArray(), "Test".ToArray(), deleteCost: -1));
		}

		[Test]
		public void It_failed_when_replace_cost_less_than_0()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				EditDistanceCalculator<char>.Default.Get("Test".ToArray(), "Test".ToArray(), replaceCost: -1));
		}
	}
}
