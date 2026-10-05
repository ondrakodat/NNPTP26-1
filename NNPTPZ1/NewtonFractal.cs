using System;
using System.Collections.Generic;
using System.Drawing;

namespace NNPTPZ1
{
    public class NewtonFractal
    {
        private const int ExpectedArgumentsCount = 7;
        private const double ReplacementForZero = 0.0001;
        private const double RootTolerance = 0.01;
        private const int MaxIterationsCount = 30;
        private const double NewtonStepLimit = 0.5;
        private const int ColorDarkeningFactor = 2;

        public int AreaWidth { get; set; }
        public int AreaHeight { get; set; }

        public double MinRealAxis { get; set; }
        public double MaxRealAxis { get; set; }

        public double MinImaginaryAxis { get; set; }
        public double MaxImaginaryAxis { get; set; }

        public string OutputFile { get; set; }

        public Bitmap OutputImage { get; set; }

        public bool PrepareEnvironment(string[] args) {

            if (args.Length < ExpectedArgumentsCount) {
                throw new ArgumentException($"Expected {ExpectedArgumentsCount} arguments.");
            }

            try
            {
                AreaWidth = int.Parse(args[0]);
                AreaHeight = int.Parse(args[1]);

                MinRealAxis = double.Parse(args[2]);
                MaxRealAxis = double.Parse(args[3]);

                MinImaginaryAxis = double.Parse(args[4]);
                MaxImaginaryAxis = double.Parse(args[5]);

                OutputFile = args[6];

                return true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Some of input has invalid format");
                return false;
            }
        }


        public void DoCalculation() {
            double[] coefficients = {1, 0, 0, 1 };
            Polynomial polynomial = Polynomial.CreatePolynomialWithCoefficients(coefficients);
            Polynomial polynomialDerivation = polynomial.Derive();

            List<ComplexNumber> polynomialRoots = new List<ComplexNumber>();
            OutputImage = new Bitmap(AreaWidth, AreaHeight);

            var rootColors = new Color[]
            {
                Color.Red, 
                Color.Blue,
                Color.Green, 
                Color.Yellow, 
                Color.Orange,
                Color.Fuchsia, 
                Color.Gold,
                Color.Cyan,
                Color.Magenta
            };
            double xStep = (MaxRealAxis - MinRealAxis) / AreaWidth;
            double yStep = (MaxImaginaryAxis - MinImaginaryAxis) / AreaHeight;

            for (int row = 0; row < AreaHeight; row++)
            {
                for (int column = 0; column < AreaWidth; column++)
                {
                    // find "world" coordinates of pixel
                    double y = MinImaginaryAxis + row * yStep;
                    double x = MinRealAxis + column * xStep;

                    ComplexNumber pointInArea = new ComplexNumber()
                    {
                        RealPart = x,
                        ImaginaryPart = y
                    };

                    if (pointInArea.RealPart == 0)
                        pointInArea.RealPart = ReplacementForZero;
                    if (pointInArea.ImaginaryPart == 0)
                        pointInArea.ImaginaryPart = ReplacementForZero;

                    // find solution of equation using newton'vysledek iteration
                    int iteration = 0;

                    pointInArea = DoNewtonIteration(pointInArea ,polynomial, polynomialDerivation, out iteration);

                    // find solution root number
                    var isKnownRoot = false;
                    var rootNumber = 0;
                    for (int rootIndex = 0; rootIndex < polynomialRoots.Count; rootIndex++)
                    {
                        if (Math.Pow(pointInArea.RealPart - polynomialRoots[rootIndex].RealPart, 2) + Math.Pow(pointInArea.ImaginaryPart - polynomialRoots[rootIndex].ImaginaryPart, 2) <= RootTolerance)
                        {
                            isKnownRoot = true;
                            rootNumber = rootIndex;
                        }
                    }
                    if (!isKnownRoot)
                    {
                        polynomialRoots.Add(pointInArea);
                        rootNumber = polynomialRoots.Count -1;
                    }
                    ColorizePixel(OutputImage, row, column, rootColors, iteration, rootNumber);
                }
            }
        }

        private void ColorizePixel(Bitmap outputImage, int i, int j, Color[] colors, int iterations, int rootIndex ) {
            Color color = colors[rootIndex % colors.Length];

            color = Color.FromArgb(
                Math.Min(Math.Max(0, color.R - iterations * ColorDarkeningFactor), 255),
                Math.Min(Math.Max(0, color.G - iterations * ColorDarkeningFactor), 255),
                Math.Min(Math.Max(0, color.B - iterations * ColorDarkeningFactor), 255)
            );

            outputImage.SetPixel(j, i, color);
        }

        private ComplexNumber DoNewtonIteration(ComplexNumber pointInArea, Polynomial polynomial, Polynomial polynomialDerivation, out int iterations) {

            iterations = 0;
            for (int iterationNumber = 0; iterationNumber < MaxIterationsCount; iterationNumber++)
            {
                var stepOfNewtonIteration = polynomial.CalculateValue(pointInArea).Divide(polynomialDerivation.CalculateValue(pointInArea));
                pointInArea = pointInArea.Subtract(stepOfNewtonIteration);

                if (Math.Pow(stepOfNewtonIteration.RealPart, 2) + Math.Pow(stepOfNewtonIteration.ImaginaryPart, 2) >= NewtonStepLimit)
                {
                    iterationNumber--;
                }
                iterations++;
            }

            return pointInArea;
        }

        public void SaveOutput() {
            OutputImage.Save(OutputFile ?? "../../../out.png");
            OutputImage.Dispose();
        }
        
    }
}
