// stores information of Current Run
public static class RunData
{
    public static int CurrentFloor = 1;
    public static int FinalFloor = 5;
    public static RunResult Result;

    public static void StartNewRun()
    {
        CurrentFloor = 1;
    }

    public static void AdvanceFloor()
    {
        CurrentFloor++;
    }
}