// stores information of CurrentFloor only
public static class RunData
{
    public static int CurrentFloor = 1;

    public static void StartNewRun()
    {
        CurrentFloor = 1;
    }

    public static void AdvanceFloor()
    {
        CurrentFloor++;
    }
}