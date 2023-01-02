# Storing passwords in SQL-Server database

**Important**: Make sure to following instructions under the Script folder readme file.

# Introduction

Developers just starting working with Windows Forms, WPF or Console applications tend to use plain text to store passwords in a database which is not wise as anyone that can open the database can see these passwords stored in plain text.

# SqlServerUtilities project

This project contains code to perform login and add users to the backend database.


**Screenshot for both examples**

![Login](assets/login.png)

:stop_sign: Code for example 1 

# Example 1
An easy method to store passwords is [PWDENCRYPT](https://learn.microsoft.com/en-us/sql/t-sql/functions/pwdencrypt-transact-sql?view=sql-server-ver16) which might not be supported in future releases of SQL-Server, the second example uses [HASHBYTES](https://learn.microsoft.com/en-us/sql/t-sql/functions/hashbytes-transact-sql?view=sql-server-ver16) which offers more options.

```sql
INSERT INTO dbo.Users (Username,Password) VALUES ('payneoregon',PWDENCRYPT('!FirstOnMonday'))
```

To return the password we use [PWDCOMPARE](https://learn.microsoft.com/en-us/sql/t-sql/functions/pwdcompare-transact-sql?view=sql-server-ver16)


```csharp
public static bool ValidateUser(string username, SecureString password)
{
    using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
    using var cmd = new SqlCommand() { Connection = cn };
    cmd.CommandText = "SELECT Id from dbo.Users WHERE Username = @UserName AND PWDCOMPARE(@Password,[Password]) = 1";
    cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = username;
    cmd.Parameters.Add("@Password", SqlDbType.NChar).Value = password.ToUnSecureString();

    cn.Open();
    return cmd.ExecuteScalar() != null;
}
```

# Example 2

This method provides more options using [HASHBYTES](https://learn.microsoft.com/en-us/sql/t-sql/functions/hashbytes-transact-sql?view=sql-server-ver16). In the following example only the password is protected, if you like, the next step would be to concatenate the user name and password together.

```sql
INSERT INTO Users1 (UserName, [Password]) VALUES ('payneoregon', HASHBYTES('SHA2_512', '!FirstOnMonday'));
```

To validate the password use the following SQL-Server function.

```sql
CREATE OR ALTER FUNCTION Password_Check ( @v1 VARCHAR(500) )
RETURNS VARCHAR(7)
AS
BEGIN
    DECLARE @result VARCHAR(7);

    SELECT @result = (CASE
                          WHEN [Password] = HASHBYTES('SHA2_512', @v1) THEN
                              'Valid'
                          ELSE
                              'Invalid'
                      END
                     )
    FROM Users1
    WHERE [Password] = HASHBYTES('SHA2_512', @v1);

    RETURN @result;

END;
```

Code

```csharp
public static bool ValidateUser1(string username, SecureString password)
{
    using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
    using var cmd = new SqlCommand() { Connection = cn };

    cmd.CommandText = "SELECT Id from dbo.Users WHERE Username = @UserName";
    cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = username;
    cn.Open();

    var result = cmd.ExecuteScalar();

    if (cmd.ExecuteScalar() == null)
    {
        return false;
    }

    cmd.CommandText = "SELECT [dbo].[Password_Check](@Password);";
    cmd.Parameters.Add("@Password", SqlDbType.NChar).Value = password.ToUnSecureString();


    return Convert.ToString(cmd.ExecuteScalar()) == "Valid";

}
```

# Example 3

Where example 2 used two statements, this version uses one statement. This is the `best` of the three to use.

```csharp
public static bool ValidateUser2(string username, SecureString password)
{
    using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
    using var cmd = new SqlCommand() { Connection = cn };

    cmd.CommandText = "SELECT Id from dbo.Users WHERE Username = @UserName  AND [dbo].[Password_Check](@Password) = 'Valid'";
    cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = username;
    cmd.Parameters.Add("@Password", SqlDbType.NChar).Value = password.ToUnSecureString();
    cn.Open();
        
    return Convert.ToString(cmd.ExecuteScalar()) == "1";

}
```

# Add new user

- When adding a new record `HASHBYTES('SHA2_512', @Password))` hashs the given password.
- The second query `SELECT CAST(scope_identity() AS int)` is used to obtain the new primary key for the user and is returned to the caller, an alternate is to set the `User.Id` and return nothing.

```csharp
public static int AddUser(User user)
{
    using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
    using var cmd = new SqlCommand() { Connection = cn };


    cmd.CommandText = "INSERT INTO Users1 (UserName, [Password]) VALUES (@UserName, HASHBYTES('SHA2_512', @Password));" +
                        "SELECT CAST(scope_identity() AS int);";

    cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = user.Name;
    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Password.ToUnSecureString();

    cn.Open();

    return Convert.ToInt32(cmd.ExecuteScalar());

}
```

# Scripts

Recommend running these scripts in SQL-Server Management Studio.

- script.sql need to run to create the database
- InsertStatements.sql provides insert statements to get started with rather than needing to add several records in the app which in turn allows testing the login right away.
- PasswordCheckFunction.sql is needed to check password hashes and is hard coded to `Users1` table.


## Next steps

- Placing desired option into a class project.
- If option 2, create the function presented modified for your databases


# Credentials

Use the following to see the code work then provide invalid information.

- **UserName** payneoregon
- **Password** !FirstOnMonday


# Requires

- Microsoft Visual Studio 2022 version 17.4 or higher
- LocalDb installed which should be done when Visual Studio is installed.


# Ending notes

I kept the code sample simple so they can be used `as is` or modified to suit indivdual requirements. There are variations to each method that a developer can choice and optionally add assertion to ensure user name and password are not empty and met password rules, there are libraries for this like [Easy.Password.Validator](https://www.nuget.org/packages/Easy.Password.Validator) and exception handling.

