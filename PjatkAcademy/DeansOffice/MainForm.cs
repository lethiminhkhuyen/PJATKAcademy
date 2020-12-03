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
using System.IO;

namespace DeansOffice
{
    public partial class MainForm : Form
    {
        public static string IndexNumberTextBox = "";
        public static string FirstNameTextBox = "";
        public static string LastNameTextBox = "";
        public static string EmailTextBox = "";
        public static string TelephoneTextBox = "";
        public static string PeselTextBox = "";
        public static string PassportNumberTextBox = "";
        public static string CitizenshipTextBox = "";
        public static string CityTextBox = "";
        public static DateTime BirthdayDateTimePicker;
        public static string DepartmentComboBox = "";
        public static DateTime RegisteredYear;
        public static Image img;
        public MainForm()
        {
            InitializeComponent();
            ComboFill();
            DisplayData();
        }
        public void ComboFill()
        {
            using (var con = new SqlConnection("Data Source=DESKTOP-6NU2H6J;Initial Catalog=DeanOffice;Integrated Security=True"))
            {
                var com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "SELECT * FROM semesters";
                con.Open();
                using (var dr = com.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string sName = dr["S_Name"].ToString();
                        SemesterComboBox.Items.Add(sName);
                    }
                }
                con.Dispose();
            }
        }
        private void LoadDataFromSqlServer(SqlCommand com)
        {
            using (var con = new SqlConnection("Data Source=DESKTOP-6NU2H6J;Initial Catalog=DeanOffice;Integrated Security=True"))
            {
                com.Connection = con;
                con.Open();
                var students = new List<Student>();
                using (var dr = com.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var newStudent = new Student();
                        newStudent.IdNumber = Convert.ToInt32(dr["IdStudent"]);
                        newStudent.Name = dr["St_FirstName"].ToString();
                        newStudent.Surname = dr["St_LastName"].ToString();
                        newStudent.Email = dr["St_Email"].ToString();
                        newStudent.Telephone = Convert.ToInt32(dr["St_TelephoneNum"]);
                        newStudent.Pesel = dr["St_PeselNum"].ToString();
                        newStudent.PassportNumber = dr["St_PassportNum"].ToString();
                        newStudent.Citizenship = dr["St_Citizenship"].ToString();
                        newStudent.CityofBirth = dr["St_CityOfBirth"].ToString();
                        int colIndex = dr.GetOrdinal("St_DataOfBirth");
                        newStudent.DataofBirth = dr.GetDateTime(colIndex);
                        newStudent.Faculty = dr["St_Faculty"].ToString();
                        int dateRegistered = dr.GetOrdinal("St_RegYear");
                        newStudent.RegisteredYear = dr.GetDateTime(dateRegistered);
                        byte[] img = (byte[])dr["St_Image"];
                        MemoryStream ms = new MemoryStream(img);
                        newStudent.PhotoLink = Image.FromStream(ms);
                        students.Add(newStudent);
                    }
                    StudentsDataGridView.DataSource = students;
                }
                con.Dispose();
            }
        }
        private void DisplayData()
        {
            var com = new SqlCommand();
            com.CommandText = "SELECT * FROM students";
            LoadDataFromSqlServer(com);
        }
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new StudentDialog();
            form.ShowDialog();
        }

        private void EditStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
            var form = new StudentDialog();
            if (StudentsDataGridView.SelectedRows.Count == 1)
            {
                DataGridViewRow row = StudentsDataGridView.SelectedRows[0];
                IndexNumberTextBox = row.Cells["IdNumber"].Value.ToString();
                FirstNameTextBox = row.Cells["Name"].Value.ToString();
                LastNameTextBox = row.Cells["Surname"].Value.ToString();
                EmailTextBox = row.Cells["Email"].Value.ToString();
                TelephoneTextBox = row.Cells["Telephone"].Value.ToString();
                PeselTextBox = row.Cells["Pesel"].Value.ToString();
                PassportNumberTextBox = row.Cells["PassportNumber"].Value.ToString();
                CitizenshipTextBox = row.Cells["Citizenship"].Value.ToString();
                CityTextBox = row.Cells["CityofBirth"].Value.ToString();
                BirthdayDateTimePicker = Convert.ToDateTime(row.Cells["DataofBirth"].Value);
                DepartmentComboBox = row.Cells["Faculty"].Value.ToString();
                RegisteredYear = Convert.ToDateTime(row.Cells["RegisteredYear"].Value);
                img = (Image)row.Cells["PhotoLink"].Value;
            }
            form.StudentEditBox_Load();
            form.ShowDialog();
        }

        private void RemoveStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StudentsDataGridView.SelectedRows.Count == 1)
            {
                DataGridViewRow row = StudentsDataGridView.SelectedRows[0];
                string value1 = row.Cells["IdNumber"].Value.ToString();
                using (var con = new SqlConnection("Data Source=DESKTOP-6NU2H6J;Initial Catalog=DeanOffice;Integrated Security=True"))
                {
                    var com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = "DELETE FROM students WHERE IdStudent = @VALUE";
                    com.Parameters.Add("@VALUE", SqlDbType.NVarChar).Value = value1;
                    con.Open();
                    using (SqlCommand command = com)
                    {
                        command.ExecuteNonQuery();
                    }
                    con.Dispose();
                }
                DisplayData();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var com = new SqlCommand();

                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    if (LastNameCheckBox.Checked && !StudentNumberCheckBox.Checked)
                    {
                        com.CommandText = "SELECT * FROM students WHERE St_LastName = @VALUE";
                    }
                    else if (!LastNameCheckBox.Checked && StudentNumberCheckBox.Checked)
                    {
                        com.CommandText = "SELECT * FROM students WHERE IdStudent = @VALUE";
                    }
                    else if (LastNameCheckBox.Checked && StudentNumberCheckBox.Checked)
                    {
                         int n;
                        bool isNumeric = int.TryParse(textBox1.Text, out n);
                        if (isNumeric)
                        {
                            com.CommandText = "SELECT * FROM students WHERE IdStudent = @VALUE";
                        }
                        else
                        {
                            com.CommandText = "SELECT * FROM students WHERE St_LastName = @VALUE";
                        }
                        
                    }
                }
                com.Parameters.Add("@VALUE", SqlDbType.NVarChar).Value = textBox1.Text;
                LoadDataFromSqlServer(com);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selectedItem = SemesterComboBox.SelectedItem;
            var theSemester = selectedItem.ToString();
            var startDate = new DateTime();
            var endDate = new DateTime();
            using (var con = new SqlConnection("Data Source=DESKTOP-6NU2H6J;Initial Catalog=DeanOffice;Integrated Security=True"))
            {
                var com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "SELECT * FROM semesters WHERE S_Name = @VALUE";
                com.Parameters.Add("@VALUE", SqlDbType.NVarChar).Value = theSemester;
                
                con.Open();
                using (var dr2 = com.ExecuteReader())
                {
                    while (dr2.Read())
                    {
                        int colIndex = dr2.GetOrdinal("S_StartDate");
                        startDate = dr2.GetDateTime(colIndex);
                        int colIndex2 = dr2.GetOrdinal("S_EndDate");
                        endDate = dr2.GetDateTime(colIndex2);
                    }
                }
                con.Dispose();
                
            }
            var com2 = new SqlCommand();
            com2.CommandText = "SELECT * FROM students WHERE St_RegYear < @VALUE1 AND St_RegYear > @VALUE2";
            com2.Parameters.Add("@VALUE2", SqlDbType.NVarChar).Value = startDate.ToString("yyyy - MM - dd HH: mm: ss");
            com2.Parameters.Add("@VALUE1", SqlDbType.NVarChar).Value = endDate.ToString("yyyy - MM - dd HH: mm: ss");
            //MessageBox.Show("Selected Item Text: " + startDate.Year + "\n" + "Index: " + endDate.Year);
            LoadDataFromSqlServer(com2);


        }
        }
}
