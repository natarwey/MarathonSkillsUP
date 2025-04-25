using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonSkillsUP
{
    public class UserManager
    {
        private static UserManager instance;
        private int currentUserId;
        private string currentUserRole;
        private string currentUserEmail;
        private string currentUserName;

        // Singleton паттерн
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserManager();
                return instance;
            }
        }

        private UserManager()
        {
            currentUserId = -1;
            currentUserRole = null;
            currentUserEmail = null;
            currentUserName = null;
        }

        // Свойства для доступа к информации о текущем пользователе
        public int CurrentUserId => currentUserId;
        public string CurrentUserRole => currentUserRole;
        public string CurrentUserEmail => currentUserEmail;
        public string CurrentUserName => currentUserName;
        public bool IsAuthenticated => currentUserId != -1;

        // Метод для аутентификации пользователя
        public bool AuthenticateUser(string email, string password)
        {
            string role;
            if (DatabaseManager.Instance.AuthenticateUser(email, password, out role))
            {
                // Получаем информацию о пользователе
                string query = "SELECT UserId, FirstName, LastName FROM [User] WHERE Email = @Email";
                var parameters = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "@Email", email }
                };

                DataTable userInfo = DatabaseManager.Instance.ExecuteQuery(query, parameters);
                if (userInfo != null && userInfo.Rows.Count > 0)
                {
                    currentUserId = Convert.ToInt32(userInfo.Rows[0]["UserId"]);
                    currentUserRole = role;
                    currentUserEmail = email;
                    currentUserName = $"{userInfo.Rows[0]["FirstName"]} {userInfo.Rows[0]["LastName"]}";
                    return true;
                }
            }
            return false;
        }

        // Метод для выхода пользователя
        public void LogoutUser()
        {
            currentUserId = -1;
            currentUserRole = null;
            currentUserEmail = null;
            currentUserName = null;
        }

        // Получение ID бегуна для текущего пользователя (если он бегун)
        public int GetCurrentRunnerId()
        {
            if (!IsAuthenticated || currentUserRole != "Runner")
                return -1;

            string query = "SELECT RunnerId FROM Runner WHERE UserId = @UserId";
            var parameters = new System.Collections.Generic.Dictionary<string, object>
            {
                { "@UserId", currentUserId }
            };

            object result = DatabaseManager.Instance.ExecuteScalar(query, parameters);
            if (result != null)
                return Convert.ToInt32(result);

            return -1;
        }

        // Проверка, является ли текущий пользователь бегуном
        public bool IsRunner()
        {
            return IsAuthenticated && currentUserRole == "Runner";
        }

        // Проверка, является ли текущий пользователь координатором
        public bool IsCoordinator()
        {
            return IsAuthenticated && currentUserRole == "Coordinator";
        }

        // Проверка, является ли текущий пользователь администратором
        public bool IsAdministrator()
        {
            return IsAuthenticated && currentUserRole == "Administrator";
        }
    }
}
