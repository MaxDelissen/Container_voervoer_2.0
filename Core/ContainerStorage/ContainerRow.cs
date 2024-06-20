using Core.Enums;

namespace Core.ContainerStorage
{
    public class ContainerRow
    {
        private const decimal MaxWeightDifferencePercentage = 0.2m;
        public List<ContainerStack> Stacks = new();
        public List<ContainerStack> SortedStacks
        {
            get
            {
                return Stacks.OrderBy(stack => stack.LeftRightIndex).ToList();
            }
        }
        public List<Container> FailedContainers { get; private set; } = new();

        private bool IsFull { get; set; }

        #region Init

        public ContainerRow(int shipWidth)
        {
            bool hasCenterStack = shipWidth % 2 != 0;
            if (hasCenterStack)
            {
                int centerIndex = (int)Math.Ceiling(shipWidth / 2.0);
                Stacks.Add(new ContainerStack(ShipSide.Center, centerIndex));
                shipWidth--;
                AddStacks(shipWidth);
            }
            else
            {
                AddStacks(shipWidth);
            }
        }

        private void AddStacks(int shipWidth)
        {
            int firstIndex = 1;
            int lastIndex = shipWidth;
            for (int i = 0; i < shipWidth / 2; i++)
            {
                Stacks.Add(new ContainerStack(ShipSide.Left, firstIndex));
                firstIndex++;
                Stacks.Add(new ContainerStack(ShipSide.Right, lastIndex));
                lastIndex--;
            }
        }

        #endregion

        public bool TryAddContainer(Container container, ContainerRow? nextRow, ContainerRow? previousRow)
        {
            if (IsFull)
                return false;

            var sortedStacks = Stacks.OrderBy(stack => stack.CalculateTotalWeight()).ToList();
            var leftWeight = CalculateSideWeight(ShipSide.Left);
            var rightWeight = CalculateSideWeight(ShipSide.Right);
            var centerWeight = CalculateSideWeight(ShipSide.Center);

            if (centerWeight < leftWeight && centerWeight < rightWeight) //Check if center is lightest
            {
                if (TryPlaceContainerOnSide(container, ShipSide.Center, nextRow, previousRow))
                {
                    return true;
                }
            }
            
            ShipSide lightestSide = leftWeight < rightWeight ? ShipSide.Left : ShipSide.Right; //Place on lightest side
            if (TryPlaceContainerOnSide(container, lightestSide, nextRow, previousRow))
            {
                return true;
            }

            var totalWeight = leftWeight + rightWeight + CalculateSideWeight(ShipSide.Center); 
            //if it doesn't fit on lightest side, check if it fits on other side while keeping weight difference in mind
            var difference = Math.Abs(leftWeight - rightWeight);
            var maxDifference = totalWeight * MaxWeightDifferencePercentage;

            if (difference < maxDifference)
            {
                if (TryPlaceContainerOnSide(container, lightestSide == ShipSide.Left ? ShipSide.Right : ShipSide.Left, nextRow, previousRow))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool TryPlaceContainerOnSide(Container container, ShipSide lightestSide, ContainerRow? nextRow, ContainerRow? previousRow)
        {
            Stacks = Stacks.OrderBy(stack => stack.CalculateTotalWeight()).ToList();
            foreach (var stack in Stacks)
            {
                if (stack.Position == lightestSide)
                {
                    var nextStack = nextRow?.Stacks[stack.LeftRightIndex - 1];
                    var previousStack = previousRow?.Stacks[stack.LeftRightIndex - 1];
                    if (stack.TryAddContainer(container, nextStack, previousStack))
                    {
                        UpdateFullStatus();
                        return true;
                    }
                }
            }
            return false;
        }

        public void MakeValuableRow(List<Container> valuableContainers, ContainerRow? previousRow, ContainerRow? nextRow)
        {
            if (IsFull)
                return;
            
            bool hasPreviousRow = previousRow != null;
            bool hasNextRow = nextRow != null;

            bool containsWrongType = valuableContainers.Any(container =>
                container.Type != ContainerType.Valuable && container.Type != ContainerType.ValuableCooled);

            if (containsWrongType)
                throw new ArgumentException("Not all containers are of type Valuable");

            if (valuableContainers.Count > Stacks.Count)
                throw new ArgumentException(
                $"Too many valuable containers for this row, only add the ships width amount of valuable containers in 1 row. (= {Stacks.Count})");

            foreach (var stack in Stacks)
            {
                if (valuableContainers.Count == 0)
                    break;
                
                int stackHeight = stack.Containers.Count;
                int stackIndex = stack.LeftRightIndex -1;
                int previousRowHeight = hasPreviousRow ? previousRow!.Stacks[stackIndex].Containers.Count : 0;
                int nextRowHeight = hasNextRow ? nextRow!.Stacks[stackIndex].Containers.Count : 0;
                
                bool canPlace = stackHeight >= previousRowHeight || stackHeight >= nextRowHeight;

                if (!canPlace)
                {
                    FailedContainers.Add(valuableContainers[0]);
                    valuableContainers.RemoveAt(0);
                    continue;
                }
                bool result = stack.TryAddContainer(valuableContainers[0], null, null);
                if (!result)
                    FailedContainers.Add(valuableContainers[0]);

                valuableContainers.RemoveAt(0);
                if (valuableContainers.Count == 0)
                    break;
            }

            if (valuableContainers.Count > 0)
                throw new Exception(
                "Not all valuable containers could be added to the row, this should not happen, and should have been caught earlier");

            UpdateFullStatus();
        }

        private void UpdateFullStatus()
        {
            IsFull = Stacks.All(stack => stack.IsFull());
        }

        private int CalculateSideWeight(ShipSide side)
        {
            int totalWeight = 0;
            foreach (var stack in Stacks)
            {
                if (stack.Position == side)
                {
                    totalWeight += stack.CalculateTotalWeight();
                }
            }
            return totalWeight;
        }

        public bool MoveBottomContainersToTop()
        {
            foreach (var stack in Stacks)
            {
                bool success = stack.MoveBottomContainerToTop();
                if (!success)
                    return false;
            }
            return true;
        }

        public object CalculateTotalWeight()
        {
            return Stacks.Sum(stack => stack.CalculateTotalWeight());
        }
    }
}