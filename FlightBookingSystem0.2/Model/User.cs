using System;

namespace FlightBookingSystem.Model
{
    public abstract class User
    {
        // private fields (attributes)
        private string userId;
        private string email;
        private string passwordHash;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string address;
        private string phoneNumber;
        

        // User constructor 
        public User(string userId, string email, string passwordHash, string firstName, string lastName, 
                   DateTime dateOfBirth, string address, string phoneNumber)
        {
            this.userId = userId;
            this.email = email;
            this.passwordHash = passwordHash;
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
            get {return firstName + " " + LastName;}
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

        public void UpdateProfile(string? newEmail = null, 
                                  string? newFirstName = null, 
                                  string? newLastName = null, 
                                  string? newAddress = null, 
                                  string? newPhoneNumber = null)
        {
            if (!string.IsNullOrWhiteSpace(newEmail))
                Email = newEmail;
            
            if (!string.IsNullOrWhiteSpace(newFirstName))
                FirstName = newFirstName;
            
            if (!string.IsNullOrWhiteSpace(newLastName))
                LastName = newLastName;

            if (!string.IsNullOrWhiteSpace(newAddress))
                Address = newAddress;

            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
                PhoneNumber = newPhoneNumber;
            
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            PasswordHash = newPasswordHash;
        }

    }
}