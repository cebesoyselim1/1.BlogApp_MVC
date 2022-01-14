using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "VARBINARY(500)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CommentCont = table.Column<int>(type: "int", nullable: false),
                    SeoAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SeoTags = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "CreatedName", "Description", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Name", "Note" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2183), "InitialCreate", "Everthing about C# programming language", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2197), "InitialCreate", "C#", "C# Programming Language" },
                    { 2, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2221), "InitialCreate", "Everthing about C++ programming language", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2222), "InitialCreate", "C++", "C++ Programming Language" },
                    { 3, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2226), "InitialCreate", "Everthing about Javascript programming language", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 435, DateTimeKind.Local).AddTicks(2227), "InitialCreate", "Javascript", "Javascript Programming Language" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "CreatedName", "Description", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Name", "Note" },
                values: new object[] { 1, new DateTime(2022, 1, 13, 20, 57, 47, 438, DateTimeKind.Local).AddTicks(7710), "InitialCreate", "Access anywhere", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 438, DateTimeKind.Local).AddTicks(7726), "InitialCreate", "Admin", "Admin Role" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "CreatedName", "Description", "Email", "FirstName", "Image", "IsActive", "IsDeleted", "LastName", "ModifiedDate", "ModifiedName", "Note", "PasswordHash", "RoleId", "UserName" },
                values: new object[] { 1, new DateTime(2022, 1, 13, 20, 57, 47, 444, DateTimeKind.Local).AddTicks(6331), "InitialCreate", null, "alidmn@gmail.com", "Ali", "default.jpg", true, false, "Duman", new DateTime(2022, 1, 13, 20, 57, 47, 444, DateTimeKind.Local).AddTicks(6346), "InitialCreate", "User", new byte[] { 48, 49, 57, 50, 48, 50, 51, 97, 55, 98, 98, 100, 55, 51, 50, 53, 48, 53, 49, 54, 102, 48, 54, 57, 100, 102, 49, 56, 98, 53, 48, 48 }, 1, "alidmn" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "CommentCont", "Content", "CreatedDate", "CreatedName", "Date", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Note", "SeoAuthor", "SeoDescription", "SeoTags", "Thumbnail", "Title", "UserId", "ViewCount" },
                values: new object[] { 1, 1, 57, "C# 9.0 introduces record types. You use the record keyword to define a reference type that provides built-in functionality for encapsulating data. You can create record types with immutable properties by using positional parameters or standard property syntax:", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(6511), "InitialCreate", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(4985), true, false, new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(6770), "InitialCreate", "C# 9.0 news", "Ali Duman", "What are new on C# 9.0", "C#, C# 9.0, C# news", "Default.jpg", "What are new on C# 9.0", 1, 4332 });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "CommentCont", "Content", "CreatedDate", "CreatedName", "Date", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Note", "SeoAuthor", "SeoDescription", "SeoTags", "Thumbnail", "Title", "UserId", "ViewCount" },
                values: new object[] { 2, 2, 32, "C++11 is a version of the ISO/IEC 14882 standard for the C++ programming language. C++11 replaced the prior version of the C++ standard, called C++03,[1] and was later replaced by C++14. The name follows the tradition of naming language versions by the publication year of the specification, though it was formerly named C++0x because it was expected to be published before 2010.", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8113), "InitialCreate", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8111), true, false, new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8114), "InitialCreate", "C++ 11.0 news", "Ali Duman", "What are news on C++ 11", "C++, C++ 11.0, C++ news", "Default.jpg", "Briliant changes on C++ 11.0", 1, 2701 });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "CommentCont", "Content", "CreatedDate", "CreatedName", "Date", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Note", "SeoAuthor", "SeoDescription", "SeoTags", "Thumbnail", "Title", "UserId", "ViewCount" },
                values: new object[] { 3, 3, 13, "ECMAScript 2015 was the second major revision to JavaScript.ECMAScript 2015 is also known as ES6 and ECMAScript 6.This chapter describes the most important features of ES6.", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8120), "InitialCreate", new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8119), true, false, new DateTime(2022, 1, 13, 20, 57, 47, 432, DateTimeKind.Local).AddTicks(8121), "InitialCreate", "ES6 news", "Ali Duman", "What are news on ES6", "Javascript, ES6, Javascript ES6", "Default.jpg", "What are news on ES6", 1, 1003 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "CreatedDate", "CreatedName", "IsActive", "IsDeleted", "ModifiedDate", "ModifiedName", "Note", "Text" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(409), "InitialCreate", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(426), "InitialCreate", "C# Comment", "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like)" },
                    { 2, 1, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(458), "InitialCreate", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(459), "InitialCreate", "C# Comment", "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc." },
                    { 3, 2, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(464), "InitialCreate", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(465), "InitialCreate", "C++ Comment", "Phasellus eu vehicula massa. Sed viverra ut dui ac lacinia. Aenean vestibulum eget ex vel finibus. Morbi non porttitor metus. Nam rhoncus quam vitae quam elementum tincidunt. Fusce vel eros tempus, ullamcorper massa vitae, varius nunc. In ut bibendum diam. Duis sit amet vestibulum lectus. Morbi lacinia quam vitae viverra condimentum. In porta, augue id pharetra cursus, odio felis molestie turpis, sit amet scelerisque urna ex sed neque." },
                    { 4, 3, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(469), "InitialCreate", true, false, new DateTime(2022, 1, 13, 20, 57, 47, 437, DateTimeKind.Local).AddTicks(470), "InitialCreate", "Javascript Comment", "Aenean finibus nibh at purus dictum, quis condimentum eros viverra. Proin sed imperdiet dolor. Maecenas at est risus. Quisque commodo tortor eu ligula porttitor feugiat ut id nulla. Fusce et volutpat nibh, id ultrices diam. Sed est sem, consequat in arcu et, consequat eleifend orci. Pellentesque dignissim nec ex bibendum tincidunt." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
