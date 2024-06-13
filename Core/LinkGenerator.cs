using System.Text;

namespace Core;

public class LinkGenerator
{
    public string ConvertShipToLink(List<ContainerRow> sortedContainers, Ship ship)
    {
        (string stacksString, string weightsString) = GetParameterStrings(sortedContainers);

        return ConvertShipToLink(ship, stacksString, weightsString);
    }

    private static string ConvertShipToLink(Ship ship, string stacksString, string weightsString) =>
        $"https://i872272.luna.fhict.nl/ContainerVisualizer/index.html?length={ship.Length}&width={ship.Width}&stacks={stacksString}&weights={weightsString}";

    private (string stacksString, string weightsString) GetParameterStrings(List<ContainerRow> ship)
    {
        StringBuilder stacksBuilder = new StringBuilder();
        StringBuilder weightsBuilder = new StringBuilder();

        foreach (ContainerRow row in ship)
        {
            if (stacksBuilder.Length > 0)
            {
                stacksBuilder.Append("/");
                weightsBuilder.Append("/");
            }
            foreach (ContainerStack stack in row.Stacks)
            {
                if (stacksBuilder.Length > 0 && stacksBuilder[^1] != '/')
                {
                    stacksBuilder.Append(",");
                    weightsBuilder.Append(",");
                }
                foreach (Container container in stack.Containers)
                {
                    if (stacksBuilder.Length > 0 && stacksBuilder[^1] != ',' && stacksBuilder[^1] != '/')
                    {
                        stacksBuilder.Append("-");
                        weightsBuilder.Append("-");
                    }

                    stacksBuilder.Append((int)container.Type);
                    weightsBuilder.Append(container.Weight);
                }
            }
        }
        return (stacksBuilder.ToString(), weightsBuilder.ToString());
    }
}