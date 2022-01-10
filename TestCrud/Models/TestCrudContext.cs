using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TestCrudContext : DbContext
    {
        public TestCrudContext()
        {
        }

        public TestCrudContext(DbContextOptions<TestCrudContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAlquiler> TAlquiler { get; set; }
        public virtual DbSet<TGenero> TGenero { get; set; }
        public virtual DbSet<TGeneroPelicula> TGeneroPelicula { get; set; }
        public virtual DbSet<TPelicula> TPelicula { get; set; }
        public virtual DbSet<TRol> TRol { get; set; }
        public virtual DbSet<TUsers> TUsers { get; set; }
        public virtual DbSet<TVenta> TVenta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=localhost;database=TestCrud;User Id=sa;Password=asdasdx2;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAlquiler>(entity =>
            {
                entity.HasKey(e => e.CodAlquiler)
                    .HasName("PK__tAlquile__28866C2C66A6EA37");

                entity.ToTable("tAlquiler");

                entity.Property(e => e.CodAlquiler).HasColumnName("cod_alquiler");

                entity.Property(e => e.CodPelicula).HasColumnName("cod_pelicula");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaDevolucion)
                    .HasColumnName("fechaDevolucion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.CodPeliculaNavigation)
                    .WithMany(p => p.TAlquiler)
                    .HasForeignKey(d => d.CodPelicula)
                    .HasConstraintName("fk_cod_peliculaAlquiler");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.TAlquiler)
                    .HasForeignKey(d => d.CodUsuario)
                    .HasConstraintName("fk_cod_usuarioAlquiler");
            });

            modelBuilder.Entity<TGenero>(entity =>
            {
                entity.HasKey(e => e.CodGenero)
                    .HasName("PK__tGenero__0DACB9D501E96F58");

                entity.ToTable("tGenero");

                entity.Property(e => e.CodGenero).HasColumnName("cod_genero");

                entity.Property(e => e.TxtDesc)
                    .HasColumnName("txt_desc")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TGeneroPelicula>(entity =>
            {
                entity.HasKey(e => new { e.CodPelicula, e.CodGenero })
                    .HasName("PK__tGeneroP__6285A59544F233E2");

                entity.ToTable("tGeneroPelicula");

                entity.Property(e => e.CodPelicula).HasColumnName("cod_pelicula");

                entity.Property(e => e.CodGenero).HasColumnName("cod_genero");

                entity.HasOne(d => d.CodGeneroNavigation)
                    .WithMany(p => p.TGeneroPelicula)
                    .HasForeignKey(d => d.CodGenero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pelicula_genero");

                entity.HasOne(d => d.CodPeliculaNavigation)
                    .WithMany(p => p.TGeneroPelicula)
                    .HasForeignKey(d => d.CodPelicula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_genero_pelicula");
            });

            modelBuilder.Entity<TPelicula>(entity =>
            {
                entity.HasKey(e => e.CodPelicula)
                    .HasName("PK__tPelicul__225F6E08C0EBBB7E");

                entity.ToTable("tPelicula");

                entity.Property(e => e.CodPelicula).HasColumnName("cod_pelicula");

                entity.Property(e => e.CantDisponiblesAlquiler).HasColumnName("cant_disponibles_alquiler");

                entity.Property(e => e.CantDisponiblesVenta).HasColumnName("cant_disponibles_venta");

                entity.Property(e => e.PrecioAlquiler)
                    .HasColumnName("precio_alquiler")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PrecioVenta)
                    .HasColumnName("precio_venta")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TxtDesc)
                    .HasColumnName("txt_desc")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TRol>(entity =>
            {
                entity.HasKey(e => e.CodRol)
                    .HasName("PK__tRol__F13B121151A62298");

                entity.ToTable("tRol");

                entity.Property(e => e.CodRol).HasColumnName("cod_rol");

                entity.Property(e => e.SnActivo).HasColumnName("sn_activo");

                entity.Property(e => e.TxtDesc)
                    .HasColumnName("txt_desc")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TUsers>(entity =>
            {
                entity.HasKey(e => e.CodUsuario)
                    .HasName("PK__tUsers__EA3C9B1A1AACFA57");

                entity.ToTable("tUsers");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.CodRol).HasColumnName("cod_rol");

                entity.Property(e => e.NroDoc)
                    .HasColumnName("nro_doc")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SnActivo).HasColumnName("sn_activo");

                entity.Property(e => e.TxtApellido)
                    .HasColumnName("txt_apellido")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TxtNombre)
                    .HasColumnName("txt_nombre")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TxtPassword)
                    .HasColumnName("txt_password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TxtUser)
                    .HasColumnName("txt_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodRolNavigation)
                    .WithMany(p => p.TUsers)
                    .HasForeignKey(d => d.CodRol)
                    .HasConstraintName("fk_user_rol");
            });

            modelBuilder.Entity<TVenta>(entity =>
            {
                entity.HasKey(e => e.CodVenta)
                    .HasName("PK__tVenta__27326095CFD14AF0");

                entity.ToTable("tVenta");

                entity.Property(e => e.CodVenta).HasColumnName("cod_venta");

                entity.Property(e => e.CodPelicula).HasColumnName("cod_pelicula");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.CodPeliculaNavigation)
                    .WithMany(p => p.TVenta)
                    .HasForeignKey(d => d.CodPelicula)
                    .HasConstraintName("fk_cod_peliculaVenta");

                entity.HasOne(d => d.CodUsuarioNavigation)
                    .WithMany(p => p.TVenta)
                    .HasForeignKey(d => d.CodUsuario)
                    .HasConstraintName("fk_cod_usuarioVenta");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
