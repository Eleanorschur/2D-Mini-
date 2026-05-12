using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ExcelMapJsonConverter
{
    [MenuItem("Tools/Map/Convert Excel To MapData JSON")]
    public static void ConvertExcelToJson()
    {
        string excelPath = EditorUtility.OpenFilePanel(
            "맵 엑셀 파일 선택",
            Application.dataPath,
            "xlsx"
        );

        if (string.IsNullOrEmpty(excelPath))
            return;

        string outputFolder = Path.Combine(Application.dataPath, "Resources/Maps");

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        try
        {
            List<StageJsonData> stages = ReadAllSheets(excelPath);

            foreach (StageJsonData stage in stages)
            {
                string fileName = $"{stage.stageName}.json";
                string outputPath = Path.Combine(outputFolder, fileName);

                string json = BuildSingleStageJson(stage);

                File.WriteAllText(outputPath, json, Encoding.UTF8);

                Debug.Log($"{fileName} 생성 완료 : {outputPath}");
                Debug.Log($"{stage.stageName} 크기 : Columns={stage.mapColumnCount}, Rows={stage.mapRowCount}");
            }

            AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.LogError($"엑셀 변환 실패: {e.Message}\n{e.StackTrace}");
        }
    }

    private static List<StageJsonData> ReadAllSheets(string excelPath)
    {
        List<StageJsonData> stages = new List<StageJsonData>();

        using ZipArchive archive = ZipFile.OpenRead(excelPath);

        XNamespace mainNs = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        XNamespace relNs = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        XNamespace packageRelNs = "http://schemas.openxmlformats.org/package/2006/relationships";

        List<string> sharedStrings = ReadSharedStrings(archive, mainNs);

        XDocument workbookDoc = ReadXml(archive, "xl/workbook.xml");
        XDocument workbookRelsDoc = ReadXml(archive, "xl/_rels/workbook.xml.rels");

        Dictionary<string, string> relationTargets = new Dictionary<string, string>();

        foreach (XElement rel in workbookRelsDoc.Root.Elements(packageRelNs + "Relationship"))
        {
            string id = rel.Attribute("Id")?.Value;
            string target = rel.Attribute("Target")?.Value;

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(target))
                relationTargets[id] = target;
        }

        foreach (XElement sheet in workbookDoc.Root.Element(mainNs + "sheets").Elements(mainNs + "sheet"))
        {
            string sheetName = sheet.Attribute("name")?.Value;
            string relationId = sheet.Attribute(relNs + "id")?.Value;

            if (string.IsNullOrEmpty(sheetName) || string.IsNullOrEmpty(relationId))
                continue;

            if (!relationTargets.TryGetValue(relationId, out string target))
                continue;

            string sheetPath = target.Replace("\\", "/");

            if (sheetPath.StartsWith("/"))
                sheetPath = sheetPath.TrimStart('/');

            if (!sheetPath.StartsWith("xl/"))
                sheetPath = "xl/" + sheetPath;

            StageJsonData stage = ReadSheet(
                archive,
                sheetPath,
                sheetName,
                sharedStrings,
                mainNs
            );

            stages.Add(stage);
        }

        return stages;
    }

    private static StageJsonData ReadSheet(
        ZipArchive archive,
        string sheetPath,
        string sheetName,
        List<string> sharedStrings,
        XNamespace mainNs)
    {
        XDocument sheetDoc = ReadXml(archive, sheetPath);

        Dictionary<int, Dictionary<int, string>> cellMap = new Dictionary<int, Dictionary<int, string>>();

        int maxRow = 0;
        int maxCol = 0;

        XElement sheetData = sheetDoc.Root.Element(mainNs + "sheetData");

        foreach (XElement rowElement in sheetData.Elements(mainNs + "row"))
        {
            foreach (XElement cellElement in rowElement.Elements(mainNs + "c"))
            {
                string cellRef = cellElement.Attribute("r")?.Value;

                if (string.IsNullOrEmpty(cellRef))
                    continue;

                int rowIndex = GetRowIndex(cellRef);
                int colIndex = GetColumnIndex(cellRef);

                string value = GetCellValue(cellElement, sharedStrings, mainNs);

                if (!cellMap.ContainsKey(rowIndex))
                    cellMap[rowIndex] = new Dictionary<int, string>();

                cellMap[rowIndex][colIndex] = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();

                maxRow = Mathf.Max(maxRow, rowIndex);
                maxCol = Mathf.Max(maxCol, colIndex);
            }
        }

        StageJsonData stage = new StageJsonData();
        stage.stageName = sheetName;
        stage.mapColumnCount = maxCol;
        stage.mapRowCount = maxRow;

        for (int row = 1; row <= maxRow; row++)
        {
            List<string> cells = new List<string>();

            for (int col = 1; col <= maxCol; col++)
            {
                string value = "";

                if (cellMap.ContainsKey(row) && cellMap[row].ContainsKey(col))
                    value = cellMap[row][col];

                cells.Add(value);
            }

            stage.rows.Add(cells);
        }

        return stage;
    }

    private static List<string> ReadSharedStrings(ZipArchive archive, XNamespace mainNs)
    {
        List<string> sharedStrings = new List<string>();

        ZipArchiveEntry entry = archive.GetEntry("xl/sharedStrings.xml");

        if (entry == null)
            return sharedStrings;

        XDocument doc = ReadXml(entry);

        foreach (XElement si in doc.Root.Elements(mainNs + "si"))
        {
            StringBuilder text = new StringBuilder();

            foreach (XElement t in si.Descendants(mainNs + "t"))
            {
                text.Append(t.Value);
            }

            sharedStrings.Add(text.ToString());
        }

        return sharedStrings;
    }

    private static string GetCellValue(
        XElement cellElement,
        List<string> sharedStrings,
        XNamespace mainNs)
    {
        string cellType = cellElement.Attribute("t")?.Value;

        if (cellType == "inlineStr")
        {
            XElement inlineText = cellElement.Descendants(mainNs + "t").FirstOrDefault();
            return inlineText?.Value ?? "";
        }

        XElement valueElement = cellElement.Element(mainNs + "v");

        if (valueElement == null)
            return "";

        string rawValue = valueElement.Value;

        if (cellType == "s")
        {
            if (int.TryParse(rawValue, out int sharedStringIndex))
            {
                if (sharedStringIndex >= 0 && sharedStringIndex < sharedStrings.Count)
                    return sharedStrings[sharedStringIndex];
            }

            return "";
        }

        return rawValue;
    }

    private static int GetColumnIndex(string cellReference)
    {
        int column = 0;

        foreach (char c in cellReference)
        {
            if (!char.IsLetter(c))
                break;

            column *= 26;
            column += char.ToUpper(c) - 'A' + 1;
        }

        return column;
    }

    private static int GetRowIndex(string cellReference)
    {
        StringBuilder number = new StringBuilder();

        foreach (char c in cellReference)
        {
            if (char.IsDigit(c))
                number.Append(c);
        }

        return int.Parse(number.ToString());
    }

    private static XDocument ReadXml(ZipArchive archive, string path)
    {
        ZipArchiveEntry entry = archive.GetEntry(path);

        if (entry == null)
            throw new FileNotFoundException($"xlsx 내부에서 파일을 찾을 수 없음: {path}");

        return ReadXml(entry);
    }

    private static XDocument ReadXml(ZipArchiveEntry entry)
    {
        using Stream stream = entry.Open();
        return XDocument.Load(stream);
    }

    private static string BuildSingleStageJson(StageJsonData stage)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("{");
        sb.AppendLine($"  \"stageName\": \"{Escape(stage.stageName)}\",");
        sb.AppendLine($"  \"mapColumnCount\": {stage.mapColumnCount},");
        sb.AppendLine($"  \"mapRowCount\": {stage.mapRowCount},");
        sb.AppendLine("  \"rows\": [");

        for (int r = 0; r < stage.rows.Count; r++)
        {
            List<string> row = stage.rows[r];

            sb.Append("    { \"cells\": [");

            for (int c = 0; c < row.Count; c++)
            {
                sb.Append($"\"{Escape(row[c])}\"");

                if (c < row.Count - 1)
                    sb.Append(", ");
            }

            sb.Append("] }");

            if (r < stage.rows.Count - 1)
                sb.Append(",");

            sb.AppendLine();
        }

        sb.AppendLine("  ]");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string Escape(string value)
    {
        if (value == null)
            return "";

        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"");
    }

    private class StageJsonData
    {
        public string stageName;
        public int mapColumnCount;
        public int mapRowCount;
        public List<List<string>> rows = new List<List<string>>();
    }
}