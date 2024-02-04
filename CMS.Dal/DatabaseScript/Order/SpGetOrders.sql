USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetOrders') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetOrders
GO

CREATE PROCEDURE cnt.SpGetOrders
	@Type TINYINT,	
	@PageSize INT,
	@PageIndex INT
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	IF @PageIndex = 0 OR @PageIndex IS NULL 
	BEGIN
		SET @pagesize = 100
		SET @PageIndex = 1
	END

	; WITH MainSelect AS
	(
		SELECT 
			*
 		FROM pbl.Orders
		WHERE
			(@Type = 0 OR @Type = Type)
	)
	SELECT 
		count(*) OVER() Total,* 
	FROM MainSelect
	ORDER BY [Date] DESC
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
	OPTION (RECOMPILE);

END 