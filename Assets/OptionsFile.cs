using System;

[System.Serializable]
public class OptionsFile
{
    public int pathFindCalculations;
    public int autoSaveInterval;
    public float viewHeight;
    public OptionsFile(Options options)
    {
        pathFindCalculations = options.pathfindingCalculations;
        autoSaveInterval = options.autoSaveInterval;
        viewHeight = options.viewHeight;
    }
}
