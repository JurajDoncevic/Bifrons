-- Students table
CREATE TABLE Students (
    StudentID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Major VARCHAR(50),
    EnrollmentDate DATE
);

-- Professors table
CREATE TABLE Professors (
    ProfessorID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Department VARCHAR(50)
);

-- Courses table
CREATE TABLE Courses (
    CourseID INT AUTO_INCREMENT PRIMARY KEY,
    CourseName VARCHAR(100),
    Credits INT,
    Department VARCHAR(50),
    HeadProfessorID INT,
    FOREIGN KEY (HeadProfessorID) REFERENCES Professors(ProfessorID)
);

-- Enrollments table
CREATE TABLE Enrollments (
    EnrollmentID INT AUTO_INCREMENT PRIMARY KEY,
    StudentID INT,
    CourseID INT,
    EnrollmentDate DATE,
    Grade CHAR(2),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

-- Teaching Assistants table
CREATE TABLE TeachingAssistants (
    AssistantID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    ProfessorID INT,
    FOREIGN KEY (ProfessorID) REFERENCES Professors(ProfessorID)
);

/*----------- INSERT STATEMENTS -----------*/
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
INSERT INTO Courses (CourseName, Credits, Department, HeadProfessorID) VALUES
('Introduction to Programming', 3, 'Computer Science', 1),
('Calculus I', 4, 'Mathematics', 2),
('Physics I', 4, 'Physics', 3),
('General Chemistry', 4, 'Chemistry', 4),
('Biology 101', 3, 'Biology', 5);

-- Insert into Enrollments table
INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate, Grade) VALUES
(1, 2, '2023-02-01', 'B+'),
(1, 3, '2023-02-01', 'A-'),
(2, 1, '2023-02-01', 'A'),
(2, 3, '2023-02-01', 'B'),
(3, 1, '2023-02-01', 'B+'),
(3, 2, '2023-02-01', 'A-'),
(4, 1, '2023-02-01', 'A'),
(4, 2, '2023-02-01', 'B+'),
(5, 1, '2023-02-01', 'A-'),
(5, 2, '2023-02-01', 'A');
-- Insert into Teaching Assistants table
INSERT INTO TeachingAssistants (FirstName, LastName, Email, PhoneNumber, ProfessorID) VALUES
('Emily', 'Davis', 'emily.davis@example.com', '555-234-5678', 1),
('Michael', 'Brown', 'michael.brown@example.com', '555-876-5432', 2),
('Sophia', 'Wilson', 'sophia.wilson@example.com', '555-234-5679', 3),
('James', 'Moore', 'james.moore@example.com', '555-876-5433', 4),
('Olivia', 'Taylor', 'olivia.taylor@example.com', '555-234-5680', 5);