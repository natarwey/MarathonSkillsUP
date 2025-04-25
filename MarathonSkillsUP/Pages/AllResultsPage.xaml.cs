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
    public partial class AllResultsPage : Page
    {
        public class MarathonResult
        {
            public string Marathon { get; set; }
            public string Event { get; set; }
            public string Runner { get; set; }
            public string Country { get; set; }
            public string Time { get; set; }
            public int Place { get; set; }
        }

        public AllResultsPage()
        {
            InitializeComponent();
            LoadResults();
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
                    Runner = "Иван Иванов",
                    Country = "Россия",
                    Time = "4ч 15м 22с",
                    Place = 183
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2015",
                    Event = "42km Полный марафон",
                    Runner = "Петр Петров",
                    Country = "Россия",
                    Time = "4ч 05м 10с",
                    Place = 151
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2015",
                    Event = "21km Полумарафон",
                    Runner = "Анна Смирнова",
                    Country = "Россия",
                    Time = "2ч 05м 15с",
                    Place = 57
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2014",
                    Event = "42km Полный марафон",
                    Runner = "Иван Иванов",
                    Country = "Россия",
                    Time = "4ч 20м 30с",
                    Place = 200
                },
                new MarathonResult
                {
                    Marathon = "Marathon Skills 2014",
                    Event = "21km Полумарафон",
                    Runner = "Иван Иванов",
                    Country = "Россия",
                    Time = "2ч 05м 15с",
                    Place = 151
                }
            };

            ResultsDataGrid.ItemsSource = results;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
