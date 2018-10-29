namespace DataStructures
{
    class Util
    {
        // Simplifies some testing interfaces.
        public static byte[] StringToBytes(string source)
        {
            byte[] equiv = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                equiv[i] = (byte)source[i];
            }

            return equiv;
        }
    }
}