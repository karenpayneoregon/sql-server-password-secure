# Storing passwords in SQL-Server database

Developers just starting working with Windows Forms, WPF or Console applications tend to use plain text to store passwords in a database which is not wise as anyone that can open the database can see these passwords stored in plain text.

Presented are two ways to keep passwords secure. The first [PWDENCRYPT](https://learn.microsoft.com/en-us/sql/t-sql/functions/pwdencrypt-transact-sql?view=sql-server-ver16) is easy but is marked as `obsolete` while the second, [HASHBYTES](https://learn.microsoft.com/en-us/sql/t-sql/functions/hashbytes-transact-sql?view=sql-server-ver16) is recommended.

Code sample are provided for both `PWDENCRYPT` and `HASHBYTES`

:heavy_check_mark: Make sure to read instructions in the readme file in the project for setting up the database.

:heavy_check_mark: Requires Visual Studio 2022, version 17.4.x