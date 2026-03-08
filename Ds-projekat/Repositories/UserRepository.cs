using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public int Add(User u)
        {
            using var conn = Open();

            string sql =
@"INSERT INTO Users(FirstName, LastName, Email, Phone, MembershipTypeID, MembershipStartDate, MembershipEndDate, AccountStatus)
  VALUES(@fn,@ln,@em,@ph,@mt,@ms,@me,@st);
  " + Factory.LastInsertIdSql;

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@fn", u.FirstName));
            cmd.Parameters.Add(Factory.CreateParameter("@ln", u.LastName));
            cmd.Parameters.Add(Factory.CreateParameter("@em", u.Email));
            cmd.Parameters.Add(Factory.CreateParameter("@ph", u.Phone));
            cmd.Parameters.Add(Factory.CreateParameter("@mt", u.MembershipTypeID));
            cmd.Parameters.Add(Factory.CreateParameter("@ms", u.MembershipStartDate));
            cmd.Parameters.Add(Factory.CreateParameter("@me", u.MembershipEndDate));
            cmd.Parameters.Add(Factory.CreateParameter("@st", u.AccountStatus));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(User u)
        {
            using var conn = Open();

            string sql =
@"UPDATE Users
  SET FirstName=@fn, LastName=@ln, Email=@em, Phone=@ph,
      MembershipTypeID=@mt, MembershipStartDate=@ms, MembershipEndDate=@me, AccountStatus=@st
  WHERE UserID=@id";

            using var cmd = Factory.CreateCommand(sql, conn);
            cmd.Parameters.Add(Factory.CreateParameter("@fn", u.FirstName));
            cmd.Parameters.Add(Factory.CreateParameter("@ln", u.LastName));
            cmd.Parameters.Add(Factory.CreateParameter("@em", u.Email));
            cmd.Parameters.Add(Factory.CreateParameter("@ph", u.Phone));
            cmd.Parameters.Add(Factory.CreateParameter("@mt", u.MembershipTypeID));
            cmd.Parameters.Add(Factory.CreateParameter("@ms", u.MembershipStartDate));
            cmd.Parameters.Add(Factory.CreateParameter("@me", u.MembershipEndDate));
            cmd.Parameters.Add(Factory.CreateParameter("@st", u.AccountStatus));
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
            return r.Read() ? Map(r) : null;
        }

        public User GetByEmail(string email)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users WHERE Email=@em", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@em", email));

            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public List<User> GetAll()
        {
            var list = new List<User>();
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Users", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        private static User Map(dynamic r)
        {
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
                AccountStatus = r["AccountStatus"].ToString() ?? "Active"
            };
        }
    }
}
