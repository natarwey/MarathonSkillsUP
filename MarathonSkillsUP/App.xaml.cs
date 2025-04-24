using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarathonSkillsUP
{
    public partial class App : Application
    {
        public static DateTime MarathonStart { get; } = new DateTime(2016, 11, 24, 6, 0, 0);

        public static string GetRemainingTimeString()
        {
            TimeSpan timeLeft = MarathonStart - DateTime.Now;
            if (timeLeft.TotalSeconds > 0)
            {
                return $"{timeLeft.Days} дней {timeLeft.Hours} часов и {timeLeft.Minutes} минут до старта марафона!";
            }
            else
            {
                return "Марафон уже начался!";
            }
        }

        // Вспомогательные методы для работы с сообщениями и навигацией
        public static void ShowMessageBox(string message, string title = "Информация", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(message, title, buttons, icon);
        }

        public static void ShowContactsPopup()
        {
            // Создаем всплывающее окно
            Window popup = new Window
            {
                Title = "Контакты",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            // Создаем содержимое окна
            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            TextBlock titleLabel = new TextBlock
            {
                Text = "Контакты",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock infoLabel = new TextBlock
            {
                Text = "Для получения дополнительной информации\nпожалуйста свяжитесь с координаторами",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock phoneLabel = new TextBlock
            {
                Text = "Телефон: +55 11 9988 7766",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            };

            TextBlock emailLabel = new TextBlock
            {
                Text = "Email: coordinators@marathonskills.org",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 20)
            };

            Button closeButton = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            closeButton.Click += (s, e) => popup.Close();

            // Добавляем элементы в панель
            panel.Children.Add(titleLabel);
            panel.Children.Add(infoLabel);
            panel.Children.Add(phoneLabel);
            panel.Children.Add(emailLabel);
            panel.Children.Add(closeButton);

            // Устанавливаем содержимое окна
            popup.Content = panel;

            // Показываем окно
            popup.ShowDialog();
        }
    }
}
