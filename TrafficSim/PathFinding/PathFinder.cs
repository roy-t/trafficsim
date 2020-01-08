using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrafficSim.Network;

namespace TrafficSim.PathFinding
{
    public static class PathFinder
    {
        public static IReadOnlyList<Road> FindPath(Road from, Road to)
        {
            if (from == to)
            {
                return new List<Road> { from };
            }

            var all = new Dictionary<Road, MinHeapNode>();

            var head = new MinHeapNode(from, null, TraversalCost(from), ExpectedCost(from, to));
            var open = new MinHeap();

            open.Push(head);
            all.Add(head.Road, head);


            while (open.HasNext())
            {
                var current = open.Pop();
                if (current.Road == to)
                {
                    return ReconstructPath(current);
                }

                for (var i = 0; i < current.Road.EndJunction.Outgoing.Count; i++)
                {
                    Step(current, current.Road.EndJunction.Outgoing[i], to, all, open);
                }

                if (current.Road.RoadType == RoadType.TwoWay)
                {
                    for (var i = 0; i < current.Road.StartJunction.Outgoing.Count; i++)
                    {
                        Step(current, current.Road.StartJunction.Outgoing[i], to, all, open);
                    }
                }
            }

            return new List<Road>();
        }

        private static void Step(MinHeapNode path, Road road, Road goal, Dictionary<Road, MinHeapNode> nodes, MinHeap open)
        {
            var nodeCostSoFar = path.CostSoFar + TraversalCost(path.Road);
            if (nodes.TryGetValue(road, out var node))
            {
                if (node.CostSoFar > nodeCostSoFar)
                {
                    node.CostSoFar = nodeCostSoFar;
                    node.CameFrom = path;
                    node.ExpectedCost = nodeCostSoFar + ExpectedCost(road, goal);
                    open.Push(node);
                }
            }
            else
            {
                node = new MinHeapNode(road, path, nodeCostSoFar, nodeCostSoFar + ExpectedCost(road, goal));
                nodes.Add(road, node);
                open.Push(node);
            }
        }

        private static IReadOnlyList<Road> ReconstructPath(MinHeapNode node)
        {
            var path = new List<Road>();

            var current = node;
            while (current != null)
            {
                path.Add(current.Road);
                current = current.CameFrom;
            }

            path.Reverse();
            return path;
        }

        private static float TraversalCost(Road road)
            => TraversalCost(road.Start, road.End, road.SpeedLimit);

        private static float TraversalCost(Vector2 start, Vector2 end, float kmph)
        {
            var length = Vector2.Distance(start, end);

            var metersPerSecond = (kmph / 60 / 60) / 1000;

            return length / metersPerSecond;
        }

        private static float ExpectedCost(Road from, Road to) => TraversalCost(from.End, to.Start, Road.MaxSpeed);
    }
}
