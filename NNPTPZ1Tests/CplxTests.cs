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
                RealneCislo = 10,
                KomplexniCislo = 20
            };
            KomplexniCisla b = new KomplexniCisla()
            {
                RealneCislo = 1,
                KomplexniCislo = 2
            };

            KomplexniCisla actual = a.Add(b);
            KomplexniCisla shouldBe = new KomplexniCisla()
            {
                RealneCislo = 11,
                KomplexniCislo = 22
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
                RealneCislo = 1,
                KomplexniCislo = -1
            };
            b = new KomplexniCisla() { RealneCislo = 0, KomplexniCislo = 0 };
            shouldBe = new KomplexniCisla() { RealneCislo = 1, KomplexniCislo = -1 };
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
            Polynom poly = new Mathematics.Polynom();
            poly.Koeficienty.Add(new KomplexniCisla() { RealneCislo = 1, KomplexniCislo = 0 });
            poly.Koeficienty.Add(new KomplexniCisla() { RealneCislo = 0, KomplexniCislo = 0 });
            poly.Koeficienty.Add(new KomplexniCisla() { RealneCislo = 1, KomplexniCislo = 0 });
            KomplexniCisla result = poly.VypocitelHodnotu(new KomplexniCisla() { RealneCislo = 0, KomplexniCislo = 0 });
            var expected = new KomplexniCisla() { RealneCislo = 1, KomplexniCislo = 0 };
            Assert.AreEqual(expected, result);
            result = poly.VypocitelHodnotu(new KomplexniCisla() { RealneCislo = 1, KomplexniCislo = 0 });
            expected = new KomplexniCisla() { RealneCislo = 2, KomplexniCislo = 0 };
            Assert.AreEqual(expected, result);
            result = poly.VypocitelHodnotu(new KomplexniCisla() { RealneCislo = 2, KomplexniCislo = 0 });
            expected = new KomplexniCisla() { RealneCislo = 5.0000000000, KomplexniCislo = 0 };
            Assert.AreEqual(expected, result);

            var r2 = poly.ToString();
            var e2 = "(1 + 0i) + (0 + 0i)x + (1 + 0i)xx";
            Assert.AreEqual(e2, r2);
        }
    }
}


