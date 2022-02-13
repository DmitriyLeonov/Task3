namespace Task3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLogIn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLogIn");
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
        }
    }
}
