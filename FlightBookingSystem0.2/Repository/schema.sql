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

CREATE TABLE TICKETS (
    TicketId VARCHAR2(50) PRIMARY KEY,
    TicketNumber VARCHAR2(50) NOT NULL UNIQUE,
    IssueDate TIMESTAMP NOT NULL,
    SeatNumber VARCHAR2(20) NOT NULL,
    GateNumber VARCHAR2(20) NOT NULL,
    BoardingTime TIMESTAMP NOT NULL,
    TicketStatus VARCHAR2(50) NOT NULL,
    BookingId VARCHAR2(50) NOT NULL UNIQUE,

    CONSTRAINT chk_ticket_status
        CHECK (TicketStatus IN ('ISSUED', 'USED', 'CANCELLED', 'EXPIRED')),

    CONSTRAINT fk_tickets_bookings
        FOREIGN KEY (BookingId) REFERENCES BOOKINGS(BookingId)
);

ALTER TABLE USERS
DROP CONSTRAINT CHK_USER_ROLE;

ALTER TABLE USERS
DROP COLUMN Role;

CREATE TABLE CUSTOMERS (
    UserId VARCHAR2(50) PRIMARY KEY,
    LoyaltyPoints NUMBER DEFAULT 0 NOT NULL,
    MembershipTier VARCHAR2(50) DEFAULT 'BRONZE' NOT NULL,

    CONSTRAINT chk_membership_tier
        CHECK (
            MembershipTier IN (
                    'BRONZE',
                    'SILVER',
                    'GOLD'
            )
        ),

    CONSTRAINT fk_customers_users
        FOREIGN KEY (UserId)
        REFERENCES USERS(UserId)
);

CREATE TABLE ADMINS (
    UserId VARCHAR2(50) PRIMARY KEY,
    AdminLevel VARCHAR2(50) NOT NULL,

    CONSTRAINT chk_admin_level
        CHECK (
            AdminLevel IN (
                'SUPPORT_ADMIN',
                'FLIGHT_MANAGER',
                'SYSTEM_ADMIN'
            )
        ),

    CONSTRAINT fk_admins_users
        FOREIGN KEY (UserId)
        REFERENCES USERS(UserId)
);

