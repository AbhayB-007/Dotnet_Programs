using TestProject;

namespace TestProjectNUTest
{
    public class Tests
    {
        [Test]
        [TestCase(5, 5, ExpectedResult = 5 + 5)]
        [TestCase(50, 34, ExpectedResult = 50 + 34)]
        [TestCase(23, 42, ExpectedResult = 23 + 42)]
        public int AddNumbers_InputTwoInt_GetCorrectAddition(int a, int b)
        {
            // Arrange 
            Calculator calc = new();

            // Act 
            int result = calc.AddNumbers(a, b);

            // Assert
            //Assert.That(result, Is.EqualTo(10));
            return result;
        }

        [Test]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(16, ExpectedResult = false)]
        [TestCase(8, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = false)]
        public bool IsOddChecker_InputEvenNumber_ReturnFalse(int a)
        {
            Calculator calc = new();           
            return calc.IsOddNumber(a);
        }
        
        [Test]
        [TestCase(9, ExpectedResult = true)]
        [TestCase(13, ExpectedResult = true)]
        [TestCase(15, ExpectedResult = true)]
        [TestCase(19, ExpectedResult = true)]
        public bool IsOddChecker_InputOddNumber_ReturnTrue(int a)
        {
            Calculator calc = new();            
            return calc.IsOddNumber(a);
        }

        [Test]
        [TestCase(9.1, 8.9)]
        [TestCase(5.43, 10.45)]
        [TestCase(15.545, 45.64)]        
        public void AddNumbersDouble_InputTwoDouble_GetCorrectAddition(double a, double b)
        {
            Calculator calc = new();               
            var result = calc.AddDouble(a, b);
            Assert.That(result, Is.EqualTo(a + b));
        }
    }
}