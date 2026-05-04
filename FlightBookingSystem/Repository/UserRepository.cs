using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    class UserRepository : IUserRepository
    {
        public User? FindUserByEmail(string email)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM USERS WHERE EMAIL = :email";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("email", email));

            using OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                Role role = Enum.Parse<Role>(reader["ROLE"].ToString()!);

                return new User(
                    reader["USERID"].ToString()!,
                    reader["EMAIL"].ToString()!,
                    reader["PASSWORDHASH"].ToString()!,
                    role,
                    reader["FIRSTNAME"].ToString()!,
                    reader["LASTNAME"].ToString()!,
                    Convert.ToDateTime(reader("DATEOFBIRTH")),
                    reader["ADDRESS"].ToString()!,
                    reader["PHONENUMBER"].ToString()!
                );
            }

            return null;
        }

        public void SaveUser(User user)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = @"INSERT INTO USERS (USERID, EMAIL, PASSWORDHASH, ROLE, FIRSTNAME, LASTNAME, DATEOFBIRTH, ADDRESS, PHONENUMBER) 
                            VALUES (:userId, :email, :passwordHash, :role, :firstName, :lastName, :dateOfBirth, :address, :phoneNumber)";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("userId", user.UserId));
            cmd.Parameters.Add(new OracleParameter("email", user.Email));
            cmd.Parameters.Add(new OracleParameter("passwordHash", user.PasswordHash));
            cmd.Parameters.Add(new OracleParameter("role", user.Role.ToString()));
            cmd.Parameters.Add(new OracleParameter("firstName", user.FirstName));
            cmd.Parameters.Add(new OracleParameter("lastName", user.LastName));
            cmd.Parameters.Add(new OracleParameter("dateOfBirth", user.DateOfBirth));  
            cmd.Parameters.Add(new OracleParameter("address", user.Address));
            cmd.Parameters.Add(new OracleParameter("phoneNumber", user.PhoneNumber));

            cmd.ExecuteNonQuery();              
        }

        public bool EmailExists(string email)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT 1 FROM USERS WHERE EMAIL = :email FETCH FIRST 1 ROWS ONLY";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("email", email));

            object result = cmd.ExecuteScalar();

            return result != null;
        }

    }
}