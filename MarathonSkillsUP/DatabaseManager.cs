using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MarathonSkillsUP
{
    public class DatabaseManager
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MarathonSkillsConnection"].ConnectionString;
        private static DatabaseManager instance;

        // Singleton паттерн для доступа к менеджеру БД
        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DatabaseManager();
                return instance;
            }
        }

        private DatabaseManager() { }

        // Проверка подключения к базе данных
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Выполнение запроса без возврата данных (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        // Выполнение запроса с возвратом скалярного значения
        public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Выполнение запроса с возвратом DataTable
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Методы для работы с пользователями
        public bool AuthenticateUser(string email, string password, out string role)
        {
            role = null;
            string query = @"
                SELECT Role.RoleName 
                FROM [User]
                JOIN Role ON [User].RoleId = Role.RoleId
                WHERE Email = @Email AND Password = @Password";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@Password", password }
            };

            object result = ExecuteScalar(query, parameters);
            if (result != null)
            {
                role = result.ToString();
                return true;
            }
            return false;
        }

        // Получение списка стран
        public DataTable GetCountries()
        {
            string query = "SELECT * FROM Country ORDER BY CountryName";
            return ExecuteQuery(query);
        }

        // Получение списка полов
        public DataTable GetGenders()
        {
            string query = "SELECT * FROM Gender";
            return ExecuteQuery(query);
        }

        // Регистрация нового бегуна
        public bool RegisterRunner(string email, string password, string firstName, string lastName,
            string gender, DateTime dateOfBirth, string countryCode, byte[] photo = null)
        {
            // Проверка возраста
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;

            if (age < 10)
            {
                MessageBox.Show("Бегун должен быть не моложе 10 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Транзакция для создания пользователя и бегуна
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Получаем ID роли "Бегун"
                        int runnerRoleId;
                        using (SqlCommand roleCommand = new SqlCommand(
                            "SELECT RoleId FROM Role WHERE RoleName = 'Runner'", connection, transaction))
                        {
                            object roleResult = roleCommand.ExecuteScalar();
                            if (roleResult == null)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Роль 'Бегун' не найдена в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                            runnerRoleId = Convert.ToInt32(roleResult);
                        }

                        // Создаем пользователя
                        using (SqlCommand userCommand = new SqlCommand(
                            "INSERT INTO [User] (Email, Password, FirstName, LastName, RoleId) " +
                            "VALUES (@Email, @Password, @FirstName, @LastName, @RoleId); " +
                            "SELECT SCOPE_IDENTITY()", connection, transaction))
                        {
                            userCommand.Parameters.AddWithValue("@Email", email);
                            userCommand.Parameters.AddWithValue("@Password", password);
                            userCommand.Parameters.AddWithValue("@FirstName", firstName);
                            userCommand.Parameters.AddWithValue("@LastName", lastName);
                            userCommand.Parameters.AddWithValue("@RoleId", runnerRoleId);

                            object userIdResult = userCommand.ExecuteScalar();
                            if (userIdResult == null)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Ошибка при создании пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                            int userId = Convert.ToInt32(userIdResult);

                            // Создаем запись бегуна
                            using (SqlCommand runnerCommand = new SqlCommand(
                                "INSERT INTO Runner (UserId, Gender, DateOfBirth, CountryCode, Photo) " +
                                "VALUES (@UserId, @Gender, @DateOfBirth, @CountryCode, @Photo)", connection, transaction))
                            {
                                runnerCommand.Parameters.AddWithValue("@UserId", userId);
                                runnerCommand.Parameters.AddWithValue("@Gender", gender);
                                runnerCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                                runnerCommand.Parameters.AddWithValue("@CountryCode", countryCode);

                                if (photo != null)
                                    runnerCommand.Parameters.AddWithValue("@Photo", photo);
                                else
                                    runnerCommand.Parameters.AddWithValue("@Photo", DBNull.Value);

                                runnerCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при регистрации бегуна: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Получение списка марафонов
        public DataTable GetMarathons()
        {
            string query = "SELECT * FROM Event WHERE EventTypeId = 1 ORDER BY EventName";
            return ExecuteQuery(query);
        }

        // Получение списка благотворительных организаций
        public DataTable GetCharities()
        {
            string query = "SELECT * FROM Charity ORDER BY CharityName";
            return ExecuteQuery(query);
        }

        // Регистрация бегуна на марафон
        public bool RegisterRunnerForMarathon(int runnerId, int eventId, int charityId, decimal sponsorshipTarget,
            bool needKit, bool needBracelet, bool needTShirt, bool needCap, bool needWaterBottle)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Создаем запись о регистрации
                        using (SqlCommand command = new SqlCommand(
                            "INSERT INTO Registration (RunnerId, EventId, CharityId, SponsorshipTarget, " +
                            "RegistrationDateTime, RaceKitOptionId) " +
                            "VALUES (@RunnerId, @EventId, @CharityId, @SponsorshipTarget, @RegistrationDateTime, @RaceKitOptionId); " +
                            "SELECT SCOPE_IDENTITY()", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@RunnerId", runnerId);
                            command.Parameters.AddWithValue("@EventId", eventId);
                            command.Parameters.AddWithValue("@CharityId", charityId);
                            command.Parameters.AddWithValue("@SponsorshipTarget", sponsorshipTarget);
                            command.Parameters.AddWithValue("@RegistrationDateTime", DateTime.Now);

                            // Определяем тип комплекта
                            int raceKitOptionId = 1; // По умолчанию - базовый комплект
                            if (needTShirt && needCap && needWaterBottle)
                                raceKitOptionId = 3; // Полный комплект
                            else if (needTShirt)
                                raceKitOptionId = 2; // Средний комплект

                            command.Parameters.AddWithValue("@RaceKitOptionId", raceKitOptionId);

                            object registrationIdResult = command.ExecuteScalar();
                            if (registrationIdResult == null)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Ошибка при регистрации на марафон.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при регистрации на марафон: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Получение результатов бегуна
        public DataTable GetRunnerResults(int runnerId)
        {
            string query = @"
                SELECT 
                    M.MarathonName,
                    E.EventName,
                    R.RaceTime,
                    (SELECT COUNT(*) + 1 FROM RegistrationEvent RE2 
                     WHERE RE2.EventId = RE.EventId AND RE2.RaceTime < RE.RaceTime) AS OverallPosition,
                    (SELECT COUNT(*) + 1 FROM RegistrationEvent RE3 
                     JOIN Runner RN ON RE3.RunnerId = RN.RunnerId
                     WHERE RE3.EventId = RE.EventId 
                     AND RN.Gender = (SELECT Gender FROM Runner WHERE RunnerId = @RunnerId)
                     AND DATEDIFF(YEAR, RN.DateOfBirth, GETDATE()) BETWEEN 
                        (SELECT CASE 
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) < 18 THEN 0
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 18 AND 29 THEN 18
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 30 AND 39 THEN 30
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 40 AND 55 THEN 40
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 56 AND 70 THEN 56
                            ELSE 70 END FROM Runner WHERE RunnerId = @RunnerId) 
                        AND 
                        (SELECT CASE 
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) < 18 THEN 17
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 18 AND 29 THEN 29
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 30 AND 39 THEN 39
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 40 AND 55 THEN 55
                            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 56 AND 70 THEN 70
                            ELSE 150 END FROM Runner WHERE RunnerId = @RunnerId)
                     AND RE3.RaceTime < RE.RaceTime) AS CategoryPosition
                FROM RegistrationEvent RE
                JOIN Event E ON RE.EventId = E.EventId
                JOIN Marathon M ON E.MarathonId = M.MarathonId
                WHERE RE.RunnerId = @RunnerId
                ORDER BY RE.RaceTime";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@RunnerId", runnerId }
            };

            return ExecuteQuery(query, parameters);
        }

        // Получение информации о бегуне
        public DataTable GetRunnerInfo(int runnerId)
        {
            string query = @"
                SELECT 
                    U.FirstName,
                    U.LastName,
                    R.Gender,
                    R.DateOfBirth,
                    C.CountryName,
                    R.Photo
                FROM Runner R
                JOIN [User] U ON R.UserId = U.UserId
                JOIN Country C ON R.CountryCode = C.CountryCode
                WHERE R.RunnerId = @RunnerId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@RunnerId", runnerId }
            };

            return ExecuteQuery(query, parameters);
        }

        // Обновление профиля бегуна
        public bool UpdateRunnerProfile(int runnerId, string firstName, string lastName, string gender,
            DateTime dateOfBirth, string countryCode, string password = null, byte[] photo = null)
        {
            // Проверка возраста
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;

            if (age < 10)
            {
                MessageBox.Show("Бегун должен быть не моложе 10 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Получаем UserId бегуна
                        int userId;
                        using (SqlCommand userIdCommand = new SqlCommand(
                            "SELECT UserId FROM Runner WHERE RunnerId = @RunnerId", connection, transaction))
                        {
                            userIdCommand.Parameters.AddWithValue("@RunnerId", runnerId);
                            object userIdResult = userIdCommand.ExecuteScalar();
                            if (userIdResult == null)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Бегун не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                            userId = Convert.ToInt32(userIdResult);
                        }

                        // Обновляем данные пользователя
                        string userUpdateQuery = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName";
                        if (!string.IsNullOrEmpty(password))
                            userUpdateQuery += ", Password = @Password";
                        userUpdateQuery += " WHERE UserId = @UserId";

                        using (SqlCommand userCommand = new SqlCommand(userUpdateQuery, connection, transaction))
                        {
                            userCommand.Parameters.AddWithValue("@FirstName", firstName);
                            userCommand.Parameters.AddWithValue("@LastName", lastName);
                            userCommand.Parameters.AddWithValue("@UserId", userId);
                            if (!string.IsNullOrEmpty(password))
                                userCommand.Parameters.AddWithValue("@Password", password);

                            userCommand.ExecuteNonQuery();
                        }

                        // Обновляем данные бегуна
                        string runnerUpdateQuery = "UPDATE Runner SET Gender = @Gender, DateOfBirth = @DateOfBirth, CountryCode = @CountryCode";
                        if (photo != null)
                            runnerUpdateQuery += ", Photo = @Photo";
                        runnerUpdateQuery += " WHERE RunnerId = @RunnerId";

                        using (SqlCommand runnerCommand = new SqlCommand(runnerUpdateQuery, connection, transaction))
                        {
                            runnerCommand.Parameters.AddWithValue("@Gender", gender);
                            runnerCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                            runnerCommand.Parameters.AddWithValue("@CountryCode", countryCode);
                            runnerCommand.Parameters.AddWithValue("@RunnerId", runnerId);
                            if (photo != null)
                                runnerCommand.Parameters.AddWithValue("@Photo", photo);

                            runnerCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при обновлении профиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Получение данных для формы "Насколько долгий марафон"
        public DataTable GetSpeedItems()
        {
            string query = "SELECT * FROM SpeedItem ORDER BY Name";
            return ExecuteQuery(query);
        }

        public DataTable GetDistanceItems()
        {
            string query = "SELECT * FROM DistanceItem ORDER BY Name";
            return ExecuteQuery(query);
        }

        // Получение данных об инвентаре
        public DataTable GetInventory()
        {
            string query = @"
                SELECT 
                    I.InventoryId,
                    I.InventoryName,
                    I.InventoryDescription,
                    I.InventoryCount,
                    (SELECT COUNT(*) FROM Registration R 
                     JOIN RaceKitOption RKO ON R.RaceKitOptionId = RKO.RaceKitOptionId
                     WHERE RKO.NeedsBracelet = 1) AS TypeACount,
                    (SELECT COUNT(*) FROM Registration R 
                     JOIN RaceKitOption RKO ON R.RaceKitOptionId = RKO.RaceKitOptionId
                     WHERE RKO.NeedsTShirt = 1) AS TypeBCount,
                    (SELECT COUNT(*) FROM Registration R 
                     JOIN RaceKitOption RKO ON R.RaceKitOptionId = RKO.RaceKitOptionId
                     WHERE RKO.NeedsWaterBottle = 1) AS TypeCCount,
                    (SELECT COUNT(*) FROM Registration) AS TotalRegistrations
                FROM Inventory I";
            return ExecuteQuery(query);
        }

        // Обновление инвентаря
        public bool UpdateInventory(int inventoryId, int count)
        {
            string query = "UPDATE Inventory SET InventoryCount = InventoryCount + @Count WHERE InventoryId = @InventoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@InventoryId", inventoryId },
                { "@Count", count }
            };

            int result = ExecuteNonQuery(query, parameters);
            return result > 0;
        }
    }
}
