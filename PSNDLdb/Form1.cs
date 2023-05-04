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
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace PSNDLdb
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int r, g, b;

        public const int WM_NCLBUTTONDOWN = 0xA1; 
        public const int HT_CAPTION = 0x2; 
        [DllImportAttribute("user32.dll")] 
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam); 
        [DllImportAttribute("user32.dll")] 
        public static extern bool ReleaseCapture();
        
        public Form1()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += timer1_Tick;
            timer.Start();
            r = 0;
            g = 0;
            b = 0;
            



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.VirtualMode = false;
            LoadTXT();
            
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Search(string name)
        {
            // Itera sobre as linhas do DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Verifica se a célula na coluna "Name" contém o nome pesquisado
                if (row.Cells["Name"].Value != null && row.Cells["Name"].Value.ToString().Contains(name))
                {
                    // Se sim, torna a linha visível
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadTXT()
        {
            if (File.Exists("db.txt"))
            {
                // Lê todas as linhas do arquivo
                string[] lines = File.ReadAllLines("db.txt");

                // Cria um DataTable para armazenar os dados
                DataTable dt = new DataTable();

                // Adiciona as colunas no DataTable
                dt.Columns.Add("TitleID");
                dt.Columns.Add("Name");
                dt.Columns.Add("Type");
                dt.Columns.Add("Region");
                dt.Columns.Add("PKGLink");
                dt.Columns.Add("RAP");
                dt.Columns.Add("ContentID");
                dt.Columns.Add("Description");
                dt.Columns.Add("Uploader");

                // Adiciona as linhas no DataTable
                foreach (string line in lines)
                {
                    string[] data = line.Split(';');

                    // Verifica se o número de elementos na matriz data é igual ao número de colunas no DataTable
                    if (data.Length != dt.Columns.Count)
                    {
                        // Trata o erro aqui, se necessário
                        continue; // Pula para a próxima linha do arquivo
                    }

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (string.IsNullOrEmpty(data[i]))
                        {
                            data[i] = "Unavailable";
                        }
                    }


                    dt.Rows.Add(data);

 
                    
                }

                // Define o DataTable como DataSource do DataGridView
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Database not found.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            string name = dataGridView1.Rows[selectedRowIndex].Cells["PKGLink"].Value.ToString();

            System.Diagnostics.Process.Start(name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            string rap = dataGridView1.Rows[selectedRowIndex].Cells["RAP"].Value.ToString();
            string contentid = dataGridView1.Rows[selectedRowIndex].Cells["ContentID"].Value.ToString();


            string[] parts = rap.Split(new string[] { ".RAP" }, StringSplitOptions.None);
            string output = parts[0];

            string link = "https://nopaystation.com/tools/rap2file/";

            System.Diagnostics.Process.Start(link + output + "/" + contentid);



        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            { 
                ReleaseCapture(); 
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); 
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            { 
                ReleaseCapture(); 
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(r, g, b);

            r += 10;
            g += 20;
            b += 30;

            if (r > 255) r = 0;
            if (g > 255) g = 0;
            if (b > 255) b = 0;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            

        }
    }
}
