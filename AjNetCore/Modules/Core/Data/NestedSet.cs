using Microsoft.EntityFrameworkCore;

namespace AjNetCore.Modules.Core.Data
{
	public interface INestedSet
	{
		
	}

	public static class NestedSet
	{
		public static void BuildTree(this SqlContext sqlContext, string tableName, string extraCondition = null)
		{
            var extraWhereCondition = extraCondition.IsNotNullOrEmpty() ? "AND " + extraCondition : "";

            sqlContext.Database.ExecuteSqlRaw(
                string.Format(
            @"WITH DbLevels AS
			(
				SELECT
				Id,
				CONVERT(VARCHAR(MAX), Id) AS thePath,
				1 AS Level
				FROM [{0}]
				WHERE ParentId IS NULL {1}

				UNION ALL

				SELECT
				e.Id,
				x.thePath + '.' + CONVERT(VARCHAR(MAX), e.Id) AS thePath,
					x.Level + 1 AS Level
				FROM DbLevels x 
					JOIN [{0}] e on e.ParentId = x.Id {1}
					),
				DbRows AS
				(
					SELECT
						DbLevels.*,
				ROW_NUMBER() OVER (ORDER BY thePath) AS Row
				FROM DbLevels
					)
				UPDATE
					[{0}]
				SET
				[{0}].[Left] = (ER.Row * 2) - ER.Level,
				[{0}].[Right] = ((ER.Row * 2) - ER.Level) + 
										   (
											   SELECT COUNT(*) * 2
				FROM DbRows ER2 
					WHERE ER2.thePath LIKE ER.thePath + '.%'
					) + 1
				FROM
					DbRows AS ER
				WHERE [{0}].Id = ER.Id;", tableName, extraWhereCondition));
            
        }

		internal sealed class TempDeleteDataEntity
		{
			public int NewLeft { get; set; }
			public int NewRight { get; set; }
			public int HasLeafs { get; set; }
			public int Width { get; set; }
			public int? SuperiorParent { get; set; }
		}

		public static void DeleteNode(this INestedSet nestedSet, string tableName, int nodeId, string extraCondition = null)
		{
			//var extraWhereCondition = extraCondition.IsNotNullOrEmpty() ? "AND " + extraCondition : "";

			//using (var sqlContext = new SqlContext())
			//{
			//	var tempDeleteDataEntity = sqlContext.Database.SqlQuery<TempDeleteDataEntity>(
			//		$@"SELECT [Left] AS NewLeft, [Right] As NewRight, ([Right] - [Left] - 1) as HasLeafs, ([Right] - [Left] + 1) as Width, ParentId As SuperiorParent
			//			FROM {tableName} WHERE Id = {nodeId}").First();

			//	sqlContext.Database.ExecuteSqlCommand($"DELETE FROM {tableName} WHERE Id = {nodeId} {extraWhereCondition};");

			//	if (tempDeleteDataEntity.HasLeafs == 0)
			//	{
			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"DELETE FROM {tableName} WHERE [Left] BETWEEN {tempDeleteDataEntity.NewLeft} AND {tempDeleteDataEntity.NewRight} {extraWhereCondition};");

			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"UPDATE {tableName} SET [Right] = [Right] - {tempDeleteDataEntity.Width} WHERE [Right] > {tempDeleteDataEntity.NewRight} {extraWhereCondition};");

			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"UPDATE {tableName} SET [Left] = [Left] - {tempDeleteDataEntity.Width} WHERE [Left] > {tempDeleteDataEntity.NewRight} {extraWhereCondition};");
			//	}
			//	else
			//	{
			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"DELETE FROM {tableName} WHERE [Left] = {tempDeleteDataEntity.NewLeft} {extraWhereCondition};");

			//		sqlContext.Database.ExecuteSqlCommand(
			//			$@"UPDATE {tableName} SET [Right] = [Right] - 1, [Left] = [Left] - 1, ParentId = {tempDeleteDataEntity.SuperiorParent} 
			//				WHERE [Left] BETWEEN {tempDeleteDataEntity.NewLeft} AND {tempDeleteDataEntity.NewRight} {extraWhereCondition};");

			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"UPDATE {tableName} SET [Right] = [Right] - 2 WHERE [Right] > {tempDeleteDataEntity.NewRight} {extraWhereCondition};");

			//		sqlContext.Database.ExecuteSqlCommand(
			//			$"UPDATE {tableName} SET [Left] = [Left] - 2 WHERE [Left] > {tempDeleteDataEntity.NewRight} {extraWhereCondition};");
			//	}
			//}
		}
	}
}