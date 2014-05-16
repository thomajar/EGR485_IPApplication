using log4net;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.relay
{
    class USBRelayController
    {
        // constants
        private const char CARRIAGE_RETURN = (char)13;

        // private vars
        private Semaphore sem;
        private static readonly ILog log = LogManager.GetLogger(typeof(USBRelayController));
        private SerialPort sp;
        private bool isOpen;

        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        private static USBRelayController instance;

        /// <summary>
        /// Retrieves an instance of the singleton.
        /// </summary>
        public static USBRelayController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new USBRelayController();
                }
                return instance;
            }
        }

        /// <summary>
        /// Creates the singleton Relay controller.
        /// </summary>
        private USBRelayController()
        {
            sem = new Semaphore(0, 1);
            isOpen = false;
            sp = new SerialPort();
            sem.Release();
        }


        /// <summary>
        /// Attempts to open up serial port connection to com port specified by portNum.
        /// </summary>
        /// <param name="portNum">Com port to connect to.</param>
        /// <exception cref="RelayControllerException"></exception>
        public void Open(int portNum)
        {
            sem.WaitOne();
            sp.PortName = "COM" + portNum.ToString();
            sp.BaudRate = 9600;
            sp.DataBits = 8;
            try
            {
                sp.Open();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.USBRelayController : Unable to open serial port.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            isOpen = true;
            sem.Release();
        }

        public void Close()
        {
            sem.WaitOne();
            try
            {
                sp.Close();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.Close : Unable to close serial port.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            sem.Release();
        }

        /// <summary>
        /// Reads relay status of port 0.
        /// </summary>
        /// <exception cref="RelayControllerException"></exception>
        /// <returns>True if on, False if off.</returns>
        public bool ReadRelay0Status()
        {
            sem.WaitOne();
            try
            {
                sp.DiscardInBuffer();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Unable to discard input buffer.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }

            try
            {
                sp.Write("relay read 0" + CARRIAGE_RETURN);
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Unable to write command.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            // wait a bit for response from relay board
            Thread.Sleep(15);
            String message;
            try
            {
                message = sp.ReadExisting();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Unable to read response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            bool status;
            try
            {
                status = message.Contains("on");
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Received null response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            sem.Release();
            return status;
        }

        /// <summary>
        /// Reads relay status of port 1
        /// </summary>
        /// <exception cref="RelayControllerException"></exception>
        /// <returns>True if on, False if off.</returns>
        public bool ReadRelay1Status()
        {
            sem.WaitOne();
            try
            {
                sp.DiscardInBuffer();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Unable to discard input buffer.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }

            try
            {
                sp.Write("relay read 1" + CARRIAGE_RETURN);
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Unable to write command.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }

            // sleep a bit to wait for response
            Thread.Sleep(15);
            String message;
            try
            {
                message = sp.ReadExisting();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Unable to read response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            bool status;
            try
            {
                status = message.Contains("on");
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Received null response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
                log.Error(errMsg, ex);
                sem.Release();
                throw ex;
            }
            sem.Release();
            return status;
        }

        /// <summary>
        /// Turns port 0 on or off.
        /// </summary>
        /// <param name="status">True to turn on, False to turn off.</param>
        /// <exception cref="RelayControllerException"></exception>
        public void SetRelay0Status(bool status)
        {
            sem.WaitOne();
            if (status)
            {
                try
                {
                    sp.Write("relay on 0" + CARRIAGE_RETURN);
                }
                catch (Exception inner)
                {
                    string errMsg = "USBRelayController.SetRelay0Status : Unable to write command to relay board.";
                    RelayControllerException ex = new RelayControllerException(errMsg, inner);
                    log.Error(errMsg, ex);
                    sem.Release();
                    throw ex;
                }
            }
            else
            {
                try
                {
                    sp.Write("relay off 0" + CARRIAGE_RETURN);
                }
                catch (Exception inner)
                {
                    string errMsg = "USBRelayController.SetRelay0Status : Unable to write command to relay board.";
                    RelayControllerException ex = new RelayControllerException(errMsg, inner);
                    log.Error(errMsg, ex);
                    sem.Release();
                    throw ex;
                }
            }
            sem.Release();
        }

        /// <summary>
        /// Turns port 1 on or off.
        /// </summary>
        /// <param name="status">True for on, False for off.</param>
        /// <exception cref="FailureDetectorException"></exception>
        public void SetRelay1Status(bool status)
        {
            sem.WaitOne();
            if (status)
            {
                try
                {
                    sp.Write("relay on 1" + CARRIAGE_RETURN);
                }
                catch (Exception inner)
                {
                    string errMsg = "USBRelayController.SetRelay1Status : Unable to write command to relay board.";
                    RelayControllerException ex = new RelayControllerException(errMsg, inner);
                    log.Error(errMsg, ex);
                    sem.Release();
                    throw ex;
                }
            }
            else
            {
                try
                {
                    sp.Write("relay off 1" + CARRIAGE_RETURN);
                }
                catch (Exception inner)
                {
                    string errMsg = "USBRelayController.SetRelay1Status : Unable to write command to relay board.";
                    RelayControllerException ex = new RelayControllerException(errMsg, inner);
                    log.Error(errMsg, ex);
                    sem.Release();
                    throw ex;
                }
            }
            sem.Release();
        }
    }
    public class RelayControllerException : System.Exception
    {
        public RelayControllerException() : base() { }
        public RelayControllerException(string message) : base(message) { }
        public RelayControllerException(string message, System.Exception inner) : base(message, inner) { }
        protected RelayControllerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
