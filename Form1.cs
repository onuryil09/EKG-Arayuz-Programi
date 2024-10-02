using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{

    
    public partial class Form1 : Form
    {
        long b;     // b= BPM
        long h;     // h= HRV
        long n;     // n= Nefes
        long minm1 = 0, maxm1 = 30;
        long minm2 = 0, maxm2 = 30;
        long minm3 = 0, maxm3 = 30;
        


        public Form1()
        {
            InitializeComponent();
            //serialPort1.BaudRate = 9600;
        }

        private string data;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                timer1.Start();
                button1.Enabled = false;
                button2.Enabled = true;
                label4.Text = "Bağlantı açık";
                label4.ForeColor = Color.Green;
                chart1.Series[0].Color = Color.Red;     //line color = red
                chart2.Series[0].Color = Color.Green;   //line color = green

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ("hata:"));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            string[] ports = SerialPort.GetPortNames();                 //Port isimlerini çekiyoruz
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);                              //comboBox a port isimlerini ekledik
            }
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);        // bu event i datalarımızı almak için yapıyoruz
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                data = serialPort1.ReadLine();                              //data çekme işlemi
                this.Invoke(new EventHandler(displaydata));                 // bu dataları progress bar ve label a yazdırmak için Invoke oluşturuyoruz
            }
            catch (Exception ex)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                serialPort1.Open();

            }
             //System.Threading.Thread.Sleep(100);             //!!!  Veri Okuma İşlemine Zaman Tanı:
        }

        private void displaydata(object sender, EventArgs e)
        {
            

                string[] value = data.Split(',');

                if (value[0] == "START")
                {
                    try
                    {
                        b = Convert.ToInt32(value[1]);          //BPM
                    }
                    catch (Exception ex)
                    {
                        b = 0;
                        //throw;
                        //HEllo 
                }
                    try
                    {
                        h = Convert.ToInt32(value[2]);          //HRV
                    }
                    catch (Exception ex)
                    {
                        h = 0;
                        //throw;
                }
                    try
                    {
                        n = Convert.ToInt32(value[3]);          //NEFES   
                    }
                    catch (Exception ex)
                    {
                        n = 0;
                        //throw;
                    }

                
                    
                }

            




        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();
                timer1.Stop();
                button1.Enabled = true;
                button2.Enabled = false;
                label4.Text = "Bağlantı kapalı";
                label4.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ("hata: "));

            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serialPort1.IsOpen) 
            { 
                serialPort1.Close(); 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            label1.Text = "BPM: " + b;
            label2.Text = "HRV: " + h;
            label3.Text = "Nefes: " + n;

            label5.Text = Convert.ToString(b);
            label6.Text = Convert.ToString(h);
            label7.Text = Convert.ToString(n);


            
            
                chart1.ChartAreas[0].AxisX.Minimum = minm1;
                chart1.ChartAreas[0].AxisX.Maximum = maxm1;
           
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 250;

                chart1.ChartAreas[0].AxisX.ScaleView.Zoom(minm1, maxm1);

                if (b != null)
                {
                    this.chart1.Series[0].Points.AddXY((maxm1 + minm1) / 2, b);                 
                    maxm1++;
                    minm1++;
                }


            chart2.ChartAreas[0].AxisX.Minimum = minm2;
            chart2.ChartAreas[0].AxisX.Maximum = maxm2;

            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 150;

            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(minm2, maxm2);

            if (h != null)
            {
                this.chart2.Series[0].Points.AddXY((maxm2 + minm2) / 2, h);
                maxm2++;
                minm2++;
            }


            chart3.ChartAreas[0].AxisX.Minimum = minm3;
            chart3.ChartAreas[0].AxisX.Maximum = maxm3;

            chart3.ChartAreas[0].AxisY.Minimum = 0;
            chart3.ChartAreas[0].AxisY.Maximum = 5;

            chart3.ChartAreas[0].AxisX.ScaleView.Zoom(minm3, maxm3);

            if (n != null)
            {
                this.chart3.Series[0].Points.AddXY((maxm3 + minm3) / 2, n);
                maxm3++;
                minm3++;
            }
        
            serialPort1.DiscardInBuffer();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
