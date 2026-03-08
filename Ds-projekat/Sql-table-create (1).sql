CREATE TABLE MembershipTypes(
	MembershipTypeID INT IDENTITY(1, 1) PRIMARY KEY,
	PackageName NVARCHAR(100) NOT NULL,
	Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
	DurationDays INT NOT NULL CHECK (DurationDays > 0),
	MaxReservationHoursPerMonth INT NOT NULL CHECK (MaxReservationHoursPerMonth >= 0),
	MeetingRoomAccess BIT NOT NULL,
	MeetingRoomHoursPerMonth INT NULL,

	CONSTRAINT CK_MembershipTypes_MeetingRoomHours_NonNegative
		CHECK (MeetingRoomHoursPerMonth IS NULL OR MeetingRoomHoursPerMonth >= 0),

	CONSTRAINT CK_MembershipTypes_MeetingRoomLogic
		CHECK (
			(MeetingRoomAccess = 0 AND MeetingRoomHoursPerMonth IS NULL)
		 OR (MeetingRoomAccess = 1 AND MeetingRoomHoursPerMonth IS NOT NULL AND MeetingRoomHoursPerMonth > 0)
		)
)

CREATE TABLE Users (
	UserID INT IDENTITY(1, 1) PRIMARY KEY,
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	Email NVARCHAR(120) NOT NULL UNIQUE,
	Phone NVARCHAR(20) NOT NULL,
	MembershipTypeID INT NOT NULL
				FOREIGN KEY REFERENCES MembershipTypes(MembershipTypeID),
	MembershipStartDate DATE NOT NULL,
	MembershipEndDate DATE NOT NULL,
	AccountStatus NVARCHAR(50) NOT NULL
		CHECK (AccountStatus IN ('Active', 'Paused', 'Expired'))
)

CREATE TABLE Locations (
	LocationID INT IDENTITY(1, 1) PRIMARY KEY,
	LocationName NVARCHAR(50) NOT NULL,
	AddressName NVARCHAR(100) NOT NULL,
	City NVARCHAR(50) NOT NULL,
	WorkingHours NVARCHAR(100) NOT NULL,
	MaxUsers INT NOT NULL CHECK (MaxUsers > 0)
)

CREATE TABLE Resources (
	ResourceID INT IDENTITY (1, 1) PRIMARY KEY,
	LocationID INT NOT NULL 
				FOREIGN KEY REFERENCES Locations(LocationID),
	ResourceName NVARCHAR(50) NOT NULL,
	ResourceType NVARCHAR(50) NOT NULL,
	IsActive BIT NOT NULL DEFAULT 0,
	Description NVARCHAR(300) NULL
)

CREATE TABLE DeskDetails (
	ResourceID INT PRIMARY KEY 
			FOREIGN KEY	REFERENCES Resources (ResourceID),
	DeskSubType NVARCHAR(20) NOT NULL
		CHECK (DeskSubType IN ('Hot', 'Dedicated'))
)

CREATE TABLE RoomDetails (
	ResourceID INT PRIMARY KEY 
			FOREIGN KEY	REFERENCES Resources (ResourceID),
	Capacity INT NOT NULL CHECK (Capacity > 0),
	HasProjector BIT NOT NULL,
	HasTV BIT NOT NULL,
	HasBoard BIT NOT NULL,
	HasOnlineEquipment BIT NOT NULL
)

CREATE TABLE Reservations (
	ReservationID INT IDENTITY (1, 1) PRIMARY KEY,
	UserID INT NOT NULL
			FOREIGN KEY REFERENCES Users (UserID),
	ResourceID INT NOT NULL
			FOREIGN KEY	REFERENCES Resources (ResourceID),
	UsersCount INT NULL CHECK (UsersCount IS NULL OR UsersCount > 0),
	StartDateTime DATETIME2 NOT NULL,
	EndDateTime DATETIME2 NOT NULL,
	ReservationStatus NVARCHAR(50) NOT NULL
			CHECK (ReservationStatus IN ('Active', 'Finished', 'Canceled')),

	CONSTRAINT CK_Reservations_EndAfterStart CHECK (EndDateTime > StartDateTime)
)

CREATE TABLE Admins (
	UserID INT PRIMARY KEY
			FOREIGN KEY REFERENCES Users (UserID),
	RoleName NVARCHAR(50) NOT NULL,
	Username NVARCHAR(50) NOT NULL UNIQUE,
	HashedPass NVARCHAR(200) NOT NULL
)




-- BRISANJE TABELA:

DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) 
              + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) 
              + ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.foreign_keys;

EXEC sp_executesql @sql;

-------

DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 'DROP TABLE ' 
              + QUOTENAME(SCHEMA_NAME(schema_id)) 
              + '.' + QUOTENAME(name) + ';'
FROM sys.tables;

EXEC sp_executesql @sql;
