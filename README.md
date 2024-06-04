This project is a web application made with MVC(Model-View-Controller),designed to manage university courses and user interactions, supporting four distinct user roles: Student, Professor, Secretary, and Moderator. The application utilizes a local database with Entity Framework Core and SQL Server for data management and cookie-based authentication for user access control.

## Thechnologies Used:
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Cookie Authentication**
- **Dependency Injection**
- **Razor Pages**
- **Bootstrap**
- **JavaScript**
- **CSS**
- **Bootstrap**

## Functionalities for every role:
### Student:
- View list of enrolled courses, grades, and yearly averages
- Sort courses alphabetically or by grade
- Request student certificates in PDF format if enrolled in a current study program.
- Receive alerts for new course enrollments and grade postings

### Teacher:
- Add grades for students enrolled in their courses.
- Receive messages from the Secretary

### Secretary:
- Access all course and group catalogs.
- Export catalogs as PDF files

### Moderator
-Create, modify, or delete courses.
-Add students and professors to courses.
-Assign roles to users.(Only moderators can assign a role after sign up, you can not sign up as a teacher or as a student, you need to have your role assigned by the moderator)


