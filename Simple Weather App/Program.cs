using MySqlConnector;

namespace Simple_Weather_App
{
    public class Program
    {
        static bool loggedIn = false;
        static bool quit = false;
        //make sure to enter your database credentials!
        //run this sql code once:
        /*
        CREATE TABLE `users` (
          `userid` int NOT NULL AUTO_INCREMENT,
          `username` varchar(45) NOT NULL,
          `password` varchar(255) NOT NULL,
          `location` varchar(45) NOT NULL,
          PRIMARY KEY (`userid`),
          UNIQUE KEY `username_UNIQUE` (`username`));
        */
        static string connStr = $"Server=ND-COMPSCI;User ID=sly;Password={Password.getPassword()};Database=tl_sly_weather";
        static User user;
        static void Main(string[] args)
        {
            Menu();
        }
        static void Menu()
        {

            string username;
            string password;
            string location;
            while (!loggedIn && !quit)
            {
                Console.WriteLine("Do you want to:\n(1) Log In\n(2) Create New Account\n(3) Quit");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":

                        Console.WriteLine("Enter your username: ");
                        username = presenceCheck();
                        Console.WriteLine("Enter your password");
                        password = formatCheck();
                        logIn(username, password);
                        break;
                    case "2":
                        Console.WriteLine("Enter your username: ");
                        username = Console.ReadLine();
                        Console.WriteLine("Enter your password");
                        password = Console.ReadLine();
                        Console.WriteLine("Enter your location");
                        location = Console.ReadLine();
                        register(username, password, location);
                        break;
                    case "3":
                        quit = true;
                        break;
                }
            }
            if (loggedIn)
            {
                Console.WriteLine($"getting weather in {user._location}...");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(500);
                    Console.Write("#");
                }
                Console.WriteLine($"\nCould not get weather in {user._location}");
            }
        }
        public static void logIn(string username, string password)
        {           
            using var connection = new MySqlConnection(connStr);
            connection.Open();
            using var command = new MySqlCommand("SELECT userid, username, location FROM users WHERE username = @username AND password = SHA2(@password,256)", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                loggedIn = true;
                Console.WriteLine($"user {user._name} has logged in successfully!");
            }
            else
            {
                Console.WriteLine("Could not log in, would you like to try again? (Y/N)");
            }
            connection.Close();
        }
        public static void register(string username, string password, string location)
        {
            using var connection = new MySqlConnection(connStr);
            connection.Open();
            using var command = new MySqlCommand("INSERT INTO users (username, password, location) VALUES (@username, @password, @location)", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@location", location);
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"user {username} was created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"user {username} could not be created");
            }
        }
        public static string presenceCheck()
        {
            string input = "";
            input = Console.ReadLine();
            return input;
        }
        public static string formatCheck()
        {
            //password must be at least 8 characters
            string input = "";
            input = presenceCheck();
            return input;
        }
    }
    public class User
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public string _location { get; set; }
        public User(int id, string name, string location)
        {
            _id = id;
            _name = name;
            _location = location;
        }
    }
}