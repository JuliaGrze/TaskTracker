using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetTasks_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllTasks = @"
                CREATE PROCEDURE [dbo].[GetAllTasks]
                AS BEGIN
                    SELECT TaskID, Title, Description, CreatedDate, Status FROM [dbo].[Tasks]
                END";

            migrationBuilder.Sql(sp_GetAllTasks);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllTasks = @"
                DROP PROCEDURE [dbo].[GetAllTasks]";

            migrationBuilder.Sql(sp_GetAllTasks);
        }
    }
}
