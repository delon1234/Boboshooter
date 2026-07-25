using UnityEngine;

// Information passed around when Floors changed
public struct OnFloorChangedArgs
{
    public int Current;
    public int Final;

    public OnFloorChangedArgs(int Current, int Final)
    {
        this.Current = Current;
        this.Final = Final;
    }
}