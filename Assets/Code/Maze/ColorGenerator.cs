using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class ColorGenerator : MonoBehaviour
{    
    [SerializeField] GameObject spritePrefab;
    [SerializeField] TaskGenerator taskGenerator;

    //Queue<Colour> colorQueue;
    //Colour[] colors;

    public void Initialize(int seed)
    {
        //System.Random random = new System.Random(seed);        
        //colorQueue = new Queue<Colour>();        

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        //for (int i = colors.Length - 1; i > 0; i--)
        //{
        //    int j = random.Next(i + 1);
        //    Colour tmp = colors[i];
        //    colors[i] = colors[j];
        //    colors[j] = tmp;
        //}

        for (int i = 0; i < taskGenerator.TaskCount; i++)
        {
            //colorQueue.Enqueue(colors[i]);
            GameObject obj = Instantiate(spritePrefab, transform);
            obj.GetComponent<Image>().color = taskGenerator.TaskColorsContainer[i];
        }
    }    

    public void ColorCompleted()
    {
        //colorQueue.Dequeue();
    }
}