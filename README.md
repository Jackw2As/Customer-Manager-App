# Description
This project was originally created to teach me the WPF and WinUI 3 UI Frameworks. It began as apart of an online learning course teaching the fundamental of .NET Desktop Applications.
Since then its scope has changed multiple times and only shares a few similarities to the original project. I also used to learn about architecture considerations and I’ve decided to use this project 
as a reference of my work for potential employers, or people acting on their behalf, to demonstrate my ability to write C# for a desktop
with standard libraries and frameworks to build simple applications. 

This project is meant to show:
- My ability to write readable code in C#.
- Some understanding of Clean Architexture and Domain Driven Design.
- The Code should be easy to extend and expand if I want to expand the scope of the project in the future.

What I learned from this project:
- Planning out a project before implementing it will greatly reduce the timescale needed to implement the project.
- When a particular solution isn't working instead of trying to get that solution to work, consider alternative implementations that might be faster or more straightforward.
- Don't write code at 10PM when your absolutely exhausted.
- When facing a bug that you don't quite understand take a break and then try to solve it from a different angle (I can't tell you the amount of hours I wasted trying 
to solve some simple bug I didn't understand only to get a cup of tea and solve it 5 minutes later)
- In the future I need to figure out how to better test my code. Alot of the code I wrote required constant manual testing to make sure it worked.

# Installation Instructions
**Note:** This project was designed to run on Windows 10. while it should work with newer versions of Windows it may not run on older versions. 

The simplest way to Run this project is to download the project into Visual Studio and then Run either the WpfApp project, or the WinUI project. 
I might offer a deployed solution in the future. However as of writing this there are still a few bugs that need to be ironed out.

By default the project uses the MockCustomerRepository, which only stores data in-memory and data won't be saved upon closing the application. 
If you want to project to store data to a file on your computer then you'll have to make a small change in the DataService Class(See switching to JsonCustomerRepository).
However this program doesn't have a feature to delete the file and you'll have to remove the file created for storage manually. 
The File should be saved somewhere inside the project folder, however I offer no guarentee that it will be saved there. 

### Switching to JsonCustomerRepository
The DataService Class is located in the namesplace CustomerManagerApp.Backend.Service or CustomerManagerApp/Backend/Service.
Open this file and go to line 24. It should look something like:

    customerRepository = await setupMockCustomerRepository();

Replace this line with:

    customerRepository = new JsonCustomerRepo();

The program should now save any data to a File in a json format.

# Contributing
Jackw2As - John Peter Aalders
Special Thanks to Thomas Claudius Huber who provided the original logo and inspiration for the project through his online course.

# License
**All Rights Reserved**. This code is not free for use, I'm not providing it so that it can be copied, 
modified, written, used as the basis of a new project or added to an existing project. 
Its avaliable purely as an example of my ability to program. 
There is also nothing particularly intresting or special that couldn't be created by another developer 
anyway so why you would feel the need to take any code from this project is beyond me.
