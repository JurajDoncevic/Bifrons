-- Students table (redundant)
CREATE TABLE Students (
    StudentID CHAR(10) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Major VARCHAR(50)
);

-- Fee Payments table
CREATE TABLE FeePayments (
    PaymentID SERIAL PRIMARY KEY,
    StudentID CHAR(10) REFERENCES Students(StudentID),
    PaymentDate DATE,
    Amount DECIMAL(10, 2),
    PaymentMethod VARCHAR(50)
);

-- Professors table
CREATE TABLE Professors (
    ProfessorID CHAR(10) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    HomeAddress VARCHAR(255),
    BankAccountNumber VARCHAR(20)
);

-- Teaching Assistants table
CREATE TABLE TeachingAssistants (
    AssistantID CHAR(10) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    HomeAddress VARCHAR(255),
    BankAccountNumber VARCHAR(20)
);

-- Salary Payments table
CREATE TABLE SalaryPayments (
    PaymentID SERIAL PRIMARY KEY,
    Recepient CHAR(10),
    RecepientType VARCHAR(20),
    Amount DECIMAL(10, 2),
    PaymentDate DATE,
    CHECK (RecepientType IN ('Professor', 'TeachingAssistant'))
);

----------- INSERT STATEMENTS -----------
-- Insert into Professors table
INSERT INTO Professors (ProfessorID, FirstName, LastName, Email, HomeAddress, BankAccountNumber) VALUES
('Z6A7B8C9D0', 'Alice', 'Johnson', 'alice.johnson@example.com', '123 Main St, City, State, ZIP', '1234567890'),
('E1F2G3H4I5', 'Bob', 'Williams', 'bob.williams@example.com', '456 Oak St, City, State, ZIP', '0987654321'),
('J6K7L8M9N0', 'Carol', 'Taylor', 'carol.taylor@example.com', '789 Pine St, City, State, ZIP', '1122334455'),
('O1P2Q3R4S5', 'David', 'Brown', 'david.brown@example.com', '321 Elm St, City, State, ZIP', '5566778899'),
('T6U7V8W9X0', 'Eve', 'Miller', 'eve.miller@example.com', '654 Willow St, City, State, ZIP', '9988776655');

-- Insert into Teaching Assistants table
INSERT INTO TeachingAssistants (AssistantID, FirstName, LastName, Email, HomeAddress, BankAccountNumber) VALUES
('Z1A2B3C4D5', 'Emily', 'Davis', 'emily.davis@example.com', '987 Maple St, City, State, ZIP', '1234567891'),
('E6F7G8H9I0', 'Michael', 'Brown', 'michael.brown@example.com', '654 Cedar St, City, State, ZIP', '0987654322'),
('J1K2L3M4N5', 'Sophia', 'Wilson', 'sophia.wilson@example.com', '321 Birch St, City, State, ZIP', '1122334466'),
('O6P7Q8R9S0', 'James', 'Moore', 'james.moore@example.com', '789 Spruce St, City, State, ZIP', '5566778800'),
('T1U2V3W4X5', 'Olivia', 'Taylor', 'olivia.taylor@example.com', '123 Pine St, City, State, ZIP', '9988776644');

-- Insert into Students table
INSERT INTO Students (StudentID, FirstName, LastName, Email, PhoneNumber, Major) VALUES
('A1B2C3D4E5', 'John', 'Doe', 'john.doe@example.com', '123-456-7890', 'Computer Science'),
('F6G7H8I9J0', 'Jane', 'Smith', 'jane.smith@example.com', '987-654-3210', 'Mathematics'),
('K1L2M3N4O5', 'Alice', 'Brown', 'alice.brown@example.com', '123-456-7891', 'Physics'),
('P6Q7R8S9T0', 'Bob', 'Johnson', 'bob.johnson@example.com', '987-654-3211', 'Chemistry'),
('U1V2W3X4Y5', 'Charlie', 'Davis', 'charlie.davis@example.com', '123-456-7892', 'Biology');

-- Insert into FeePayments table
INSERT INTO FeePayments (StudentID, PaymentDate, Amount, PaymentMethod) VALUES
('A1B2C3D4E5', '2023-01-10', 5000.00, 'Credit Card'),
('F6G7H8I9J0', '2023-01-12', 4800.00, 'Bank Transfer'),
('K1L2M3N4O5', '2023-01-15', 5200.00, 'Credit Card'),
('P6Q7R8S9T0', '2023-01-17', 4900.00, 'Cash'),
('U1V2W3X4Y5', '2023-01-20', 5100.00, 'Credit Card');

-- Insert into Salary Payments table
INSERT INTO SalaryPayments (Recepient, RecepientType, Amount, PaymentDate) VALUES
('Z6A7B8C9D0', 'Professor', 5000.00, '2023-02-01'),
('E1F2G3H4I5', 'Professor', 5000.00, '2023-02-01'),
('J6K7L8M9N0', 'Professor', 5000.00, '2023-02-01'),
('O1P2Q3R4S5', 'Professor', 5000.00, '2023-02-01'),
('T6U7V8W9X0', 'Professor', 5000.00, '2023-02-01'),
('Z1A2B3C4D5', 'TeachingAssistant', 2000.00, '2023-02-01'),
('E6F7G8H9I0', 'TeachingAssistant', 2000.00, '2023-02-01'),
('J1K2L3M4N5', 'TeachingAssistant', 2000.00, '2023-02-01'),
('O6P7Q8R9S0', 'TeachingAssistant', 2000.00, '2023-02-01'),
('T1U2V3W4X5', 'TeachingAssistant', 2000.00, '2023-02-01'),
('Z6A7B8C9D0', 'Professor', 5000.00, '2023-03-01'),
('E1F2G3H4I5', 'Professor', 5000.00, '2023-03-01'),
('J6K7L8M9N0', 'Professor', 5000.00, '2023-03-01'),
('Z1A2B3C4D5', 'TeachingAssistant', 2000.00, '2023-03-01'),
('E6F7G8H9I0', 'TeachingAssistant', 2000.00, '2023-03-01');
