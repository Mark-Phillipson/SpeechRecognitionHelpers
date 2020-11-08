using ControlWSR.Speech;
using Xunit;



namespace UnitTests
{

	public class WordsToNumbersTest
	{
		[Fact]
		public void TestWithValidInput()
		{
			string stringNumber = "one hundred and eighteen";

			var actual = WordsToNumbers.ConvertToNumbers(stringNumber);
			Assert.Equal(118, actual);

		}
	}
}
