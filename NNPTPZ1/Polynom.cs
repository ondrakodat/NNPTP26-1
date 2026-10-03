using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1
{
    public class Polynom
    {
        public List<KomplexniCisla> Koeficienty { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Polynom() => Koeficienty = new List<KomplexniCisla>();

        public static Polynom PolynomialCoeficientAddition(double koeficient1, double koeficient2, double koeficient3, double koeficient4)
        {
            Polynom polynom = new Polynom();
            polynom.Add(new KomplexniCisla() { RealnaCast = koeficient1 });
            polynom.Add(new KomplexniCisla() { RealnaCast = koeficient2 });
            polynom.Add(new KomplexniCisla() { RealnaCast = koeficient3 });
            polynom.Add(new KomplexniCisla() { RealnaCast = koeficient4 });
            return polynom;
        }

        public void Add(KomplexniCisla koeficient) =>
            Koeficienty.Add(koeficient);

        /// <summary>
        /// Derives this polynom and creates new one
        /// </summary>
        /// <returns>Derivated polynom</returns>
        public Polynom Derivuj()
        {
            Polynom polynom = new Polynom();
            for (int q = 1; q < Koeficienty.Count; q++)
            {
                polynom.Koeficienty.Add(Koeficienty[q].Vynasob(new KomplexniCisla() { RealnaCast = q }));
            }

            return polynom;
        }

        /// <summary>
        /// Evaluates polynom at given point
        /// </summary>
        /// <param name="x">point of evaluation</param>
        /// <returns>y</returns>
        public KomplexniCisla Eval(double x)
        {
            var y = VypocitelHodnotu(new KomplexniCisla() { RealnaCast = x, ImaginarniCast = 0 });
            return y;
        }

        /// <summary>
        /// Evaluates polynom at given point
        /// </summary>
        /// <param name="x">point of evaluation</param>
        /// <returns>y</returns>
        public KomplexniCisla VypocitelHodnotu(KomplexniCisla x)
        {
            KomplexniCisla s = KomplexniCisla.Zero;
            for (int i = 0; i < Koeficienty.Count; i++)
            {
                KomplexniCisla koeficient = Koeficienty[i];
                KomplexniCisla mocnina = x;
                int exponent = i;

                if (i > 0)
                {
                    for (int j = 0; j < exponent - 1; j++)
                        mocnina = mocnina.Vynasob(x);

                    koeficient = koeficient.Vynasob(mocnina);
                }

                s = s.Add(koeficient);
            }

            return s;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String repr of polynom</returns>
        public override string ToString()
        {
            string vysledek = "";
            int i = 0;
            for (; i < Koeficienty.Count; i++)
            {
                vysledek += Koeficienty[i];
                if (i > 0)
                {
                    int j = 0;
                    for (; j < i; j++)
                    {
                        vysledek += "x";
                    }
                }
                if (i + 1 < Koeficienty.Count)
                    vysledek += " + ";
            }
            return vysledek;
        }
    }
}
