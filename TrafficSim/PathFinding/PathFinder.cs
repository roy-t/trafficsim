using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrafficSim.Network;

namespace TrafficSim.PathFinding
{
    public static class PathFinder
    {
        public static PathFinderResult FindPath(Junction start, Junction goal)
        {
            if (start == goal)
            {
                return new PathFinderResult(new List<Road>(), 0.0f);
            }

            var open = new MinHeap();
            var nodes = new Dictionary<Junction, MinHeapNode>();

            var head = new MinHeapNode(start, null, null, 0, ExpectedCost(start, goal));
            open.Push(head);
            nodes.Add(head.Junction, head);

            while (open.HasNext)
            {
                var current = open.Pop();
                if (current.Junction == goal)
                {
                    return ReconstructPath(current);
                }

                foreach (var road in current.Junction.Outgoing)
                {
                    var nextJunction = GetOpposite(current.Junction, road);
                    var costSoFar = current.CostSoFar + TraversalCost(road);

                    if (nodes.TryGetValue(nextJunction, out var node))
                    {
                        if (node.CostSoFar > costSoFar)
                        {
                            node = new MinHeapNode(nextJunction, current, road, costSoFar, costSoFar + ExpectedCost(nextJunction, goal));
                            open.Push(node);
                            nodes[nextJunction] = node;
                        }
                    }
                    else
                    {
                        node = new MinHeapNode(nextJunction, current, road, costSoFar, costSoFar + ExpectedCost(nextJunction, goal));
                        open.Push(node);
                        nodes.Add(node.Junction, node);
                    }
                }
            }

            return new PathFinderResult();
        }

        private static Junction GetOpposite(Junction from, Road road)
        {
            if (road.StartJunction == from)
            {
                return road.EndJunction;
            }

            return road.StartJunction;
        }

        private static PathFinderResult ReconstructPath(MinHeapNode node)
        {
            var path = new List<Road>();

            var current = node;
            while (current != null && current.CameVia != null)
            {
                path.Add(current.CameVia);
                current = current.CameFrom;
            }

            path.Reverse();
            return new PathFinderResult(path, node.CostSoFar);
        }

        private static float TraversalCost(Road road)
            => TraversalCost(road.Start, road.End, road.SpeedLimit);

        private static float TraversalCost(Vector2 start, Vector2 end, float kmph)
        {
            var length = Vector2.Distance(start, end);

            var metersPerSecond = (kmph * 1000) / 60 / 60;

            return length / metersPerSecond;
        }

        private static float ExpectedCost(Junction from, Junction to) => TraversalCost(from.Position, to.Position, Road.MaxSpeedLimit);
    }
}
