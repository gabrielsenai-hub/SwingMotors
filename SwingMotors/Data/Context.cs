using AvaliacaoFinalWestn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoFinalWestn.Data
{
    public class Context : IdentityDbContext<Usuario>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ESSENCIAL: primeiro o Identity configura as tabelas dele
            base.OnModelCreating(modelBuilder);

            // Depois suas configs customizadas
            modelBuilder.Entity<CarroImagem>()
                .HasOne(p => p.Carro)
                .WithMany(c => c.Fotos)
                .HasForeignKey(p => p.CarroId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Endereco>()
                .HasOne(p => p.Usuario)
                .WithOne(n => n.Endereco)
                .HasForeignKey<Endereco>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<CarroImagem> CarroImagens { get; set; }
        public DbSet<CarroComprado> CarrosComprados { get; set; }
    }
}