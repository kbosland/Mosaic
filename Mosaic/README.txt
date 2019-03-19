This project is called Mosaic, and it is a web version of a University Manager. This project is an MVC ASP.NET Core Web Application, made with Visual Studio 2017 and Entity Framework, programmed in C#, HTML, Javascript and SQL.

This project has two main types of users: Students and Professors. 
The functionalities for the Students include:
- Registering a new Student
- Logging in using your username/password from registered account
- Viewing all other registered students
- Checking your personal profile - including classes you are enrolled in, username, password etc.
- Enrolling in a Class
- Dropping a Class

The functionalities for the Professors include:
- Registering a new Professor
- Logging in using your username/password from registered account
- Viewing all other registered students
- Checking your personal profile - including classes you are teaching, username, password etc.
- Signing up to teach a Class
- Dropping a Class

The project also includes an Announcement and Email functionality where the Students and Professors can send an email to any other registered user on the system, view their inbox (and also reply, forward or mark emails as read), view their contacts list and view their sent emails folder. The Students also have an option to view all the class announcements for the classes they are enrolled in, and the Professors have an option to broadcast announcements to the classes they are teaching. 

All of the information regarding the Classes, Students, Professors, Emails and Announcements are stored in a SQL Server database, and I alter and retrieve the contents of the database using Entity Framework through the data layer of my application, mostly using LINQ statements. To ensure the security of my file, I implemented a Service Layer which I used to confirm the identity of the Students and Professors who are trying to access the web application during the Login process. While the user is logged in, the maintain access because my program stores their username and access level in cookies, which expire when the user logs out. I added client side scripting using Javascript embedded in my HTML Views which ensure that the user does not enter any invalid information in my application, and when combined with error handling in my Controllers, my application completely avoids resulting in an fatal error while the user is using the application.
