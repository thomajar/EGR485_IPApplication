using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class USBRelayController
    {
        private SerialPort sp;

        private const char CARRIAGE_RETURN = (char)13;

        public USBRelayController(int portNum)
        {
            sp = new SerialPort();
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
                throw ex;
            }
        }

        public bool ReadRelay0Status()
        {
            try
            {
                sp.DiscardInBuffer();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Unable to discard input buffer.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
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
                throw ex;
            }
            
            Thread.Sleep(10);
            String message;
            try
            {
                message = sp.ReadExisting();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay0Status : Unable to read response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
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
                throw ex;
            }
            return status;
        }

        public bool ReadRelay1Status()
        {
            try
            {
                sp.DiscardInBuffer();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Unable to discard input buffer.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
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
                throw ex;
            }

            Thread.Sleep(10);
            String message;
            try
            {
                message = sp.ReadExisting();
            }
            catch (Exception inner)
            {
                string errMsg = "USBRelayController.ReadRelay1Status : Unable to read response from controller.";
                RelayControllerException ex = new RelayControllerException(errMsg, inner);
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
                throw ex;
            }
            return status;
        }

        public void SetRelay0Status(bool status)
        {
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
                    throw ex;
                }
            }
        }

        public void SetRelay1Status(bool status)
        {
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
                    throw ex;
                }
            }
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
