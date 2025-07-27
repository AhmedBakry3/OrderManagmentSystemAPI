using Microsoft.EntityFrameworkCore.Migrations;

public partial class ConvertPaymentMethod_StatusToEnum_ChangeDataTypeOfUsersModelId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Step 1: Add a new column with the correct type (Guid)
        migrationBuilder.AddColumn<Guid>(
            name: "TempId",  // Temporary name for new Guid column
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        // Step 2: Drop the old Id column
        migrationBuilder.DropColumn(
            name: "Id",
            table: "Users");

        // Step 3: Rename TempId to Id
        migrationBuilder.RenameColumn(
            name: "TempId",
            table: "Users",
            newName: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Reverse the process to restore the original structure

        // Step 1: Add the old Id column back
        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "Users",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        // Step 2: Drop the TempId column
        migrationBuilder.DropColumn(
            name: "TempId",
            table: "Users");
    }
}
