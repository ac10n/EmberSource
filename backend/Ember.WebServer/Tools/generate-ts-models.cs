using System.Reflection;
using System.Text;

public class TypeScriptModelGenerator
{
    public void GenerateTypeScriptModels()
    {
        string[] inputTypeNamespaces = [
            "Ember.WebServer.Models",
            "Ember.WebServer.Areas.Knowledge.Models",
            "Ember.WebServer.Areas.People.Models",
        ];

        List<Type> types = LoadTypes(inputTypeNamespaces, typeof(TypeScriptModelGenerator).Assembly);

        var generatedModels = new Dictionary<Type, StringBuilder>();
        foreach (var type in types)
        {
            GenerateType(type, generatedModels);
        }

        var outputDir = Path.GetFullPath("../../frontend/EmberUI/src/app/models");
        Directory.CreateDirectory(outputDir);
        using var writer = new StreamWriter(Path.Combine(outputDir, "contract-models.ts"));
        foreach (var sb in generatedModels.Values)
        {
            writer.Write(sb.ToString());
        }
    }

    void GenerateType(Type type, Dictionary<Type, StringBuilder> generatedModels)
    {
        if (generatedModels.ContainsKey(type))
        {
            return;
        }

        var sb = new StringBuilder();
        generatedModels.Add(type, sb);

        if (type.IsEnum)
        {
            sb.AppendLine($"export enum {type.Name} {{");
            foreach (var name in Enum.GetNames(type))
            {
                var value = Convert.ToInt32(Enum.Parse(type, name));
                sb.AppendLine($"  {name} = {value},");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
        else if (type.IsClass)
        {
            var baseType = (type.BaseType != null && type.BaseType != typeof(object)) ? $" {type.BaseType.Name} &" : "";
            sb.AppendLine($"export type {type.Name} = {baseType} {{");
            foreach (var prop in type.GetProperties())
            {
                var propType = GetTypeScriptType(prop.PropertyType, generatedModels);
                var optionalMark = IsNullable(prop) ? "?" : "";
                sb.AppendLine($"  {ToCamelCase(prop.Name)}{optionalMark}: {propType};");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }
    }

    string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
        {
            return name;
        }
        return char.ToLower(name[0]) + name.Substring(1);
    }

    bool IsNullable(PropertyInfo prop)
    {
        if (!prop.PropertyType.IsValueType)
        {
            return true;
        }
        if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
        {
            return true;
        }
        return false;
    }

    string GetTypeScriptType(Type type, Dictionary<Type, StringBuilder> generatedModels)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return GetTypeScriptType(type.GetGenericArguments()[0], generatedModels) + " | null | undefined";
        }
        else if (type == typeof(string))
        {
            return "string";
        }
        else if (type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return "number";
        }
        else if (type == typeof(bool))
        {
            return "boolean";
        }
        else if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
        {
            return "string";
        }
        else if (type == typeof(Guid))
        {
            return "string";
        }
        else if (type.IsEnum)
        {
            GenerateType(type, generatedModels);
            return type.Name;
        }
        else if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(Array)))
        {
            var itemType = type.GetGenericArguments()[0];
            var tsItemType = GetTypeScriptType(itemType, generatedModels);
            return tsItemType.Contains(" ") ? $"({tsItemType})[]" : $"{tsItemType}[]";
        }
        else if (type.IsClass)
        {
            GenerateType(type, generatedModels);
            return type.Name;
        }
        else
        {
            return "any"; // Fallback for unsupported types
        }
    }

    static List<Type> LoadTypes(string[] inputTypeNamespaces, Assembly assembly)
    {
        try
        {
            return assembly.GetTypes()
              .Where(t => inputTypeNamespaces.Any(ns => (t.Namespace ?? "").StartsWith(ns)))
              .ToList();
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.WriteLine(ex.Message);

            foreach (var le in ex.LoaderExceptions)
            {
                Console.WriteLine("---- Loader exception ----");
                Console.WriteLine(le?.GetType().FullName);
                Console.WriteLine(le?.Message);

                // If it's a FileNotFoundException, the FusionLog can be useful on some runtimes
                if (le is FileNotFoundException fnf)
                    Console.WriteLine("FileName: " + fnf.FileName);

                // TypeLoadException often includes the missing type/assembly in Message
            }

            // ex.Types is parallel to the attempted load: some entries are null
            var loaded = ex.Types.Count(t => t != null);
            var total = ex.Types.Length;
            Console.WriteLine($"Loaded {loaded}/{total} types.");
            throw;
        }
    }
}