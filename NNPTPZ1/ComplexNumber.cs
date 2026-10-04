using System;

namespace NNPTPZ1
{

    public class ComplexNumber
    {

        public double RealPart { get; set; }

        public double ImaginaryPart { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ComplexNumber)
            {
                ComplexNumber x = obj as ComplexNumber;
                return x.RealPart == RealPart && x.ImaginaryPart == ImaginaryPart;
            }
            return base.Equals(obj);
        }

        public static readonly ComplexNumber Zero = new ComplexNumber()
        {
            RealPart = 0,
            ImaginaryPart = 0
        };


        public ComplexNumber Multiply(ComplexNumber otherNumber)
        {
            ComplexNumber currentNumber = this;
            return new ComplexNumber()
            {
                RealPart = currentNumber.RealPart * otherNumber.RealPart - currentNumber.ImaginaryPart * otherNumber.ImaginaryPart,
                ImaginaryPart = currentNumber.RealPart * otherNumber.ImaginaryPart + currentNumber.ImaginaryPart * otherNumber.RealPart
            };
        }

        public double GetAbsoluteValue()
        {
            return Math.Sqrt(RealPart * RealPart + ImaginaryPart * ImaginaryPart);
        }

        public ComplexNumber Add(ComplexNumber otherNumber)
        {
            ComplexNumber currentNumber = this;
            return new ComplexNumber()
            {
                RealPart = currentNumber.RealPart + otherNumber.RealPart,
                ImaginaryPart = currentNumber.ImaginaryPart + otherNumber.ImaginaryPart
            };
        }

        public double GetAngleInDegrees()
        {
            return Math.Atan2(ImaginaryPart, RealPart)*180 / Math.PI;
        }

        public ComplexNumber Subtract(ComplexNumber otherNumber)
        {
            ComplexNumber currentNumber = this;
            return new ComplexNumber()
            {
                RealPart = currentNumber.RealPart - otherNumber.RealPart,
                ImaginaryPart = currentNumber.ImaginaryPart - otherNumber.ImaginaryPart
            };
        }

        public override string ToString()
        {
            return $"({RealPart} + {ImaginaryPart}i)";
        }

        internal ComplexNumber Divide(ComplexNumber otherNumber)
        {
            var numerator = this.Multiply(new ComplexNumber() { RealPart = otherNumber.RealPart, ImaginaryPart = -otherNumber.ImaginaryPart });
            var denominator = otherNumber.RealPart * otherNumber.RealPart + otherNumber.ImaginaryPart * otherNumber.ImaginaryPart;

            return new ComplexNumber()
            {
                RealPart = numerator.RealPart / denominator,
                ImaginaryPart = numerator.ImaginaryPart / denominator
            };
        }
    }
}
