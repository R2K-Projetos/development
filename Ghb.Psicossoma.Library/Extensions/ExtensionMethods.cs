using System.Text;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Ghb.Psicossoma.Library.Extensions;

public static partial class ExtensionMethods
{
    [GeneratedRegex("\\p{Mn}", RegexOptions.Compiled)]
    private static partial Regex NonSpacingMarkRegex();

    public static string GetErrorMessage(this Exception ex)
    {
        string result = ex.Message;
        Exception? innerException = ex.InnerException;
        int index = 0;

        while (innerException is not null)
        {
            if (index == 0)
                result = innerException.Message;
            else
                result += $"{Environment.NewLine}{"".PadRight(index, '-')}>{innerException.Message}";

            innerException = innerException.InnerException;
            index += 3;
        }

        return result;
    }

    /// <summary>
    /// Verifica se o double possui conteúdo válido, ou seja, não é NaN nem Infinity
    /// </summary>
    /// <param name="value">O valor para verificação</param>
    /// <returns></returns>
    public static bool HasValidContent(this double value)
    {
        return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    public static double? ConvertToDouble(this string text, string culture = "PT-br")
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text)) return null;

        if (double.TryParse(text, NumberStyles.Number, CultureInfo.GetCultureInfo(culture), out double result))
            return result;
        else
            return null;
    }

    public static DateTime? ConvertToDateTime(this string str)
    {
        if (string.IsNullOrWhiteSpace(str) || string.IsNullOrEmpty(str)) return null;

        DateTime? result = null;

        if (DateTime.TryParseExact(str, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dateResult))
        {
            result = new DateTime(dateResult.Year, dateResult.Month, dateResult.Day, 0, 0, 0, DateTimeKind.Utc);
        }
        else
        {
            if (DateTime.TryParse(str, out dateResult))
            {
                result = new DateTime(dateResult.Year, dateResult.Month, dateResult.Day, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        return result;
    }

    public static bool IsStrengthPassword(this string value, string pattern)
    {
        Regex regex = new(pattern);
        Match match = regex.Match(value);

        return match.Success;
    }

    public static string ReplaceDiacritics(this string value, bool useLiteralChars)
    {
        string result;
        string regexFileNameValidation = "[^a-zA-Z0-9]";

        if (useLiteralChars)
        {
            Regex nonSpacingMarkRegex = NonSpacingMarkRegex();
            Regex regexFileValidation = new(regexFileNameValidation);

            var normalizedText = value.Normalize(NormalizationForm.FormD);
            string noDiacritics = nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
            result = regexFileValidation.Replace(noDiacritics, "_");
        }
        else
        {
            Regex regexFileValidation = new(regexFileNameValidation);
            result = regexFileValidation.Replace(value, "_");
        }

        return result;
    }

    public static string GetEnumDescription<T>(this T enumValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) return "SEM DESCRIÇÃO";

        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString()!);

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (attrs != null && attrs.Length > 0) description = ((DescriptionAttribute)attrs[0]).Description;
        }

        return description ?? "SEM DESCRIÇÃO";
    }

    public static List<EnumerationItem> GetEnumListValues(this Type enumObject)
    {
        List<EnumerationItem> result = new();

        if (!enumObject.IsEnum) return result;

        FieldInfo[] fieldInfos = enumObject.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        foreach (var enumItem in fieldInfos)
        {
            string description = enumItem.ToString()!;
            var enumValue = enumItem.GetRawConstantValue();
            var descriptionInfo = enumItem.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (descriptionInfo is not null && descriptionInfo.Length > 0) description = ((DescriptionAttribute)descriptionInfo[0]).Description;

            result.Add(new EnumerationItem
            {
                Id = (int)enumValue!,
                Name = enumItem.ToString()!,
                Description = description
            });
        }

        return result;
    }

    public static string SetFirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} é nula", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

    public static string GetDefinitionFromBool(this bool value)
    {
        return value ? "True" : "False";
    }

    public static void SetValue<T>(this T sender, string propertyName, object? value)
    {
        if (sender is not null)
        {
            var propertyInfo = sender.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo is null) return;

            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            if (propertyInfo.PropertyType.IsEnum)
            {
                if (value is not null)
                    propertyInfo.SetValue(sender, Enum.Parse(propertyInfo.PropertyType, value.ToString()!));
            }
            else if (propertyInfo.PropertyType.IsArray)
            {
                if (value is not null)
                    propertyInfo.SetValue(sender, new Dictionary<string, object>[] { (Dictionary<string, object>)value }, null);
            }
            else
            {
                var safeValue = value == null ? null : Convert.ChangeType(value, type);
                propertyInfo.SetValue(sender, safeValue, null);
            }
        }
    }

    public static object? GetPropValue(this object? sender, string name)
    {
        foreach (var part in name.Split('.'))
        {
            if (sender is null) return null;

            Type type = sender.GetType();
            PropertyInfo? info = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (info is null) return null;

            sender = info.GetValue(sender, null);
        }

        return sender;
    }

    public static T? GetPropValue<T>(this object sender, string name)
    {
        var result = sender.GetPropValue(name);

        if (result is null) { return default; }

        return (T)result;
    }

    public static string GetPropertyName<T, TReturn>(this Expression<Func<T, TReturn>> expression)
    {
        var body = (MemberExpression)expression.Body;
        return body.Member.Name;
    }

    public static DateTime FirstDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    public static int DaysInMonth(this DateTime value)
    {
        return DateTime.DaysInMonth(value.Year, value.Month);
    }

    public static DateTime LastDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.DaysInMonth());
    }
}

public class EnumerationItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
