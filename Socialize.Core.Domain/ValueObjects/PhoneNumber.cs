using Socialize.Core.Domain.ValueObjects.Base;
using System.Text.RegularExpressions;

public class PhoneNumber: ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El numero de telefono no puede estar vacío.", nameof(value));
        }

        if (!IsValidPhoneNumber(value))
        {
            throw new ArgumentException("Formato de numero de telefono invalido.", nameof(value));
        }

        Value = NormalizePhoneNumber(value);
    }

    public static PhoneNumber Create(string value)
    {
        return new PhoneNumber(value);
    }

    private bool IsValidPhoneNumber(string value)
    {
        // Definir los patrones de número de teléfono válidos para República Dominicana
        var patterns = new[]
        {
            @"^\+1\s?(\(?829\)?|\(?849\)?|\(?809\)?)\s?\d{3}\s?\d{4}$",  // +1 (829) XXX XXXX, +1 (849) XXX XXXX, +1 (809) XXX XXXX
            @"^\+1\s?(829|849|809)\s?\d{3}\s?\d{4}$",                   // +1 829 XXX XXXX, +1 849 XXX XXXX, +1 809 XXX XXXX
            @"^\+1(829|849|809)\d{7}$",                                 // +1829XXXXXXX, +1849XXXXXXX, +1809XXXXXXX
            @"^1(829|849|809)\d{7}$",                                   // 1829XXXXXXX, 1849XXXXXXX, 1809XXXXXXX
            @"^(829|849|809)\d{7}$"                                     // 829XXXXXXX, 849XXXXXXX, 809XXXXXXX
        };

        return patterns.Any(pattern => Regex.IsMatch(value, pattern));
    }

    private string NormalizePhoneNumber(string value)
    {
        // Eliminar espacios, paréntesis y guiones
        return Regex.Replace(value, @"[\s\(\)-]", string.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
