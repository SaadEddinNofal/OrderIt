using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderITDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class addToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "CategoryId", "Name", "Description", "Price", "IsExist" },
                values: new object[,]
                {  
                // أصناف العصائر  
                { 10, "عصير البرتقال", "عصير برتقال طازج مُعصَر.", 250, true },
                { 10, "عصير التفاح", "عصير التفاح المقرمش والمنعش.", 230, true },
                { 10, "سموذي مانجو", "سموذي كريمي مصنوع من المانجو الطازج.", 300, true },
                { 10, "عصير الأناناس", "عصير أناناس استوائي، مثالي لفصل الصيف.", 280, true },
                { 10, "مزيج التوت", "عصير مزيج التوت، غني بمضادات الأكسدة.", 350, true },
                { 10, "عصير الجزر", "عصير جزر صحي ومنعش.", 270, true },
                { 10, "عصير البطيخ", "عصير بطيخ مرطب.", 290, true },
                { 10, "عصير السبانخ والتفاح", "عصير غني بالمغذيات مع السبانخ والتفاح.", 320, true },
                { 10, "عصير الليمون بالخيار", "ليمونادة خفيفة ومنعشة مع الخيار.", 260, true },
                { 10, "شاي الخ_peach", "شاي مثلج حلو مع لمسة من الخ_peach.", 210, true },  

                // أصناف اللحوم  
                { 11, "دجاج مشوي", "صدر دجاج مشوي طازج ومُتبّل.", 1000, true },
                { 11, "ستيك لحم البقر", "ستيك لحم بقر طري مشوي إلى الكمال.", 1500, true },
                { 11, "أضلاع لحم الضأن", "أضلاع لحم ضأن مُعالجة ومشوية.", 1800, true },
                { 11, "أضلاع لحم الخنزير", "أضلاع لحم خنزير مُدخنة مع صلصة باربكيو.", 1200, true },
                { 11, "كرات اللحم", "كرات لحم لذيذة في صلصة طماطم غنية.", 900, true },
                { 11, "أجنحة الدجاج", "أجنحة دجاج حارة مقدمة مع صلصة غمس.", 800, true },
                { 11, "برغر لحم البقر", "برغر لحم بقري كلاسيكي مع جبنة وتوابل.", 1100, true },
                { 11, "صدر البط", "صدر بط مُقلي مع صلصة حلوة.", 2000, true },
                { 11, "نقانق", "نقانق مشوية تقدم مع خردل.", 700, true },
                { 11, "كفتة العجل", "كفتة العجل الطرية مع قشرة بالأعشاب.", 2200, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // يمكنك إضافة كود لإزالة البيانات إذا لزم الأمر  
            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "CategoryId",
                keyValue: 11);
        }
    }
}
