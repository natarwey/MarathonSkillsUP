using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class EditProfilePage : Page
    {
        private string selectedPhotoPath;
        private bool isPasswordValid = false;
        private bool isPasswordsMatch = false;
        private bool isBirthDateValid = false;

        public EditProfilePage()
        {
            InitializeComponent();
            LoadGenders();
            LoadCountries();
            LoadUserData();

            // Устанавливаем минимальную дату рождения (не менее 10 лет)
            BirthDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-10);
        }

        private void LoadGenders()
        {
            // В реальном приложении загрузка из базы данных
            GenderComboBox.Items.Add("Мужской");
            GenderComboBox.Items.Add("Женский");
            GenderComboBox.SelectedIndex = 0;
        }

        private void LoadCountries()
        {
            // В реальном приложении загрузка из базы данных
            CountryComboBox.Items.Add("Россия");
            CountryComboBox.Items.Add("США");
            CountryComboBox.Items.Add("Германия");
            CountryComboBox.Items.Add("Китай");
            CountryComboBox.Items.Add("Япония");
            CountryComboBox.SelectedIndex = 0;
        }

        private void LoadUserData()
        {
            // В реальном приложении загрузка из базы данных
            // Для демонстрации используем тестовые данные
            EmailTextBlock.Text = "runner@example.com";
            FirstNameTextBox.Text = "Иван";
            LastNameTextBox.Text = "Иванов";
            GenderComboBox.SelectedIndex = 0;
            BirthDatePicker.SelectedDate = new DateTime(1990, 1, 1);
            CountryComboBox.SelectedIndex = 0;
            RegionTextBox.Text = "Московская область";
            CityTextBox.Text = "Москва";

            // Загрузка фото
            try
            {
                // В реальном приложении загрузка из базы данных
                // Для демонстрации используем плейсхолдер
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("/Images/photo_placeholder.png", UriKind.Relative);
                bitmap.EndInit();
                RunnerImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                App.ShowMessageBox($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BrowsePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif",
                Title = "Выберите фотографию бегуна"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    selectedPhotoPath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedPhotoPath);
                    bitmap.EndInit();
                    RunnerImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    App.ShowMessageBox($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangePasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordChangePanel.Visibility = Visibility.Visible;
        }

        private void ChangePasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordChangePanel.Visibility = Visibility.Collapsed;
            CurrentPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
            ValidatePasswordsMatch();
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePasswordsMatch();
        }

        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateBirthDate();
        }

        private bool ValidatePassword()
        {
            if (!ChangePasswordCheckBox.IsChecked == true)
            {
                isPasswordValid = true;
                return true;
            }

            string password = NewPasswordBox.Password;

            if (password.Length < 6)
            {
                ShowError("Пароль должен содержать минимум 6 символов");
                isPasswordValid = false;
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                ShowError("Пароль должен содержать минимум 1 прописную букву");
                isPasswordValid = false;
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                ShowError("Пароль должен содержать минимум 1 цифру");
                isPasswordValid = false;
                return false;
            }

            if (!Regex.IsMatch(password, "[!@#$%^]"))
            {
                ShowError("Пароль должен содержать минимум один из символов: ! @ # $ % ^");
                isPasswordValid = false;
                return false;
            }

            isPasswordValid = true;
            HideError();
            return true;
        }

        private bool ValidatePasswordsMatch()
        {
            if (!ChangePasswordCheckBox.IsChecked == true)
            {
                isPasswordsMatch = true;
                return true;
            }

            isPasswordsMatch = NewPasswordBox.Password == ConfirmPasswordBox.Password;

            if (!isPasswordsMatch)
            {
                ShowError("Пароли не совпадают");
                return false;
            }

            HideError();
            return true;
        }

        private bool ValidateBirthDate()
        {
            if (BirthDatePicker.SelectedDate == null)
            {
                ShowError("Выберите дату рождения");
                isBirthDateValid = false;
                return false;
            }

            DateTime birthDate = BirthDatePicker.SelectedDate.Value;
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age--;

            if (age < 10)
            {
                ShowError("Бегуну должно быть не менее 10 лет");
                isBirthDateValid = false;
                return false;
            }

            isBirthDateValid = true;
            HideError();
            return true;
        }

        private bool ValidateAllFields()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                ShowError("Введите имя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                ShowError("Введите фамилию");
                return false;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                ShowError("Выберите дату рождения");
                return false;
            }

            if (string.IsNullOrWhiteSpace(RegionTextBox.Text))
            {
                ShowError("Введите регион");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                ShowError("Введите город");
                return false;
            }

            if (ChangePasswordCheckBox.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(CurrentPasswordBox.Password))
                {
                    ShowError("Введите текущий пароль");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(NewPasswordBox.Password))
                {
                    ShowError("Введите новый пароль");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
                {
                    ShowError("Повторите новый пароль");
                    return false;
                }

                if (!ValidatePassword() || !ValidatePasswordsMatch())
                {
                    return false;
                }
            }

            if (!ValidateBirthDate())
            {
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAllFields())
            {
                try
                {
                    // В реальном приложении здесь был бы код для сохранения данных в базу данных

                    App.ShowMessageBox("Профиль успешно обновлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    App.ShowMessageBox($"Ошибка при сохранении профиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
