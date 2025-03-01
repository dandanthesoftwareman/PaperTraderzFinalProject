﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Updatingto14.Models
{
    public partial class StonksDBContext : DbContext
    {
        public StonksDBContext()
        {
        }

        public StonksDBContext(DbContextOptions<StonksDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<InvestedStock> InvestedStocks { get; set; } = null!;
        public virtual DbSet<Stonk> Stonks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WatchingStock> WatchingStocks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer($"Data Source={Secret.server};Initial Catalog=StonksDB; User Id={Secret.userName}; Password={Secret.password};");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestedStock>(entity =>
            {
                entity.Property(e => e.InvestedTicker).HasMaxLength(30);

                entity.HasOne(d => d.InvestedTickerNavigation)
                    .WithMany(p => p.InvestedStocks)
                    .HasForeignKey(d => d.InvestedTicker)
                    .HasConstraintName("FK__InvestedS__Inves__73BA3083");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InvestedStocks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__InvestedS__UserI__74AE54BC");
            });

            modelBuilder.Entity<Stonk>(entity =>
            {
                entity.HasKey(e => e.Ticker)
                    .HasName("PK__STONKS__42AC12F1AFE12EBA");

                entity.ToTable("STONKS");

                entity.Property(e => e.Ticker).HasMaxLength(30);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.GoogleId)
                    .HasMaxLength(255)
                    .HasColumnName("GoogleID");

                entity.Property(e => e.ProfileName).HasMaxLength(255);

                entity.Property(e => e.UserIcon).HasMaxLength(1000);
            });

            modelBuilder.Entity<WatchingStock>(entity =>
            {
                entity.Property(e => e.WatchingTicker).HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WatchingStocks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__WatchingS__UserI__70DDC3D8");

                entity.HasOne(d => d.WatchingTickerNavigation)
                    .WithMany(p => p.WatchingStocks)
                    .HasForeignKey(d => d.WatchingTicker)
                    .HasConstraintName("FK__WatchingS__Watch__6FE99F9F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
