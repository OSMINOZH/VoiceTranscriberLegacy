using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Security.Cryptography;


namespace Transcriber
{
    public partial class Authorization : MaterialForm
    {
        public Authorization()
        {
            InitializeComponent();
            RegistrationPANEL.Hide();
            // MaterialSkin
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }
        public bool userExist = false;

        //private string LoadData(string sql) // Запрос данных
        //{
        //    SqlConnection con = new SqlConnection("Data Source=DESKTOP-LIU1R6H; Initial Catalog = Security; " + "Integrated Security=True;");
        //    con.Open();
        //    string sqlcom = sql;
        //    SqlCommand cmd = new SqlCommand(sqlcom, con);
        //    object data = cmd.ExecuteScalar();
        //    if (data != null)
        //    {
        //        string udata = data.ToString();
        //        con.Close();
        //        return udata;
        //    }
        //    else
        //    {
        //        con.Close();
        //        MessageBox.Show("Информация не найдена в базе данных");
        //    }
        //    return "null";
        //}

        //private void Authorizate()
        //{
        //    if (!string.IsNullOrWhiteSpace(LoginTB.Text) && !string.IsNullOrWhiteSpace(PasswordTB.Text))
        //    {
        //        string username = LoginTB.Text;
        //        string password = PasswordTB.Text;
        //        string usernameBD = LoadData("SELECT userName FROM [User] WHERE userName ='" + username + "'").Replace(" ", "");
        //        string passwordBD = LoadData("SELECT userPassword FROM [User] WHERE userName ='" + username + "'").Replace(" ", "");
        //        if ((username == usernameBD) && (Encode(password) == passwordBD))
        //        {
        //            this.Hide();
        //            Form mainmenu = new MainMenu(username);
        //            mainmenu.ShowDialog();
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Неверные данные для входа!");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Заполните все поля!");
        //    }
        //}

        private void Registration()
        {
            string username = LoginRegTB.Text;
            string password = PasswordRegTB.Text;
            string passwordRepeat = RepeatPasswordTB.Text;
            if (!string.IsNullOrWhiteSpace(LoginRegTB.Text) && !string.IsNullOrWhiteSpace(PasswordRegTB.Text) && !string.IsNullOrWhiteSpace(RepeatPasswordTB.Text))
            {
                if (password != passwordRepeat) MessageBox.Show("Пароли отличаются!");
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-LIU1R6H; Initial Catalog = Security; " + "Integrated Security=True;");
                con.Open();
                string sqlcom = $"INSERT INTO [User] (userName, userPassword, userRole) VALUES ('{username}', '{Encode(password)}', 'Operator')";
                SqlCommand cmd = new SqlCommand(sqlcom, con);
                object data = cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private string Encode(string s)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF32.GetBytes(s);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        // LOGGER CODE BELOW
        private void logger_new(string some_info)
        {
            //Logger.WriteLog("[Таблица '" + name + "' - " + action_name + "] Изменена строка: id='" + textID.Text + "'");
            Logger.WriteLog(some_info);
        }


        private void RegistrationBTN_Click(object sender, EventArgs e)
        {
            AuthorizationPANEL.Hide();
            RegistrationPANEL.Show();
        }

        private void EnterBTN_Click(object sender, EventArgs e)
        {
            //Authorizate();
            string username = "admin";
            this.Hide();
            Form mainmenu = new MainMenu(username);
            mainmenu.ShowDialog();
            this.Close();
        }

        private void RegisterNewUserBTN_Click(object sender, EventArgs e)
        {
            Registration();
            AuthorizationPANEL.Show();
            RegistrationPANEL.Hide();
        }
    }
}
