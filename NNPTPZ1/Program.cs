namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            NewtonFractal fraktal = new NewtonFractal();
            fraktal.PrepareEnvironment(args);
            fraktal.DoCalculation();
            fraktal.SaveOutput();
        } 
    }
}
