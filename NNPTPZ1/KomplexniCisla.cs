using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1
{
    public class KomplexniCisla
    {
        public double RealnaCast { get; set; }
        public double ImaginarniCast { get; set; }

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
                ImaginarniCast = a.RealnaCast * b.ImaginarniCast + a.ImaginarniCast * b.RealnaCast
            };
        }
        public double DejAbsolutniHodnotu()
        {
            return Math.Sqrt(RealnaCast * RealnaCast + ImaginarniCast * ImaginarniCast);
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
                ImaginarniCast = citatel.ImaginarniCast / jmenovatel
            };
        }
    }
}
