using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            
            // Act
            var result = calculator.Add(2, 3);
            
            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Subtract_TwoNumbers_ReturnsCorrectDifference()
        {
            // Arrange
            var calculator = new Calculator();
            
            // Act
            var result = calculator.Subtract(5, 3);
            
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }
    }

    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
    }
} 