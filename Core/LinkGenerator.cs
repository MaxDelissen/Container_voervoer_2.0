using System.Text;
using Core.ContainerStorage;

namespace Core;

public class LinkGenerator
{
    public string ConvertShipToLink(Ship ship)
    {
        List<ContainerRow> sortedContainers = ship.SortedRows;
        (string stacksString, string weightsString) = GetParameterStrings(sortedContainers, ship.Length, ship.Width);

        return ConvertShipToLink(ship, stacksString, weightsString);
    }

    private static string ConvertShipToLink(Ship ship, string stacksString, string weightsString) =>
        $"https://app6i872272.luna.fhict.nl/?length={ship.Length}&width={ship.Width}&stacks={stacksString}&weights={weightsString}";

    private (string stacksString, string weightsString) GetParameterStrings(List<ContainerRow> ship, int shipLength, int shipWidth)
    {
        StringBuilder stacksBuilder = new StringBuilder();
        StringBuilder weightsBuilder = new StringBuilder();

        // Iterate over columns (width)
        for (int i = 0; i < shipWidth; i++)
        {
            // Iterate over rows (length)
            for (int j = 0; j < shipLength; j++)
            {
                if (j >= ship.Count || i >= ship[j].SortedStacks.Count)
                {
                    continue;
                }

                foreach (var container in ship[j].SortedStacks[i].Containers)
                {
                    stacksBuilder.Append((int)container.Type);
                    weightsBuilder.Append(container.Weight);
                    
                    // Append '-' after each container, except the last one in a stack
                    stacksBuilder.Append("-");
                    weightsBuilder.Append("-");
                }

                // Remove the last '-' and append ',' after each stack, except the last one in a column
                if (stacksBuilder.Length > 0 && stacksBuilder[^1] == '-')
                {
                    stacksBuilder.Length--; // Remove the last '-'
                    stacksBuilder.Append(",");
                }
                if (weightsBuilder.Length > 0 && weightsBuilder[^1] == '-')
                {
                    weightsBuilder.Length--; // Remove the last '-'
                    weightsBuilder.Append(",");
                }
            }

            // Remove the last ',' and append '/' after each column, except the last one
            if (stacksBuilder.Length > 0 && stacksBuilder[^1] == ',')
            {
                stacksBuilder.Length--; // Remove the last ','
                stacksBuilder.Append("/");
            }
            if (weightsBuilder.Length > 0 && weightsBuilder[^1] == ',')
            {
                weightsBuilder.Length--; // Remove the last ','
                weightsBuilder.Append("/");
            }
        }
        
        // Remove the last '/' from both strings
        if (stacksBuilder.Length > 0 && stacksBuilder[^1] == '/')
        {
            stacksBuilder.Length--;
            weightsBuilder.Length--;
        }

        return (stacksBuilder.ToString(), weightsBuilder.ToString());
    }
}