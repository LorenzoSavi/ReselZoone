using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ReleselZoone_Prog.Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ReleselZoone_Prog
{
    public partial class ProfileControl : UserControl
    {

        public ProfileControl()
        {
            InitializeComponent();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }



        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }


        public void ProfileControl_Load(object sender, EventArgs e)
        {
            lbl_vis_vendite.Visible = false;
            lbl_vis_ricavi.Visible = false;
            lbl_ricavi.Visible = false;
            lbl_vendite.Visible = false;

            lbl_livello.Text = GlobalVariables.livello.ToString();
            lbl_ricavi.Text = GlobalVariables.ricavo.ToString() + " €";
            lbl_vendite.Text = GlobalVariables.vendite.ToString() + " Vendite";

            lbl_Id.Text = GlobalVariables.id;
            lbl_livello.Text = GlobalVariables.livello.ToString();
            lbl_email_p.Text = GlobalVariables.email;
            lbl_nome_p.Text = GlobalVariables.nome;
            lbl_cog_p.Text = GlobalVariables.cognome;
            lbl_eta_p.Text = GlobalVariables.eta.ToString();


        }


        public void Aggiorna()
        {

            string email = GlobalVariables.email;

            Cliente utenteTrovato = GlobalVariables.listCliente.FirstOrDefault(u => u.Email == email);


            Venditore clienteVenditore = (Venditore)utenteTrovato;

            lbl_livello.Text = clienteVenditore.Lv.ToString();
            lbl_ricavi.Text = clienteVenditore.Ricavo.ToString() + " €";
            lbl_vendite.Text = clienteVenditore.Vendite.ToString() + " Vendite";




            if (utenteTrovato != null)
            {
                if (utenteTrovato.GetType() == typeof(Venditore))
                {
                    btn_diventa_venditore.Visible = false;
                    label6.Visible = false;
                    lbl_ricavi.Visible = true;
                    lbl_vendite.Visible = true;
                    lbl_vis_vendite.Visible = true;
                    lbl_vis_ricavi.Visible = true;

                }
            }
        }
        private void btn_diventa_venditore_Click(object sender, EventArgs e)
        {

            if (GlobalVariables.eta < 18)
            {
                MessageBox.Show("Solo per Maggiorenni", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var utenteTrovato = GlobalVariables.listCliente.FirstOrDefault(u => u.Email == GlobalVariables.email);
            string idutente = utenteTrovato.Id;

            if (utenteTrovato != null && utenteTrovato is Cliente)
            {
                Cliente cliente = utenteTrovato as Cliente;
                cliente.Dispose();
                Venditore nuovoVenditore = new Venditore(cliente.Id, cliente.Email, cliente.Psw, cliente.Name, cliente.Cognome, cliente.Sesso, cliente.Age, cliente.Lv, 0, 0);

                GlobalVariables.listCliente.Remove(cliente);


                GlobalVariables.listCliente.Add(nuovoVenditore);

                btn_diventa_venditore.Visible = false;

                MessageBox.Show("Sei Diventato Venditore", "Evviva", MessageBoxButtons.OK);
            }


        }

        public void lbl_vendite_Click(object sender, EventArgs e)
        {

        }

        private void lbl_ricavi_Click(object sender, EventArgs e)
        {

        }

        private void lbl_livello_Click(object sender, EventArgs e)
        {

        }

    }
}
