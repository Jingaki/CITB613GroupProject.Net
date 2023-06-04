using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupProjectDepositCatalogWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class initSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeDeposit = table.Column<bool>(type: "bit", nullable: false),
                    OverdraftPossability = table.Column<bool>(type: "bit", nullable: false),
                    CreditPossability = table.Column<bool>(type: "bit", nullable: false),
                    MonthlyCompounding = table.Column<bool>(type: "bit", nullable: false),
                    TerminalCapitalization = table.Column<bool>(type: "bit", nullable: false),
                    ValidForClientsOnly = table.Column<bool>(type: "bit", nullable: false),
                    TypeOfDeposit = table.Column<int>(type: "int", nullable: false),
                    TypeOfInterest = table.Column<int>(type: "int", nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: false),
                    InterestPaymentType = table.Column<int>(type: "int", nullable: false),
                    OwnershipType = table.Column<int>(type: "int", nullable: false),
                    EffectiveAnnualInterestRate = table.Column<float>(type: "real", nullable: false),
                    WebLinkToOffer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionOfNegotiatedInterestRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinSum = table.Column<float>(type: "real", nullable: false),
                    MinSumDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinDuration = table.Column<int>(type: "int", nullable: false),
                    MaxSum = table.Column<float>(type: "real", nullable: false),
                    MaxSumDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxDuration = table.Column<int>(type: "int", nullable: false),
                    DurationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposits_AspNetUsers_MyApplicationUserId",
                        column: x => x.MyApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_MyApplicationUserId",
                table: "Deposits",
                column: "MyApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DropTable(
                name: "SearchFormEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
