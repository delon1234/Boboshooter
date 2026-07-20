using System.Collections.Generic;
using UnityEngine;

// This class takes in a List of items where its children have inherits from IWeighted
// All children items have Weight, therefore this helper picks a random item based on the List
// Using Sum of Weights to get a weighted item
public static class WeightedRandom
{
    public static T Pick<T>(IList<T> items) where T : IWeighted
    {
        // Constantly makes sure item.Weight is >=0
        // Sum all weights
        int totalWeight = 0;
        foreach(var item in items)
        {
            totalWeight += Mathf.Max(0, item.Weight);
        }

        // Picks item based on roll
        // Loops through table, subtracts weight from roll until it < 0
        int roll = Random.Range(0, totalWeight);
        foreach(var item in items)
        {
            roll -= Mathf.Max(0, item.Weight);
            if(roll < 0)
            {
                return item;
            }
        }
        return items[0];
    }
}

