using Core;
using Core.Enums;
using Core.ContainerStorage;

namespace Gui
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintAsciiIntro();
            Console.WriteLine(" Max Delissen 2024");
            Console.WriteLine("Semester 2 - Software - Fontys ICT Eindhoven\n");

            while (true)
            {
                Ship? ship = CreateShip();
                if (ship == null) continue;

                while (true)
                {
                    if (!AddContainersToShip(ship)) break;
                }
               
                SortResult result = SortAndDisplayResults(ship);

                if (result == SortResult.Success || result == SortResult.SuccesWithFailedContainers)
                {
                    if (!GenerateVisualizerLink(ship)) break;
                }
            }
            
            Console.WriteLine("Thank you for using Containervervoer.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }

        private static void PrintAsciiIntro()
        {
            Console.WriteLine("   _____            _        _                  __      __                            ");
            Console.WriteLine("  / ____|          | |      (_)                 \\ \\    / /                            ");
            Console.WriteLine(" | |     ___  _ __ | |_ __ _ _ _ __   ___ _ __   \\ \\  / /__ _ ____   _____   ___ _ __ ");
            Console.WriteLine(" | |    / _ \\| '_ \\| __/ _` | | '_ \\ / _ \\ '__|   \\ \\/ / _ \\ '__\\ \\ / / _ \\ / _ \\ '__|");
            Console.WriteLine(" | |___| (_) | | | | || (_| | | | | |  __/ |       \\  /  __/ |   \\ V / (_) |  __/ |   ");
            Console.WriteLine("  \\_____\\___/|_| |_|\\__\\__,_|_|_| |_|\\___|_|        \\/ \\___|_|    \\_/ \\___/ \\___|_|   ");
            Console.WriteLine("                                                                                      ");
        }

        private static Ship? CreateShip()
        {
            Console.WriteLine("Please enter the ship's width and length, separated by a comma.");
            Console.WriteLine("The width is what you see when you look at the ship from the front.");
            Console.WriteLine("The length is what you see when you look at the ship from the side.\n");
            Console.Write("Dimensions: ");

            string? input = Console.ReadLine();
            int[] dimensions;

            while (!ContainerGeneratorHelper.IsValidInput(input, 2, out dimensions))
            {
                Console.WriteLine("Please enter a valid input.");
                input = Console.ReadLine();
            }

            try
            {
                Ship ship = new Ship(dimensions[0], dimensions[1]);
                Console.WriteLine($"\nShip created with the following dimensions: W:{ship.Width}, L:{ship.Length}\n");
                return ship;
            }
            catch (Exception)
            {
                Console.WriteLine("The values you entered were invalid. Please try again.");
                return null;
            }
        }


        private static bool AddContainersToShip(Ship ship)
        {
            Console.WriteLine("Please choose a method to add containers to the ship.");
            Console.WriteLine("1. Enter an amount of each type, with random weights.");
            Console.WriteLine("2. Enter an amount of each type, and the weight of each type.");
            Console.WriteLine("3. Enter all container types and weights manually.");
            Console.Write("Method: ");

            string? method = Console.ReadLine();
            while (!new[] { "1", "2", "3" }.Contains(method))
            {
                Console.WriteLine("Please enter a valid input.");
                method = Console.ReadLine();
            }

            List<Container> containers = method switch
            {
                "1" => ContainerGeneratorHelper.GenerateRandomContainers(),
                "2" => ContainerGeneratorHelper.GenerateContainersWithWeight(),
                "3" => ContainerGeneratorHelper.GenerateContainersManually(),
                _ => new List<Container>()
            };

            ship.AddContainers(containers);
            Console.WriteLine("Containers have been added to the ship.\n");
            Console.WriteLine("Do you want to see a list of all containers in the ship? (Y/N)");
            if (ContainerGeneratorHelper.GetYesNoInput())
                PrintContainers(ship);
            
            Console.WriteLine("Do you want to add more containers? (Y/N)");
            return ContainerGeneratorHelper.GetYesNoInput();
        }

        private static SortResult SortAndDisplayResults(Ship ship)
        {
            Console.WriteLine("Press enter to confirm sorting the containers.");
            Console.ReadLine();

            var sortingResult = ship.SortContainers();
            string resultMessage = sortingResult switch
            {
                SortResult.Success => "Containers have been sorted successfully.",
                SortResult.SuccesWithFailedContainers => "Containers have been sorted successfully, but some containers could not be placed.",
                SortResult.OverWeight => "The ship is over weight, and the ship could not be sorted.",
                SortResult.UnderWeight => "The ship is under weight, and the ship could not be sorted.",
                SortResult.TooManyCooledValuableContainers => "There are too many cooled valuable containers, and the ship could not be sorted.",
                SortResult.TooManyCooledContainers => "There are too many cooled containers, and the ship could not be sorted.",
                _ => "An unknown error occurred during sorting."
            };
            Console.WriteLine(resultMessage);
            Console.WriteLine();
            if (sortingResult == SortResult.SuccesWithFailedContainers)
            {
                Console.WriteLine("The following containers could not be placed:");
                foreach (Container container in ship.GetTotalFailedContainers())
                {
                    Console.WriteLine($"Type: {container.Type}, Weight: {container.Weight}");
                }
                Console.WriteLine("--------------------------------------------------------\n");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
            return sortingResult;
        }

        /// <summary>
        /// Generates a visualizer link for the ship, if the user wants to.
        /// Also asks the user if they want to clear the current ship and start over, or exit the program.
        /// </summary>
        /// <param name="ship">The current ship</param>
        /// <returns>bool: True if the user wants to clear the ship and start over, false if the user wants to exit the program.</returns>
        private static bool GenerateVisualizerLink(Ship ship)
        {
            Console.WriteLine("\nDo you want to generate a visualizer link for this ship? (Y/N)");
            if (ContainerGeneratorHelper.GetYesNoInput())
            {
                LinkGenerator linkGenerator = new LinkGenerator();
                string link = linkGenerator.ConvertShipToLink(ship);
                Console.WriteLine($"\nVisualizer link: {link}\n");
            }

            Console.WriteLine("Do you want to clear the current ship and start over? Choosing No will exit the program. (Y/N)");
            return ContainerGeneratorHelper.GetYesNoInput();
        }

        private static void PrintContainers(Ship ship)
        {
            foreach (Container container in ship.ContainersToSort)
            {
                Console.WriteLine($"Type: {container.Type}, Weight: {container.Weight}");
            }
            Console.WriteLine("--------------------------------------------------------\n");
        }
    }
}
