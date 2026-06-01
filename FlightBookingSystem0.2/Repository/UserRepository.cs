using FlightBookingSystem.Factory;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    internal class UserRepository : IUserRepository
    {
        public User? FindUserById(string userId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    u.USERID,
                    u.EMAIL,
                    u.PASSWORDHASH,
                    u.FIRSTNAME,
                    u.LASTNAME,
                    u.DATEOFBIRTH,
                    u.ADDRESS,
                    u.PHONENUMBER,
                    c.LOYALTYPOINTS,
                    c.MEMBERSHIPTIER,
                    a.ADMINLEVEL
                FROM USERS u
                LEFT JOIN CUSTOMERS c ON u.USERID = c.USERID
                LEFT JOIN ADMINS a ON u.USERID = a.USERID
                WHERE u.USERID = :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {

                return MapUser(reader);
            }

            return null;
        }
        public User? FindUserByEmail(string email)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT
                    u.USERID,
                    u.EMAIL,
                    u.PASSWORDHASH,
                    u.FIRSTNAME,
                    u.LASTNAME,
                    u.DATEOFBIRTH,
                    u.ADDRESS,
                    u.PHONENUMBER,
                    c.LOYALTYPOINTS,
                    c.MEMBERSHIPTIER,
                    a.ADMINLEVEL
                FROM USERS u
                LEFT JOIN CUSTOMERS c ON u.USERID = c.USERID
                LEFT JOIN ADMINS a ON u.USERID = a.USERID
                WHERE u.EMAIL = :email";


            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("email", email));

            using OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        public void SaveUser(User user)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            using OracleTransaction transaction = conn.BeginTransaction();
            
            try
            {
                string query = @"
                    INSERT INTO USERS 
                    (
                        USERID, EMAIL, PASSWORDHASH, FIRSTNAME, LASTNAME, 
                        DATEOFBIRTH, ADDRESS, PHONENUMBER
                    ) 
                    VALUES 
                    (
                        :userId, :email, :passwordHash, :firstName, :lastName, 
                        :dateOfBirth, :address, :phoneNumber
                    )";

                using OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;

                cmd.Parameters.Add(new OracleParameter("userId", user.UserId));
                cmd.Parameters.Add(new OracleParameter("email", user.Email));
                cmd.Parameters.Add(new OracleParameter("passwordHash", user.PasswordHash));
                cmd.Parameters.Add(new OracleParameter("firstName", user.FirstName));
                cmd.Parameters.Add(new OracleParameter("lastName", user.LastName));
                cmd.Parameters.Add(new OracleParameter("dateOfBirth", user.DateOfBirth));  
                cmd.Parameters.Add(new OracleParameter("address", user.Address));
                cmd.Parameters.Add(new OracleParameter("phoneNumber", user.PhoneNumber));

                cmd.ExecuteNonQuery();   

                if(user is Customer customer)
                {
                    string customerQuery = @"
                        INSERT INTO CUSTOMERS
                        (
                            USERID, LOYALTYPOINTS, MEMBERSHIPTIER
                        )
                        VALUES
                        (
                            :userId, :loyaltyPoints, :membershipTier
                        )";
                    
                    using OracleCommand customerCmd = new OracleCommand(customerQuery, conn);
                    customerCmd.Transaction = transaction;

                    customerCmd.Parameters.Add(new OracleParameter("userId", customer.UserId));
                    customerCmd.Parameters.Add(new OracleParameter("loyaltyPoints", customer.LoyaltyPoints));
                    customerCmd.Parameters.Add(new OracleParameter("membershipTier", customer.MembershipTier.ToString()));

                    customerCmd.ExecuteNonQuery();
                }
                else if (user is Admin admin)
                {
                    string adminQuery = @"
                        INSERT INTO ADMINS
                        (
                            USERID, ADMINLEVEL
                        )
                        VALUES
                        (
                            :userId, :adminLevel
                        )";
                    
                    using OracleCommand adminCmd = new OracleCommand(adminQuery, conn);
                    adminCmd.Transaction = transaction;

                    adminCmd.Parameters.Add(new OracleParameter("userId", admin.UserId));
                    adminCmd.Parameters.Add(new OracleParameter("adminLevel", admin.AdminLevel.ToString()));

                    adminCmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool UpdateUser(User user)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                UPDATE USERS
                SET
                    EMAIL = :email,
                    FIRSTNAME = :firstName,
                    LASTNAME = :lastName,
                    DATEOFBIRTH = :dateOfBirth,
                    ADDRESS = :address,
                    PHONENUMBER = :phoneNumber
                WHERE USERID = :userId";
            
            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("email", user.Email));
            cmd.Parameters.Add(new OracleParameter("firstName", user.FirstName));
            cmd.Parameters.Add(new OracleParameter("lastName", user.LastName));
            cmd.Parameters.Add(new OracleParameter("dateOfBirth", user.DateOfBirth));
            cmd.Parameters.Add(new OracleParameter("address", user.Address));
            cmd.Parameters.Add(new OracleParameter("phoneNumber", user.PhoneNumber));
            cmd.Parameters.Add(new OracleParameter("userId", user.UserId));

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;

        }

        public bool UpdatePassword(string userId, string newPasswordHash)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                UPDATE USERS
                SET PASSWORDHASH = :passwordHash
                WHERE USERID = :userId";
            
            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("passwordHash", newPasswordHash));
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        private User MapUser(OracleDataReader reader)
        {
            bool isCustomer = reader["LOYALTYPOINTS"] != DBNull.Value;
            bool isAdmin = reader["ADMINLEVEL"] != DBNull.Value;

            if(isCustomer)
            {
                string userId = reader["USERID"].ToString()!;
                string email = reader["EMAIL"].ToString()!;
                string passwordHash = reader["PASSWORDHASH"].ToString()!;
                string firstName = reader["FIRSTNAME"].ToString()!;
                string lastName = reader["LASTNAME"].ToString()!;
                DateTime dateOfBirth = Convert.ToDateTime(reader["DATEOFBIRTH"]);
                string address = reader["ADDRESS"].ToString()!;
                string phoneNumber = reader["PHONENUMBER"].ToString()!;
                int loyaltyPoints = Convert.ToInt32(reader["LOYALTYPOINTS"]);
                MembershipTier membershipTier = Enum.Parse<MembershipTier>(reader["MEMBERSHIPTIER"].ToString()!
                );

                return UserFactory.CreateCustomerFromDatabase(
                    userId,
                    email,
                    passwordHash,
                    firstName,
                    lastName,
                    dateOfBirth,
                    address,
                    phoneNumber,
                    loyaltyPoints,
                    membershipTier
                );
            }

            if(isAdmin)
            {
                string userId = reader["USERID"].ToString()!;
                string email = reader["EMAIL"].ToString()!;
                string passwordHash = reader["PASSWORDHASH"].ToString()!;
                string firstName = reader["FIRSTNAME"].ToString()!;
                string lastName = reader["LASTNAME"].ToString()!;
                DateTime dateOfBirth = Convert.ToDateTime(reader["DATEOFBIRTH"]);
                string address = reader["ADDRESS"].ToString()!;
                string phoneNumber = reader["PHONENUMBER"].ToString()!;
                AdminLevel adminLevel = Enum.Parse<AdminLevel>(reader["ADMINLEVEL"].ToString()!
                );

                return UserFactory.CreateAdmin(
                    userId,
                    email,
                    passwordHash,
                    firstName,
                    lastName,
                    dateOfBirth,
                    address,
                    phoneNumber,
                    adminLevel
                );
            }

            throw new Exception("User exists in USERS table but is not registered as Customer or Admin");

        }
        

        public bool EmailExists(string email)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "SELECT 1 FROM USERS WHERE EMAIL = :email FETCH FIRST 1 ROWS ONLY";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("email", email));

            object result = cmd.ExecuteScalar();

            return result != null;
        }

        public void UpdateCustomerLoyalty(Customer customer)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                UPDATE CUSTOMERS
                SET 
                    LOYALTYPOINTS = :loyaltyPoints,
                    MEMBERSHIPTIER = :membershipTier
                WHERE USERID = :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add("loyaltyPoints", OracleDbType.Int32).Value = customer.LoyaltyPoints;
            cmd.Parameters.Add("membershipTier", OracleDbType.Varchar2).Value = customer.MembershipTier;
            cmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = customer.UserId;

            cmd.ExecuteNonQuery();
        }


        public bool UserIdExists(string userId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM USERS WHERE USERID = :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

    }
}