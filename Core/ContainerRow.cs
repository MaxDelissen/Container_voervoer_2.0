namespace Core
{
    public class ContainerRow
    {
        private const decimal MaxWeightDifferencePercentage = 0.2m;
        public readonly List<ContainerStack> Stacks = new();
        public List<ContainerStack> SortedStacks
        {
            get
            {
                return Stacks.OrderBy(stack => stack.Position).ToList();
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
                Stacks.Add(new ContainerStack(ShipSide.Center));
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
            for (int i = 0; i < shipWidth / 2; i++)
            {
                Stacks.Add(new ContainerStack(ShipSide.Left));
                Stacks.Add(new ContainerStack(ShipSide.Right));
            }
        }

        #endregion

        public bool TryAddContainer(Container container)
        {
            if (IsFull)
                return false;

            var sortedStacks = Stacks.OrderBy(stack => stack.CalculateTotalWeight()).ToList();
            var leftWeight = CalculateSideWeight(ShipSide.Left);
            var rightWeight = CalculateSideWeight(ShipSide.Right);
            ShipSide lightestSide = leftWeight < rightWeight ? ShipSide.Left : ShipSide.Right;

            if (TryPlaceContainerOnSide(container, lightestSide))
            {
                return true;
            }

            var totalWeight = leftWeight + rightWeight + CalculateSideWeight(ShipSide.Center);
            var difference = Math.Abs(leftWeight - rightWeight);
            var maxDifference = totalWeight * MaxWeightDifferencePercentage;

            if (difference < maxDifference)
            {
                if (TryPlaceContainerOnSide(container, ShipSide.Right))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool TryPlaceContainerOnSide(Container container, ShipSide lightestSide)
        {
            foreach (var stack in Stacks)
            {
                if (stack.Position == lightestSide)
                {
                    if (stack.TryAddContainer(container))
                    {
                        UpdateFullStatus();
                        return true;
                    }
                }
            }
            return false;
        }

        public void MakeValuableRow(List<Container> valuableContainers)
        {
            if (IsFull)
                return;

            bool containsWrongType = valuableContainers.Any(container =>
                container.Type != ContainerType.Valuable && container.Type != ContainerType.ValuableCooled);

            if (containsWrongType)
                throw new ArgumentException("Not all containers are of type Valuable");

            if (valuableContainers.Count > Stacks.Count)
                throw new ArgumentException(
                $"Too many valuable containers for this row, only add the ships width amount of valuable containers in 1 row. (= {Stacks.Count})");

            foreach (var stack in Stacks)
            {
                bool result = stack.TryAddContainer(valuableContainers[0]);
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
    }
}