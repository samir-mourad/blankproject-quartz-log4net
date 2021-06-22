using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace BlankProject.Infra.Data.Context
{
    public static class ContextExtensions
    {
        public static void DisableDeleteCascade(this ModelBuilder m)
        {
            var foreignKeys = m.Model
                               .GetEntityTypes()
                               .SelectMany(i => i.GetForeignKeys())
                               .Where(fk => !fk.IsOwnership
                                         && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var f in foreignKeys)
                f.DeleteBehavior = DeleteBehavior.Restrict;
        }

        public static void LoadMappings(this ModelBuilder m)
        {
            var typesToMap = Assembly.GetExecutingAssembly()
                                     .GetTypes()
                                     .Where(i => i.IsClass
                                              && i.Namespace == "Namespace.Name.Class" //TODO: resolve this
                                              && !i.IsSealed)
                                     .ToArray();

            foreach (var t in typesToMap)
            {
                dynamic mappingClass = Activator.CreateInstance(t);
                m.ApplyConfiguration(mappingClass);
            }
        }

        public static void SetStringToVarchar(this ModelBuilder m)
        {
            var props = m.Model.GetEntityTypes()
                               .SelectMany(i => i.GetProperties())
                               .Where(i => i.ClrType == typeof(string))
                               .Select(i => i);

            foreach (var prop in props)
            {
                var p = m.Entity(prop.DeclaringEntityType.ClrType).Property(prop.Name);
                var type = prop.GetMaxLength() > 0 ? $"varchar({prop.GetMaxLength()})" : "text";

                p.HasColumnType(type);
            }
        }

        public static void SetDateTime(this ModelBuilder m)
        {
            var props = m.Model.GetEntityTypes()
                               .SelectMany(i => i.GetProperties())
                               .Where(i => i.ClrType == typeof(DateTime)
                                        || i.ClrType == typeof(DateTime?))
                               .Select(i => i);

            foreach (var prop in props)
            {
                var p = m.Entity(prop.DeclaringEntityType.ClrType).Property(prop.Name);
                p.HasColumnType("datetime");
            }
        }
    }
}
