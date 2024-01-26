using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind.Mvc.Migrations.FileDb
{
    /// <inheritdoc />
    public partial class FileUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileOnFileSystemModels",
                table: "FileOnFileSystemModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileOnDatabaseModels",
                table: "FileOnDatabaseModels");

            migrationBuilder.RenameTable(
                name: "FileOnFileSystemModels",
                newName: "FileOnFileSystem");

            migrationBuilder.RenameTable(
                name: "FileOnDatabaseModels",
                newName: "FileOnDataBase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileOnFileSystem",
                table: "FileOnFileSystem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileOnDataBase",
                table: "FileOnDataBase",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileOnFileSystem",
                table: "FileOnFileSystem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileOnDataBase",
                table: "FileOnDataBase");

            migrationBuilder.RenameTable(
                name: "FileOnFileSystem",
                newName: "FileOnFileSystemModels");

            migrationBuilder.RenameTable(
                name: "FileOnDataBase",
                newName: "FileOnDatabaseModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileOnFileSystemModels",
                table: "FileOnFileSystemModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileOnDatabaseModels",
                table: "FileOnDatabaseModels",
                column: "Id");
        }
    }
}
