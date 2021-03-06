using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace Ann_Birthday
{
    public partial class Form1 : Form
    {
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter="Excel Workbook|*.xlsx",Multiselect=false})
            {
                if(ofd.ShowDialog()==DialogResult.OK)
                {

                    //Cursor.Current = Cursor.WaitCursor;
                    Cursor.Current = Cursors.WaitCursor;
                    DataTable dt = new DataTable();
                    using(XLWorkbook workbook = new XLWorkbook(ofd.FileName) )
                    {
                        bool isFirstRow = true;
                        var rows = workbook.Worksheet(1).RowsUsed();
                        foreach(var row in rows)
                        {
                            if (isFirstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                    dt.Columns.Add(cell.Value.ToString());
                                isFirstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                    dt.Rows[dt.Rows.Count - 1][i++] = cell.Value.ToString();

                            }
                        }
                        
                        dataGridView1.DataSource = dt.DefaultView;
                        
                        lbl_record.Text = $"Total Records :{(dataGridView1.RowCount)-1}";
                        Cursor.Current = Cursors.Default;
                    }
                }
                //}
                // using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx", Multiselect = false })
                //{
               /* if (ofd.ShowDialog() == DialogResult.OK)
                {*/

                    //Cursor.Current = Cursor.WaitCursor;
                    Cursor.Current = Cursors.WaitCursor;
                    DataTable dt1 = new DataTable();
                    using (XLWorkbook workbook = new XLWorkbook(ofd.FileName))
                    {
                        bool isFirstRow = true;
                        var rows = workbook.Worksheet(1).RowsUsed();
                        foreach (var row in rows)
                        {
                            if (isFirstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                    dt1.Columns.Add(cell.Value.ToString());
                                isFirstRow = false;
                            }
                            else
                            {
                                dt1.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                    dt1.Rows[dt1.Rows.Count - 1][i++] = cell.Value.ToString();

                            }
                        }

                        dataGridView2.DataSource = dt1.DefaultView;

                       /* lbl_record.Text = $"Total Records :{(dataGridView1.RowCount) - 1}";*/
                        Cursor.Current = Cursors.Default;
                    //}
                }
                //}
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Birthday")
            {
                try
                {

                    DataView dv = dataGridView1.DataSource as DataView;
                    if (dv != null)
                    {

                        //dv.RowFilter = txtSearch.Text;
                        String date = dateTimePicker1.Text.Substring(0, 5);
                        dv.RowFilter = "Birthday like'" + date + "%'";
                        lbl_record.Text = $"Total Records :{(dataGridView1.RowCount) - 1}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if(comboBox1.Text == "Anniversaryday")
            {
                try
                {

                    DataView dv = dataGridView1.DataSource as DataView;
                    if (dv != null)
                    {

                        //dv.RowFilter = txtSearch.Text;
                        String date = dateTimePicker1.Text.Substring(0,5);
                        dv.RowFilter = "Anniversaryday like'" + date + "%'";
                        lbl_record.Text = $"Total Records :{(dataGridView1.RowCount) - 1}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnSearch.PerformClick();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            dateTimePicker1.Format= DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void send_Click(object sender, EventArgs e)
        {

           
                login = new NetworkCredential("suryafandasydream11@gmail.com", "Surya@123");
                client = new SmtpClient("smtp.gmail.com");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;

                client.Credentials = login;

                msg = new MailMessage { From = new MailAddress("suryafandasydream11@gmail.com", "Surya Kumar", Encoding.UTF8) };
                msg.Subject = msgbox.Text;
            //msg.Body = "Wishes you all NORDEX Management Trainees multiple cc:)";
            string mailbody = "";
            for (int r = 0; r < (dataGridView1.RowCount - 1); r++)
            {
             
                String name = Convert.ToString(dataGridView1.Rows[r].Cells["name"].Value);
                String dep = Convert.ToString(dataGridView1.Rows[r].Cells["department"].Value);
                mailbody += name +"-"+ dep + "<br>";
            }
            msg.Body = mailbody;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            msg.Priority = MailPriority.Normal;
            for (int r = 0; r < (dataGridView1.RowCount - 1); r++)
            {
                String addr = Convert.ToString(dataGridView1.Rows[r].Cells["email"].Value);
                //MessageBox.Show(addr);
                msg.To.Add(new MailAddress(addr));
            }
            for (int r = 0; r < (dataGridView2.RowCount - 1); r++)
            {
                String ccaddr = Convert.ToString(dataGridView2.Rows[r].Cells["email"].Value);
                if (!string.IsNullOrEmpty(ccaddr))//cc account
                {
                    msg.CC.Add(new MailAddress(ccaddr));
                    //msg.To.Add//cc account
                }
            }
            foreach(string filename in openFileDialog1.FileNames)
            {
                if (File.Exists(filename))
                {
                    string fname = Path.GetFileName(filename);
                    msg.Attachments.Add(new Attachment(filename));
                }
            } 

                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                string userstate = "Sending...";
                client.SendAsync(msg, userstate);
 
                    client.SendCompleted += new SendCompletedEventHandler(SendCompleteCallback);
              
            




        }
        private static void SendCompleteCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show(string.Format("{0} send cancelled.", e.UserState), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (e.Error != null)
                MessageBox.Show(string.Format("{0} {1}", e.UserState, e.Error), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Your message sended successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbl_record_Click(object sender, EventArgs e)
         {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
              
                mail.Subject = msgbox.Text;
                string mailbody = "";
                for (int r = 0; r < (dataGridView1.RowCount - 1); r++)
                {

                    String name = Convert.ToString(dataGridView1.Rows[r].Cells["name"].Value);
                    String dep = Convert.ToString(dataGridView1.Rows[r].Cells["department"].Value);
                    mailbody += name + "-" + dep ;
                }
                //mail.Body = mailbody;
                Outlook.Recipients oRecips = mail.Recipients;
                for (int r = 0; r < (dataGridView1.RowCount - 1); r++)
                {
                    String addr = Convert.ToString(dataGridView1.Rows[r].Cells["email"].Value);
                    Outlook.Recipient oRecip = oRecips.Add(addr);
                    oRecip.Resolve();
                }


              
                for (int r = 0; r < (dataGridView2.RowCount - 1); r++)
                {
                    String ccaddr = Convert.ToString(dataGridView2.Rows[r].Cells["email"].Value);
                    Outlook.Recipient recipCc = mail.Recipients.Add(ccaddr);
                    recipCc.Type = (int)Outlook.OlMailRecipientType.olCC;

                }

                //Attachment
                String attachmentDisplayName = "MyAttachment";
                string imageSrc = "";
                foreach (string filename in openFileDialog1.FileNames)
                {
                    if (File.Exists(filename))
                    {
                        imageSrc = filename;
                    }
                }
                Outlook.Attachment oAttach = mail.Attachments.Add(imageSrc, Outlook.OlAttachmentType.olByValue, null, attachmentDisplayName);
                string imageContentid = "someimage.jpg";
                oAttach.PropertyAccessor.SetProperty("http://schemas.microsoft.com/mapi/proptag/0x3712001E", imageContentid);
                String wishes_header = "May this happy day in your life be the beginning of a year filled  with joy, good health and great success. Enjoy it to the fullest because today is your day. ";
                String wishes_footer = "Happy Birthday & have a great year ahead!";
               /* mail.Body = mailbody;*/
                mail.HTMLBody= String.Format(
               "<body> <h2 style='font-family: cursive; font-size: 15px;'>Greetings! </h2><br><br> <h2 style='font-family: cursive;font-size: 15px;'>{0}</h1><br><br><h2>{1}</h2>/> <br><br><img src=\"cid:{2}\"><br><br><h1 style='font-family: cursive; font-size: 15px;'>{3}</h1>Regards,<br>Nordex PLC</body>", wishes_header, mailbody,
               imageContentid, wishes_footer);


                mail.Importance = Outlook.OlImportance.olImportanceNormal;
                mail.Send();

                MessageBox.Show("Message sended successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private MailMessage MailMessage()
        {
            throw new NotImplementedException();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.ShowDialog();
            foreach (string filename in openFileDialog1.FileNames)
            {
                label2.Text = filename.ToString();
            }
        }

        private void msgbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
