using BusinessObject.Models;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MemberManagement : UserControl
    {
        IMemberRepository memberRepository;
        MyDbContext dbContext;
        int _memberId;
        public MemberManagement(IMemberRepository repository, MyDbContext myDbContext, int memberId)
        {
            InitializeComponent();
            memberRepository = repository;
            dbContext = myDbContext;
            _memberId = memberId;
        }
        public void LoadMemberList(MyDbContext db)
        {
            lvMembers.ItemsSource = memberRepository.GetMembers(db);
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMemberList(dbContext);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberId == 0) {
                MemberDetail detail = new MemberDetail(new Member(), dbContext, true, _memberId);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("You can not add new member");
            }
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != memberId && _memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit other member");
                return;
            }
            var member = memberRepository.GetMemberByID(memberId, dbContext);
            if (member != null)
            {
                MemberDetail detail = new MemberDetail(member, dbContext, false, _memberId);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }
        // Event handler for when the MessageBoxWindow is closed
        private void MessageBoxWindow_Closed(object? sender, System.EventArgs e)
        {
            // Refresh the ListBox when the MessageBoxWindow is closed
            LoadMemberList(dbContext);
            // Refresh item
            lvMembers.SelectedItem = null;
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != memberId && _memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot delete other member");
                return;
            }
            var member = memberRepository.GetMemberByID(memberId, dbContext);
            if (member != null)
            {
                memberRepository.DeleteMember(member, dbContext);
                LoadMemberList(dbContext);
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }

        private void lvMembers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Please enter ID ");
                return;
            };
            if (_memberId != memberId && _memberId != 0)
            {
                MessageBox.Show("You are not admin, you cannot edit other member");
                return;
            }
            var member = memberRepository.GetMemberByID(memberId, dbContext);
            if (member != null)
            {
                MemberDetail detail = new MemberDetail(member, dbContext, false, _memberId);
                detail.Closed += MessageBoxWindow_Closed; // Subscribe to the Closed event
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please enter ID");
            }
        }
    }
}