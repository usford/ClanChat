using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using ClanChat.Shared.Database;
using ClanChat.Shared.Database.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Desktop
{
    public partial class MainWindow : Window
    {
        private HubConnection _connection;
        private Guid _userId;
        private Clan _clan;
        private string _username;
        private string _token;

        private readonly ClanChatDbContext _dbContext;

        public ViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(ClanChatDbContext dbContext)
        {
            InitializeComponent();

            _dbContext = dbContext;

            ViewModel = new ViewModel();
            this.DataContext = ViewModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            using (var client = new HttpClient())
            {
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginData), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5240/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject result = (JObject) JsonConvert.DeserializeObject(responseContent)!;
                    _token = result["token"]!.ToString();
                    _userId = new Guid(result["userId"]!.ToString());
                    _username = result["username"]!.ToString();

                    await ConnectToHub();
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Вы не зарегистрированы");
                    return;
                }

                MessageBox.Show("Проблема с подключением к api");
            }
        }

        private async Task ConnectToHub()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5240/clanChatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token)!;
                })
                .Build();

            _connection.On<string, string, DateTime>("ReceiveMessage", (user, message, dispatchDate) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var userName = (_username == user) ? "Вы" : user;

                    ChatListBox.Items.Add($"[{dispatchDate}] {userName}: {message}");
                });
            });

            try
            {
                await _connection.StartAsync();
                MessageBox.Show("Подключение к чату!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к чату: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection == null) return;

            if (_clan == null)
            {
                MessageBox.Show("Ты не подключен к клану");
                return;
            }

            var message = MessageTextBox.Text;
            var timeDispatchDate = DateTime.UtcNow;

            await _connection.InvokeAsync("SendMessageToClan", _clan.Name, _username, message, timeDispatchDate);
            MessageTextBox.Clear();

            _dbContext.Chats.Add(new Chat
            {
                ClanId = _clan.Id,
                UserId = _userId,
                Message = message,
                TimeDispatchDate = timeDispatchDate
            });

            await _dbContext.SaveChangesAsync();
        }

        private async void JoinClanButton_Click(object sender, RoutedEventArgs e)
        {
            if (_connection == null) return;

            ChatListBox.Items.Clear();

            var clan = await _dbContext.Clans.FirstOrDefaultAsync(x => x.Name == ClanTextBox.Text);

            if (clan == null)
            {
                MessageBox.Show("Такого клана не существует");
                return;
            }

            
            if (_clan != null)
            {
                await _connection.InvokeAsync("LeaveClan", _clan.Name);
            }

            await _connection.InvokeAsync("JoinClan", clan.Name);

            MessageBox.Show($"Ты подключен к {clan.Name}");
            ViewModel.ClanNameText = $"Клан: {clan.Name}";

            _clan = clan;
            await GetLatestMessages(clan.Name, _username);
            ClanTextBox.Clear();
        }

        private async Task GetLatestMessages(string clanName, string userName)
        {
            var latestMessages = await _dbContext.Chats
                .Where(x => x.Clan.Name == clanName)
                .Include(x => x.User)
                .OrderBy(x => x.TimeDispatchDate)
                .Take(50)
                .ToListAsync();

            foreach (Chat chat in latestMessages)
            {
                var userNameMsg = (chat.User.Name == userName) ? "Вы" : chat.User.Name;

                ChatListBox.Items.Add($"[{chat.TimeDispatchDate}] {userNameMsg}: {chat.Message}");
            }
        }
    }
}