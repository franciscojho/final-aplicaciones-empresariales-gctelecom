USE MASTER

-- CLOSE ALL CONNECTIONS TO DATABASE
GO
	IF EXISTS(SELECT * FROM sys.databases WHERE name='librarydb')  
BEGIN
	ALTER DATABASE librarydb SET SINGLE_USER
WITH ROLLBACK IMMEDIATE
END
GO

-- SEARCH IF THE DATABASE EXISTS, ELSE WE DELETE IT
IF EXISTS(SELECT * FROM sys.databases WHERE name='librarydb')  
BEGIN 
	USE MASTER
    DROP DATABASE librarydb
END 
GO

-- CREATE THE DATABASE
CREATE DATABASE librarydb
GO

-- WE SELECT THE DATABASE
USE librarydb
GO

-- BEGIN CREATE USER TABLE
CREATE TABLE library_user(
	user_id INTEGER PRIMARY KEY IDENTITY(1,1),
	name varchar(30) NOT NULL,
	last_name varchar(30) NOT NULL,
	email varchar(30) NOT NULL,
	phone varchar(9) NOT NULL,
	address varchar(100) NOT NULL,
	user_type VARCHAR(20) NOT NULL CHECK (user_type IN('estudiante', 'profesor', 'visitante')),
	status bit NOT NULL,
)
GO

INSERT INTO library_user VALUES ('Francisco', 'Huapaya', 'franciscojho@hotmail.com', '940361504', 'Av. Chavito 123, El Olivar, Lima', 'estudiante', 1)
GO

SELECT * FROM library_user
-- END CREATE USER TABLE

-- BEGIN CREATE BORROW TABLE
CREATE TABLE borrow(
	borrow_id INTEGER PRIMARY KEY IDENTITY(1,1),
	user_id INT NOT NULL,
	FOREIGN KEY (user_id) REFERENCES library_user(user_id),
	borrow_date DATETIME NOT NULL,
	return_date DATETIME NOT NULL,
	status BIT NOT NULL
)

INSERT INTO borrow VALUES (1, '2025-07-03 14:30:00', '2025-07-14 14:30:00', 1)
GO

SELECT * FROM borrow
-- END CREATE BORROW TABLE