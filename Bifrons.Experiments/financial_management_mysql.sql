-- Students table
CREATE TABLE Students (
    StudentID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Major VARCHAR(50)
);

-- Fee Payments table
CREATE TABLE FeePayments (
    PaymentID INT AUTO_INCREMENT PRIMARY KEY,
    StudentID INT,
    PaymentDate DATE,
    Amount DECIMAL(10, 2),
    PaymentMethod VARCHAR(50),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
);

-- Professors table
CREATE TABLE Professors (
    ProfessorID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    HomeAddress VARCHAR(255),
    BankAccountNumber VARCHAR(20)
);

-- Teaching Assistants table
CREATE TABLE TeachingAssistants (
    AssistantID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    HomeAddress VARCHAR(255),
    BankAccountNumber VARCHAR(20)
);

-- Salary Payments table
CREATE TABLE SalaryPayments (
    PaymentID INT AUTO_INCREMENT PRIMARY KEY,
    Recepient INT,
    RecepientType VARCHAR(20),
    Amount DECIMAL(10, 2),
    PaymentDate DATE,
    CHECK (RecepientType IN ('Professor', 'TeachingAssistant'))
);

/*----------- INSERT STATEMENTS -----------*/
-- Insert into Professors table (redundant)
INSERT INTO Professors (FirstName, LastName, Email, HomeAddress, BankAccountNumber) VALUES
('Alice', 'Johnson', 'alice.johnson@example.com', '123 Main St, City, State, ZIP', '1234567890'),
('Bob', 'Williams', 'bob.williams@example.com', '456 Oak St, City, State, ZIP', '0987654321'),
('Carol', 'Taylor', 'carol.taylor@example.com', '789 Pine St, City, State, ZIP', '1122334455'),
('David', 'Brown', 'david.brown@example.com', '321 Elm St, City, State, ZIP', '5566778899'),
('Eve', 'Miller', 'eve.miller@example.com', '654 Willow St, City, State, ZIP', '9988776655');

-- Insert into Teaching Assistants table (redundant)
INSERT INTO TeachingAssistants (FirstName, LastName, Email, HomeAddress, BankAccountNumber) VALUES
('Emily', 'Davis', 'emily.davis@example.com', '987 Maple St, City, State, ZIP', '1234567891'),
('Michael', 'Brown', 'michael.brown@example.com', '654 Cedar St, City, State, ZIP', '0987654322'),
('Sophia', 'Wilson', 'sophia.wilson@example.com', '321 Birch St, City, State, ZIP', '1122334466'),
('James', 'Moore', 'james.moore@example.com', '789 Spruce St, City, State, ZIP', '5566778800'),
('Olivia', 'Taylor', 'olivia.taylor@example.com', '123 Pine St, City, State, ZIP', '9988776644');

-- Insert into Students table (redundant)
INSERT INTO Students (FirstName, LastName, Email, PhoneNumber, Major) VALUES
('John', 'Doe', 'john.doe@example.com', '123-456-7890', 'Computer Science'),
('Jane', 'Smith', 'jane.smith@example.com', '987-654-3210', 'Mathematics'),
('Alice', 'Brown', 'alice.brown@example.com', '123-456-7891', 'Physics'),
('Bob', 'Johnson', 'bob.johnson@example.com', '987-654-3211', 'Chemistry'),
('Charlie', 'Davis', 'charlie.davis@example.com', '123-456-7892', 'Biology');

-- Insert into FeePayments table
INSERT INTO FeePayments (StudentID, PaymentDate, Amount, PaymentMethod) VALUES
(1, '2023-01-10', 5000.00, 'Credit Card'),
(2, '2023-01-12', 4800.00, 'Bank Transfer'),
(3, '2023-01-15', 5200.00, 'Credit Card'),
(4, '2023-01-17', 4900.00, 'Cash'),
(5, '2023-01-20', 5100.00, 'Credit Card');

-- Insert into Salary Payments table
INSERT INTO SalaryPayments (Recepient, RecepientType, Amount, PaymentDate) VALUES
(1, 'Professor', 5000.00, '2023-02-01'),
(2, 'Professor', 5000.00, '2023-02-01'),
(3, 'Professor', 5000.00, '2023-02-01'),
(4, 'Professor', 5000.00, '2023-02-01'),
(5, 'Professor', 5000.00, '2023-02-01'),
(1, 'TeachingAssistant', 2000.00, '2023-02-01'),
(2, 'TeachingAssistant', 2000.00, '2023-02-01'),
(3, 'TeachingAssistant', 2000.00, '2023-02-01'),
(4, 'TeachingAssistant', 2000.00, '2023-02-01'),
(5, 'TeachingAssistant', 2000.00, '2023-02-01'),
(1, 'Professor', 5000.00, '2023-03-01'),
(2, 'Professor', 5000.00, '2023-03-01'),
(3, 'Professor', 5000.00, '2023-03-01'),
(1, 'TeachingAssistant', 2000.00, '2023-03-01'),
(2, 'TeachingAssistant', 2000.00, '2023-03-01');
