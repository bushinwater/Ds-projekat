using Ds_projekat.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds_projekat.Repositories
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public int Add(User u)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO Users(FirstName, LastName, Email, Phone, MembershipTypeID, MembershipStartDate, MembershipEndDate, AccountStatus)
  VALUES(@fn, @ln, @email, @phone, @mtid, @start, @end, @status);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@fn", u.FirstName));
            cmd.Parameters.Add(Factory.CreateParameter("@ln", u.LastName));
            cmd.Parameters.Add(Factory.CreateParameter("@email", u.Email));
            cmd.Parameters.Add(Factory.CreateParameter("@phone", u.Phone));
            cmd.Parameters.Add(Factory.CreateParameter("@mtid", u.MembershipTypeID));
            cmd.Parameters.Add(Factory.CreateParameter("@start", u.MembershipStartDate));
            cmd.Parameters.Add(Factory.CreateParameter("@end", u.MembershipEndDate));
            cmd.Parameters.Add(Factory.CreateParameter("@status", u.AccountStatus));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(User u)
        {
            using var conn = Open();

            string sql =
@"UPDATE Users
  SET FirstName=@fn, LastName=@ln, Email=@email, Phone=@phone,
      MembershipTypeID=@mtid, MembershipStartDate=@start, MembershipEndDate=@end, AccountStatus=@status
  WHERE UserID=@id";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@fn", u.FirstName));
            cmd.Parameters.Add(Factory.CreateParameter("@ln", u.LastName));
            cmd.Parameters.Add(Factory.CreateParameter("@email", u.Email));
            cmd.Parameters.Add(Factory.CreateParameter("@phone", u.Phone));
            cmd.Parameters.Add(Factory.CreateParameter("@mtid", u.MembershipTypeID));
            cmd.Parameters.Add(Factory.CreateParameter("@start", u.MembershipStartDate));
            cmd.Parameters.Add(Factory.CreateParameter("@end", u.MembershipEndDate));
            cmd.Parameters.Add(Factory.CreateParameter("@status", u.AccountStatus));
            cmd.Parameters.Add(Factory.CreateParameter("@id", u.UserID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM Users WHERE UserID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));
            return cmd.ExecuteNonQuery() > 0;
        }

        public User GetById(int id)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users WHERE UserID=@id", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@id", id));

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new User
            {
                UserID = Convert.ToInt32(r["UserID"]),
                FirstName = r["FirstName"].ToString() ?? "",
                LastName = r["LastName"].ToString() ?? "",
                Email = r["Email"].ToString() ?? "",
                Phone = r["Phone"].ToString() ?? "",
                MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                MembershipStartDate = Convert.ToDateTime(r["MembershipStartDate"]),
                MembershipEndDate = Convert.ToDateTime(r["MembershipEndDate"]),
                AccountStatus = r["AccountStatus"].ToString() ?? ""
            };
        }

        public List<User> GetAll()
        {
            var list = new List<User>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users", conn);
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new User
                {
                    UserID = Convert.ToInt32(r["UserID"]),
                    FirstName = r["FirstName"].ToString() ?? "",
                    LastName = r["LastName"].ToString() ?? "",
                    Email = r["Email"].ToString() ?? "",
                    Phone = r["Phone"].ToString() ?? "",
                    MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                    MembershipStartDate = Convert.ToDateTime(r["MembershipStartDate"]),
                    MembershipEndDate = Convert.ToDateTime(r["MembershipEndDate"]),
                    AccountStatus = r["AccountStatus"].ToString() ?? ""
                });
            }
            return list;
        }

        public List<User> GetByMembershipType(int membershipTypeId)
        {
            var list = new List<User>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users WHERE MembershipTypeID=@mtid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@mtid", membershipTypeId));
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new User
                {
                    UserID = Convert.ToInt32(r["UserID"]),
                    FirstName = r["FirstName"].ToString() ?? "",
                    LastName = r["LastName"].ToString() ?? "",
                    Email = r["Email"].ToString() ?? "",
                    Phone = r["Phone"].ToString() ?? "",
                    MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                    MembershipStartDate = Convert.ToDateTime(r["MembershipStartDate"]),
                    MembershipEndDate = Convert.ToDateTime(r["MembershipEndDate"]),
                    AccountStatus = r["AccountStatus"].ToString() ?? ""
                });
            }
            return list;
        }

        public List<User> GetByStatus(string status)
        {
            var list = new List<User>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users WHERE AccountStatus=@status", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@status", status));
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new User
                {
                    UserID = Convert.ToInt32(r["UserID"]),
                    FirstName = r["FirstName"].ToString() ?? "",
                    LastName = r["LastName"].ToString() ?? "",
                    Email = r["Email"].ToString() ?? "",
                    Phone = r["Phone"].ToString() ?? "",
                    MembershipTypeID = Convert.ToInt32(r["MembershipTypeID"]),
                    MembershipStartDate = Convert.ToDateTime(r["MembershipStartDate"]),
                    MembershipEndDate = Convert.ToDateTime(r["MembershipEndDate"]),
                    AccountStatus = r["AccountStatus"].ToString() ?? ""
                });
            }
            return list;
        }
    }
}
