

using Socialize.Core.Domain.ValueObjects.Base;

namespace Socialize.Core.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; }
        public Email() { }
        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Las direcciones de correo electronico no pueden estar vacias.", nameof(value));
            }

            if (!IsValidEmail(value))
            {
                throw new ArgumentException("Formato de correo electronico inválido.", nameof(value));
            }

            Value = value.ToLower(); // Normalizamos a minúsculas
        }

        public static Email Create(string value)
        {
            return new Email(value);
        }

        private bool IsValidEmail(string value)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(value);
                return mailAddress.Address == value;
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

}
