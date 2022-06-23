using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperationsWindow
{
    public partial class Form1 : Form
    {

        DataBase dataBase = new DataBase();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.AutoResizeColumns();

        }

        private void CreateColums(DataGridView dgw)
        {
            dgw.Columns.Add("OperFromName", "Операционная");
            dgw.Columns.Add("PatientName", "Пациент");
            dgw.Columns.Add("FinSourceTypeSign", "Источник финансирования");
            dgw.Columns.Add("Diagnos", "Функциональная группа");
            dgw.Columns.Add("OperationDesc", "Название операции");
            dgw.Columns.Add("OperFromTime", "Начало операции");
            dgw.Columns.Add("OperToTime", "Окончание операции");
            dgw.Columns.Add("SurgeonGroup", "Хирургическая бригада");
            dgw.Columns.Add("BloodLoss", "Кровепотеря");
            dgw.Columns.Add("GBK", "Костный банк");
            dgw.Columns.Add("WarId", "Отправлен в");
        }

        private void LoadData()
        {
            string queryString = $"select * from Operation_ListForMonitor";
            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());
            Label[] labels = new Label[6];
            DataGridView[] dataGrids = new DataGridView[6];
            dataGrids[0] = dataGridView1;
            dataGrids[1] = dataGridView2;
            dataGrids[2] = dataGridView3;
            dataGrids[3] = dataGridView4;
            dataGrids[4] = dataGridView5;
            dataGrids[5] = dataGridView6;

            labels[0] = label1;
            labels[1] = label2;
            labels[2] = label3;
            labels[3] = label4;
            labels[4] = label5;
            labels[5] = label6;
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[11]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
                data[data.Count - 1][8] = reader[8].ToString();
                data[data.Count - 1][9] = reader[9].ToString();
                data[data.Count - 1][10] = reader[10].ToString();
            }

            reader.Close();
            dataBase.closeConnection();
            List<char> temp = new List<char>();
            List<char> oproom = new List<char>();
            for (int i = 0; i < data.Count; i++)
            {
                char[] c = data[i][0].ToCharArray();
                char num1 = c[2];
                temp.Add(num1);
                oproom = temp.Distinct().ToList();
            }
            for(int i = 1; i < oproom.Count; i++)
            {
                for(int k = 0; k < oproom.Count-1; k++)
                {
                    if(oproom[k] > oproom[k + 1])
                    {
                        char a = oproom[k];
                        oproom[k] = oproom[k + 1];
                        oproom[k + 1] = a;
                    }
                }
            }
            for(int i = 0; i < labels.Length; i++)
            {
                try
                {
                    labels[i].Text = "Операционная №" + oproom[i];
                    dataGrids[i].Rows.Clear();
                    CreateColums(dataGrids[i]);
                    foreach (string[] s in data)
                    {
                        dataGrids[i].Rows.Add(s);
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    labels[i].Visible = false;
                    dataGrids[i].Visible = false;
                }
            }
        }

        //private void RefreshDataGrid(DataGridView dgw)
        //{
        //    dgw.Rows.Clear();
        //    foreach (string[] s in data)
        //        dgw.Rows.Add(s);
        //}
    }
}
