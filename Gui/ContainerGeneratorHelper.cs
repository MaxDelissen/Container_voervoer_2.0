using Core.ContainerStorage;
using Core.Enums;

namespace Gui
{
    public static class ContainerGeneratorHelper
    {
        public static List<Container> GenerateRandomContainers()
        {
            while (true)
            {
                Console.WriteLine("Please enter the amount of each type of container you want to add.");
                Console.WriteLine("The amount of each type should be separated by a comma.");
                Console.WriteLine("The order is: Valuable + Cooled, Valuable, Cooled, Normal.\n");
                Console.Write("Amounts: ");
                string? input = Console.ReadLine();

                int[] amounts;
                while (!IsValidInput(input, 4, out amounts))
                {
                    Console.WriteLine("Please enter a valid input.");
                    input = Console.ReadLine();
                }

                ContainerGenerator generator = new ContainerGenerator();
                List<Container> containers = generator.GenerateRandomContainers(amounts[0], amounts[1], amounts[2], amounts[3]);
                Console.WriteLine("Thank you, Containers have been generated.\n");
                return containers;
            }
        }


        public static List<Container> GenerateContainersWithWeight()
        {
            var containerDetails = new List<(int amount, int weight)>();
            containerDetails.Add(GetContainerAmountAndWeight(ContainerType.ValuableCooled));
            containerDetails.Add(GetContainerAmountAndWeight(ContainerType.Valuable));
            containerDetails.Add(GetContainerAmountAndWeight(ContainerType.Cooled));
            containerDetails.Add(GetContainerAmountAndWeight(ContainerType.Normal));

            ContainerGenerator generator = new ContainerGenerator();
            List<Container> containers = generator.GenerateContainersWithWeight( //CooledValuable, Valuable, Cooled, Normal
                containerDetails[0].amount, containerDetails[0].weight,
                containerDetails[1].amount, containerDetails[1].weight,
                containerDetails[2].amount, containerDetails[2].weight,
                containerDetails[3].amount, containerDetails[3].weight); //Not completely resilient to changes in ContainerType

            Console.WriteLine("Thank you, Containers have been generated.\n");
            return containers;
        }

        public static List<Container> GenerateContainersManually()
        {
            List<Container> containers = new List<Container>();
            while (true)
            {
                var newContainer = RequestContainer();
                containers.Add(newContainer);
                Console.WriteLine("Container added. Would you like to add another container? (Y/N)");
                if (!GetYesNoInput())
                {
                    Console.WriteLine("Thank you, Containers have been added.\n");
                    return containers;
                }
                Console.WriteLine();
            }
        }

        private static (int amount, int weight) GetContainerAmountAndWeight(ContainerType type)
        {
            int amount = GetValidatedIntInput($"Please enter the amount of {type} containers: ");
            int weight = GetValidatedIntInput($"Please enter the weight of the {type} containers: ");
            return (amount, weight);
        }

        private static Container RequestContainer()
        {
            Console.WriteLine("What type of container would you like to add?");
            ContainerType type = GetValidatedEnumInput<ContainerType>("Type: ");
            int weight = GetValidatedIntInput("Please enter the weight of the container: ");
            return new Container(type, weight);
        }

        /// <summary>
        /// Checks if the input is valid for the specified amount of parts, and parses the parts to an array.
        /// </summary>
        /// <param name="input">The user input to validate.</param>
        /// <param name="expectedParts">The amount of parts which the input should container, and the amount of parts to parse.</param>
        /// <param name="parts">Output parameter for the parsed parts.</param>
        /// <returns>True if the input is valid, false otherwise.</returns>
        public static bool IsValidInput(string? input, int expectedParts, out int[] parts)
        {
            parts = Array.Empty<int>();
            if (input == null || !input.Contains(","))
                return false;

            string[] splitInput = input.Split(",");
            if (splitInput.Length != expectedParts)
                return false;

            parts = new int[expectedParts];
            for (int i = 0; i < expectedParts; i++)
            {
                if (!int.TryParse(splitInput[i], out parts[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the user input for an enum type. Enum type is specified by the generic type parameter.
        /// </summary>
        /// <param name="prompt">The prompt to display to the user.</param>
        /// <typeparam name="T">The enum type to validate.</typeparam>
        /// <returns>The enum value that the user entered.</returns>
        private static T GetValidatedEnumInput<T>(string prompt) where T : struct
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            while (input == null || !Enum.TryParse<T>(input, true, out _))
            {
                Console.WriteLine("Please enter a valid input.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return Enum.Parse<T>(input, true);
        }

        private static int GetValidatedIntInput(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            while (input == null || !int.TryParse(input, out _))
            {
                Console.WriteLine("Please enter a valid input.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }

        /// <summary>
        /// Asks the user for a yes or no input, and returns true if the input is 'Y'.
        /// </summary>
        public static bool GetYesNoInput()
        {
            string? input = Console.ReadLine()?.ToUpper();
            while (input == null || !new[] { "Y", "N" }.Contains(input))
            {
                Console.WriteLine("Please enter a valid input (Y/N):");
                input = Console.ReadLine()?.ToUpper();
            }
            return input == "Y";
        }
    }
}
