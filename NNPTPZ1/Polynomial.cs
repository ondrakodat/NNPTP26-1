using System.Collections.Generic;

namespace NNPTPZ1
{
    public class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Polynomial() => Coefficients = new List<ComplexNumber>();

        public static Polynomial CreatePolynomialWithCoefficients(double[] coeficients)
        {
            Polynomial polynomial = new Polynomial();

            for (int coefficientIndex = 0; coefficientIndex < coeficients.Length; coefficientIndex++) {
                polynomial.Add(new ComplexNumber { RealPart = coeficients[coefficientIndex] });
            }

            return polynomial;
        }

        public void Add(ComplexNumber coeficient) =>
            Coefficients.Add(coeficient);

        /// <summary>
        /// Derives this polynomial and creates new one
        /// </summary>
        /// <returns>Derivated polynomial</returns>
        public Polynomial Derive()
        {
            Polynomial polynomial = new Polynomial();
            for (int coefficientIndex = 1; coefficientIndex < Coefficients.Count; coefficientIndex++)
            {
                polynomial.Coefficients.Add(Coefficients[coefficientIndex].Multiply(new ComplexNumber() { RealPart = coefficientIndex }));
            }

            return polynomial;
        }

        /// <summary>
        /// Evaluates polynomial at given point
        /// </summary>
        /// <param name="evaluationNumber">point of evaluation</param>
        /// <returns>result</returns>
        public ComplexNumber Evaluate(double evaluationNumber)
        {
            var result = CalculateValue(new ComplexNumber() { RealPart = evaluationNumber, ImaginaryPart = 0 });
            return result;
        }

        /// <summary>
        /// Evaluates polynomial at given point
        /// </summary>
        /// <param name="point">point of evaluation</param>
        /// <returns>result</returns>
        public ComplexNumber CalculateValue(ComplexNumber point)
        {
            ComplexNumber result = ComplexNumber.Zero;
            for (int coefficientIndex = 0; coefficientIndex < Coefficients.Count; coefficientIndex++)
            {
                ComplexNumber coefficient = Coefficients[coefficientIndex];
                ComplexNumber power = point;
                int exponent = coefficientIndex;

                if (coefficientIndex > 0)
                {
                    for (int powerIndex = 0; powerIndex < exponent - 1; powerIndex++)
                        power = power.Multiply(point);

                    coefficient = coefficient.Multiply(power);
                }

                result = result.Add(coefficient);
            }

            return result;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String representation of polynomial</returns>
        public override string ToString()
        {
            string result = "";
            int coefficientIndex = 0;
            for (; coefficientIndex < Coefficients.Count; coefficientIndex++)
            {
                result += Coefficients[coefficientIndex];
                if (coefficientIndex > 0)
                {
                    int powerIndex = 0;
                    for (; powerIndex < coefficientIndex; powerIndex++)
                    {
                        result += "x";
                    }
                }
                if (coefficientIndex + 1 < Coefficients.Count)
                    result += " + ";
            }
            return result;
        }
    }
}
