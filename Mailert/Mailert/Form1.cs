using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mailert
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();                        
        }
        public string version;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            checkTimer.Enabled = true;
            this.Hide();
            notifyIcon1.BalloonTipText = "End Process.";
            //notifyIcon1.BalloonTipTitle = "End";
            
            //version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label4.Text = "Ver " + version;
            notifyIcon1.Text = "Mailert Email Notifier" + Environment.NewLine + "Ver" + version;


        }

  

        public bool RunCheck()
        {
            try
            {
                Microsoft.Office.Interop.Outlook.Application myApp = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.NameSpace mapiNameSpace = myApp.GetNamespace("MAPI");
                Microsoft.Office.Interop.Outlook.MAPIFolder myInbox = mapiNameSpace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);

                Microsoft.Office.Interop.Outlook.Items oItems = myInbox.Items.Restrict("[UnRead] = true");

                if (oItems.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                DialogResult dl = MessageBox.Show("A connection issue prevented Mailert from checking.  To shut down Mailert, select Abort.  Otherwise you can Retry now or Ignore this hiccup and let Mailert go back to monitoring.", "Warning", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);
                if (dl == DialogResult.Abort)
                {
                    Application.Exit();
                }
                else if (dl == DialogResult.Retry)
                {

                }
                else
                {
                    this.Hide();
                }
                return false;
            }
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {            

            if (RunCheck())
            {
                Screen screen = Screen.FromPoint(Cursor.Position);
                this.Location = screen.Bounds.Location;                
                this.Show();
                this.CenterToScreen();
            }
            else
            {
                this.Hide();
            }            

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            checkTimerLong.Enabled = false; 
            checkTimer.Enabled = true;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            
            notifyIcon1.ShowBalloonTip(1);
           
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dl = MessageBox.Show("End the Mailert Notifier?", "End Mailert Notifier", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dl == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnWait_Click(object sender, EventArgs e)
        {
            this.Hide();
            checkTimer.Enabled = false;
            checkTimerLong.Enabled = true;
        }

        private void checkTimerLong_Tick(object sender, EventArgs e)
        {           

            if (RunCheck())
            {
                Screen screen = Screen.FromPoint(Cursor.Position);
                this.Location = screen.Bounds.Location;
                this.Show();
                this.CenterToScreen();
            }
            else
            {
                this.Hide();
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            this.Size = new Size(400, 222);
            label2.Visible = true;
            label3.Visible = true;
            tbOkMins.Visible = true;
            tbWaitMins.Visible = true;            
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            

            checkTimer.Interval = Convert.ToInt32(Convert.ToDouble(tbOkMins.Text)*1000*60);
            checkTimerLong.Interval = Convert.ToInt32(Convert.ToDouble(tbWaitMins.Text)*1000*60);

            this.Size = new Size(400, 188);
            label2.Visible = false;
            label3.Visible = false;
            tbOkMins.Visible = false;
            tbWaitMins.Visible = false;

        }
    }
}
