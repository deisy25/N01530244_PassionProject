namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        StudentName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.StudentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
