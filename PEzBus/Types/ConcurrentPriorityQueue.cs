using System.Diagnostics.CodeAnalysis;

namespace PEzBus.Types;

public class ConcurrentPriorityQueue <TElement,TPriority> where TPriority: notnull
{
       /// <summary>
        /// Lock object to synchronize access to the priority queue
        /// </summary>
        private object m_Lock = new();
        /// <summary>
        /// Nested PriorityQueue (no need to reinvent the wheel)
        /// </summary>
        protected PriorityQueue<TElement, ValueTuple<TPriority,int>> m_Queue;
        /// <summary>
        /// The Lookup table that helps the PriorityQueue to conserve the order of objects with the same priority.
        /// The int value represents the count of elements of priority TPriority.
        /// It's used by the comparator as a second comparison property.
        /// </summary>
        private Dictionary<TPriority, int> m_QHelper = new();

        public ConcurrentPriorityQueue()
        {
            m_Queue = new();
        }

        public ConcurrentPriorityQueue(IComparer<ValueTuple<TPriority, int>> comparer)
        {
            m_Queue = new(comparer);
        }
        public int Count => m_Queue.Count;
        public bool IsSynchronized => true;
        public object SyncRoot => m_Lock;
        /// <summary>
        /// Adds one element to the queue.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryEnqueue(KeyValuePair<TElement, TPriority> item)
        {
            lock (m_Lock)
            {
                var count = IncrementCountForPriority(item.Value)+1;
                m_Queue.Enqueue(item.Key, (item.Value, IncrementCountForPriority(item.Value)));
                return true;
            } 
        }
        /// <summary>
        /// Removes the top most element with highest priority from the queue and returns it.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryDequeue(out KeyValuePair<TElement, TPriority> item)
        {   
            lock (m_Lock)
            {
                var result = m_Queue.TryDequeue(out var key, out var value);
                item = result ? new KeyValuePair<TElement, TPriority>(key!,value!.Item1) : new KeyValuePair<TElement, TPriority>();
                if(result) DecrementCountForPriority(item.Value);
                return result;
            }
        }
        /// <summary>
        /// Increments the messages count for a priority in the lookup table 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns>The count of messages that have the same priority</returns>
        private int IncrementCountForPriority(TPriority priority)
        {
            if(! m_QHelper.TryAdd(priority,0) )
            {
                m_QHelper[priority] = m_QHelper[priority] + 1;
                return m_QHelper[priority];
            }
            return 0;        
        }
        /// <summary>
        /// Decrements the messages count for a priority in the lookup table 
        /// </summary>
        /// <param name="priority"></param>
        private void DecrementCountForPriority(TPriority priority)
        {
            if(m_QHelper.TryGetValue(priority, out var count))
            {
                m_QHelper[priority] = count == 1 ? 0 : count - 1;
            }
        }
}