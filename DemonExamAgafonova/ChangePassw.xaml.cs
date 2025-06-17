using DemonExamAgafonova;
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
using System.Windows.Shapes;

namespace DemoExamAgafonova
{
    /// <summary>
    /// Логика взаимодействия для ChangePassw.xaml
    /// </summary>
    public partial class ChangePassw : Window
    {
        private readonly int _userId;

        public ChangePassw(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = txtCurrentPassword.Password;
            string newPassword = txtNewPassword.Password;
            string confirNewPassword = txtCurrentPassword.Password;

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirNewPassword))
            {
                MessageBox.Show(messageBoxText: "Все поля обязательны для заполнения", caption: "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword != confirNewPassword)
            {
                MessageBox.Show(messageBoxText: "Пароли не совпадают", caption: "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new HotelTable2Entities())
            {
                var user = context.Users.FirstOrDefault(u => u.id == _userId);

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                if (user.password != currentPassword)
                {
                    MessageBox.Show("Текущий пароль неверен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                user.password = newPassword;
                user.firstLogin = false;

                context.SaveChanges();

                MessageBox.Show("Пароль успешно изменён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}
