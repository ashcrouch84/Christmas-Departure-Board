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
using System.Windows.Threading;
using System.IO;
using System.Net;
using Departure_Board.Properties;

namespace Departure_Board
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer mon_Timer = new DispatcherTimer();
        int i, intQuestion;
        int rInt;
        int SF1a,SF2a,SF3a,SF4a,SF5a;
        string unique;
        List<string> lst_Questions = new List<string>();
        List<string> lst_Answers = new List<string>();
        DispatcherTimer snow_Timer = new DispatcherTimer();
        DispatcherTimer display_Timer = new DispatcherTimer();
        Random r = new Random();
        DispatcherTimer question_Timer = new DispatcherTimer();
        private MediaPlayer mediaplayer;

        public MainWindow()
        {
            InitializeComponent();
            config_Timer();
            load_Questions();
            lblName.Visibility = Visibility.Hidden;
            lblCalling.Visibility = Visibility.Hidden;
            Call_Canvas.Visibility = Visibility.Hidden;
            setting_Canvas.Visibility = Visibility.Hidden;
            unique = Properties.Settings.Default.savedUnique.ToString();

        }
        void load_Questions()
        {
            var list = new List<string>();
            string str_grotto = "Trivia.txt";
            string str_path = Properties.Settings.Default.savedLoc;
            string str_address = System.IO.Path.Combine(str_path, str_grotto);
            var fileStream = new FileStream(@str_address, FileMode.Open, System.IO.FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            i = 0;
            while (i<list.Count)
            {
                lst_Questions.Add( list[i].ToString());
                lst_Answers.Add( list[i + 1].ToString());
                i = i + 2;
            }
            intQuestion = 0;
            lblAnswer.Visibility = Visibility.Hidden;
            txtQuestion.Text = lst_Questions[intQuestion];
            lblAnswer.Content = lst_Answers[intQuestion];
        }

        void config_Timer()
        {
            //monitor timer
            mon_Timer.Interval = TimeSpan.FromMilliseconds(Properties.Settings.Default.savedInterval);
            mon_Timer.Tick += monitor_Pro;
            mon_Timer.Start();

            //Display Name Timer
            display_Timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.savedDisplay);
            display_Timer.Tick += display_Name;
            display_Timer.Stop();

            //snow fall timer
            snow_Timer.Interval = TimeSpan.FromMilliseconds(1);
            snow_Timer.Tick += snow_Fall;
            snow_Timer.Start();
            SF1a = 0;

            //question timer
            question_Timer.Interval = TimeSpan.FromSeconds(10);
            question_Timer.Tick += show_Question;
            question_Timer.Start();

        }
        void show_Question(object sender, EventArgs e)
        {
            next_question();
        }

        private void Main_Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                setting_Canvas.Visibility = Visibility.Visible;
                txtDisplay.Text = Settings.Default.savedDisplay.ToString();
                txtLoc.Text = Properties.Settings.Default.savedLoc.ToString();
                txtMonitor.Text = Properties.Settings.Default.savedInterval.ToString();
            }
        }

        private void cmdCancel_TouchDown(object sender, TouchEventArgs e)
        {
            setting_Canvas.Visibility = Visibility.Hidden;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.savedDisplay = Convert.ToDouble(txtDisplay.Text);
            Properties.Settings.Default.savedLoc = txtLoc.Text;
            Properties.Settings.Default.savedInterval = Convert.ToDouble(txtMonitor.Text);
            Properties.Settings.Default.Save();
            setting_Canvas.Visibility = Visibility.Hidden;
            display_Timer.Interval = TimeSpan.FromSeconds(Properties.Settings.Default.savedDisplay);
            mon_Timer.Interval = TimeSpan.FromMilliseconds(Properties.Settings.Default.savedInterval);
        }

        void next_question()
        {
            if (lblAnswer.Visibility == Visibility.Visible)
            {
                question_Timer.Interval = TimeSpan.FromSeconds(10);
                lblAnswer.Visibility = Visibility.Hidden;
                intQuestion = intQuestion + 1;
                if (intQuestion >= lst_Questions.Count)
                {
                    intQuestion = 0;
                }
                txtQuestion.Text = lst_Questions[intQuestion];
                //lblQuestion.Content = lst_Questions[intQuestion];
                lblAnswer.Content = lst_Answers[intQuestion];
            }
            else
            {
                lblAnswer.Visibility = Visibility.Visible;
                question_Timer.Interval = TimeSpan.FromSeconds(5);
            }
        }

        void randomAngle()
        {
            rInt = r.Next(1, 5);
            if (rInt == 1)
            {
                rInt = -1;
            }
            if (rInt == 2)
            {
                rInt = -2;
            }
            if (rInt == 3)
            {
                rInt = -3;
            }
            if (rInt == 4)
            {
                rInt = -4;
            }
            if (rInt == -5)
            {
                rInt = -5;
            }
        }

        void snow_Fall(object sender, EventArgs e)
        {

            double xx1 = Canvas.GetTop(img_SF1);
            double xx2 = Canvas.GetLeft(img_SF1);
            if (xx1 > 780 || xx2 < -140)
            {
                Canvas.SetTop(img_SF1, -125);
                rInt = r.Next(1, 1250);
                Canvas.SetLeft(img_SF1, rInt);
                randomAngle();
                SF1a = rInt;
            }
            else
            {
                xx2 = xx2 +SF1a;
                xx1 = xx1 + 2;
                Canvas.SetTop(img_SF1, xx1);
                Canvas.SetLeft(img_SF1, xx2);
            }
             xx1 = Canvas.GetTop(img_SF2);
             xx2 = Canvas.GetLeft(img_SF2);
            if (xx1 > 780 || xx2 < -140)
            {
                Canvas.SetTop(img_SF2, -125);
                rInt = r.Next(1, 1250);
                Canvas.SetLeft(img_SF2, rInt);
                randomAngle();
                SF2a = rInt;
            }
            else
            {
                xx2 = xx2 + SF2a; ;
                xx1 = xx1 + 2;
                Canvas.SetTop(img_SF2, xx1);
                Canvas.SetLeft(img_SF2, xx2);
            }
            xx1 = Canvas.GetTop(img_SF3);
            xx2 = Canvas.GetLeft(img_SF3);
            if (xx1 > 780 || xx2 < -140)
            {
                Canvas.SetTop(img_SF3, -125);
                rInt = r.Next(1, 1250);
                Canvas.SetLeft(img_SF3, rInt);
                randomAngle();
                SF3a = rInt;
            }
            else
            {
                xx2 = xx2 + SF3a; ;
                xx1 = xx1 + 2;
                Canvas.SetTop(img_SF3, xx1);
                Canvas.SetLeft(img_SF3, xx2);
            }
            xx1 = Canvas.GetTop(img_SF4);
            xx2 = Canvas.GetLeft(img_SF4);
            if (xx1 > 780 || xx2 < -140)
            {
                Canvas.SetTop(img_SF4, -125);
                rInt = r.Next(1, 1250);
                Canvas.SetLeft(img_SF4, rInt);
                randomAngle();
                SF4a = rInt;
            }
            else
            {
                xx2 = xx2 + SF4a; ;
                xx1 = xx1 + 2;
                Canvas.SetTop(img_SF4, xx1);
                Canvas.SetLeft(img_SF4, xx2);
            }
            xx1 = Canvas.GetTop(img_SF5);
            xx2 = Canvas.GetLeft(img_SF5);
            if (xx1 > 780 || xx2 < -140)
            {
                Canvas.SetTop(img_SF5, -125);
                rInt = r.Next(1, 1250);
                Canvas.SetLeft(img_SF5, rInt);
                randomAngle();
                SF5a = rInt;
            }
            else
            {
                xx2 = xx2 + SF5a; ;
                xx1 = xx1 + 2;
                Canvas.SetTop(img_SF5, xx1);
                Canvas.SetLeft(img_SF5, xx2);
            }

        }
        void display_Name(object sender, EventArgs e)
        {
            lblName.Visibility = Visibility.Hidden;
            lblCalling.Visibility = Visibility.Hidden;
            Call_Canvas.Visibility = Visibility.Hidden;
            mon_Timer.Start();
            question_Timer.Start();
            display_Timer.Stop();
        }

        void monitor_Pro(object sender, EventArgs e)
        {
            try
            {
                var list = new List<string>();
                string str_grotto = "Call.txt";
                string str_path = Properties.Settings.Default.savedLoc;
                string str_address = System.IO.Path.Combine(str_path, str_grotto);
                var fileStream = new FileStream(@str_address, FileMode.Open, System.IO.FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                fileStream.Close();

                if (unique == list[0].ToString())
                {   }
                else
                {
                    unique = list[0].ToString();
                    Properties.Settings.Default.savedUnique = unique;
                    Properties.Settings.Default.Save();
                    lblName.Content = list[1].ToString();
                    lblElf.Content = "Please see " + list[2].ToString() + " the elf";
                    lblName.Visibility = Visibility.Visible;
                    lblCalling.Visibility = Visibility.Visible;
                    Call_Canvas.Visibility = Visibility.Visible;
                    display_Timer.Start();
                    mon_Timer.Stop();
                    question_Timer.Stop();
                    str_grotto = "Ding Dong.wav";

                    str_address = Properties.Settings.Default.savedSound;
                    mediaplayer = new MediaPlayer();
                    //mediaplayer.MediaFailed += (o,args) =>
                    //{
                      //  MessageBox.Show("Media Failed",str_address);
                    //};
                    mediaplayer.Open(new Uri(str_address, UriKind.RelativeOrAbsolute));
                    mediaplayer.Play();

                }
            }
            catch
            { }
        }
    }
}
