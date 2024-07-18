using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.FluentConfigurations.MBAFluent
{
    /// <summary>
    /// Defines the EntityFramework configuration for the <see cref="MbaOptions"/> entity.
    /// </summary>
    internal class MBAOptionsFluentConfiguration : IEntityTypeConfiguration<MbaOptions>
    {
        /// <summary>
        /// Configures the properties of the <see cref="MbaOptions"/> entity.
        /// </summary>
        /// <param name="builder">Provides a simple API surface for configuring an <see cref="MbaOptions"/> entity.</param>
        public void Configure(EntityTypeBuilder<MbaOptions> builder)
        {
            // Define the table name in the database.
            builder.ToTable("MBA_OPTIONS");

            // Define the Country property.
            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(256);

            // Define the CountryCode property.
            builder.Property(x => x.CountryCode)
                .IsRequired()
                .HasMaxLength(16);

            // Define the relationship between MbaOptions and Mba entities.
            builder.HasMany(x => x.Mbas).WithOne(v => v.MbaOptions).HasForeignKey(x => x.MbaOptionsId);
        }
    }

    /// <summary>
    /// Defines the EntityFramework configuration for the <see cref="Mba"/> entity.
    /// </summary>
    internal class MBAFluentConfiguration : IEntityTypeConfiguration<Mba>
    {
        /// <summary>
        /// Configures the properties of the <see cref="Mba"/> entity.
        /// </summary>
        /// <param name="builder">Provides a simple API surface for configuring an <see cref="Mba"/> entity.</param>
        public void Configure(EntityTypeBuilder<Mba> builder)
        {
            // Define the table name in the database.
            builder.ToTable("MBA");

            // Define the Name property.
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Define the Code property.
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(16);
        }
    }

}
