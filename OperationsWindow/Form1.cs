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
            int j = 1;
            //Занос объектов label и datagridview в массивы
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

            //Внесение данных из базы в список
            string queryString = ($"select * from Operation_ListForMonitor"); //выбор всех из таблицы
            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

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

            //Сортировка списка данных
            for (int i = 1; i < data.Count; i++)
            {
                for (int k = 0; k < data.Count - 1; k++)
                {
                    char num1 = opnum(k, data);
                    char num2 = opnum(k + 1, data);
                    if ((num1 & 0x0f) > (num2 & 0x0f))
                    {
                        string[] a = data[k];
                        data[k] = data[k + 1];
                        data[k + 1] = a;
                    }
                }
            }

            //Сортировка номеров операционных
            List<char> temp = new List<char>();
            List<char> oproom = new List<char>();

            for (int i = 0; i < data.Count; i++)
            {
                char num = opnum(i, data);
                temp.Add(num);
                oproom = temp.Distinct().ToList();
            }

            for (int i = 1; i < oproom.Count; i++)
            {
                for (int k = 0; k < oproom.Count - 1; k++)
                {
                    if (oproom[k] > oproom[k + 1])
                    {
                        char a = oproom[k];
                        oproom[k] = oproom[k + 1];
                        oproom[k + 1] = a;
                    }
                }
            }

            LoadData(labels, dataGrids, oproom, data, j);
            dataGridView1.AutoResizeColumns();
            dataGridView2.AutoResizeColumns();
            dataGridView3.AutoResizeColumns();
            dataGridView4.AutoResizeColumns();
            dataGridView5.AutoResizeColumns();
            dataGridView6.AutoResizeColumns();

            //Таймер обновления данных
            Timer Timer1 = new Timer();
            Timer1.Interval = (1 * 60 * 1000);
            Timer1.Tick += (x, y) => { timer1_Tick(labels, dataGrids, j); } ;
            Timer1.Start();

            //таймер переключения экрана
            //if(oproom.Count > labels.Length)
            //{
            //    Timer Timer2 = new Timer();
            //    Timer2.Interval = (1 * 30 * 1000);
            //    Timer2.Tick += (x, y) => { timer2_Tick(labels, dataGrids, data, oproom); };
            //    Timer2.Start();
            //}
        }

        //создание колонок
        private void CreateColums(DataGridView dgw)
        {
            dgw.Columns.Clear();

            dgw.Columns.Add("OperFromName", "Операционная");
            dgw.Columns.Add("PatientName", "Пациент");
            dgw.Columns.Add("FinSourceTypeSign", "Источник финансирования");
            dgw.Columns.Add("Diagnos", "Функциональная группа");
            dgw.Columns.Add("OperationDesc", "Название операции");
            dgw.Columns.Add("OperFromTime", "Начало операции");
            dgw.Columns.Add("OperToTime", "Окончание операции");
            dgw.Columns.Add("SurgeonGroup", "Хирургическая бригада");
            dgw.Columns.Add("BloodLoss", "Кровопотеря");
            dgw.Columns.Add("GBK", "Костный банк");
            dgw.Columns.Add("WarId", "Отправлен в");
        }

        //Первый таймер
        private void timer1_Tick(Label[] labels, DataGridView[] dataGrids, int j)
        {
            labels[0].Text = label1.Text;
            labels[1].Text = label2.Text;
            labels[2].Text = label3.Text;
            labels[3].Text = label4.Text;
            labels[4].Text = label5.Text;
            labels[5].Text = label6.Text;

            //Внесение данных из базы в список
            string queryString = ($"select * from Operation_ListForMonitor"); //выбор всех из таблицы
            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

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

            //Сортировка списка данных
            for (int i = 1; i < data.Count; i++)
            {
                for (int k = 0; k < data.Count - 1; k++)
                {
                    char num1 = opnum(k, data);
                    char num2 = opnum(k + 1, data);
                    if ((num1 & 0x0f) > (num2 & 0x0f))
                    {
                        string[] a = data[k];
                        data[k] = data[k + 1];
                        data[k + 1] = a;
                    }
                }
            }

            //Сортировка номеров операционных
            List<char> temp = new List<char>();
            List<char> oproom = new List<char>();

            for (int i = 0; i < data.Count; i++)
            {
                char num = opnum(i, data);
                temp.Add(num);
                oproom = temp.Distinct().ToList();
            }

            for (int i = 1; i < oproom.Count; i++)
            {
                for (int k = 0; k < oproom.Count - 1; k++)
                {
                    if (oproom[k] > oproom[k + 1])
                    {
                        char a = oproom[k];
                        oproom[k] = oproom[k + 1];
                        oproom[k + 1] = a;
                    }
                }
            }

            LoadData(labels, dataGrids, oproom, data, j);
            dataGridView1.AutoResizeColumns();
            dataGridView2.AutoResizeColumns();
            dataGridView3.AutoResizeColumns();
            dataGridView4.AutoResizeColumns();
            dataGridView5.AutoResizeColumns();
            dataGridView6.AutoResizeColumns();
        }

        ////Второй таймер
        //private void timer2_Tick(Label[] labels, DataGridView[] dataGrids, List<string[]> data, List<char> oproom)
        //{
        //    labels[0].Text = label1.Text;
        //    labels[1].Text = label2.Text;
        //    labels[2].Text = label3.Text;
        //    labels[3].Text = label4.Text;
        //    labels[4].Text = label5.Text;
        //    labels[5].Text = label6.Text;

        //    int k = oproom.Count;

        //    for(int i = 0; i < labels.Length; i++)
        //    {
        //        for(int j = oproom.Count; j > 0; j--)
        //        {
        //            if (labels[i].Text == "Операционная №" + oproom[oproom.Count - 1])
        //            {
        //                k = 1;
        //            }
        //        }
        //    }

        //    LoadData(labels, dataGrids, oproom, data, k);
        //    dataGridView1.AutoResizeColumns();
        //    dataGridView2.AutoResizeColumns();
        //    dataGridView3.AutoResizeColumns();
        //    dataGridView4.AutoResizeColumns();
        //    dataGridView5.AutoResizeColumns();
        //    dataGridView6.AutoResizeColumns();
        //}

        //Создание интерфейса

        private void LoadData(Label[] labels, DataGridView[] dataGrids, List<char> oproom, List<string[]> data, int k)
        {
            int j = 1;
            for (int i = 1; i < labels.Length + 1; i++)
            {
                try
                {
                    labels[i - 1].Text = "Операционная №" + oproom[k - 1];
                    dataGrids[i - 1].Refresh();
                    dataGrids[i - 1].Rows.Clear();
                    CreateColums(dataGrids[i - 1]);

                    foreach (string[] s in data.Skip(j))
                    {
                        char cnum = opnum(j, data);
                        int num = cnum & 0x0f;
                        if (num == (oproom[k - 1] & 0x0f))
                        {
                            dataGrids[i - 1].Rows.Add(s);
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (DataGridViewRow row in dataGrids[i - 1].Rows)
                    {
                        if((Convert.ToString(row.Cells[5].Value) != "") && (Convert.ToString(row.Cells[6].Value) == ""))
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else if((Convert.ToString(row.Cells[5].Value) != "") && (Convert.ToString(row.Cells[6].Value) != ""))
                        {
                            row.DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                        k++;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    labels[i - 1].Visible = false;
                    dataGrids[i - 1].Visible = false;
                }
            }
        }

        //Получение номера операционной из записи
        private char opnum(int k, List<string[]> data)
        {
            char[] c = data[k][0].ToCharArray();
            char num = c[2];

            return num;
        }

        private void Colors(List<string[]> data, DataGridView[] dataGrids)
        {
            foreach (string[] s in data)
            {
                for(int i = 0; i < s.Length; i++)
                {
                    if(s[5] == "")
                    {

                    }
                }
            }
        }
    }
}
// 5 6