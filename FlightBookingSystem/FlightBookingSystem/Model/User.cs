using System;

namespace FBS.Model
{
    class User
    {
        // private fields (attributes)
        private string userId;
        private string email;
        private string password;
        private Role role;
        private string name;
        private string dateOfBirth;
        private string address;
        private string phoneNumber;

        // User Constructor 
        public User(string userId, string email, string password, Role role, string name, 
                   string dateOfBirth, string address, string phoneNumber)
        {
            this.userId = userId;
            this.email = email;
            this.password = password;
            this.role = role;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

        // public properties (read-only)
        public string UserId
        {
            get{return userId;}     
        }

        public string Email
        {
            get{return email;}   
        }

        public string Password
        {
            get{return password;}    
        }

        public Role UserRole
        {
            get {return role;}    
        }

        public string Name
        {
            get{return name;}
        }
        
        public string DateOfBirth
        {
            get{return dateOfBirth;}
        }
        
        public string Address
        {
            get{return address;}
        }

        public string PhoneNumber
        {
            get{return phoneNumber;}
        }

    }
}