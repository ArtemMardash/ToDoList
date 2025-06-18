using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Migrations
{
    /// <inheritdoc />
    public partial class FixSubtask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_TaskDbId",
                table: "Subtasks");

            migrationBuilder.DropIndex(
                name: "IX_Subtasks_TaskDbId",
                table: "Subtasks");

            migrationBuilder.DropColumn(
                name: "TaskDbId",
                table: "Subtasks");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_ParentId",
                table: "Subtasks",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_ParentId",
                table: "Subtasks",
                column: "ParentId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_ParentId",
                table: "Subtasks");

            migrationBuilder.DropIndex(
                name: "IX_Subtasks_ParentId",
                table: "Subtasks");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskDbId",
                table: "Subtasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_TaskDbId",
                table: "Subtasks",
                column: "TaskDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_TaskDbId",
                table: "Subtasks",
                column: "TaskDbId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
