using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ForBelfort
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ForBELFORTEntities1())
                {
                    string enteredEmail = EmailTextBox.Text;
                    string enteredPassword = PasswordBox.Password;

                    var user = db.Users.FirstOrDefault(u => u.Email == enteredEmail);

                    if (user != null)
                    {
                        string hashedPassword = HashPassword(enteredPassword, user.Salt);
                        if (user.PasswordHash == hashedPassword)
                        {
                            MessageBox.Show("Успешный вход!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким email не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

    }
}
