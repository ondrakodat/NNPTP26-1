using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1
{
    public class NewtonuvFraktal
    {

        public int SirkaPlochy { get; set; }
        public int VyskaPlochy { get; set; }

        public double MinRealnaOsa { get; set; }
        public double MaxRealnaOsa { get; set; }

        public double MinImaginarniOsa { get; set; }
        public double MaxImaginarniOsa { get; set; }

        public string VystupniSoubor { get; set; }

        public Bitmap VystupniObrazek { get; set; }

        public bool PripravProstredi(string[] args) {
            try
            {
                SirkaPlochy = int.Parse(args[0]);
                VyskaPlochy = int.Parse(args[1]);

                MinRealnaOsa = double.Parse(args[2]);
                MaxRealnaOsa = double.Parse(args[3]);

                MinImaginarniOsa = double.Parse(args[4]);
                MaxImaginarniOsa = double.Parse(args[5]);

                VystupniSoubor = args[6];

                return true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Některý argument má nesprávný formát.");
                return false;
            }
        }


        public void ProvedVypocet() {
            Polynom polynom = Polynom.PridaniKoeficientu(1, 0, 0, 1);
            Polynom polynomDerivace = polynom.Derivuj();

            List<KomplexniCisla> korenyPolynomu = new List<KomplexniCisla>();
            var pocetKorenuCelkem = 0;
            //Bitmap vystupniObrazek = new Bitmap(SirkaPlochy, VyskaPlochy);
            VystupniObrazek = new Bitmap(SirkaPlochy, VyskaPlochy);

            var barvy = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };
            double xKrok = (MaxRealnaOsa - MinRealnaOsa) / SirkaPlochy;
            double yKrok = (MaxImaginarniOsa - MinImaginarniOsa) / VyskaPlochy;

            for (int i = 0; i < SirkaPlochy; i++)
            {
                for (int j = 0; j < VyskaPlochy; j++)
                {
                    // find "world" coordinates of pixel
                    double y = MinImaginarniOsa + i * yKrok;
                    double x = MinRealnaOsa + j * xKrok;

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
                    for (int cisloIterace = 0; cisloIterace < 30; cisloIterace++)
                    {
                        var krokNewtonovyFunkce = polynom.VypocitelHodnotu(bodVProstoru).Vydel(polynomDerivace.VypocitelHodnotu(bodVProstoru));
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
                    for (int w = 0; w < korenyPolynomu.Count; w++)
                    {
                        // Pokud jsme z bodu nedošli ke kořeni ale máme např. malou odchylku tedy 0.01
                        if (Math.Pow(bodVProstoru.RealnaCast - korenyPolynomu[w].RealnaCast, 2) + Math.Pow(bodVProstoru.ImaginarniCast - korenyPolynomu[w].ImaginarniCast, 2) <= 0.01)
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
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - (int)iterace * 2), 255), Math.Min(Math.Max(0, vv.G - (int)iterace * 2), 255), Math.Min(Math.Max(0, vv.B - (int)iterace * 2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    VystupniObrazek.SetPixel(j, i, vv);
                    //image.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }
        }
        public void UlozVysledek() {
            VystupniObrazek.Save(VystupniSoubor ?? "../../../out.png");
        }
        
    }
}
