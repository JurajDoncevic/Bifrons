-- Person definition

CREATE TABLE `Person` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `FirstName` TEXT NOT NULL,
    `LastName` TEXT NOT NULL,
    `DateOfBirth` DATE
);


-- "Role" definition

CREATE TABLE `Role` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Name` VARCHAR(255) NOT NULL,
    UNIQUE (`Name`)
);

-- PersonRole definition

CREATE TABLE `PersonRole` (
    `PersonId` INT NOT NULL,
    `RoleId` INT NOT NULL,
    `GivenOn` TIMESTAMP NOT NULL,
    `ExpiresOn` TIMESTAMP,
    PRIMARY KEY (`PersonId`,`RoleId`),
    FOREIGN KEY (`PersonId`) REFERENCES `Person`(`Id`),
    FOREIGN KEY (`RoleId`) REFERENCES `Role`(`Id`)
);

INSERT INTO `Person` (`FirstName`,`LastName`,`DateOfBirth`) VALUES
     ('John','Smith','1964-10-19'),
     ('Adam','Doe','1982-06-23'),
     ('Andy','Black','1973-06-25'),
     ('Emma','Brown','1985-05-22'),
     ('Oliver','Johnson','1990-07-14'),
     ('Sophia','Williams','1978-11-30'),
     ('Liam','Jones','1982-03-05'),
     ('Ava','Taylor','1995-01-15'),
     ('string','string','2024-03-10 16:54:34');


INSERT INTO `Role` (`Name`) VALUES
     ('Programmer'),
     ('Tester'),
     ('Project manager'),
     ('SCRUM master');


INSERT INTO `PersonRole` (`PersonId`,`RoleId`,`GivenOn`,`ExpiresOn`) VALUES
     (2,2,'2022-11-01 00:00:00',NULL),
     (3,3,'2022-12-01 00:00:00','2023-12-01 00:00:00'),
     (4,4,'2023-01-01 00:00:00',NULL),
     (5,1,'2023-02-01 00:00:00','2024-02-01 00:00:00'),
     (6,2,'2023-03-01 00:00:00','2024-03-01 00:00:00'),
     (7,3,'2023-04-01 00:00:00',NULL),
     (8,4,'2023-05-01 00:00:00','2024-05-01 00:00:00'),
     (9,1,'2023-06-01 00:00:00',NULL),
     (3,2,'2023-07-01 00:00:00','2024-07-01 00:00:00'),
     (1,4,'2024-03-10 17:55:32',NULL),
     (1,2,'2024-03-10 17:58:23',NULL);