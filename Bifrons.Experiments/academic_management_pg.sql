-- Students table
CREATE TABLE "Students" (
    "StudentID" CHAR(10) PRIMARY KEY,
    "FirstName" VARCHAR(50),
    "LastName" VARCHAR(50),
    "Email" VARCHAR(100),
    "PhoneNumber" VARCHAR(15),
    "Major" VARCHAR(50),
    "EnrollmentDate" DATE
);

-- Professors table
CREATE TABLE "Professors" (
    "ProfessorID" CHAR(10) PRIMARY KEY,
    "FirstName" VARCHAR(50),
    "LastName" VARCHAR(50),
    "Email" VARCHAR(100),
    "PhoneNumber" VARCHAR(15),
    "Department" VARCHAR(50)
);

-- Courses table
CREATE TABLE "Courses" (
    "CourseID" SERIAL PRIMARY KEY,
    "CourseName" VARCHAR(100),
    "Credits" INT,
    "Department" VARCHAR(50),
    "HeadProfessorID" CHAR(10),
    FOREIGN KEY ("HeadProfessorID") REFERENCES "Professors"("ProfessorID")
);

-- Enrollments table
CREATE TABLE "Enrollments" (
    "EnrollmentID" SERIAL PRIMARY KEY,
    "StudentID" CHAR(10) REFERENCES "Students"("StudentID"),
    "CourseID" INT REFERENCES "Courses"("CourseID"),
    "EnrollmentDate" DATE,
    "Grade" CHAR(2)
);

-- Teaching Assistants table
CREATE TABLE "TeachingAssistants" (
    "AssistantID" CHAR(10) PRIMARY KEY,
    "FirstName" VARCHAR(50),
    "LastName" VARCHAR(50),
    "Email" VARCHAR(100),
    "PhoneNumber" VARCHAR(15),
    "ProfessorID" CHAR(10) REFERENCES "Professors"("ProfessorID")
);

----------- INSERT STATEMENTS -----------
-- Insert into Students table
INSERT INTO "Students" ("StudentID", "FirstName", "LastName", "Email", "PhoneNumber", "Major", "EnrollmentDate") VALUES
('A1B2C3D4E5', 'John', 'Doe', 'john.doe@example.com', '123-456-7890', 'Computer Science', '2022-09-01'),
('F6G7H8I9J0', 'Jane', 'Smith', 'jane.smith@example.com', '987-654-3210', 'Mathematics', '2023-01-15'),
('K1L2M3N4O5', 'Alice', 'Brown', 'alice.brown@example.com', '123-456-7891', 'Physics', '2022-09-01'),
('P6Q7R8S9T0', 'Bob', 'Johnson', 'bob.johnson@example.com', '987-654-3211', 'Chemistry', '2023-01-15'),
('U1V2W3X4Y5', 'Charlie', 'Davis', 'charlie.davis@example.com', '123-456-7892', 'Biology', '2022-09-01');

-- Insert into Professors table
INSERT INTO "Professors" ("ProfessorID", "FirstName", "LastName", "Email", "PhoneNumber", "Department") VALUES
('Z6A7B8C9D0', 'Alice', 'Johnson', 'alice.johnson@example.com', '555-123-4567', 'Computer Science'),
('E1F2G3H4I5', 'Bob', 'Williams', 'bob.williams@example.com', '555-987-6543', 'Mathematics'),
('J6K7L8M9N0', 'Carol', 'Taylor', 'carol.taylor@example.com', '555-123-4568', 'Physics'),
('O1P2Q3R4S5', 'David', 'Brown', 'david.brown@example.com', '555-987-6544', 'Chemistry'),
('T6U7V8W9X0', 'Eve', 'Miller', 'eve.miller@example.com', '555-123-4569', 'Biology');

-- Insert into Courses table
INSERT INTO "Courses" ("CourseID", "CourseName", "Credits", "Department", "HeadProfessorID") VALUES
(1, 'Introduction to Programming', 3, 'Computer Science', 'Z6A7B8C9D0'),
(2, 'Calculus I', 4, 'Mathematics', 'E1F2G3H4I5'),
(3, 'Physics I', 4, 'Physics', 'J6K7L8M9N0'),
(4, 'General Chemistry', 4, 'Chemistry', 'O1P2Q3R4S5'),
(5, 'Biology 101', 3, 'Biology', 'T6U7V8W9X0');

-- Insert into Enrollments table
INSERT INTO "Enrollments" ("StudentID", "CourseID", "EnrollmentDate", "Grade") VALUES
('A1B2C3D4E5', 2, '2023-02-01', 'B+'),
('A1B2C3D4E5', 3, '2023-02-01', 'A-'),
('F6G7H8I9J0', 1, '2023-02-01', 'A'),
('F6G7H8I9J0', 3, '2023-02-01', 'B'),
('K1L2M3N4O5', 1, '2023-02-01', 'B+'),
('K1L2M3N4O5', 2, '2023-02-01', 'A-'),
('P6Q7R8S9T0', 1, '2023-02-01', 'A'),
('P6Q7R8S9T0', 2, '2023-02-01', 'B+'),
('U1V2W3X4Y5', 1, '2023-02-01', 'A-'),
('U1V2W3X4Y5', 2, '2023-02-01', 'A');

-- Insert into Teaching Assistants table
INSERT INTO "TeachingAssistants" ("AssistantID", "FirstName", "LastName", "Email", "PhoneNumber", "ProfessorID") VALUES
('Z1A2B3C4D5', 'Emily', 'Davis', 'emily.davis@example.com', '555-234-5678', 'Z6A7B8C9D0'),
('E6F7G8H9I0', 'Michael', 'Brown', 'michael.brown@example.com', '555-876-5432', 'E1F2G3H4I5'),
('J1K2L3M4N5', 'Sophia', 'Wilson', 'sophia.wilson@example.com', '555-234-5679', 'J6K7L8M9N0'),
('O6P7Q8R9S0', 'James', 'Moore', 'james.moore@example.com', '555-876-5433', 'O1P2Q3R4S5'),
('T1U2V3W4X5', 'Olivia', 'Taylor', 'olivia.taylor@example.com', '555-234-5680', 'T6U7V8W9X0');