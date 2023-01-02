USE [Passwording]

--INSERT INTO dbo.Users (Username,Password) VALUES ('payneoregon',PWDENCRYPT('!FirstOnMonday'))
--INSERT INTO dbo.Users (Username,Password) VALUES ('jimbeam',PWDENCRYPT('@MySilly1Password'))
--INSERT INTO dbo.Users (Username,Password) VALUES('smithone',PWDENCRYPT('P@ss123'))



--SELECT Username, [Password] FROM dbo.Users AS u
--SELECT Username,[Password] FROM dbo.Users WHERE Username ='payneoregon' AND PWDCOMPARE(N'Test',[Password]) = 1
--SELECT Username,[Password] FROM dbo.Users WHERE Username ='payneoregon' AND PWDCOMPARE(N'!FirstOnMonday',[Password]) = 1

---TRUNCATE TABLE dbo.Users

--INSERT INTO Users1 (UserName, [Password]) VALUES ('payneoregon', HASHBYTES('SHA2_512', '!FirstOnMonday'));
--INSERT INTO Users1 (UserName, [Password]) VALUES ('jimbeam', HASHBYTES('SHA2_512', '@MySilly1Password'));


---INSERT INTO Users1 (UserName, [Password]) VALUES ('karen', HASHBYTES('SHA2_512', 'Password'));



TRUNCATE TABLE dbo.Users1;
---INSERT INTO Users1 (UserName, [Password]) VALUES ('payneoregon', HASHBYTES('SHA2_512', '!FirstOnMonday'));
---INSERT INTO Users1 (UserName, [Password]) VALUES (@UserName, HASHBYTES('SHA2_512', @Password))
---INSERT INTO Users1 (UserName, [Password]) VALUES ('payneoregon', HASHBYTES('SHA2_512', '!FirstOnMonday'));
INSERT INTO Users1 (UserName, [Password]) VALUES ('payne', HASHBYTES('SHA2_512', 'pword'));

DECLARE @UserName AS VARCHAR(100) = 'payne';
DECLARE @Password AS VARCHAR(250)= 'pword';


SELECT Id,[dbo].[Password_Check] (@Password) FROM dbo.Users1 AS u  WHERE u.UserName = @UserName

---SELECT * FROM dbo.Users1 AS u WHERE dbo.Password_Check  ''

SELECT  [Id],[UserName] ,[Password]  FROM [Passwording].[dbo].[Users1] 



