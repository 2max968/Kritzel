using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineLib
{
    public class MatrixDimensionException : Exception
    {
        public MatrixNxM Matrix { get; private set; }

        public MatrixDimensionException(MatrixNxM m, string message)
            : base(message)
        {
            this.Matrix = m;
        }
    }

    public class MatrixDimensionExceptionNonSquare : MatrixDimensionException
    {
        public MatrixDimensionExceptionNonSquare(MatrixNxM m)
            : base(m, "Expected Square Matrix")
        {

        }
    }

    public class MatrixDimensionMismatchException : MatrixDimensionException
    {
        public MatrixNxM MatrixA { get; private set; }
        public MatrixNxM MatrixB { get; private set; }

        public MatrixDimensionMismatchException(MatrixNxM a, MatrixNxM b)
            : base(a, "Matrix Dimensions mismatch")
        {
            this.MatrixA = a;
            this.MatrixB = b;
        }
    }
}
