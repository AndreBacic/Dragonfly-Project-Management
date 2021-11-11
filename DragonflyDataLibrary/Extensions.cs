using System.Collections.Generic;

namespace DragonflyDataLibrary
{
    public static class Extensions
    {
        public static string ListToString<T>(this List<T> @this)
        {
            string output = "";
            foreach (T obj in @this)
            {
                output += $"{obj},";
            }
            return output.Remove(output.Length - 1);
        }
    }
}
