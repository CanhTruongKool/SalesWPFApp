using BusinessObject.Models;

namespace DataAccess.Repository.Interface
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers(MyDbContext dbContext);
        Member? GetMemberByID(int id, MyDbContext dbContext);
        void InsertMember(Member member, MyDbContext dbContext);
        void UpdateMember(Member member, MyDbContext dbContext);
        void DeleteMember(Member member, MyDbContext dbContext);
    }
}
