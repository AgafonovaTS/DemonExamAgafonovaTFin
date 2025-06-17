using DemoExamAgafonova;
using DemonExamAgafonova;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace DemonExamAgafonova
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = txtUserName.Text;
            string password = txtPassw.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Значение обоих полей должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            login = login.Trim();
            password = password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Значение обоих полей должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            login = login.Trim();
            password = password.Trim();

            using (var Context = new HotelTable2Entities())
            {
                var user = Context.Users
                    .Where(u => u.lastname == login && u.password == password).FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("Вы ввели неправильные логин и пароль. Проверьте введенные данные и попробуйте еще раз.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (user.isLocked.HasValue && user.isLocked.Value)
                {
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору.", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (user.lastLoginDate != null && (DateTime.Now - user.lastLoginDate.Value).TotalDays > 30 && user.role != "admin")
                {
                    user.isLocked = true;
                    Context.SaveChanges();
                    MessageBox.Show("Ваша учетная запись заблокирована из-за длительного отсутствия входа. Обратитесь к администратору.", "Доступ запрещен",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (user.password == password)
                {
                    user.lastLoginDate = DateTime.Now;
                    user.FailedLoginAttempts = 0;
                    Context.SaveChanges();
                    MessageBox.Show("Вы успешно авторизовались. Добро пожаловать!", "Успех", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    this.Close();
                }

                else
                {

                    user.FailedLoginAttempts++;
                    Context.SaveChanges();
                    MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }
            }
        }
    }
}