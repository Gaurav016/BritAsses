CREATE TABLE [dbo].[Item]

(

    [Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),

    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES Product(Id),

    [Quantity] INT NOT NULL

)