namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Teacher : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherID = c.Int(nullable: false, identity: true),
                        TeacherName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address = c.String(),
                        PostalCode = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TeacherID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teachers");
        }
    }
}
