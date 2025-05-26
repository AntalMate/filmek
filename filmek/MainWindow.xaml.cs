using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace filmek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection kapcs = new MySqlConnection("server = localhost;database = filmek; uid = root; password = ''");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void lekerdez()
        {
            kapcs.Open();
            var sql = "SELECT * FROM filmek;";
            var parancs = new MySqlCommand(sql, kapcs);
            var lekerdezes = parancs.ExecuteReader();
            lbAdatok.Items.Clear();
            while (lekerdezes.Read())
            {
                lbAdatok.Items.Add(lekerdezes["filmazon"] + ";" + lekerdezes["cim"] + ";" + lekerdezes["ev"] + ";" + lekerdezes["szines"] + ";" + lekerdezes["mufaj"] + ";" + lekerdezes["hossz"]);
            }
            lekerdezes.Close();

            kapcs.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lekerdez();
        }

        private void lbAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //az éppen kivásztott elem adatainak kiírása a textboxokba, az id kiírása a labelbe
            if (lbAdatok.SelectedItem != null)
            {
                var adat = lbAdatok.SelectedItem.ToString().Split(';');
                lbFilmAzon.Content = adat[0];
                tb1.Text = adat[1];
                tb2.Text = adat[2];
                tb3.Text = adat[3];
                tb4.Text = adat[4];
                tb5.Text = adat[5];
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //módosítás a textboxba írt adatokkal
            if (lbFilmAzon.Content.ToString() != "-")
            {
                kapcs.Open();
                new MySqlCommand($"update filmek set cim = '{tb1.Text}', ev = {tb2.Text}, szines = '{tb3.Text}', mufaj = '{tb4.Text}', hossz = {tb5.Text} where filmazon = '{lbFilmAzon.Content}'", kapcs).ExecuteNonQuery();
                MessageBox.Show("Sikeres módosítás!");
                kapcs.Close();
                lekerdez();
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva film azonosító!");
            }
        }
    }
}
