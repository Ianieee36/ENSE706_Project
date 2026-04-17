using System;

namespace FlightBookingSystem.Model
{
    public class User
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

        // User constructor 
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
            private set{userId = value;}     
        }

        public string Email
        {
            get{return email;}
            private set{email = value;}   
        }

        public string Password
        {
            get{return password;}    
            private set{password = value;}
        }

        public Role UserRole
        {
            get {return role;}    
        }

        public string Name
        {
            get{return name;}
            private set{name = value;}
        }
        
        public DateTime DateOfBirth
        {
            get{return dateOfBirth;}
            private set{dateOfBirth = value;}
        }
        
        public string Address
        {
            get{return address;}
            private set{address = value;}
        }

        public string PhoneNumber
        {
            get{return phoneNumber;}
            private set{phoneNumber = value;}
        }

        public void UpdateProfile(string newName, string newAddress, string newPhoneNumber)
        {
            Name = newName;
            Address = newAddress;
            PhoneNumber = newPhoneNumber;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if(Password != oldPassword)
            {
                return false;
            }
            
            Password = newPassword;
            return true;
        }

    }
}