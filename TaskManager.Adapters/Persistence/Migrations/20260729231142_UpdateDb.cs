using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Adapters.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TwoFactores",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionPlan",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JustifyPriority",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrioritySort",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactores",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExecutionPlan",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "JustifyPriority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PrioritySort",
                table: "Tasks");
        }
    }
}
