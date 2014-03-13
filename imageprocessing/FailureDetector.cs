using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAF_OpticalFailureDetector.threading;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    public unsafe class FailureDetector
    {
        private int minimumContrast;
        private int noiseRange;

        List<CircularQueue<QueueElement>> consumers;
        List<CircularQueue<QueueElement>> producers;

        public FailureDetector()
        {
            consumers = new List<CircularQueue<QueueElement>>();
            producers = new List<CircularQueue<QueueElement>>();
        }
        /*
        public void AddConsumer(CircularQueue<QueueElement> consumer)
        {
            bool foundConsumer = false;
            // check if already in consumer list
            foreach (CircularQueue<QueueElement> item in consumers)
            {
                if (item.Name.Equals(consumer.Name))
                {
                    foundConsumer = true;
                }
            }
            if (!foundConsumer)
            {
                consumers.Add(consumer);
            }
        }

        public void AddProducer(CircularQueue<QueueElement> producer)
        {
            bool foundProducer = false;
            // check if already in producer list
            foreach (CircularQueue<QueueElement> item in producers)
            {
                if (item.Name.Equals(producer.Name))
                {
                    foundProducer = true;
                }
            }
            if (!foundProducer)
            {
                producers.Add(producer);
            }
        }

        public void RemoveConsumer(CircularQueue<QueueElement> consumer)
        {
            for (int i = 0; i < consumers.Count; i++)
            {
                if (consumers[i].Name.Equals(consumer.Name))
                {
                    consumers.Remove(consumer);
                    i--;
                }
            }
        }

        public void RemoveProducer(CircularQueue<QueueElement> producer)
        {
            for (int i = 0; i < producers.Count; i++)
            {
                if (producers[i].Name.Equals(producer.Name))
                {
                    consumers.Remove(producer);
                    i--;
                }
            }
        }*/

        public void Start()
        {

        }
        public void Stop()
        {

        }

        private void Process()
        {

        }


        private enum IPState
        {
            RESET,
            INIT,
            ROI,
            PROC
        };
        

    }
}
