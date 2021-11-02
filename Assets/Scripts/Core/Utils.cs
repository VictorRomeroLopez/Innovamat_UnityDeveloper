using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Utils : MonoBehaviour
    {
        public static string NumberToString(int number)
        {
            Dictionary<int, string> numberToString = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
                { 4, "four" },
                { 5, "five" },
                { 6, "six" },
                { 7, "seven" },
                { 8, "eight" },
                { 9, "nine" },
                { 10, "ten" }
            };
            
            return numberToString[number];
        }
    }
}