namespace MsClean.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MsClean.Domain;

public class PersonConfiguration : EntityMapBase<Person>
{
    protected override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Person");
        builder.Property(b => b.Name).HasMaxLength(150).IsRequired();
        builder.Property(b => b.Email).HasMaxLength(150).IsRequired();
        builder.Property(b => b.LastName).HasMaxLength(150).IsRequired();
    }
}
