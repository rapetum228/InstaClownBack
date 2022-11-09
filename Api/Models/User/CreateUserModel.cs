using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Введите имя.")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Посторонние символы. Используйте только буквы латинского алфавита или Кириллицу")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Email нельзя оставлять пустым.")]
        [EmailAddress(ErrorMessage = "Email введен некорректно!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле пароля нельзя оставлять пустым.")]
        [MinLength(8, ErrorMessage = "Пароль должен быть длиннее 8 символов.")]
        public string Password { get; set; }
        
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string RetryPassword { get; set; } 
        
        [Required(ErrorMessage = "Укажите дату рождения.")]
        public DateTimeOffset BirthDate { get; set; }

        public CreateUserModel(string name, string email, string password, string retryPassword, DateTimeOffset birthDate)
        {
            Name = name;
            Email = email;
            Password = password;
            RetryPassword = retryPassword;
            BirthDate = birthDate;
        }   
    }
}
