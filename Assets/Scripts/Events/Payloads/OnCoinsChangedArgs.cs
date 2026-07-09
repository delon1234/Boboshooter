using UnityEngine;

// Information passed around when coins changed
public struct OnCoinsChangedArgs
{
    public float Change;
    public float Total;

    public OnCoinsChangedArgs(float amt, float total)
    {
        this.Change = amt;
        this.Total = total;
    }
}