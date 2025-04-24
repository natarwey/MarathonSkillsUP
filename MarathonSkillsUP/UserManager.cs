using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonSkillsUP
{
    public static class UserManager
    {
        public enum UserRole
        {
            Runner,
            Coordinator,
            Administrator
        }

        public static string CurrentUserEmail { get; private set; }
        public static UserRole CurrentUserRole { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        public static bool Login(string email, string password)
        {
            // В реальном приложении здесь была бы проверка в базе данных
            // Для демонстрации просто возвращаем true
            CurrentUserEmail = email;
            CurrentUserRole = UserRole.Runner; // По умолчанию
            IsLoggedIn = true;
            return true;
        }

        public static void Logout()
        {
            CurrentUserEmail = null;
            IsLoggedIn = false;
        }

        // Для тестирования
        public static bool TestLogin(UserRole role)
        {
            CurrentUserRole = role;
            CurrentUserEmail = "test@example.com";
            IsLoggedIn = true;
            return true;
        }
    }
}
