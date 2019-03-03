using System;
using System.Management;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MbnApi;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CSharp_SMS
{
    public partial class Form_SMS_Sender : Form
    {
        private IMbnInterfaceManager m_MbnInterfaceManager;
        private IMbnConnectionManager m_MbnConnectionManager;
        private IMbnInterface m_MbnInterface;
        private IMbnConnection m_MbnConnection;
        private IMbnDeviceServicesContext m_MbnDeviceServicesContext;
        private IMbnDeviceServicesManager m_MbnDeviceServicesManager;
        private IMbnInterface inf;
        System.Timers.Timer t = new System.Timers.Timer(10000);

        public Form_SMS_Sender()
        {
            InitializeComponent();
            InitializeManagers();
            this.TopMost = true;
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();

            MbnInterfaceManager mbnInfMgr = new MbnInterfaceManager();
            IMbnInterfaceManager infMgr = (IMbnInterfaceManager)mbnInfMgr;
            IMbnInterface[] interfaces = (IMbnInterface[])infMgr.GetInterfaces();
            inf = interfaces[0];
            MBN_INTERFACE_CAPS infcap = inf.GetInterfaceCapability();
            IMbnRegistration registrationInterface = m_MbnInterface as IMbnRegistration;
            label1.Text = ((infcap.manufacturer + " - " + infcap.model).ToString());
        }

        private void aanpassen(object sender, EventArgs e)
        {
            Thread.Sleep(1000);

            if (radioButton1.Checked)
            {              
                IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
                uint requestId;
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20610", UInt32.Parse("2147483708"), out requestId);
            }

            else if (radioButton2.Checked)
            {
                IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
                uint requestId;
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20408", UInt32.Parse("2147483708"), out requestId);
            }
            else if (radioButton3.Checked)
            {
                IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
                uint requestId;
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20416", UInt32.Parse("2147483708"), out requestId);
            }
            else if (radioButton4.Checked)
            {
                IMbnRegistration registrationInterface = m_MbnInterfaceManager.GetInterface(inf.InterfaceID) as IMbnRegistration;
                uint requestId;
                registrationInterface.SetRegisterMode(MBN_REGISTER_MODE.MBN_REGISTER_MODE_MANUAL, "20601", UInt32.Parse("2147483708"), out requestId);
            }
            Thread.Sleep(1000);

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(3,e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(2, e);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(1, e);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            aanpassen(4, e);
        }
        public void InitializeManagers()
        {
            try
            {
                if (m_MbnInterfaceManager == null)
                {
                    m_MbnInterfaceManager = (IMbnInterfaceManager)new MbnInterfaceManager();
                }

                if (m_MbnConnectionManager == null)
                {
                    m_MbnConnectionManager = (IMbnConnectionManager)new MbnConnectionManager();
                }

                if (m_MbnDeviceServicesManager == null)
                {
                    m_MbnDeviceServicesManager = (IMbnDeviceServicesManager)new MbnDeviceServicesManager();
                }

            }
            catch (Exception e)
            {

            }
        }
        private void t_Elapsed(object sender,System.Timers.ElapsedEventArgs e)
        {
            //IMbnSignal sig = (IMbnSignal)inf;
            //uint signalStrength = sig.GetSignalStrength();
            //label2.Invoke((Action)delegate
            //{ label2.Text = "Signaalsterkte: " + signalStrength; });

            Process cmd = new Process();
            cmd.StartInfo.FileName = "netsh.exe";
            cmd.StartInfo.Arguments = "mbn show signal interface=\"mobiel*\"";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.Start();
            string output = cmd.StandardOutput.ReadToEnd();
            output = output.Substring(output.IndexOf("%") - 2, 3);
            label2.Invoke((Action)delegate
            { label2.Text = "Signaalsterkte: " + output; });
            cmd.WaitForExit();
        }
    }
}
