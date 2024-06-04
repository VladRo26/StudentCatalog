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

### Moderator:
- Create, modify, or delete courses.
- Add students and professors to courses.
- Modify user roles

### Secretary: 
- Access all course and group catalogs.
- Export catalogs as PDF files.
- Can send messages to teachers.
### Other Functionalities
- Search courses by name or professor.
- Sign up
-Attractive and responsive user interface.

### Tables used:
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/4844e37e-1595-4734-8f06-7ff103204a09" width="900" height="600">

## How it works:

## No account/Not logged in/No role

-When you entry on the website and you are not logeed in or you don t have an account you will see this page:

<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/5ca68a6e-fb27-48ce-8669-30ced78e5afd" width="900" height="600">

- If you will go to register you will have this page, with validation for fields like email and password:
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/4aee3519-4f9b-48cf-8b10-3ca52175fe32" width="900" height="600">

- After you logged in, you will have access to specific functionalities based on your assigned role:
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/15147e9e-ed99-4664-b8e3-d915f7035f0f" width="900" height="600">

### Moderator:
- Can assign and change roles of the users from the drop down list(after the update you modify you will need to press save and you will have a pop-up confirmation and the update will be seen), and also for student can modify the student info, for example the year of study or the group
<img src=https://github.com/VladRo26/StudentCatalog/assets/100710098/cc4aeeb8-601a-4d9f-9e54-55c8d8661712 width="1000" height="500">
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/4b1b46bd-b9ef-4923-8a70-28cefb72d74d" width="1000" height="500">

- Also, the moderator can add new courses, modify the existing ones, delete and add student to a course. The interaction will be made from the Cursuri page, after. If a course is not selected you can search by the course and teacher and add a course. After you select a course the buttons for modify, delete and add student will be enabled. A student can be enrolled in a course only if the student is enrolled the study program and it's in the same university year as the course, otherwise the student can not be enrolled to a course.
  
- **Courses page**
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/20c88fb9-2b92-46a7-a6ad-d3a27515dadc" width="1000" height="500">

- **Select the course to enable the buttons**
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/30552805-1894-42f2-bff3-8041d57eb4e7" width="1000" height="500">

- **Here there is no student avalabile to enroll in this course because of the year of study**
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/79328310-3619-45a7-80e0-83952eb896d5" width="1000" height="500">

- **After I modified the student year I can assign it to the course**
<img src="https://github.com/VladRo26/StudentCatalog/assets/100710098/8ad5485d-bfa6-4609-9082-757776d596f5" width="1000" height="500">

### Teacher:






