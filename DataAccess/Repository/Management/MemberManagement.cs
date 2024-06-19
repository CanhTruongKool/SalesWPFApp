using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Management
{
    public class MemberManagement
    {

        //Using singleton Pattern
        private static MemberManagement? instance = null;
        private static readonly object instanceLock = new object();
        private MemberManagement() { }
        public static MemberManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberManagement();
                    }
                    return instance;
                }
            }
        }
        public void DeleteMember(Member member, MyDbContext dbContext)
        {
            try
            {
                var oldMember = GetMemberByID(member.MemberId, dbContext);
                if (oldMember != null && dbContext != null)
                {
                    dbContext.Members.Remove(member);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The member has not been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Member? GetMemberByID(int id,MyDbContext dbContext)
        {
            Member? member;
            try
            {
                if(dbContext == null) {
                    throw new Exception("DBContext is null");
                }
                member = dbContext.Members.FirstOrDefault(member => member.MemberId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public IEnumerable<Member> GetMembers(MyDbContext dbContext)
        {
            List<Member> members;
            if(dbContext.Members == null)
            {
                return new List<Member>();
            }
            try
            {
                members = dbContext.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public void InsertMember(Member member, MyDbContext dbContext)
        {
            try
            {
                var oldMember = GetMemberByID(member.MemberId, dbContext);
                if (oldMember == null)
                {
                    dbContext.Members.Add(member);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The member has been exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member, MyDbContext dbContext)
        {
            try
            {
                // Find the existing entity in the database
                var existingMember = dbContext.Members.Find(member.MemberId);
                if (existingMember == null)
                {
                    throw new Exception("The member does not exist.");
                }
                if (member != null)
                {
                    // Update the existing entity's properties
                    dbContext.Entry(existingMember).CurrentValues.SetValues(member);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The member does not exist. ");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
