using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.threading
{
    public class CircularQueue<T>
    {
        private String name = "";
        private List<T> queue;
        private int insertIndex;
        private int removeIndex;
        private int maxSize;
        private int elements;
        private const int SIZE_LIMIT = 1000;
        private static Semaphore sem;

        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Creates a new circular queue method with up to maxSize number
        /// of elements in it.  This is a multi-thread safe object.
        /// </summary>
        /// <param name="maxSize">Number of elements initially in thread
        /// queue.</param>
        public CircularQueue(String consumer, int maxSize)
        {
            // create a thread safe synchronization method that only allows
            // one thread to access the circular queue at once. It is 
            // initially owned by creating thread.
            sem = new Semaphore(0, 1);

            this.name = consumer;
            // verify passed max size is less that SIZE_LIMIT
            if (maxSize > SIZE_LIMIT)
            {
                this.maxSize = SIZE_LIMIT;
            }
            else
            {
                this.maxSize = maxSize;
            }

            // create the circular queue list so that it does not have
            // to be dynamically allocated during runtime.
            queue = new List<T>(this.maxSize);

            // start insert and remove at 0 index
            insertIndex = 0;
            removeIndex = this.maxSize-1;
            elements = 0;

            // exit critical section
            sem.Release();
        }

        /// <summary>
        /// Number of elements currently in queue.
        /// </summary>
        public int Count
        {
            get
            {
                return elements;
            }
        }

        /// <summary>
        /// Method retrieves data from circular queue at remove index.
        /// This does not increment the remove index pointer
        /// </summary>
        /// <param name="data">Place to store returned data.</param>
        /// <returns>True upon success.</returns>
        public Boolean peek(ref T data)
        {
            Boolean result = false;
            // wait for ownership of thread object
            sem.WaitOne();
            // verify not popping old object
            if (((removeIndex + 1) % maxSize) != insertIndex)
            {
                data = queue[((removeIndex + 1) % maxSize)];
                result = true;
            }
            sem.Release();
            return result;
        }

        /// <summary>
        /// Method retrieves data from circular queue at remove index.
        /// </summary>
        /// <param name="data">Place to store returned data.</param>
        /// <returns>True upon success.</returns>
        public Boolean pop(ref T data)
        {
            Boolean result = false;
            // wait for ownership of thread object
            sem.WaitOne();
            // verify not popping old object
            if (((removeIndex + 1) % maxSize) != insertIndex)
            {
                removeIndex = (removeIndex + 1) % maxSize;
                data = queue[removeIndex];
                elements--;
                result = true;
            }
            sem.Release();
            return result;
        }

        /// <summary>
        /// Method retrieves data from circular queue between
        /// remove index and insert index.
        /// </summary>
        /// <param name="data">List to store returned data to.</param>
        /// <returns>True upon success</returns>
        public Boolean popAll(ref List<T> data)
        {
            Boolean result = false;
            sem.WaitOne();
            while (elements > 0)
            {
                if(((removeIndex + 1) % maxSize) != insertIndex)
                {
                    removeIndex = (removeIndex + 1) % maxSize;
                    data.Add(queue[removeIndex]);
                    elements--;
                }
            }
            if (data.Count > 0)
            {
                result = true;
            }
            sem.Release();
            return result;
        }

        /// <summary>
        /// Method pushes data onto the circular queue at the insert index.
        /// </summary>
        /// <param name="data">Data to push to queue.</param>
        /// <returns>True upon success.</returns>
        public Boolean push(T data)
        {
            Boolean result = false;
            // wait for ownership of thread object
            sem.WaitOne();
            // verify not inserting into object that has not been removed
            if(insertIndex != removeIndex)
            {
                if (queue.Count < maxSize)
                {
                    queue.Insert(queue.Count, data);
                }
                else
                {
                    queue[insertIndex] = data;
                }
                
                elements++;
                result = true;
            }
            insertIndex = (insertIndex + 1) % maxSize;
            sem.Release();
            return result;
        }

        /// <summary>
        /// Resets the insert and remove index to default.
        /// </summary>
        public void reset()
        {
            sem.WaitOne();
            insertIndex = 0;
            removeIndex = maxSize - 1;
            sem.Release();
        }
    }    
}
