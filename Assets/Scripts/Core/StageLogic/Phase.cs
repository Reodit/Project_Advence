using System;
using System.Collections.Generic;

[Serializable]
public class Phase
{
    public PhaseTable phaseData;
    public List<PatternTable> patternList;
    public float remainTime;
    public float phaseTime;

    public Phase(PhaseTable phaseData, List<PatternTable> patternList, float phaseTime)
    {
        this.phaseData = phaseData;
        this.patternList = patternList;
        this.phaseTime = phaseTime;
        remainTime = phaseTime;
    }
    
    public Phase(PhaseTable phaseData, float phaseTime)
    {
        this.phaseData = phaseData;
        this.patternList = AddPattern();
        this.phaseTime = phaseTime;

        remainTime = phaseTime;
    }

    private List<PatternTable> AddPattern()
    {
        var patterns = new List<PatternTable>();
        
        foreach (var e in Datas.GameData.DTPatternData)
        {
            patterns.Add(e.Value);    
        }

        return patterns;
    }
}