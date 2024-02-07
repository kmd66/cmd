USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'pbl.SpGetOrders') AND type in (N'P', N'PC'))
    DROP PROCEDURE pbl.SpGetOrders
GO

CREATE PROCEDURE pbl.SpGetOrders
	@Type TINYINT,	
	@FirstName NVARCHAR(MAX),		
	@LastName NVARCHAR(MAX),	
	@TrackingCode NVARCHAR(MAX),
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
			AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
			AND (@LastName IS NULL OR LastName LIKE '%' + @LastName+ '%')
			AND (@TrackingCode IS NULL OR TrackingCode LIKE '%' + @TrackingCode+ '%')
	)
	SELECT 
		count(*) OVER() Total,* 
	FROM MainSelect
	ORDER BY Id DESC
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
	OPTION (RECOMPILE);

END 