using System;
using System.Collections.Generic;

namespace Ds_projekat
{
    internal class AdminRepository : BaseRepository, IAdminRepository
    {
        public bool Add(Admin a)
        {
            using var conn = Open();
            string sql = "INSERT INTO Admins(UserID, RoleName, Username, HashedPass) VALUES(@uid,@r,@u,@p)";
            using var cmd = Factory.CreateCommand(sql, conn);

            cmd.Parameters.Add(Factory.CreateParameter("@uid", a.UserID));
            cmd.Parameters.Add(Factory.CreateParameter("@r", a.RoleName));
            cmd.Parameters.Add(Factory.CreateParameter("@u", a.Username));
            cmd.Parameters.Add(Factory.CreateParameter("@p", a.HashedPass));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(Admin a)
        {
            using var conn = Open();
            string sql = "UPDATE Admins SET RoleName=@r, Username=@u, HashedPass=@p WHERE UserID=@uid";
            using var cmd = Factory.CreateCommand(sql, conn);

            cmd.Parameters.Add(Factory.CreateParameter("@r", a.RoleName));
            cmd.Parameters.Add(Factory.CreateParameter("@u", a.Username));
            cmd.Parameters.Add(Factory.CreateParameter("@p", a.HashedPass));
            cmd.Parameters.Add(Factory.CreateParameter("@uid", a.UserID));

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int userId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("DELETE FROM Admins WHERE UserID=@uid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
            return cmd.ExecuteNonQuery() > 0;
        }

        public Admin? GetByUserId(int userId)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Admins WHERE UserID=@uid", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@uid", userId));
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Admin
            {
                UserID = Convert.ToInt32(r["UserID"]),
                RoleName = r["RoleName"].ToString() ?? "",
                Username = r["Username"].ToString() ?? "",
                HashedPass = r["HashedPass"].ToString() ?? ""
            };
        }

        public Admin? GetByUsername(string username)
        {
            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Admins WHERE Username=@u", conn);
            cmd.Parameters.Add(Factory.CreateParameter("@u", username));
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Admin
            {
                UserID = Convert.ToInt32(r["UserID"]),
                RoleName = r["RoleName"].ToString() ?? "",
                Username = r["Username"].ToString() ?? "",
                HashedPass = r["HashedPass"].ToString() ?? ""
            };
        }
        public List<Admin> GetAll()
        {
            List<Admin> list = new List<Admin>();

            using var conn = Open();
            using var cmd = Factory.CreateCommand("SELECT * FROM Admins ORDER BY UserID", conn);
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                list.Add(new Admin
                {
                    UserID = Convert.ToInt32(r["UserID"]),
                    RoleName = r["RoleName"].ToString() ?? "",
                    Username = r["Username"].ToString() ?? "",
                    HashedPass = r["HashedPass"].ToString() ?? ""
                });
            }

            return list;
        }
    }
}