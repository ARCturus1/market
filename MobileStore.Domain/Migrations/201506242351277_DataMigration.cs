namespace MobileStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DescItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(maxLength: 4000),
                        PostedNew_NewId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PostedNews", t => t.PostedNew_NewId)
                .Index(t => t.PostedNew_NewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DescItems", "PostedNew_NewId", "dbo.PostedNews");
            DropIndex("dbo.DescItems", new[] { "PostedNew_NewId" });
            DropTable("dbo.DescItems");
        }
    }
}
