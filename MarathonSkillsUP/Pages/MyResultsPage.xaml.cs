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

namespace MarathonSkillsUP.Pages
{
    public partial class MyResultsPage : Page
    {
        public class MarathonResult
        {
            public string Marathon { get; set; }
            public string Event { get; set; }
            public string Time { get; set; }
            public int OverallPlace { get; set; }
            public int CategoryPlace { get; set; }
        }

        public MyResultsPage()
        {
            InitializeComponent();
            LoadRunnerData();
            LoadResults();
        }

        private void LoadRunnerData()
        {
            // В реальном приложении загрузка из базы данных
            // Для демонстрации используем тестовые данные
            RunnerNameTextBlock.Text = "Иван Иванов";

            // Определение возрастной категории
            DateTime birthDate = new DateTime(1985, 5, 15);
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age--;

            string ageCategory = GetAgeCategory(age);
            GenderAgeTextBlock.Text = $"Мужской, {ageCategory}";
        }

        private string GetAgeCategory(int age)
        {
            if (age < 18)
                return "до 18 лет";
            else if (age >= 18 && age <= 29)
                return "от 18 до 29 лет";
            else if (age >= 30 && age <= 39)
                return "от 30 до 39 лет";
            else if (age >= 40 && age <= 55)
                return "от 40 до 55 лет";
            else if (age >= 56 && age <= 70)
                return "от 56 до 70 лет";
            else
                return "более 70 лет";
        }

        private void LoadResults()
        {
            // В реальном приложении загрузка из базы данных
            // Для демонстрации используем тестовые данные
            List<MarathonResult> results = new List<MarathonResult>
            {
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2015",
                    Event = "42km Полный марафон",
                    Time = "4ч 15м 22с",
                    OverallPlace = 183,
                    CategoryPlace = 45
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2014",
                    Event = "21km Полумарафон",
                    Time = "2ч 05м 15с",
                    OverallPlace = 151,
                    CategoryPlace = 34
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2013",
                    Event = "5km Малая дистанция",
                    Time = "0ч 25м 12с",
                    OverallPlace = 57,
                    CategoryPlace = 12
                }
            };

            ResultsDataGrid.ItemsSource = results;
            TotalMarathonsTextBlock.Text = results.Count.ToString();
        }

        private void ShowAllResultsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AllResultsPage());
        }
    }
}
