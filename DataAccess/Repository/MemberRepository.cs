using BusinessObject.Management;
using BusinessObject.Models;
using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public void DeleteMember(Member member, MyDbContext dbContext)
        {
            MemberManagement.Instance.DeleteMember(member, dbContext);
        }

        public Member? GetMemberByID(int id, MyDbContext db)
        {
            return MemberManagement.Instance.GetMemberByID(id, db);
        }

        public IEnumerable<Member> GetMembers(MyDbContext dbContext)
        {
            return MemberManagement.Instance.GetMembers(dbContext);
        }

        public void InsertMember(Member member, MyDbContext dbContext)
        {
            MemberManagement.Instance.InsertMember(member, dbContext);
        }

        public void UpdateMember(Member member, MyDbContext dbContext)
        {
            MemberManagement.Instance.UpdateMember(member, dbContext);
        }
    }
}
