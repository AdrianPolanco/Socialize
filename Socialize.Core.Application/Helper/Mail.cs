

namespace Socialize.Core.Application.Helper
{
    public static class Mail
    {
        public static string GenerateMailTemplate(string name, string message, string callbackUrl)
        {
            return @$"
                    <section class=""max-w-2xl px-6 py-8 mx-auto bg-white dark:bg-gray-900"">
                        <main class=""mt-8"">
                            <h2 class=""text-gray-700 dark:text-gray-200"">Hi {name},</h2>

                            <p class=""mt-2 leading-loose text-gray-600 dark:text-gray-300"">
                                We’re glad to have you onboard! {message}.
        
                            <p class=""mt-2 text-gray-600 dark:text-gray-300"">
                                Thanks, <br>
                                Socialize team
                            </p>
        
                            <a href=""{callbackUrl}""
                                     class=""px-6 py-2 mt-8 text-sm font-medium tracking-wider text-white capitalize transition-colors duration-300 transform 
                                        bg-red-600 rounded-lg hover:bg-red-500 focus:outline-none focus:ring focus:ring-red-300 focus:ring-opacity-80"">
                                Confirm account
                            </a>
                        </main>
                    </section>.";
        }

        public static string GenerateResetPasswordMailTemplate(string name, string message)
        {
            return @$"Hi {name}, your new password is: {message}";
        }
    }
}
