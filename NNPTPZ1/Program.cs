using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Threading;
//using NNPTPZ1.Mathematics;

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
            Polynom polynom = Polynom.PridaniKoeficientu(1, 0, 0, 1);
            Polynom polynomDerivace = polynom.Derivuj();
            NewtonuvFraktal fraktal = new NewtonuvFraktal();
            fraktal.PripravProstredi(args);
            fraktal.ProvedVypocet();
            fraktal.UlozVysledek();
        } 
    }
}
