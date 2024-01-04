using System;
using System.Security.Cryptography;
using MMIM.Common;

namespace MMIM.Subcomponents
{
    public abstract class SubcomponentBase : PropertyChangedBase
    {
        public abstract void MakeRandomSelection(RNGCryptoServiceProvider rng);
        public abstract void Reset();
    }

    public static class RNGExtensions
    {
        public static int GetRandomInt(this RNGCryptoServiceProvider rng, int min, int max)
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var rand = BitConverter.ToInt32(bytes, 0);
            if (rand < 0) rand *= -1;
            var range = max - min + 1;
            return (rand % range) + min;
        }
    }
}
