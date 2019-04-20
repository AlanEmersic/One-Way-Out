using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeProperties
{
    public class ColorGenerator : MonoBehaviour
    {
        enum Colour
        {
            Red, Blue, Green, Yellow, Orange, Purple, Brown, White, Black
        }

        static Queue<Colour> colorQueue;

        public static void Initialize(int seed, int colorCount = 6)
        {
            System.Random random = new System.Random(seed);
            Colour[] colors = Enum.GetValues(typeof(Colour)).Cast<Colour>().ToArray();
            colorQueue = new Queue<Colour>();

            for (int i = colors.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Colour tmp = colors[i];
                colors[i] = colors[j];
                colors[j] = tmp;
            }

            for (int i = 0; i < colorCount; i++)
                colorQueue.Enqueue(colors[i]);
        }

        public static void ColorCompleted()
        {
            colorQueue.Dequeue();
        }
    }
}
