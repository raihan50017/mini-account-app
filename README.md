#  Account Management System

A basic user account management system built with ASP.NET Core. This project includes functionality for user registration, login, role-based access control, and user role assignment.

##  Project Setup

Follow these steps to set up and run the project on your local machine.

### 1. Clone the Repository

```bash
git clone https://github.com/raihan50017/mini-account-app.git
```

### 2. Open in Visual Studio

- Open **Visual Studio**.
- Load the solution from the cloned repository folder.

### 3. Configure the Database Connection

- Open the `appsettings.json` file.
- Update the `DefaultConnection` under the `ConnectionStrings` section with your SQL Server configuration:

```json
"ConnectionStrings": {
      "DefaultConnection": "Server=DESKTOP-1C6REMR;Database=MiniAccountAppDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True",

}
```

### 4. Apply Migrations & Update the Database

- Open the **Package Manager Console** and run the following command:

```bash
dotnet ef database update
```

- execute the procedure creating sql provided in data folder

### 5. Run the Project

- Press `F5` or click **Start Debugging** in Visual Studio.
- The application will launch in your default web browser.

---

## Usage Instructions

### 1. Register a New User

- Navigate to the **Register** page.
- Fill in your credentials and complete the registration process.

### 2. Confirm the Registration

- Check your email and confirm the registration (if email confirmation is enabled).
- Alternatively, for local development, manually update the user record in the database to mark the email as confirmed.

## 3. Log In

- Use the registered email and password to log in to the application.

### 4. Create Roles and Assign the Admin Role

- Navigate to the **Role Management** section.
- Create a new role, e.g., `Admin`.
- Assign the `Admin` role to your user account.

### 5. Log Out and Re-login

- Log out after assigning the role.
- Log back in — you should now have access to restricted/admin menus and features.