using System.Data;
using System.Security;
using BasicSqlServerPasswordApp.Models;
using ConfigurationLibrary.Classes;
using Microsoft.Data.SqlClient;

namespace BasicSqlServerPasswordApp.Classes;

/// <summary>
/// * Make sure to following instructions in readme under the scripts folder before running the code.
/// * Recommend <see cref="ValidateUser2"/> others are there to see how we got there and that HASHBYTES is better than PWDCOMPARE
///   but that is up to the developer.
/// </summary>
public class DataOperations
{
    /// <summary>
    /// Validate a user name then password. Password validation is done via a SQL-Server using PWDCOMPARE which may go away in
    /// future releases of SQL-Server.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Validate user name and password
    /// </summary>
    /// <param name="username">user name to validate</param>
    /// <param name="password">user password to validate</param>
    /// <returns>success or failure</returns>
    public static bool ValidateUser1(string username, SecureString password)
    {
        using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
        using var cmd = new SqlCommand() { Connection = cn };

        /*
         * Note:
         * (@Password) as ValidItem is used if you want to load results into a DataTable which
         * makes it easy for debugging, otherwise no need for the alias.
         */
        cmd.CommandText = "SELECT Id,[dbo].[Password_Check] (@Password) as ValidItem FROM dbo.Users1 AS u  WHERE u.UserName = @UserName";
        cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = username;
        cmd.Parameters.Add("@Password", SqlDbType.NChar).Value = password.ToUnSecureString();

        cn.Open();

        var reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            return reader.GetValue(1) != DBNull.Value;
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// Validate user name and password, if user input matches return their primary key
    /// </summary>
    /// <param name="username">user name to validate</param>
    /// <param name="password">user password to validate</param>
    /// <returns>success and identity</returns>
    public static (bool success, int? id) ValidateUser2(string username, SecureString password)
    {
        using var cn = new SqlConnection(ConfigurationHelper.ConnectionString());
        using var cmd = new SqlCommand() { Connection = cn };

        cmd.CommandText = "SELECT Id from dbo.Users1 WHERE Username = @UserName  AND [dbo].[Password_Check](@Password) = 'Valid'";
        cmd.Parameters.Add("@UserName", SqlDbType.NChar).Value = username;
        cmd.Parameters.Add("@Password", SqlDbType.NChar).Value = password.ToUnSecureString();

        cn.Open();

        var reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            return (true, reader.GetInt32(0));
        }
        else
        {
            return (false, null);
        }
        
    }

    /// <summary>
    /// Add new user and return the new primary key
    /// </summary>
    /// <param name="user"></param>
    /// <returns>Primary key</returns>
    /// <remarks>Exception handling left out for clarity</remarks>
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
}