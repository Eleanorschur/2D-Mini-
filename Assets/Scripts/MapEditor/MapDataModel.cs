using System;
using System.Collections.Generic;

public class StageButtonData
{
    public string stageName;

    public int mapColumnCount;
    public int mapRowCount;

    public List<MapRow> rows = new List<MapRow>();
}

[Serializable]
public class MapRow
{
    public List<string> cells = new List<string>();
}