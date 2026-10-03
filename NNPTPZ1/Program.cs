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
    ///
    /// Na začátku vytvoříme 2D plochu pro reálná a komplexní čísla pomocí 2 rozměrného pole "rozmeryVyslednehoObrazu"
    /// Poté dosadíme hranice pro obě osy dolní a horní hranici imaginární osy a dolní a horní hranici reálné osy
    /// "Im∈[−2,2]" a "RealnaCast∈[−2,2]"
    /// Vytvoříme si polynom pro Newtonovu funkci 
    /// Jako první výchozí bod pro Newtonovu funkci nám slouží -2 na Im a -2 na RealnaCast
    /// Od tohoto bodu počítáme funkci a dostáváme se do kořenů
    /// Pixel z bodu -2 -2 tedy poté dostane barvu daného kořene do kterého došel 
    /// Poté vezmeme další bod a opět spočítáme do kterého kořene jsme z něj došli a takto projdeme celou plochu
    /// A jak výpočet probíhá?
    /// Mějme polynom polynom(x) = 3x^3 + 1
    /// Z něho uděláme derivaci tedy 9x^2
    /// Proč derivace? -Netwonova metoda používá polynom i jeho derivaci zároveň 
    /// Vzorec: zn+1​=zn​− {polynom(zn​)​ /polynom′(zn​)}
    /// Jak poté poznáme, že jsme u kořene? 
    /// Výsledky newtonovy funkce dosazujeme zpět do původního polynomu a když je polynom roven 0 máme kořen
    class Program
    {
        static void Main(string[] args)
        {

            int[] rozmeryVyslednehoObrazu = new int[2];
            for (int i = 0; i < rozmeryVyslednehoObrazu.Length; i++)
            {
                rozmeryVyslednehoObrazu[i] = int.Parse(args[i]);
            }
            double[] hraniceKomplexniRoviny = new double[4];
            for (int i = 0; i < hraniceKomplexniRoviny.Length; i++)
            {
                hraniceKomplexniRoviny[i] = double.Parse(args[i + 2]);
            }
            string output = args[6];
            // TODO: add parameters from args?
            /*
             * Toto představuje plochu "Im∈[−2,2]" a "RealnaCast∈[−2,2]"
             */
            Bitmap outputPicture = new Bitmap(rozmeryVyslednehoObrazu[0], rozmeryVyslednehoObrazu[1]);

            /*
                Nastavení hranic podle vstupů 
                Ty nám říkají jakou část roviny budeme zkoumat
             */

            double levaHraniceKomplexniRoviny = hraniceKomplexniRoviny[0];
            double pravaHraniceKomplexniRoviny = hraniceKomplexniRoviny[1];
            double dolniHraniceKomplexniRoviny = hraniceKomplexniRoviny[2];
            double horniHraniceKomplexniRoviny = hraniceKomplexniRoviny[3];

            /*
                Převod pixelu na komplexní čísla 
                1 krok nám určuje o kolik se posuneme v naší komplexní rovině pokud se posuneme o 1 pixel 
                Máme reálnou a imaginární komplexní rovinu dá se uchopit jako klasický graf vysledek osami y kde y je imaginární a x je reálná
                na ose reálné máme čísla reálná na imaginární máme komplexní 
                tedy např. 
                    z = 1,5 + 2i
                    1,5 bude na reálné ose a 2i na ose imaginární    
             */
            double xKrok = (pravaHraniceKomplexniRoviny - levaHraniceKomplexniRoviny) / rozmeryVyslednehoObrazu[0];
            double yKrok = (horniHraniceKomplexniRoviny - dolniHraniceKomplexniRoviny) / rozmeryVyslednehoObrazu[1];

            List<KomplexniCisla> korenyPolynomu = new List<KomplexniCisla>(); //KomplexniCisla - Komplexní číslo // tedy list Komplexnich cisel 
            // TODO: poly should be parameterised?
            /*
             Vytvoření polynomu pro Newtonovu metodu tedy máme polynom polynom(x) = 3x^3 + 1
             Spočítáme pro něj kořeny jelikož máme kubický polynom máme 3 kořeny
             
             */
            Polynom polynom = new Polynom();
            polynom.Koeficienty.Add(new KomplexniCisla() { RealnaCast = 1 });
            polynom.Koeficienty.Add(KomplexniCisla.Zero);
            polynom.Koeficienty.Add(KomplexniCisla.Zero);
            //polynom.Koeficienty.Add(KomplexniCisla.Zero);
            polynom.Koeficienty.Add(new KomplexniCisla() { RealnaCast = 1 });
            //Polynom ptmp = polynom;
            Polynom derivacePolynomu = polynom.Derivuj();

            Console.WriteLine(polynom);
            Console.WriteLine(derivacePolynomu);

            var barvy = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var pocetKorenuCelkem = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < rozmeryVyslednehoObrazu[0]; i++)
            {
                for (int j = 0; j < rozmeryVyslednehoObrazu[1]; j++)
                {
                    // find "world" coordinates of pixel
                    double y = dolniHraniceKomplexniRoviny + i * yKrok;
                    double x = levaHraniceKomplexniRoviny + j * xKrok;

                    KomplexniCisla bodVProstoru = new KomplexniCisla()
                    {
                        RealnaCast = x,
                        ImaginarniCast = y
                    };

                    if (bodVProstoru.RealnaCast == 0)
                        bodVProstoru.RealnaCast = 0.0001;
                    if (bodVProstoru.ImaginarniCast == 0)
                        bodVProstoru.ImaginarniCast = 0.0001f;

                    //Console.WriteLine(bodVProstoru);

                    // find solution of equation using newton'vysledek iteration
                    float iterace = 0;
                    for (int cisloIterace = 0; cisloIterace< 30; cisloIterace++)
                    {
                        var krokNewtonovyFunkce  = polynom.VypocitelHodnotu(bodVProstoru).Vydel(derivacePolynomu.VypocitelHodnotu(bodVProstoru));
                        bodVProstoru = bodVProstoru.Odecti(krokNewtonovyFunkce);

                        //Console.WriteLine($"{cisloIterace} {bodVProstoru} -({krokNewtonovyFunkce})");
                        if (Math.Pow(krokNewtonovyFunkce.RealnaCast, 2) + Math.Pow(krokNewtonovyFunkce.ImaginarniCast, 2) >= 0.5)
                        {
                            cisloIterace--;
                        }
                        iterace++;
                    }

                    //Console.ReadKey();

                    // find solution root number
                    var jeZnamyKoren = false;
                    var cisloKorene = 0;
                    for (int w = 0; w <korenyPolynomu.Count;w++)
                    {
                        // Pokud jsme z bodu nedošli ke kořeni ale máme např. malou odchylku tedy 0.01
                        if (Math.Pow(bodVProstoru.RealnaCast- korenyPolynomu[w].RealnaCast, 2) + Math.Pow(bodVProstoru.ImaginarniCast - korenyPolynomu[w].ImaginarniCast, 2) <= 0.01)
                        {
                            jeZnamyKoren = true;
                            cisloKorene = w;
                        }
                    }
                    if (!jeZnamyKoren)
                    {
                        korenyPolynomu.Add(bodVProstoru);
                        cisloKorene = korenyPolynomu.Count;
                        pocetKorenuCelkem = cisloKorene + 1; 
                    }

                    // colorize pixel according to root number
                    //int vv = cisloKorene;
                    //int vv = cisloKorene * 50 + (int)iterace*5;
                    var vv = barvy[cisloKorene % barvy.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R-(int)iterace*2), 255), Math.Min(Math.Max(0, vv.G - (int)iterace*2), 255), Math.Min(Math.Max(0, vv.B - (int)iterace*2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    outputPicture.SetPixel(j, i, vv);
                    //image.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }

            // TODO: delete I suppose...
            //for (int i = 0; i < 300; i++)
            //{
            //    for (int j = 0; j < 300; j++)
            //    {
            //        Color c = image.GetPixel(j, i);
            //        int nv = (int)Math.Floor(c.R * (255.0 / pocetKorenuCelkem));
            //        image.SetPixel(j, i, Color.FromArgb(nv, nv, nv));
            //    }
            //}

            outputPicture.Save(output ?? "../../../out.png");
            //Console.ReadKey();
        }
    }
    /*
    namespace Mathematics
    {
        
        public class Polynom
        {
            

            /// <summary>
            /// Koeficienty
            /// </summary>
            public List<KomplexniCisla> Koeficienty { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public Polynom() => Koeficienty = new List<KomplexniCisla>();

            public void Add(KomplexniCisla koeficient) =>
                Koeficienty.Add(koeficient);

            /// <summary>
            /// Derives this polynomial and creates new one
            /// </summary>
            /// <returns>Derivated polynomial</returns>
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
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public KomplexniCisla Eval(double x)
            {
                var y = VypocitelHodnotu(new KomplexniCisla() { RealnaCast = x, ImaginarniCast = 0 });
                return y;
            }

            /// <summary>
            /// Evaluates polynomial at given point
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
            /// <returns>String repr of polynomial</returns>
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
                    if (i+1<Koeficienty.Count)
                    vysledek += " + ";
                }
                return vysledek;
            }
        
        }

    
        public class KomplexniCisla
        {
            public double RealnaCast { get; set; }
            public float ImaginarniCast { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is KomplexniCisla)
                {
                    KomplexniCisla x = obj as KomplexniCisla;
                    return x.RealnaCast == RealnaCast && x.ImaginarniCast == ImaginarniCast;
                }
                return base.Equals(obj);
            }

            public readonly static KomplexniCisla Zero = new KomplexniCisla()
            {
                RealnaCast = 0,
                ImaginarniCast = 0
            };

            public KomplexniCisla Vynasob(KomplexniCisla b)
            {
                KomplexniCisla a = this;
                // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
                return new KomplexniCisla()
                {
                    RealnaCast = a.RealnaCast * b.RealnaCast - a.ImaginarniCast * b.ImaginarniCast,
                    ImaginarniCast = (float)(a.RealnaCast * b.ImaginarniCast + a.ImaginarniCast * b.RealnaCast)
                };
            }
            public double DejAbsolutniHodnotu()
            {
                return Math.Sqrt( RealnaCast * RealnaCast + ImaginarniCast * ImaginarniCast);
            }

            public KomplexniCisla Add(KomplexniCisla b)
            {
                KomplexniCisla a = this;
                return new KomplexniCisla()
                {
                    RealnaCast = a.RealnaCast + b.RealnaCast,
                    ImaginarniCast = a.ImaginarniCast + b.ImaginarniCast
                };
            }
            public double DejUhelVeStupnich()
            {
                return Math.Atan(ImaginarniCast / RealnaCast);
            }
            public KomplexniCisla Odecti(KomplexniCisla b)
            {
                KomplexniCisla a = this;
                return new KomplexniCisla()
                {
                    RealnaCast = a.RealnaCast - b.RealnaCast,
                    ImaginarniCast = a.ImaginarniCast - b.ImaginarniCast
                };
            }

            public override string ToString()
            {
                return $"({RealnaCast} + {ImaginarniCast}i)";
            }

            internal KomplexniCisla Vydel(KomplexniCisla b)
            {
                // (aRe + aIm*i) / (bRe + bIm*i)
                // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
                //  bRe*bRe - bIm*bIm*i*i
                var citatel = this.Vynasob(new KomplexniCisla() { RealnaCast = b.RealnaCast, ImaginarniCast = -b.ImaginarniCast });
                var jmenovatel = b.RealnaCast * b.RealnaCast + b.ImaginarniCast * b.ImaginarniCast;

                return new KomplexniCisla()
                {
                    RealnaCast = citatel.RealnaCast / jmenovatel,
                    ImaginarniCast = (float)(citatel.ImaginarniCast / jmenovatel)
                };
            }
        
        }
        
    }

    */
}
