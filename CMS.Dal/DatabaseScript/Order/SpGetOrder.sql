USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetOrderPost') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetOrderPost
GO

CREATE PROCEDURE cnt.SpGetOrderPost
	@TrackingCode NVARCHAR(MAX)
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
	
	DECLARE @json NVARCHAR(MAX)
	SET @json = (SELECT [Basket] FROM pbl.Orders WHERE [TrackingCode] = @TrackingCode )
		
	SELECT 
		p.Title
		, p.Img
	FROM OPENJSON(@Json) j
	INNER JOIN cnt.Posts p ON p.Alias = j.value
	INNER JOIN cnt.Products pr ON pr.UnicId = p.UnicId

END 