using System;

namespace Examples
{
    /// <summary>
    /// Example calculator class for demonstrating test generation
    /// </summary>
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new ArgumentException("Cannot divide by zero", nameof(b));
            
            return a / b;
        }

        public int Power(int baseNumber, int exponent)
        {
            if (exponent < 0)
                throw new ArgumentException("Exponent must be non-negative", nameof(exponent));
            
            if (exponent == 0)
                return 1;
            
            int result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= baseNumber;
            }
            
            return result;
        }

        public double SquareRoot(double number)
        {
            if (number < 0)
                throw new ArgumentException("Cannot calculate square root of negative number", nameof(number));
            
            return Math.Sqrt(number);
        }

        public bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public int Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Factorial is not defined for negative numbers", nameof(n));
            
            if (n == 0 || n == 1)
                return 1;
            
            int result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            
            return result;
        }
    }
}
