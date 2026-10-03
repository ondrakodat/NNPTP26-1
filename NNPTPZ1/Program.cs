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

            int sirkaPlochy, vyskaPlochy;
            double minRealnaOsa, maxRealnaOsa, minImaginarniOsa, maxImaginarniOsa;
            String output;
            Polynom derivacePolynomu;

            Polynom polynom = Polynom.PolynomialCoeficientAddition(1,0,0,1);
            Polynom polynomDerivace = polynom.Derivuj();

            NastavProstredi(args,
                            out sirkaPlochy,
                            out vyskaPlochy,
                            out minRealnaOsa,
                            out maxRealnaOsa,
                            out minImaginarniOsa,
                            out maxImaginarniOsa,
                            out output
                            ); 

            Bitmap outputPicture = new Bitmap(sirkaPlochy, vyskaPlochy);

            /*
                Převod pixelu na komplexní čísla 
                1 krok nám určuje o kolik se posuneme v naší komplexní rovině pokud se posuneme o 1 pixel 
                Máme reálnou a imaginární komplexní rovinu dá se uchopit jako klasický graf vysledek osami y kde y je imaginární a x je reálná
                na ose reálné máme čísla reálná na imaginární máme komplexní 
                tedy např. 
                    z = 1,5 + 2i
                    1,5 bude na reálné ose a 2i na ose imaginární    
             */
            double xKrok = (maxRealnaOsa - minRealnaOsa) / sirkaPlochy;
            double yKrok = (maxImaginarniOsa - minImaginarniOsa) / vyskaPlochy;

            List<KomplexniCisla> korenyPolynomu = new List<KomplexniCisla>();

            var barvy = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var pocetKorenuCelkem = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < sirkaPlochy; i++)
            {
                for (int j = 0; j < vyskaPlochy; j++)
                {
                    // find "world" coordinates of pixel
                    double y = minImaginarniOsa + i * yKrok;
                    double x = minRealnaOsa + j * xKrok;

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
                        var krokNewtonovyFunkce  = polynom.VypocitelHodnotu(bodVProstoru).Vydel(polynomDerivace.VypocitelHodnotu(bodVProstoru));
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
            outputPicture.Save(output ?? "../../../out.png");



        }


        private static void NastavProstredi(String[] args, 
            out int sirkaPlochy,
            out int vyskaPlochy,
            out double minRealnaOsa,out double maxRealnaOsa, out double minImaginarniOsa,out double maxImaginarniOsa, out string vystupniSoubor) {
            sirkaPlochy = int.Parse(args[0]);
            vyskaPlochy = int.Parse(args[1]);
            minRealnaOsa = double.Parse(args[2]);
            maxRealnaOsa= double.Parse(args[3]);
            minImaginarniOsa = double.Parse(args[4]);
            maxImaginarniOsa= double.Parse(args[5]);
            vystupniSoubor = args[6];
            
        }

        private static void VypocitejNetwonuvFraktal() { }
        private static void UlozVysledek() { }
    }
}
