﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FishingForum.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropFilepathToProfilePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ProfilePicture",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ProfilePicture");
        }
    }
}
