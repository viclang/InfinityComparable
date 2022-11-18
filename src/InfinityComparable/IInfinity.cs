using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InfinityComparable
{
    public interface IInfinityNumber<T> : IInfinity<InfinityNumber<T>, T>
         where T : struct, INumber<T>
    {

    }

    public interface IInfinity<TSelf, T> where T : struct
    {
#if NET7_0
        /// <summary>Gets a value that represents negative <c>infinity</c>.</summary>
        static abstract TSelf NegativeInfinity { get; }

        /// <summary>Gets a value that represents positive <c>infinity</c>.</summary>
        static abstract TSelf PositiveInfinity { get; }
#endif


    public T? Finite { get; }

        public bool IsInfinity { get; }

        public T GetValueOrDefault();
        public T GetValueOrDefault(T defaultValue);
    }
}
