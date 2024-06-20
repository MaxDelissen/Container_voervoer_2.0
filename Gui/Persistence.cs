using Core.ContainerStorage;
using Core.Enums;

namespace Gui;

public class Persistence
{
    public int SaveLayout(int shipWidth, int shipLength, List<Container> shipContainersToSort)
    {
        string folderPath = GetFolderPath();
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var latestId = 0;
        string[] files = Directory.GetFiles(folderPath);
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            fileName = fileName.Replace("layout:", "");
            if (int.TryParse(fileName, out int id))
            {
                if (id > latestId)
                {
                    latestId = id;
                }
            }
        }
        latestId++;
        string filePath = Path.Combine(folderPath, $"layout:{latestId}.json");
        using StreamWriter newFile = File.CreateText(filePath);
        newFile.WriteLine(shipWidth);
        newFile.WriteLine(shipLength);
        foreach (Container container in shipContainersToSort)
        {
            newFile.WriteLine(container.Type);
            newFile.WriteLine(container.Weight);
        }

        return latestId;
    }

    public (int shipWidth, int shipLength, List<Container> shipContainersToSort) LoadLayout(int id)
    {
        string folderPath = GetFolderPath();
        string filePath = Path.Combine(folderPath, $"layout:{id}.json");
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException();
        }

        using StreamReader file = File.OpenText(filePath);
        int shipWidth = int.Parse(file.ReadLine());
        int shipLength = int.Parse(file.ReadLine());
        List<Container> shipContainersToSort = new();
        while (!file.EndOfStream)
        {
            string type = file.ReadLine();
            var containerType = Enum.Parse<ContainerType>(type);
            int weight = int.Parse(file.ReadLine());
            shipContainersToSort.Add(new Container(containerType, weight));
        }

        return (shipWidth, shipLength, shipContainersToSort);
    }

    private string GetFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ContainerSorter");
}