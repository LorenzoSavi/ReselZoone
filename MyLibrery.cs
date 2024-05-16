using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReleselZoone_Prog
{


    public class Cliente : IDisposable
    {

        internal static List<string> listEmail = new List<string>();

        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (value == null)
                {
                    throw new Exception("Email Error");
                }
                if (value.Contains("@") == false)
                {
                    throw new Exception("Email Inesistente");
                }

                _email = value;
            }
        }

        private string _psw;
        public string Psw
        {
            get { return _psw; }
            set
            {
                if (_psw == value)
                {
                    throw new Exception("Password Error");
                }
                if (ControlloPassword(value) == false)
                {
                    throw new Exception("La password non è abbastanza forte");

                }
                _psw = value;

            }

        }


        private string _name;

        public string Name
        {
            get { return _name; }

            set
            {
                if (_name == value)
                {
                    throw new Exception("Nome Error");
                }

                _name = value;
            }
        }

        private string _cognome;

        public string Cognome
        {
            get { return _cognome; }

            set
            {
                if (_cognome == value)
                {
                    throw new Exception("Cognome Error");
                }

                _cognome = value;
            }
        }

        private string _sesso;

        public string Sesso
        {
            get { return _sesso; }

            set { _sesso = value; }
        }

        private int _age;

        public int Age
        {
            get { return _age; }

            set { _age = value; }
        }

        private int _lv;

        public int Lv
        {
            get { return _lv; }

            set { _lv = value; }
        }

        public Cliente(string id, string email, string psw, string nome, string cognome, string sesso, int age, int lv)
        {
            if (listEmail.Contains(email))
            {
                throw new Exception("Email Gia Registrata");
            }

            if(id == "")
            {
                Id = GenId();
            }
            else
            {
                Id = id;
            }
               
            
            Email = email;
            Psw = psw;
            Name = nome;
            Cognome = cognome;
            Sesso = sesso;
            Age = age;
            Lv = lv;

            listEmail.Add(Email);
        }

        public void Dispose()
        {
            lock (listEmail)
            {
                if (listEmail.Contains(Email))
                {
                    listEmail.Remove(Email);
                }
            }
        }

        //
        //----- Metodi ---------------------------------------------------------------------------------
        //

        public static string GenId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var idBuilder = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                idBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return idBuilder.ToString();
        }

        private bool ControlloPassword(string password)
        {
            if (password.Length < 8)
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            if (!password.Any(Simboli))
            {
                throw new Exception("Simboli Accettati ('!', '@', '#', '$', '.', '?')");
            }


            return true;
        }

        private bool Simboli(char c)
        {
            char[] symbols = { '!', '@', '#', '$', '.', '?' };

            return symbols.Contains(c);
        }

    }

    internal class Venditore : Cliente
    {
        private int _vendite;
        public int Vendite
        {
            get { return _vendite; }

            set { _vendite = value; }
        }

        private double _ricavo;
        public double Ricavo
        {
            get { return _ricavo; }

            set { _ricavo = value; }
        }

        public Venditore(string id, string email, string psw, string nome, string cognome, string sesso, int age, int lv, int vendite, double ricavo) : base(id, email, psw, nome, cognome, sesso, age, lv)
        {
            Vendite = vendite;
            Ricavo = ricavo;
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }

    public class Prodotto
    {

        private string _idProd;
        public string idProd
        {
            get { return _idProd; }
            set
            {
                if (_idProd != value)
                {
                    throw new Exception("Id Prod Error");
                }
                _idProd = value;
            }
        }

        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set
            {
                if (_nome == value)
                {
                    throw new Exception("Nome Prod Error");
                }
                _nome = value;
            }
        }

        private double _costo;

        public double Costo
        {
            get { return _costo; }
            set
            {
                if (value >= 0)
                    _costo = value;
                else
                    Console.WriteLine("Il costo non può essere negativo.");
            }
        }

        public int _visualizzazioni;

        public int Visualizzazioni
        {
            get { return _visualizzazioni; }
            set
            {
                if (value >= 0)
                    _visualizzazioni = value;
                else
                    Console.WriteLine("Le visualizzazioni non possono essere negative.");
            }
        }

        private int _nMagazzino;
        public int NMagazzino
        {
            get { return _nMagazzino; }
            set
            {
                if (value >= 0)
                    _nMagazzino = value;
                else
                    Console.WriteLine("Il numero di magazzino non può essere negativo.");
            }
        }

        public int _venditeTotali;

        public int VenditeTotali
        {
            get { return _venditeTotali; }
            set
            {
                if (value >= 0)
                    _venditeTotali = value;
                else
                    Console.WriteLine("Le vendite totali non possono essere negative.");
            }
        }

        public Prodotto(string id,string nome, double costo, int visualizzazioni, int nMagazzino, int venditeTotali)
        {
            _idProd = id;
            Nome = nome;
            Costo = costo;
            Visualizzazioni = visualizzazioni;
            NMagazzino = nMagazzino;
            VenditeTotali = venditeTotali;
        }

    }
}
