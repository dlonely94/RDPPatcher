using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Drawing.Text;
using System.IO;
using System.Security.Principal;

namespace RDPPatcher
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private static String GetWindowsServiceStatus(String serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            string st = "";
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    st = "running";
                    break;
                case ServiceControllerStatus.Stopped:
                    st = "stopped";
                    break;
                case ServiceControllerStatus.Paused:
                    st = "paused";
                    break;
                default:
                    st = "hanged";
                    break;
            }
            return serviceName + " is " + st + "\r\n";
        }

        private static void StopWindowsService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            sc.Stop();
            sc.WaitForStatus(ServiceControllerStatus.Stopped);

        }

        private static void takeOwnership(TextBox tb)
        {
            //string termsrvPath = Environment.GetEnvironmentVariable("windir").ToString() + @"\system32\termsrv.dll";
            string termsrvPath = @"D:\IPA\uYouPlus_18.14.1_3.0.ipa";
            tb.AppendText("Checking termsrv path...\r\n");
            tb.AppendText(termsrvPath + "\r\n");
            var fs = File.GetAccessControl(termsrvPath);
            var sid = fs.GetOwner(typeof(SecurityIdentifier));
            var NTAccount = sid.Translate(typeof(NTAccount));
            tb.AppendText("Current file owner is " + NTAccount.ToString() + "\r\n");
            var adminGroup = new NTAccount(".", "Administrators");
            var adminSID = adminGroup.Translate(typeof(SecurityIdentifier));
            while (sid != adminSID)
            {
                tb.AppendText("Taking ownership...\r\n");
                try
                {
                    fs.SetOwner(adminGroup);
                    File.SetAccessControl(termsrvPath, fs);
                    sid = fs.GetOwner(typeof(SecurityIdentifier));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            tb.AppendText("File owned!!!");

        }

        private string termsrvStatus, sessionenvStatus, umrdpsrvStatus;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            simpleButton2.Enabled = false;
            textBox1.Clear();
            textBox1.Text = "Checking servies status...\r\n";
            termsrvStatus = GetWindowsServiceStatus("TermService");
            sessionenvStatus = GetWindowsServiceStatus("SessionEnv");
            umrdpsrvStatus = GetWindowsServiceStatus("UmRdpService");
            textBox1.AppendText(termsrvStatus);
            textBox1.AppendText(sessionenvStatus);
            textBox1.AppendText(umrdpsrvStatus);
            takeOwnership(textBox1);
            simpleButton2.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
