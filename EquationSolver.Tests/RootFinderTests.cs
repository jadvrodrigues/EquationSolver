using System.Numerics;

namespace EquationSolver.Tests
{
    [TestClass]
    public class RootFinderTests
    {
        const double AssertDelta = 0.00000001;

        static Complex Real(double real) => new(real, 0.0);
        static Complex Complex(double real, double imaginary) => new(real, imaginary);

        static void ComplexArrayAssert_AreEqual(Complex[] expected, Complex[] actual, double delta = AssertDelta)
        {
            Assert.IsTrue(expected.Length == actual.Length);

            for (int i = expected.Length - 1; i >= 0; i--)
            {
                Complex expectedElement = expected[i];
                bool equalElementFound = false;

                for (int j = 0; j <= i; j++)
                {
                    Complex actualElement = actual[j];

                    if (Math.Abs(expectedElement.Real - actualElement.Real) <= delta &&
                        Math.Abs(expectedElement.Imaginary - actualElement.Imaginary) <= delta)
                    {
                        equalElementFound = true;
                        (actual[j], actual[i]) = (actual[i], actual[j]);
                        break;
                    }
                }

                if (!equalElementFound) throw new AssertFailedException();
            }
        }

        #region Linear
        [TestMethod]
        public void Linear_NonZeroNumber_ReturnsNegativeCounterpart()
        {
            (double a, double result)[] solutions = new[]
            {
                (0.0, 0.0),
                (1.0, -1.0),
                (-1.0, 1.0)
            };

            foreach ((double a, double result) in solutions)
            {
                Assert.AreEqual(RootFinder.Linear(a), result);
            }
        }

        [TestMethod]
        public void Linear_ZeroNumber_ReturnsZero()
        {
            Assert.AreEqual(RootFinder.Linear(0.0), 0.0);
        }
        #endregion

        #region Quadratic

        void AssertQuadratic(params (double a, double b, (Complex, Complex) expected)[] solutions)
        {
            foreach ((double a, double b, (Complex, Complex) expected) in solutions)
            {
                var actual = RootFinder.Quadratic(a, b);

                Complex[] actualArray = new Complex[] { actual.Item1, actual.Item2 };
                Complex[] expectedArray = new Complex[] { expected.Item1, expected.Item2 };

                ComplexArrayAssert_AreEqual(expectedArray, actualArray);
            }
        }

        [TestMethod]
        public void Quadratic_TwoComplexRoots()
        {
            AssertQuadratic(new[]
            {
                (2.0, 2.0, (Complex(-1.0, 1.0), Complex(-1.0, -1.0))),
                (3.0, 10.0, (Complex(-1.5, -Math.Sqrt(31)/2), Complex(-1.5, Math.Sqrt(31)/2)))
            });
        }

        [TestMethod]
        public void Quadratic_OneRealRationalRoot()
        {
            AssertQuadratic(new[]
            {
                (0.0, 0.0, (Real(0.0), Real(0.0))),
                (4.0, 4.0, (Real(-2.0), Real(-2.0))),
                (3.0, 2.25, (Real(-1.5), Real(-1.5)))
            });
        }

        [TestMethod]
        public void Quadratic_TwoRealRationalRoots()
        {
            AssertQuadratic(new[]
            {
                (2.0, -3.0, (Real(-3.0), Real(1.0)))
            });
        }

        [TestMethod]
        public void Quadratic_TwoRealIrrationalRoots()
        {
            AssertQuadratic(new[]
            {
                (-4.0, -20.0, (Real(2.0 + 2.0 * Math.Sqrt(6)), Real(2.0 - 2.0 * Math.Sqrt(6))))
            });
        }
        #endregion

        #region Cubic
        void AssertCubic(params (double a, double b, double c, (Complex, Complex, Complex) expected)[] solutions)
        {
            foreach ((double a, double b, double c, (Complex, Complex, Complex) expected) in solutions)
            {
                var actual = RootFinder.Cubic(a, b, c);

                Complex[] actualArray = new Complex[] { actual.Item1, actual.Item2, actual.Item3 };
                Complex[] expectedArray = new Complex[] { expected.Item1, expected.Item2, expected.Item3 };

                ComplexArrayAssert_AreEqual(expectedArray, actualArray);
            }
        }

        [TestMethod]
        public void Cubic_OneRealRoot()
        {
            AssertCubic(new[]
            {
                (-9.0, 27.0, -27.0, (Real(3.0), Real(3.0), Real(3.0)))
            });
        }

        [TestMethod]
        public void Cubic_TwoRealRoots()
        {
            AssertCubic(new[]
            {
                (-4.0, 5.0, -2.0, (Real(1.0), Real(1.0), Real(2.0)))
            });
        }

        [TestMethod]
        public void Cubic_ThreeRealRoots()
        {
            AssertCubic(new[]
            {
                (0.0, 0.0, 0.0, (Real(0.0), Real(0.0), Real(0.0))),
                (-6.0, 11.0, -6.0, (Real(1.0), Real(2.0), Real(3.0))),
                (-4.0 - Math.Sqrt(2.0), 3.0 + 4.0 * Math.Sqrt(2.0), - 3.0 * Math.Sqrt(2), (Real(Math.Sqrt(2)), Real(1), Real(3)))
            });
        }

        [TestMethod]
        public void Cubic_ThreeRealRoots_ZeroDeltaZeroAndDeltaOne()
        {
            AssertCubic(new[]
            {
                (3.0, 3.0, 1.0, (Real(-1.0), Real(-1.0), Real(-1.0)))
            });
        }

