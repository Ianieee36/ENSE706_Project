CREATE TABLE USERS (
    UserId VARCHAR2(50) PRIMARY KEY,
    Email VARCHAR2(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR2(255) NOT NULL,
    Role VARCHAR2(50) NOT NULL,
    FirstName VARCHAR2(100) NOT NULL,
    LastName VARCHAR2(100) NOT NULL,
    DateOfBirth DATE,
    Address VARCHAR2(150),
    PhoneNumber VARCHAR2(20),

    CONSTRAINT chk_user_role
        CHECK (Role IN ('GUEST', 'CUSTOMER', 'ADMIN'))
);

CREATE TABLE FLIGHTS (
    FlightId VARCHAR2(50) PRIMARY KEY,
    Origin VARCHAR2(100) NOT NULL,
    Destination VARCHAR2(100) NOT NULL,
    Departure TIMESTAMP NOT NULL,
    AvailableSeats NUMBER NOT NULL,
    TotalSeats NUMBER NOT NULL,

    CONSTRAINT chk_flight_seats
        CHECK (AvailableSeats >= 0 AND AvailableSeats <= TotalSeats)

);

CREATE TABLE BOOKINGS (
    BookingId VARCHAR2(50) PRIMARY KEY,
    BookingDate TIMESTAMP NOT NULL, 
    Status VARCHAR2(50) NOT NULL,
    UserId VARCHAR2(50) NOT NULL,
    FlightId VARCHAR2(50) NOT NULL,
    
    CONSTRAINT chk_booking_status
        CHECK (Status IN ('PENDING', 'CONFIRMED', 'CANCELLED')),
     
    CONSTRAINT fk_bookings_users
        FOREIGN KEY (UserId) REFERENCES USERS(UserId),

    CONSTRAINT fk_bookings_flights
        FOREIGN KEY (FlightId) REFERENCES FLIGHTS(FlightId)
);