namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BridgeTaught : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeacherCourses",
                c => new
                    {
                        Teacher_TeacherID = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_TeacherID, t.Course_CourseId })
                .ForeignKey("dbo.Teachers", t => t.Teacher_TeacherID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Teacher_TeacherID)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourses", "Teacher_TeacherID", "dbo.Teachers");
            DropIndex("dbo.TeacherCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.TeacherCourses", new[] { "Teacher_TeacherID" });
            DropTable("dbo.TeacherCourses");
        }
    }
}
