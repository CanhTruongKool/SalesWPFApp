using BusinessObject.Models;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
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

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MemberDetail.xaml
    /// </summary>
    public partial class MemberDetail : Window
    {
        Member editMember;
        MyDbContext db;
        IMemberRepository memberRepository = new MemberRepository();
        bool isNew;
        int _memberId;
        public MemberDetail(Member member, MyDbContext dbContext, bool IsNew,int memberId)
        {
            InitializeComponent();
            editMember = member;
            db = dbContext;
            isNew = IsNew;
            _memberId = memberId;
            if (!isNew)
            {
                InitData(editMember);
                lbTitle.Content = "Edit Member";
                if(_memberId == 0)
                {
                    txtPassword.IsEnabled = false;
                }
            }
            else
            {
                lbTitle.Content = "Create Member";
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void InitData(Member member)
        {
            txtMemberId.Text = member.MemberId.ToString();
            txtMemberName.Text = member.Fullname;
            txtCompany.Text = member.CompanyName;
            txtPassword.Password = member.Password;
            txtCity.Text = member.City;
            txtCountry.Text = member.Country;
            txtEmail.Text = member.Email;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            editMember = GetMember();
            if(isNew)
            {
                CreateMember(editMember);
            }
            else
            {
                UpdateMember(editMember);
            }
        }
        private Member GetMember()
        {
            return new Member()
            {
                Fullname = txtMemberName.Text,
                CompanyName = txtCompany.Text,
                Password = txtPassword.Password,
                City = txtCity.Text,
                Country = txtCountry.Text,
                Email = txtEmail.Text,
            };
        }
        private void CreateMember(Member member)
        {
            try
            {
                if (CheckValidEmail(txtEmail.Text)) 
                {
                    memberRepository.InsertMember(member, db);
                }
                else
                {
                    MessageBox.Show("Invalid Email");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            MessageBox.Show("Create successfully");
            this.Close();   
        }
        private void UpdateMember(Member member)
        {
            try
            { 
                if(int.TryParse(txtMemberId.Text, out int memberId))
                {
                    member.MemberId = memberId;
                }
                else
                {
                    throw new Exception("ID is not valid");
                }

                if (CheckValidEmail(txtEmail.Text))
                {
                    memberRepository.UpdateMember(member, db);
                }
                else
                {
                    MessageBox.Show("Invalid Email");
                    return;
                }
               
            }
            catch(Exception ex) { 
                throw new Exception(ex.Message);
            }
            MessageBox.Show("Update successfully");
            this.Close ();
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
