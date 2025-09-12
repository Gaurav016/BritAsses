CREATE TABLE [dbo].[Product]

(

                [Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),

                [ProductName] NVARCHAR(255) NOT NULL,

                [CreatedBy]  NVARCHAR(100) NOT NULL,

                [CreatedOn]  DATETIME NOT NULL,

                [ModifiedBy]  NVARCHAR(100) NULL,

                [ModifiedOn]  DATETIME NULL

 

)