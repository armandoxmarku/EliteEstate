﻿// <auto-generated />
using System;
using EliteEstate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EliteEstate.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20240226204808_migrimi")]
    partial class migrimi
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EliteEstate.Models.Agent", b =>
                {
                    b.Property<int>("AgentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("AgentId");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("EliteEstate.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EliteEstate.Models.Favorite", b =>
                {
                    b.Property<int>("FavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteId");

                    b.HasIndex("PropertyId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("EliteEstate.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AgentId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("PropertyId")
                        .HasColumnType("int");

                    b.HasKey("ImageId");

                    b.HasIndex("AgentId");

                    b.HasIndex("PropertyId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("EliteEstate.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AgentId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("AgentId");

                    b.HasIndex("PropertyId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("EliteEstate.Models.Property", b =>
                {
                    b.Property<int>("PropertyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AgentId")
                        .HasColumnType("int");

                    b.Property<int>("Bathrooms")
                        .HasColumnType("int");

                    b.Property<int>("Bedrooms")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Garages")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Garden")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Pool")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("PropertyTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<int>("Squaremetres")
                        .HasColumnType("int");

                    b.HasKey("PropertyId");

                    b.HasIndex("AgentId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("EliteEstate.Models.PropertyType", b =>
                {
                    b.Property<int>("PropertyTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("TypeOfproperty")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PropertyTypeId");

                    b.ToTable("PropertyTypes");
                });

            modelBuilder.Entity("EliteEstate.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EliteEstate.Models.Favorite", b =>
                {
                    b.HasOne("EliteEstate.Models.Property", "Property")
                        .WithMany("UserFavorites")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EliteEstate.Models.User", "User")
                        .WithMany("UserFavorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EliteEstate.Models.Image", b =>
                {
                    b.HasOne("EliteEstate.Models.Agent", "Agent")
                        .WithMany("Images")
                        .HasForeignKey("AgentId");

                    b.HasOne("EliteEstate.Models.Property", "Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId");

                    b.Navigation("Agent");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("EliteEstate.Models.Message", b =>
                {
                    b.HasOne("EliteEstate.Models.Agent", "Receiver")
                        .WithMany()
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EliteEstate.Models.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EliteEstate.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("EliteEstate.Models.Property", b =>
                {
                    b.HasOne("EliteEstate.Models.Agent", "Agent")
                        .WithMany("Properties")
                        .HasForeignKey("AgentId");

                    b.HasOne("EliteEstate.Models.Category", "Category")
                        .WithMany("Properties")
                        .HasForeignKey("CategoryId");

                    b.HasOne("EliteEstate.Models.PropertyType", "PropertyType")
                        .WithMany("Properties")
                        .HasForeignKey("PropertyTypeId");

                    b.Navigation("Agent");

                    b.Navigation("Category");

                    b.Navigation("PropertyType");
                });

            modelBuilder.Entity("EliteEstate.Models.Agent", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("EliteEstate.Models.Category", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("EliteEstate.Models.Property", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("UserFavorites");
                });

            modelBuilder.Entity("EliteEstate.Models.PropertyType", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("EliteEstate.Models.User", b =>
                {
                    b.Navigation("UserFavorites");
                });
#pragma warning restore 612, 618
        }
    }
}
