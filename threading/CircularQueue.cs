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
        private object _queueLock;
        private String name = "";
        private List<T> queue;
        private int insertIndex;
        private int removeIndex;
        private int maxSize;
        private int elements;
        private const int SIZE_LIMIT = 10000;

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

            _queueLock = new object();
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
            // lock is used to ensure that only thread attempts to modify the queue at once
            lock (_queueLock)
            {
                // verify not popping old object
                if (((removeIndex + 1) % maxSize) != insertIndex)
                {
                    try
                    {
                        data = queue[((removeIndex + 1) % maxSize)];
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    result = true;
                }
            }
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
            // lock is used to ensure that only thread attempts to modify the queue at once
            lock (_queueLock)
            {
                // verify not popping old object
                if (((removeIndex + 1) % maxSize) != insertIndex)
                {
                    removeIndex = (removeIndex + 1) % maxSize;
                    try
                    {
                        data = queue[removeIndex];
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                    elements--;
                    result = true;
                }
            }
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

            // lock is used to ensure that only thread attempts to modify the queue at once
            lock (_queueLock)
            {
                while (elements > 0)
                {
                    if (((removeIndex + 1) % maxSize) != insertIndex)
                    {
                        removeIndex = (removeIndex + 1) % maxSize;
                        try
                        {
                            data.Add(queue[removeIndex]);
                        }
                        catch (Exception ex)
                        {

                            return false;
                        }

                        elements--;
                    }
                }
                if (data.Count > 0)
                {
                    result = true;
                }
                
            }
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
            // lock is used to ensure that only thread attempts to modify the queue at once
            lock (_queueLock)
            {
                // verify that inserting into empty index
                if (insertIndex != removeIndex)
                {
                    // queue has not been populated all the way yet, need to fill
                    if (queue.Count < maxSize)
                    {
                        try
                        {
                            queue.Insert(queue.Count, data);
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            queue[insertIndex] = data;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }

                    }

                    elements++;
                    insertIndex = (insertIndex + 1) % maxSize;
                    result = true;
                }
                else
                {
                    string error = "Something went wrong.";
                    if (elements >= 75)
                    {
                        elements = maxSize - 1;
                    }
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Resets the insert and remove index to default.
        /// </summary>
        public void reset()
        {
            lock (_queueLock)
            {
                insertIndex = 0;
                elements = 0;
                removeIndex = maxSize - 1;
            }
        }
    }    
}
