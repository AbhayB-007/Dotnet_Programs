using TestProject;

namespace TestProjectMSTest
{
    [TestClass]
    public class CalculatorMSTest
    {

        [TestMethod]
        public void AddNumbers_InputTwoInt_GetCorrectAddition()
        {
            // Arrange 
            Calculator calc = new();

            // Act 
            int result = calc.AddNumbers(5, 5);

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void IsOddChecker_InputEvenNumber_ReturnFalse()
        {
            Calculator calc = new();
            bool isOdd = calc.IsOddNumber(10);
            Assert.IsFalse(isOdd);
        }

        [TestMethod]
        public void IsOddChecker_InputOddNumber_ReturnTrue()
        {
            Calculator calc = new();
            bool isOdd = calc.IsOddNumber(9);
            Assert.IsTrue(isOdd);
        }
    }
}