using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.IO;
using System.Security;
using static ReleselZoone_Prog.Form1;

namespace ReleselZoone_Prog
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public static class GlobalVariables
        {
            public static bool statoForm1 = false;

            public static List<Cliente> listCliente = new List<Cliente>();


            public static string id = "";

            public static string email = "";

            public static string nome = "";

            public static string cognome = "";

            public static int eta = 0;

            public static int livello = 0;

            public static int vendite = 0;

            public static double ricavo = 0;


        }
        private void Login_Load(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Visible = false;

            panel_Register.Visible= false;

            string filePathL = Path.Combine(Application.StartupPath, "cc.txt");
            CaricaDatiDaFile(filePathL);

            string filePathP = Path.Combine(Application.StartupPath, "Product.txt");
            CaricaProdottiDaFile(filePathP);

        }

        private void btn_register_panel_Click(object sender, EventArgs e)
        {
            tb_email_login.Clear();
            tb_psw_login.Clear();



            panel_Login.Visible = false;
            panel_Register.Visible = true;
            panel_Register.BringToFront();

        }

        private void btn_login_panel_Click(object sender, EventArgs e)
        {
            tb_email_register.Clear();
            tb_name_register.Clear();
            tb_psw_register.Clear();
            tb_rep_psw_register.Clear();

            panel_Login.Visible = true;
            panel_Register.Visible = false;

            panel_Login.BringToFront();
        }




        //---User-----------------------------------------------------------------------------------------------------------------------------------
        private void btn_login_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tb_email_login.Text)|| string.IsNullOrEmpty(tb_psw_login.Text))
            {
                MessageBox.Show("Errore Nel Login");
                return;
            }

            string email = tb_email_login.Text;
            string psw = tb_psw_login.Text;

            if (email == "root" || email == "Root" || email == "ROOT")
            {
                if (psw == "root" || psw == "Root" || psw == "ROOT")
                {
                    GlobalVariables.statoForm1 = true;
                    this.Close();
                    return;
                }
            }


            var cliente = GlobalVariables.listCliente.FirstOrDefault(c => c.Email == email );
            


            if (cliente != null)
            {
                if (cliente.Psw != psw)
                {
                    MessageBox.Show("Password Errata");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Email Errata");
                return;
            }

            GlobalVariables.statoForm1 = true;
            this.Close();

            GlobalVariables.id = cliente.Id;
            GlobalVariables.email = email;
            GlobalVariables.nome = cliente.Name;
            GlobalVariables.cognome = cliente.Cognome;
            GlobalVariables.livello = cliente.Lv;
            GlobalVariables.eta = cliente.Age;

            if(cliente is Venditore)
            {
                Venditore clienteVenditore = (Venditore)cliente;
                GlobalVariables.ricavo = clienteVenditore.Ricavo;
                GlobalVariables.vendite = clienteVenditore.Vendite;
            }


        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_email_register.Text) || string.IsNullOrEmpty(tb_psw_register.Text) || string.IsNullOrEmpty(tb_rep_psw_register.Text) || string.IsNullOrEmpty(tb_name_register.Text) || string.IsNullOrEmpty(txt_surname_register.Text))
            {
                MessageBox.Show("Errore Nella Registrazione");
                return;
            }

            if ((tb_psw_register.Text == tb_rep_psw_register.Text) == false)
            {
                MessageBox.Show("Password non uguali");
                return;
            }

            if (tb_psw_register.Text == tb_name_register.Text)
            {
                MessageBox.Show("Non utilizzare la Password come Nome");
                return;
            }

            if (dateTimePicker_Register.Value > DateTime.Today)
            {
                MessageBox.Show("Errore Nella Data");
                return;
            }

            string sesso = default;

            if (radioButton_M.Checked)
            {
                sesso = "M";
            }

            if (radioButton_F.Checked)
            {
                sesso = "F";
            }

            string email = tb_email_register.Text;

            DateTime dataNascita = dateTimePicker_Register.Value;

            DateTime oggi = DateTime.Today;

            int eta = oggi.Year - dataNascita.Year;

            if (oggi < dataNascita.AddYears(eta))
            {
                eta--;
            }

            Cliente nuovoCliente = new Cliente("", email, tb_psw_register.Text, tb_name_register.Text, txt_surname_register.Text, sesso, eta, 0);

            GlobalVariables.listCliente.Add(nuovoCliente);

            Cliente cliente = GlobalVariables.listCliente.FirstOrDefault(c => c.Email == email);


            string filePath = Path.Combine(Application.StartupPath, "cc.txt");

            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"{cliente.Id},{cliente.Email},{cliente.Psw},{cliente.Name},{cliente.Cognome},{cliente.Sesso},{cliente.Age},{cliente.Lv}");
            }

            GlobalVariables.statoForm1 = true;

            GlobalVariables.id = cliente.Id;

            GlobalVariables.email = email;
            GlobalVariables.nome = cliente.Name;
            GlobalVariables.cognome = cliente.Cognome;
            GlobalVariables.livello = cliente.Lv;

            this.Close();
        }

        private void btn_closeAll_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void btn_screen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CaricaProdottiDaFile(string filePath)
        {


            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 6)
                    {
                        string id = parts[0];
                        string nome = parts[1];
                        double costo = double.Parse(parts[2]);
                        int visualizzazioni = int.Parse(parts[3]);
                        int nMagazzino = int.Parse(parts[4]);
                        int venditeTotali = int.Parse(parts[5]);

                        GlobalList.listProdotto.Add(new Prodotto(id, nome, costo, visualizzazioni, nMagazzino, venditeTotali));
                    }
                }
            }
            else
            {
                MessageBox.Show($"Il file {filePath} non esiste.");
            }
        }

        private void CaricaDatiDaFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length >= 8) 
                    {
                        string _id = parts[0];
                        string email = parts[1];
                        string psw = parts[2];
                        string nome = parts[3];
                        string cognome = parts[4];
                        string sesso = parts[5];
                        int age = int.Parse(parts[6]);
                        int lv = int.Parse(parts[7]);

                        if (parts.Length == 8)
                        {
                            GlobalVariables.listCliente.Add(new Cliente(_id, email, psw, nome, cognome, sesso, age, lv));
                        }
                        else if (parts.Length >= 10) 
                        {
                            int vendite = int.Parse(parts[8]);
                            double ricavo = double.Parse(parts[9]);

                            GlobalVariables.listCliente.Add(new Venditore(_id, email, psw, nome, cognome, sesso, age, lv, vendite, ricavo));
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Riga non valida nel file: {line}");
                    }
                }
            }
            else
            {
                MessageBox.Show($"Il file {filePath} non esiste.");
            }
        }


    }


    
}
