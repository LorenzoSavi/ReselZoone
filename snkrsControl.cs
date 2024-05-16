using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ReleselZoone_Prog.Form1;
using static ReleselZoone_Prog.Login;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ReleselZoone_Prog
{
    public partial class snkrsControl : UserControl
    {
        public snkrsControl()
        {
            InitializeComponent();
            AggiornaQuantitàProdottiMagazzino();
        }


        private void snkrsControl_Load(object sender, EventArgs e)
        {
            snkrsControl userControl = new snkrsControl();
            AggiornaQuantitàProdottiMagazzino();


            string email = GlobalVariables.email;
            var utenteTrovato = GlobalVariables.listCliente.FirstOrDefault(u => u.Email == email);

            if (utenteTrovato != null)
            {
                if (utenteTrovato.GetType() == typeof(Cliente))
                {
                    HideSellButtons();
                }
            }
        }

        private void GeneratePDFBuy(string nome, int prezzo, string prod)
        {
            string filePath = "sell.pdf";

            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = prezzo + costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale + costoTasse;

            using (Document doc = new Document())
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();




                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logoForm.png");
                string prodPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, prod);


                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

                    img.ScaleToFit(250f, 250f);

                    doc.Add(img);

                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph("Shipping Address: ReselZoone "));
                    doc.Add(new Paragraph("                              Bergamo Via Guinceri 21"));
                    doc.Add(new Paragraph("Nome: " + nome));
                    doc.Add(new Paragraph("Costo degli articoli: " + prezzo + "€"));
                    doc.Add(new Paragraph("Costo di spedizione: " + costoSpedizione + "€"));
                    doc.Add(new Paragraph("Tasse (22%): " + costoTasse + "€"));
                    doc.Add(new Paragraph("Costo totale: " + costoFinale + "€"));
                    iTextSharp.text.Image prodImg = iTextSharp.text.Image.GetInstance(prodPath);
                    prodImg.ScaleToFit(200f, 200f);
                    prodImg.SetAbsolutePosition(doc.PageSize.Width - prodImg.ScaledWidth, doc.PageSize.Height - prodImg.ScaledHeight - 115f);

                    doc.Add(prodImg);


                    doc.Add(new Paragraph("Ship Code: IT" + GenShip()));

                }
                else
                {
                    MessageBox.Show("L'immagine non è stata trovata.");
                    return;
                }

                doc.Close();
            }

            System.Diagnostics.Process.Start(filePath);
        }

        private void GeneratePDFSell(string nome, int prezzo, string prod)
        {
            string filePath = "buy.pdf";

            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;

            using (Document doc = new Document())
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();




                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logoForm.png");
                string prodPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, prod);


                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

                    img.ScaleToFit(250f, 250f);

                    doc.Add(img);

                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph("Shipping Address: ReselZoone "));
                    doc.Add(new Paragraph("                              Bergamo Via Guinceri 21"));
                    doc.Add(new Paragraph("Nome: " + nome));
                    doc.Add(new Paragraph("Costo degli articoli: " + prezzo + "€"));
                    doc.Add(new Paragraph("Costo di spedizione: " + costoSpedizione + "€"));
                    doc.Add(new Paragraph("Tasse (22%): " + costoTasse + "€"));
                    doc.Add(new Paragraph("Ricavo totale: " + costoFinale + "€"));
                    iTextSharp.text.Image prodImg = iTextSharp.text.Image.GetInstance(prodPath);
                    prodImg.ScaleToFit(200f, 200f);
                    prodImg.SetAbsolutePosition(doc.PageSize.Width - prodImg.ScaledWidth, doc.PageSize.Height - prodImg.ScaledHeight - 115f);

                    doc.Add(prodImg);


                    doc.Add(new Paragraph("Ship Code: IT" + GenShip()));

                }
                else
                {
                    MessageBox.Show("L'immagine non è stata trovata.");
                    return;
                }

                doc.Close();
            }

            System.Diagnostics.Process.Start(filePath);
        }

        public static string GenShip()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var idBuilder = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                idBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return idBuilder.ToString();
        }

        public void AggiornaQuantitàProdottiMagazzino()
        {

            foreach (Control control in Controls)
            {
                if (control is Label)
                {
                    Label label = (Label)control;
                    string nomeLabel = label.Name;
                    string[] parts = nomeLabel.Split('_');
                    if (parts.Length == 3 && parts[0] == "lbl" && parts[2] == "magazine")
                    {
                        string idProdotto = parts[1];
                        int quantitàMagazzino = OttieniQuantitàProdottiMagazzino(idProdotto);
                        label.Text = quantitàMagazzino.ToString();
                    }
                }
            }
        }

        private int OttieniQuantitàProdottiMagazzino(string idProdotto)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == idProdotto);

            if (prodotto != null)
            {
                return prodotto.NMagazzino;
            }
            else
            {
                return 0;
            }
        }

        private void HideSellButtons()
        {

            var sellButtons = this.Controls.OfType<Button>().Where(b => b.Name.StartsWith("btn_sell_") || b.Name.EndsWith("_sell"));
            foreach (var button in sellButtons)
            {
                button.Hide();
            }
        }


        //
        // ---- button Vendi
        //

        private void btn_grey_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Yeezy 350 Grey reflective";
            int Prezzo = 290;
            string Prod = "img/product/grey.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "grey");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;


            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);
            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_lobster_sell_Click(object sender, EventArgs e)
        {
            string Nome = "SB Dunk Low Lobster";
            int Prezzo = 280;
            string Prod = "img/product/Lobstears.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "lobster");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;

            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_sell_wss_Click(object sender, EventArgs e)
        {
            string Nome = "SB Dunk Low 'Why So Sad?'";
            int Prezzo = 325;
            string Prod = "img/product/wss.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "wss");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_frozen_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Jordan 4 Frozen Moment";
            int Prezzo = 430;
            string Prod = "img/product/frozen.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "frozen");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_zabra_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Yeezy 350 Zebra";
            int Prezzo = 320;
            string Prod = "img/product/zebra.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "zebra");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_slideB_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Yeezy slide Black";
            int Prezzo = 125;
            string Prod = "img/product/slideB.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "slideB");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_london_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Nike Dunk Low London 2003";
            int Prezzo = 11415;
            string Prod = "img/product/london.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "london");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_sb_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Jordan 4 Retro SB Pine Green";
            int Prezzo = 465;
            string Prod = "img/product/j4sb.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "sb");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }

        private void btn_slideM_sell_Click(object sender, EventArgs e)
        {
            string Nome = "Yeezy slide Beige";
            int Prezzo = 215;
            string Prod = "img/product/slideM.png";
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "slideM");
            prodotto.NMagazzino = prodotto.NMagazzino + 1;


            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            double costoSpedizione = 10;
            double tasse = 0.22;

            double costoTotale = Prezzo - costoSpedizione;
            double costoTasse = costoTotale * tasse;
            double costoFinale = costoTotale - costoTasse;
            vendite.Lv = vendite.Lv + (int)(costoFinale * 0.050);

            vendite.Vendite = vendite.Vendite + 1;
            vendite.Ricavo = vendite.Ricavo + costoFinale;
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFSell(Nome, Prezzo, Prod);
        }


        //
        // ---- button compra
        //


        private void btn_slideB_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "slideB");

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;


            string Nome = "Yeezy slide Black";
            int Prezzo = 125;
            string Prod = "img/product/slideB.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_lobster_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "lobster");

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "SB Dunk Low Lobster";
            int Prezzo = 280;
            string Prod = "img/product/Lobstears.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_grey_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "grey");

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }

            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Yeezy 350 grey reflective";
            int Prezzo = 125;
            string Prod = "img/product/grey.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_wss_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "wss");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);


            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "SB Dunk Low 'Why So Sad?'";
            int Prezzo = 325;
            string Prod = "img/product/wss.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_frozen_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "frozen");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Jordan 4 Frozen Moment";
            int Prezzo = 430;
            string Prod = "img/product/frozen.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);
            
            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_zebra_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "zebra");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Yeezy 350 Zebra";
            int Prezzo = 320;
            string Prod = "img/product/zebra.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_london_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "london");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Nike Dunk Low London 2004";
            int Prezzo = 11415;
            string Prod = "img/product/london.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_sb_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "sb");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Jordan 4 Retro SB Pine Green";
            int Prezzo = 465;
            string Prod = "img/product/sb.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }

        private void btn_slideM_buy_Click(object sender, EventArgs e)
        {
            Prodotto prodotto = GlobalList.listProdotto.FirstOrDefault(p => p.idProd == "slideM");
            Venditore vendite = (Venditore)GlobalVariables.listCliente.FirstOrDefault(c => c.Email == GlobalVariables.email);

            if (prodotto.NMagazzino < 0)
            {
                MessageBox.Show("Non Disponibile");
                return;

            }
            prodotto.VenditeTotali = prodotto.VenditeTotali + 1;

            string Nome = "Yeezy slide Beige";
            int Prezzo = 125;
            string Prod = "img/product/slideM.png";
            prodotto.NMagazzino = prodotto.NMagazzino - 1;
            vendite.Lv = vendite.Lv + (int)(Prezzo * 0.065);

            AggiornaQuantitàProdottiMagazzino();

            GeneratePDFBuy(Nome, Prezzo, Prod);
        }
    }
}
