﻿// <auto-generated />
using System;
using System.Collections.Generic;
using App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240302104257_RenameInstitutionIdColumn")]
    partial class RenameInstitutionIdColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("App.Domain.Institutions.Institution", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("base_url");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_enabled");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "App.Domain.Institutions.Institution.Name#InstitutionName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("EnValue")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name_en");

                            b1.Property<string>("PlValue")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name_pl");
                        });

                    b.HasKey("Id");

                    b.ToTable("institutions", (string)null);
                });

            modelBuilder.Entity("App.Domain.UserAccess.Authentication.AuthenticationSession", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("RequestToken", "App.Domain.UserAccess.Authentication.AuthenticationSession.RequestToken#RequestToken", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Secret")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("request_token_secret");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("request_token");
                        });

                    b.HasKey("Id");

                    b.ToTable("authentication_sessions", (string)null);
                });

            modelBuilder.Entity("App.Domain.UserAccess.Authentication.AuthenticationSession", b =>
                {
                    b.OwnsOne("App.Domain.UserAccess.Authentication.AccessToken", "AccessToken", b1 =>
                        {
                            b1.Property<Guid>("AuthenticationSessionId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Secret")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("access_token_secret");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("access_token");

                            b1.HasKey("AuthenticationSessionId");

                            b1.ToTable("authentication_sessions");

                            b1.WithOwner()
                                .HasForeignKey("AuthenticationSessionId");
                        });

                    b.Navigation("AccessToken");
                });
#pragma warning restore 612, 618
        }
    }
}
