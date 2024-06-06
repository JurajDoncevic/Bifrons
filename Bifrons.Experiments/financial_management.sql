-- Students table (redundant)
CREATE TABLE Students (
    StudentID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Major VARCHAR(50)
);

-- Fee Payments table
CREATE TABLE FeePayments (
    PaymentID SERIAL PRIMARY KEY,
    StudentID INT REFERENCES Students(StudentID),
    PaymentDate DATE,
    Amount DECIMAL(10, 2),
    PaymentMethod VARCHAR(50)
);

-- Scholarships table
CREATE TABLE Scholarships (
    ScholarshipID SERIAL PRIMARY KEY,
    StudentID INT REFERENCES Students(StudentID),
    ScholarshipAmount DECIMAL(10, 2),
    AwardDate DATE
);

-- Financial Aid table
CREATE TABLE FinancialAid (
    AidID SERIAL PRIMARY KEY,
    StudentID INT REFERENCES Students(StudentID),
    AidAmount DECIMAL(10, 2),
    AidType VARCHAR(50),
    DisbursementDate DATE
);

-- Departments table (redundant)
CREATE TABLE Departments (
    DepartmentID SERIAL PRIMARY KEY,
    DepartmentName VARCHAR(100),
    HeadOfDepartment VARCHAR(100)
);


----------- INSERT STATEMENTS -----------
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

-- Insert into Scholarships table
INSERT INTO Scholarships (StudentID, ScholarshipAmount, AwardDate) VALUES
(1, 2000.00, '2023-01-05'),
(2, 1500.00, '2023-01-07'),
(3, 2500.00, '2023-01-10'),
(4, 1000.00, '2023-01-12'),
(5, 1800.00, '2023-01-15');

-- Insert into FinancialAid table
INSERT INTO FinancialAid (StudentID, AidAmount, AidType, DisbursementDate) VALUES
(1, 3000.00, 'Grant', '2023-01-15'),
(2, 2500.00, 'Loan', '2023-01-20'),
(3, 3500.00, 'Grant', '2023-01-25'),
(4, 2000.00, 'Loan', '2023-01-30'),
(5, 2800.00, 'Grant', '2023-02-05');

-- Insert into Departments table (redundant)
INSERT INTO Departments (DepartmentName, HeadOfDepartment) VALUES
('Computer Science', 'Alice Johnson'),
('Mathematics', 'Bob Williams'),
('Physics', 'Carol Taylor'),
('Chemistry', 'David Brown'),
('Biology', 'Eve Miller');
