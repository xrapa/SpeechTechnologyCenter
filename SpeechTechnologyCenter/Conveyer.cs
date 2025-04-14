using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTechnologyCenter
{
    public class Conveyer
    {
        private readonly LinkedList<int> _a = [];
        private readonly LinkedList<int> _b = [];
        private readonly List<Crossing> _crossings = [];
        public int PushA(int val)
        {
            _a.AddFirst(val);
            foreach (var crossing in _crossings)
            {
                _a.AddAfter(crossing.NodeA, new LinkedListNode<int>(crossing.Value));
                crossing.Value = crossing.NodeA.Value;
                crossing.NodeA = crossing.NodeA.Previous!;
                _a.Remove(crossing.NodeA.Next!);
            }
            var result = _a.Last!.Value;
            _a.RemoveLast();

            return result;
        }
        public int PushB(int val)
        {

        }
        public class Crossing
        {
            /// <summary>
            /// общее значение
            /// </summary>
            public int Value { get; set; }
            /// <summary>
            /// до пересечения
            /// </summary>
            public LinkedListNode<int> NodeA { get; set; } = null!;
            /// <summary>
            /// //до пересечения
            /// </summary>
            public LinkedListNode<int> NodeB { get; set; } = null!;
        }
    }
}
