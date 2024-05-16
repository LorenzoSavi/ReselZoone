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
using static ReleselZoone_Prog.Login;

namespace ReleselZoone_Prog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sidePanel.Height = btn_home.Height;
            sidePanel.Top = btn_home.Top;


            homeControl2.BringToFront();
        }

        public static class GlobalList
        {
            public static List<Prodotto> listProdotto = new List<Prodotto>();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btn_closeAll_Click(object sender, EventArgs e)
        {

            this.Close();


            string filePathProduct = Path.Combine(Application.StartupPath, "product.txt");

            File.WriteAllText(filePathProduct, string.Empty);


            using (StreamWriter sw = new StreamWriter(filePathProduct))
            {

                foreach (var prodotto in GlobalList.listProdotto)
                {
                    sw.WriteLine($"{prodotto.idProd},{prodotto.Nome},{prodotto.Costo},{prodotto.Visualizzazioni},{prodotto.NMagazzino},{prodotto.VenditeTotali}");
                }

            }

            string filePathCredit = Path.Combine(Application.StartupPath, "cc.txt");

            File.WriteAllText(filePathCredit, string.Empty);


            using (StreamWriter sw = new StreamWriter(filePathCredit))
            {

                foreach (var cliente in GlobalVariables.listCliente)
                {
                    if (cliente is Venditore)
                    {
                        Venditore clienteVenditore = (Venditore)cliente;
                        sw.WriteLine($"{clienteVenditore.Id},{clienteVenditore.Email},{clienteVenditore.Psw},{clienteVenditore.Name},{clienteVenditore.Cognome},{clienteVenditore.Sesso},{clienteVenditore.Age},{clienteVenditore.Lv},{clienteVenditore.Vendite},{clienteVenditore.Ricavo}");
                    }
                    else
                    {
                        sw.WriteLine($"{cliente.Id},{cliente.Email},{cliente.Psw},{cliente.Name},{cliente.Cognome},{cliente.Sesso},{cliente.Age},{cliente.Lv}");
                    }
                }

            }
        }

        private void btn_screen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            sidePanel.Height = btn_home.Height;
            sidePanel.Top = btn_home.Top;
            homeControl2.BringToFront();
            homeControl2.AggiornaQuantitàProdottiMagazzino();

            lbl_Title.Text = "Home";
        }

        private void btn_snkrs_Click(object sender, EventArgs e)
        {
            sidePanel.Height = btn_snkrs.Height;
            sidePanel.Top = btn_snkrs.Top;
            snkrsControl1.BringToFront();

            lbl_Title.Text = "Sneakers";
        }

        private void btn_profile_Click(object sender, EventArgs e)
        {
            sidePanel.Height = btn_profile.Height;
            sidePanel.Top = btn_profile.Top;
            profileControl1.BringToFront();


            lbl_Title.Text = "Profile";

            profileControl1.Aggiorna();
        }

        private void btn_telegram_Click(object sender, EventArgs e)
        {
            string url = "https://www.telegram.com";

            System.Diagnostics.Process.Start(url);
        }

        private void btn_instagram_Click(object sender, EventArgs e)
        {
            string url = "https://www.instagram.com";

            System.Diagnostics.Process.Start(url);
        }

        private void btn_twitter_Click(object sender, EventArgs e)
        {
            string url = "https://www.x.com";

            System.Diagnostics.Process.Start(url);
        }

    }
}
