namespace Pañol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class perro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cod_Barras = c.String(),
                        Detalle = c.String(),
                        F_Alta = c.DateTime(nullable: false),
                        F_Baja = c.DateTime(),
                        Disponible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prestamos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfesorId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Curso = c.String(),
                        FechaHora_E = c.DateTime(nullable: false),
                        FechaHora_D = c.DateTime(),
                        Retira = c.String(),
                        Cancela = c.Boolean(nullable: false),
                        Observacion = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profesores", t => t.ProfesorId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.ProfesorId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.PrestamoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrestamoId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Prestamos", t => t.PrestamoId, cascadeDelete: true)
                .Index(t => t.PrestamoId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Profesores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Dni = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        IdRol = c.Int(nullable: false),
                        DniId = c.String(),
                        Rol_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Rol_Id)
                .Index(t => t.Rol_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RolNombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "Rol_Id", "dbo.Roles");
            DropForeignKey("dbo.Prestamos", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Prestamos", "ProfesorId", "dbo.Profesores");
            DropForeignKey("dbo.PrestamoItems", "PrestamoId", "dbo.Prestamos");
            DropForeignKey("dbo.PrestamoItems", "ItemId", "dbo.Items");
            DropIndex("dbo.Usuarios", new[] { "Rol_Id" });
            DropIndex("dbo.PrestamoItems", new[] { "ItemId" });
            DropIndex("dbo.PrestamoItems", new[] { "PrestamoId" });
            DropIndex("dbo.Prestamos", new[] { "UsuarioId" });
            DropIndex("dbo.Prestamos", new[] { "ProfesorId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Profesores");
            DropTable("dbo.PrestamoItems");
            DropTable("dbo.Prestamos");
            DropTable("dbo.Items");
        }
    }
}
