using UnityEngine;

// Information passed around when Meta Coins changed
public struct OnMetaCoinsChangedArgs
{
    public int Change;
    public int Total;

    public OnMetaCoinsChangedArgs(int amt, int total)
    {
        this.Change = amt;
        this.Total = total;
    }
}