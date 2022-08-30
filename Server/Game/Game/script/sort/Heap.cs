using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapSort

{
	public class Heap
	{
        public static void ShowSord(PlayerInfo[] playerInfos)
        {
            foreach (var playerinfo in playerInfos)
            {
                Console.Write($"{playerinfo.score} ");
            }
            Console.WriteLine();
        }
        public static void HeapSort(PlayerInfo[] playerInfos)
        {
            MaxHeap(playerInfos);
            for (int i = playerInfos.Length - 1; i > 0; i--)
            {
                Swap(ref playerInfos[0], ref playerInfos[i]);
                Heapify(playerInfos, 0, i);
            }
        }
        public static void MaxHeap(PlayerInfo[] playerInfos)
        {
            for (int i = playerInfos.Length / 2 - 1; i >= 0; i--)
            {
                Heapify(playerInfos, i, playerInfos.Length);
            }
        }
        public static void Heapify(PlayerInfo[] playerInfos, int index, int size)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int large = index;
            if (left < size && playerInfos[left].score < playerInfos[large].score)
            {
                large = left;
            }
            if (right < size && playerInfos[right].score < playerInfos[large].score)
            {
                large = right;
            }
            if (index != large)
            {
                Swap(ref playerInfos[index], ref playerInfos[large]);
                Heapify(playerInfos, large, size);
            }
        }
        public static void Swap(ref PlayerInfo first, ref PlayerInfo second)
        {
            PlayerInfo t = new PlayerInfo();
            t.id = first.id;
            t.score = first.score;
            first = second;
            second = t;
        }
    }
}
