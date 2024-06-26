﻿using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;

            var pass = PassBox.Text;

            var pass2 = PassBox2.Text;

            var email = MailBox.Text;
            
            var context = new AppDbContext();

            var user_exists = context.Users.FirstOrDefault(x => x.Login == login);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже зарегестрирован");
                return;
            }
            if (email.Length == 0)
            {
                MailBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Введите почту!");
                return;
            }
            else if (!Regex.IsMatch(email, @"[@]"))
            {
               MailBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Некоректная почта");
                return;
            }
            if (pass.Length == 0)
            {
                PassBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Введите пароль");
                return;
            }
            else if (pass.Length < 8)
            {
                PassBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Пароль должен состоять не менее чем из 8 символов");
                return;
            }
            else if (!Regex.IsMatch(pass, @"[!@#$%^&*()_+]"))
            {
                PassBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("В пароле должен быть хотя бы 1 специальный символ");
                return;
            }
            else if (pass != pass2)
            {
                PassBox2.BorderBrush = new SolidColorBrush(Colors.Red);
                PassBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Пароли не совпадают");
                return;
            }
                      
            var user = new User { Login = login, Password = pass, Email = email };
            context.Users.Add(user);
            context.SaveChanges();
            MessageBox.Show("Вы успешно зарегестрировались");
        }
    }
}
