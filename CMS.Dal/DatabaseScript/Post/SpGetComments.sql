USE [TalaPishe]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'cnt.SpGetComments') AND type in (N'P', N'PC'))
    DROP PROCEDURE cnt.SpGetComments
GO

CREATE PROCEDURE cnt.SpGetComments
	@PostId BIGINT,	
	@Type INT
--WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

	; WITH MainSelect AS
	(
		SELECT Top 500
			com.*
			,COALESCE(sor.[Value], 0) Score
 		FROM cnt.Comments com
		LEFT JOIN cnt.Scores sor ON sor.CommentId = com.Id
		WHERE
			com.PostId = @PostId
			AND (@Type = 0 OR com.[Type] = @Type)
	)
	SELECT 
		count(*) OVER() [Count],* 
	FROM MainSelect
	ORDER BY [Date]

END 