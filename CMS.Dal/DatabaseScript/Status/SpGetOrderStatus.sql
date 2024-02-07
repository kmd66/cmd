USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpGetOrderStatus') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpGetOrderStatus
GO

CREATE PROCEDURE pbl.SpGetOrderStatus
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
	
	; WITH MainSelect AS
	(
		SELECT COUNT(Id) Total,[Type] FROM pbl.Orders
		WHERE [Type] in (1, 2, 3, 20, 50)
		GROUP BY [Type]
	)
	SELECT 
		0 Id, ''TrackingCode, [Type], ''FirstName, ''LastName, ''Mail, ''Mobile, ''[Address], ''PostalCode, ''Basket, NEwid()UnicId, ''Text, GETDATE()Date,Total
	FROM MainSelect

END 
