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

        private static void takeOwnership()
        {

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
            textBox1.Text = "Checking servies status...\r\n";
            termsrvStatus = GetWindowsServiceStatus("TermService");
            sessionenvStatus = GetWindowsServiceStatus("SessionEnv");
            umrdpsrvStatus = GetWindowsServiceStatus("UmRdpService");
            textBox1.AppendText(termsrvStatus);
            textBox1.AppendText(sessionenvStatus);
            textBox1.AppendText(umrdpsrvStatus);
            string termsrvPath = Environment.GetEnvironmentVariable("windir") + @"\system32\termsrv.dll";
            textBox1.AppendText("Checking termsrv.dll path...\r\n");
            textBox1.AppendText(termsrvPath + "\r\n");




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
