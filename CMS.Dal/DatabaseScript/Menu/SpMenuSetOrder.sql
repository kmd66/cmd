USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpMenuSetOrder') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpMenuSetOrder
GO

CREATE PROCEDURE pbl.SpMenuSetOrder
	@Json NVARCHAR(max)
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
	
	UPDATE 
		menu set [Order] = orders.[Order]
	FROM [pbl].[Menus] menu
	INNER JOIN 
	OPENJSON(@Json)
			WITH (
				UnicId UNIQUEIDENTIFIER,
				[Order] INT
	) orders ON  orders.UnicId = menu.UnicId

	SELECT * FROM [pbl].[Menus] 
	ORDER BY [Order]

END 
