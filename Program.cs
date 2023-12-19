using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public Figure() { } // Конструктор для сериализации/десериализации
    public Figure(string name, double width, double height)
    {
        Name = name;
        Width = width;
        Height = height;
    }
}

public class FileManager
{
    private static readonly DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Figure));
    private static readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(Figure));

    public Figure LoadFile(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                if (filePath.EndsWith(".json"))
                {
                    return (Figure)jsonSerializer.ReadObject(reader.BaseStream);
                }
                else if (filePath.EndsWith(".xml"))
                {
                    return (Figure)xmlSerializer.Deserialize(reader);
                }
                else if (filePath.EndsWith(".txt"))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    return new Figure(lines[0], double.Parse(lines[1]), double.Parse(lines[2]));
                }
                else
                {
                    Console.WriteLine("Unsupported file format");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }

        return null;
    }

    public void SaveFile(string filePath, Figure figure)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                if (filePath.EndsWith(".json"))
                {
                    jsonSerializer.WriteObject(writer.BaseStream, figure);
                }
                else if (filePath.EndsWith(".xml"))
                {
                    xmlSerializer.Serialize(writer, figure);
                }
                else if (filePath.EndsWith(".txt"))
                {
                    writer.WriteLine(figure.Name);
                    writer.WriteLine(figure.Width);
                    writer.WriteLine(figure.Height);
                }
                else
                {
                    Console.WriteLine("Unsupported file format");
                }
            }
            Console.WriteLine("File saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the file path:");
        string filePath = Console.ReadLine();

        FileManager fileManager = new FileManager();
        Figure loadedFigure = fileManager.LoadFile(filePath);

        if (loadedFigure != null)
        {
            Console.WriteLine($"Loaded Figure: {loadedFigure.Name}, Width: {loadedFigure.Width}, Height: {loadedFigure.Height}");

            // You can manipulate the loadedFigure object as needed

            Console.WriteLine("Press F1 to save or Escape to exit.");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.F1)
                {
                    Console.WriteLine("Enter the file path to save:");
                    string savePath = Console.ReadLine();
                    fileManager.SaveFile(savePath, loadedFigure);
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}
