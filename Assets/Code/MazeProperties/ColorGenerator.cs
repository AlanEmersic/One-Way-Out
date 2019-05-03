using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeProperties
{
    public class ColorGenerator : MonoBehaviour
    {
        enum Colour
        {
            //Red, Blue, Green, Yellow, Orange, Purple, Brown, White, Black
            Red, Blue, Green, Yellow, Orange, Purple
        }

        [SerializeField] GameObject spritePrefab;
        public static ColorGenerator Instance { get; private set; }
        Queue<Colour> colorQueue;
        Sprite[] sprites;
        Colour[] colors;

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            sprites = Resources.LoadAll<Sprite>("Art/Colors");
        }

        public void Initialize(int seed)
        {
            System.Random random = new System.Random(seed);
            colors = Enum.GetValues(typeof(Colour)).Cast<Colour>().ToArray();
            colorQueue = new Queue<Colour>();
            int colorCount = random.Next(3, colors.Length);

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            for (int i = colors.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Colour tmp = colors[i];
                colors[i] = colors[j];
                colors[j] = tmp;
            }

            for (int i = 0; i < colorCount; i++)
            {
                colorQueue.Enqueue(colors[i]);
                GameObject obj = Instantiate(spritePrefab, transform);
                obj.GetComponent<Image>().sprite = GetColor(colors[i], obj);
            }
        }

        Sprite GetColor(Colour color, GameObject obj)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                string name = sprites[i].name;
                if (Enum.TryParse(name, out Colour colour) && colour == color)
                {
                    obj.name = sprites[i].name;
                    return sprites[i];
                }
            }
            return null;
        }

        public void ColorCompleted()
        {
            colorQueue.Dequeue();
        }
    }
}
