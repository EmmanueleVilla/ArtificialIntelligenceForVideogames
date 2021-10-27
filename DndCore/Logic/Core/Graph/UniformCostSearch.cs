using Core.Map;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Graph
{
    public class UniformCostSearch
    {
        public List<CellInfo> Search(CellInfo from, IMap map)
        {
            var movement = new List<Speed>(from.Creature.Movements);
            if(movement.TrueForAll(x => x.Item2 <= 0))
            {
                return new List<CellInfo>();
            }
            return new List<CellInfo>();
        }

        /*
        Dictionary<Tuple<int, int>, int> cost = new Dictionary<Tuple<int, int>, int>();

        // returns the minimum cost in a vector( if
        // there are multiple goal states)
        List<int> Search(List<int> goal, int start)
        {
            // minimum cost upto
            // goal state from starting
            // state
            List<int> answer = new List<int>();

            // create a priority queue
            List<Tuple<int, int>> queue = new List<Tuple<int, int>>();

            // set the answer vector to max value
            for (int i = 0; i < goal.Count; i++)
                answer.Add(int.MaxValue);

            // insert the starting index
            queue.Add(new Tuple<int, int>(0, start));

            // map to store visited node
            Dictionary<int, int> visited = new Dictionary<int, int>();

            // count
            int count = 0;

            // while the queue is not empty
            while (queue.Count > 0)
            {

                // get the top element of the
                // priority queue
                Tuple<int, int> q = queue[0];
                Tuple<int, int> p = new Tuple<int, int>(-q.Item1, q.Item2);

                // pop the element
                queue.RemoveAt(0);


                // check if the element is part of
                // the goal list
                if (goal.Contains(p.Item2))
                {

                    // get the position
                    int index = goal.IndexOf(p.Item2);

                    // if a new goal is reached
                    if (answer[index] == int.MaxValue)
                        count++;

                    // if the cost is less
                    if (answer[index] > p.Item1)
                        answer[index] = p.Item1;

                    // pop the element
                    queue.RemoveAt(0);

                    // if all goals are reached
                    if (count == goal.Count)
                        return answer;
                }

                // check for the non visited nodes
                // which are adjacent to present node
                if (!visited.ContainsKey(p.Item2))
                    for (int i = 0; i < graph[p.Item2].Count; i++)
                    {

                        // value is multiplied by -1 so that
                        // least priority is at the top
                        queue.Add(new Tuple<int, int>((p.Item1 + (cost.ContainsKey(new Tuple<int, int>(p.Item2, graph[p.Item2][i])) ? cost[new Tuple<int, int>(p.Item2, graph[p.Item2][i])] : 0)) * -1,
                        graph[p.Item2][i]));
                    }

                // mark as visited
                visited[p.Item2] = 1;
            }

            return answer;
        }
        */
    }
}
