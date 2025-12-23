namespace Application.Constants;

public static class Messages
{
    public static class Auth
    {
        public const string FullNameRequired = "Полное имя обязательно";
        public const string FullNameTooShort = "Полное имя должно содержать минимум 3 символа";
        public const string EmailRequired = "Email обязателен";
        public const string EmailInvalid = "Неверный формат email";
        public const string PasswordRequired = "Пароль обязателен";
        public const string PasswordTooShort = "Пароль должен содержать минимум 6 символов";
        public const string UserAlreadyExists = "Пользователь с таким email уже существует";
        public const string RegistrationFailed = "Ошибка при регистрации";
        public const string InvalidCredentials = "Неверный email или пароль";
        public const string RefreshTokenNotFound = "Refresh token не найден";
        public const string RefreshTokenRevoked = "Refresh token был отозван";
        public const string RefreshTokenExpired = "Срок действия refresh token истек";
        public const string UserNotFound = "Пользователь не найден";
        public const string FileNotSelected = "Файл не выбран";
        public const string InvalidFileType = "Допустимы только изображения (jpg, jpeg, png, gif)";
        public const string FileTooLarge = "Размер файла не должен превышать 5 МБ";
        public const string ProfileUpdateFailed = "Ошибка обновления профиля";
    }

    public static class Marathon
    {
        public const string UserIdRequired = "UserId обязателен";
        public const string ScoreInvalid = "Баллы должны быть >= 0";
        public const string ResultNotFound = "Результат не найден";
    }
}
