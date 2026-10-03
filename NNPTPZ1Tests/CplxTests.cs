using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNPTPZ1;

namespace NNPTPZ1.Mathematics.Tests
{
    [TestClass()]
    public class CplxTests
    {

        [TestMethod()]
        public void AddTest()
        {
            KomplexniCisla a = new KomplexniCisla()
            {
                RealnaCast = 10,
                ImaginarniCast = 20
            };
            KomplexniCisla b = new KomplexniCisla()
            {
                RealnaCast = 1,
                ImaginarniCast = 2
            };

            KomplexniCisla actual = a.Add(b);
            KomplexniCisla shouldBe = new KomplexniCisla()
            {
                RealnaCast = 11,
                ImaginarniCast = 22
            };

            Assert.AreEqual(shouldBe, actual);

            var e2 = "(10 + 20i)";
            var r2 = a.ToString();
            Assert.AreEqual(e2, r2);
            e2 = "(1 + 2i)";
            r2 = b.ToString();
            Assert.AreEqual(e2, r2);

            a = new KomplexniCisla()
            {
                RealnaCast = 1,
                ImaginarniCast = -1
            };
            b = new KomplexniCisla() { RealnaCast = 0, ImaginarniCast = 0 };
            shouldBe = new KomplexniCisla() { RealnaCast = 1, ImaginarniCast = -1 };
            actual = a.Add(b);
            Assert.AreEqual(shouldBe, actual);

            e2 = "(1 + -1i)";
            r2 = a.ToString();
            Assert.AreEqual(e2, r2);

            e2 = "(0 + 0i)";
            r2 = b.ToString();
            Assert.AreEqual(e2, r2);
        }

        [TestMethod()]
        public void AddTestPolynome()
        {
            Polynom poly = new Polynom();
            poly.Koeficienty.Add(new KomplexniCisla() { RealnaCast = 1, ImaginarniCast = 0 });
            poly.Koeficienty.Add(new KomplexniCisla() { RealnaCast = 0, ImaginarniCast = 0 });
            poly.Koeficienty.Add(new KomplexniCisla() { RealnaCast = 1, ImaginarniCast = 0 });
            KomplexniCisla result = poly.VypocitelHodnotu(new KomplexniCisla() { RealnaCast = 0, ImaginarniCast = 0 });
            var expected = new KomplexniCisla() { RealnaCast = 1, ImaginarniCast = 0 };
            Assert.AreEqual(expected, result);
            result = poly.VypocitelHodnotu(new KomplexniCisla() { RealnaCast = 1, ImaginarniCast = 0 });
            expected = new KomplexniCisla() { RealnaCast = 2, ImaginarniCast = 0 };
            Assert.AreEqual(expected, result);
            result = poly.VypocitelHodnotu(new KomplexniCisla() { RealnaCast = 2, ImaginarniCast = 0 });
            expected = new KomplexniCisla() { RealnaCast = 5.0000000000, ImaginarniCast = 0 };
            Assert.AreEqual(expected, result);

            var r2 = poly.ToString();
            var e2 = "(1 + 0i) + (0 + 0i)x + (1 + 0i)xx";
            Assert.AreEqual(e2, r2);
        }
    }
}