        [TestMethod]
        public void Cubic_OneRealAndTwoComplexRoots()
        {
            AssertCubic(new[]
            {
                (-4.0, 1.0, 26.0, (Real(-2.0), Complex(3.0, 2.0), Complex(3.0, -2.0))),
            });
        }

        [TestMethod]
        public void Cubic_OneRealAndTwoComplexRoots_ZeroDeltaZero()
        {
            AssertCubic(new[]
            {
                (3.0, 3.0, 0.0, (Real(0.0), Complex(-1.5, -Math.Sqrt(3) / 2.0), Complex(-1.5, Math.Sqrt(3) / 2.0))),
            });
        }
        #endregion

        #region Quartic
        void AssertQuartic(params (double a, double b, double c, double d, (Complex, Complex, Complex, Complex) expected)[] solutions)
        {
            foreach ((double a, double b, double c, double d, (Complex, Complex, Complex, Complex) expected) in solutions)
            {
                var actual = RootFinder.Quartic(a, b, c, d);

                Complex[] actualArray = new Complex[] { actual.Item1, actual.Item2, actual.Item3, actual.Item4 };
                Complex[] expectedArray = new Complex[] { expected.Item1, expected.Item2, expected.Item3, expected.Item4 };

                ComplexArrayAssert_AreEqual(expectedArray, actualArray);
            }
        }

        [TestMethod]
        public void Quartic_FourRealRoots()
        {
            AssertQuartic(new[]
            {
                (2.0, -41.0, -42.0, 360.0, (Real(-6.0), Real(-4.0), Real(3.0), Real(5.0))),
                (0.0, -5.0, 0.0, 4.0, (Real(-2.0), Real(-1.0), Real(1.0), Real(2.0))),
                (0.0, -15000.0, 0.0, 100.0, (Real(-Math.Sqrt(7500.0 - 10 * Math.Sqrt(562499.0))), Real(Math.Sqrt(7500.0 - 10 * Math.Sqrt(562499.0))), Real(-Math.Sqrt(10.0 * (750.0 + Math.Sqrt(562499.0)))), Real(Math.Sqrt(10.0 * (750.0 + Math.Sqrt(562499.0)))))),
                (0.0, -28800.0, 0.0, 140.0, (Real(-Math.Sqrt((144000.0+Math.Sqrt(144000.0*144000.0-14000.0))/10.0)), Real(Math.Sqrt((144000.0+Math.Sqrt(144000.0*144000.0-14000.0))/10.0)), Real(-Math.Sqrt((144000.0-Math.Sqrt(144000.0*144000.0-14000.0))/10.0)), Real(Math.Sqrt((144000.0-Math.Sqrt(144000.0*144000.0-14000.0))/10.0))))
            });
        }

        [TestMethod]
        public void Quartic_ThreeRealRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, -1.0, 0.0, 0.0, (Real(-1.0), Real(0.0), Real(0.0), Real(1.0)))
            });
        }

        [TestMethod]
        public void Quartic_TwoRealRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, -2.0, 0.0, 1.0, (Real(-1.0), Real(-1.0), Real(1.0), Real(1.0))),
                (-1.0, 0.0, 0.0, 0.0, (Real(0.0), Real(0.0), Real(0.0), Real(1.0)))
            });
        }

        [TestMethod]
        public void Quartic_OneRealRoot()
        {
            AssertQuartic(new[]
            {
                (0.0, 0.0, 0.0, 0.0, (Real(0.0), Real(0.0), Real(0.0), Real(0.0))),
                (-4.0, 6.0, -4.0, 1.0, (Real(1.0), Real(1.0), Real(1.0), Real(1.0)))
            });
        }

        [TestMethod]
        public void Quartic_TwoRealAndTwoComplexRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, 0.0, 0.0, -1.0, (Real(-1.0), Real(1.0), Complex(0.0, -1.0), Complex(0.0, 1.0))),
                (-0.25, -0.85, 1.45, -4.35, (Real(-1.6820039265853495), Real(1.4875831103369117), Complex(0.22221040812421897, -1.2996721990882234), Complex(0.22221040812421897, 1.2996721990882234)))
            });
        }

        [TestMethod]
        public void Quartic_OneRealAndTwoComplexRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, 1.0, 0.0, 0.0, (Real(0.0), Real(0.0), Complex(0.0, -1.0), Complex(0.0, 1.0)))
            });
        }

        [TestMethod]
        public void Quartic_TwoComplexRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, 2.0, 0.0, 1.0, (Complex(0.0, -1.0), Complex(0.0, -1.0), Complex(0.0, 1.0), Complex(0.0, 1.0)))
            });
        }

        [TestMethod]
        public void Quartic_FourComplexRoots()
        {
            AssertQuartic(new[]
            {
                (0.0, 5.0, 0.0, 4.0, (Complex(0.0, -2.0), Complex(0.0, -1.0), Complex(0.0, 1.0), Complex(0.0, 2.0))),
                (2.0, 3.0, 4.0, 5.0, (Complex(-1.287815479557648, -0.85789675832849), Complex(-1.287815479557648, 0.85789675832849), Complex(0.28781547955764797, -1.416093080171908), Complex(0.28781547955764797, 1.416093080171908)))
            });
        }

        [TestMethod]
        public void Quartic_FourComplexRoots_ZeroFAndG()
        {
            AssertQuartic(new[]
            {
                (1.0, 0.375, 0.0625, 1.0, (Complex(0.45641523338506673, -0.7064152333850676), Complex(0.45641523338506673, 0.7064152333850676), Complex(-0.9564152333850667, -0.706415233385066), Complex(-0.9564152333850667, 0.706415233385066)))
            });
        }
        #endregion
    }
}