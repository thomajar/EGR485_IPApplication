using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.threading
{
    class QueueElement
    {
        private String type;
        private Object data;

        /// <summary>
        /// Type of data stored in QueueElement
        /// </summary>
        public String Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Data stored in QueueElement
        /// </summary>
        public Object Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// An object containing a description of the data type and data
        /// itself.
        /// </summary>
        /// <param name="type">Code for data type.</param>
        /// <param name="data">Data to store in element.</param>
        public QueueElement(String type, Object data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
