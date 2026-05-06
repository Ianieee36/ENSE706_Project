using System;

namespace FlightBookingSystem.Model
{
    public class User
    {
        // private fields (attributes)
        private string userId;
        private string email;
        private string passwordHash;
        private Role role;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string address;
        private string phoneNumber;

        // User constructor 
        public User(string userId, string email, string passwordHash, Role role, string firstName, string lastName, 
                   DateTime dateOfBirth, string address, string phoneNumber)
        {
            this.userId = userId;
            this.email = email;
            this.passwordHash = passwordHash;
            this.role = role;
            this.firstName = firstName;
            this.lastName = lastName;
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

        public string PasswordHash
        {
            get{return passwordHash;}    
            private set{passwordHash = value;}
        }

        public Role UserRole
        {
            get {return role;}
            private set{role = value;}    
        }

        public string FirstName
        {
            get{return firstName;}
            private set{firstName = value;}
        }

        public string LastName
        {
            get{return lastName;} 
            private set{lastName = value;}
        }  

        public string FullName
        {
            get {return FullName + " " + LastName;}
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

        public void UpdateProfile(string newfName, string newlName, string newAddress, string newPhoneNumber)
        {
            FirstName = newfName;
            LastName = newlName;
            Address = newAddress;
            PhoneNumber = newPhoneNumber;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if(PasswordHash != oldPassword)
            {
                return false;
            }
            
            PasswordHash = newPassword;
            return true;
        }

    }
}