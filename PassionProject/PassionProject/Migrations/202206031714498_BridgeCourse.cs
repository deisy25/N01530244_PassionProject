namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BridgeCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_StudentID = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentID, t.Course_CourseId })
                .ForeignKey("dbo.Students", t => t.Student_StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Student_StudentID)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_StudentID", "dbo.Students");
            DropIndex("dbo.StudentCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "Student_StudentID" });
            DropTable("dbo.StudentCourses");
        }
    }
}
