using BusinessObject.Models;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string _adminEmail, _adminPassword;
        public int? LoggedInMemberId { get; private set; }
        List<Member> _members;
        public LoginWindow(string adminEmail, string adminPassword, List<Member> members)
        {
            InitializeComponent();
            _adminEmail = adminEmail;
            _adminPassword = adminPassword;
            _members = members;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Perform login logic
            bool loginSuccessful = false;
            /*
            if (CheckValidEmail(Email.Text))
            {
                MessageBox.Show("Invalid email");
                return;
            }*/
            if(Email.Text.Equals(_adminEmail) && Password.Password.Equals(_adminPassword))
            {
                MessageBox.Show("Login successful");
                loginSuccessful = true;
                LoggedInMemberId = 0;
            }
            else
            if (MemberLogin(_members))
            {
                loginSuccessful = true;
            }
            else
            {
                loginSuccessful = false;
            }

            if (loginSuccessful)
            {
                // Set DialogResult to true to indicate successful login
                DialogResult = true;
            }
            else
            {
                // Optionally display error message
                MessageBox.Show("Login failed. Please try again.");
            }
        }

        private void Grid_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool MemberLogin(List<Member> members)
        {
            foreach (Member member in members)
            {
                if (member.Email.Equals(Email.Text) && member.Password.Equals(Password.Password))
                {
                    LoggedInMemberId = member.MemberId;
                    return true;
                }
            }
            return false;
        }

        private bool CheckValidEmail(string email)
        {
            Regex emailRegex = new Regex(
                                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                                        RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return emailRegex.IsMatch(email);
        }

    }
}
