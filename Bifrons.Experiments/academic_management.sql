-- Students table
CREATE TABLE Students (
    StudentID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Major VARCHAR(50),
    EnrollmentDate DATE
);

-- Professors table
CREATE TABLE Professors (
    ProfessorID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Department VARCHAR(50)
);

-- Courses table
CREATE TABLE Courses (
    CourseID SERIAL PRIMARY KEY,
    CourseName VARCHAR(100),
    Credits INT,
    Department VARCHAR(50)
);

-- Enrollments table
CREATE TABLE Enrollments (
    EnrollmentID SERIAL PRIMARY KEY,
    StudentID INT REFERENCES Students(StudentID),
    CourseID INT REFERENCES Courses(CourseID),
    EnrollmentDate DATE,
    Grade CHAR(2)
);

-- Teaching Assistants table
CREATE TABLE TeachingAssistants (
    AssistantID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    ProfessorID INT REFERENCES Professors(ProfessorID)
);

----------- INSERT STATEMENTS -----------
-- Insert into Students table
INSERT INTO Students (FirstName, LastName, Email, PhoneNumber, Major, EnrollmentDate) VALUES
('John', 'Doe', 'john.doe@example.com', '123-456-7890', 'Computer Science', '2022-09-01'),
('Jane', 'Smith', 'jane.smith@example.com', '987-654-3210', 'Mathematics', '2023-01-15'),
('Alice', 'Brown', 'alice.brown@example.com', '123-456-7891', 'Physics', '2022-09-01'),
('Bob', 'Johnson', 'bob.johnson@example.com', '987-654-3211', 'Chemistry', '2023-01-15'),
('Charlie', 'Davis', 'charlie.davis@example.com', '123-456-7892', 'Biology', '2022-09-01');

-- Insert into Professors table
INSERT INTO Professors (FirstName, LastName, Email, PhoneNumber, Department) VALUES
('Alice', 'Johnson', 'alice.johnson@example.com', '555-123-4567', 'Computer Science'),
('Bob', 'Williams', 'bob.williams@example.com', '555-987-6543', 'Mathematics'),
('Carol', 'Taylor', 'carol.taylor@example.com', '555-123-4568', 'Physics'),
('David', 'Brown', 'david.brown@example.com', '555-987-6544', 'Chemistry'),
('Eve', 'Miller', 'eve.miller@example.com', '555-123-4569', 'Biology');

-- Insert into Courses table
INSERT INTO Courses (CourseName, Credits, Department) VALUES
('Introduction to Programming', 3, 'Computer Science'),
('Calculus I', 4, 'Mathematics'),
('Physics I', 4, 'Physics'),
('General Chemistry', 4, 'Chemistry'),
('Biology 101', 3, 'Biology');

-- Insert into Enrollments table
INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate, Grade) VALUES
(1, 1, '2023-02-01', 'A'),
(2, 2, '2023-02-01', 'B'),
(3, 3, '2023-02-01', 'A-'),
(4, 4, '2023-02-01', 'B+'),
(5, 5, '2023-02-01', 'A');

-- Insert into Teaching Assistants table
INSERT INTO TeachingAssistants (FirstName, LastName, Email, PhoneNumber, ProfessorID) VALUES
('Emily', 'Davis', 'emily.davis@example.com', '555-234-5678', 1),
('Michael', 'Brown', 'michael.brown@example.com', '555-876-5432', 2),
('Sophia', 'Wilson', 'sophia.wilson@example.com', '555-234-5679', 3),
('James', 'Moore', 'james.moore@example.com', '555-876-5433', 4),
('Olivia', 'Taylor', 'olivia.taylor@example.com', '555-234-5680', 5);
