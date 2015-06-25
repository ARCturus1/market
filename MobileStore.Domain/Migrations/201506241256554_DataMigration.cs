namespace MobileStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Product_ProductID", c => c.Int());
            AddColumn("dbo.Products", "TitleImage_Id", c => c.Int());
            CreateIndex("dbo.Files", "Product_ProductID");
            CreateIndex("dbo.Products", "TitleImage_Id");
            AddForeignKey("dbo.Files", "Product_ProductID", "dbo.Products", "ProductID");
            AddForeignKey("dbo.Products", "TitleImage_Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "TitleImage_Id", "dbo.Files");
            DropForeignKey("dbo.Files", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.Products", new[] { "TitleImage_Id" });
            DropIndex("dbo.Files", new[] { "Product_ProductID" });
            DropColumn("dbo.Products", "TitleImage_Id");
            DropColumn("dbo.Files", "Product_ProductID");
        }
    }
}
