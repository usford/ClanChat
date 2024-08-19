using ClanChat.API;
using ClanChat.Shared.Database;
using ClanChat.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext(builder.Configuration);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ClanChatDbContext>();

    context.Database.Migrate();

    await services.SeedDataDb();
}

app.MapControllers();
app.MapHub<ClanChatHub>("/clanChatHub");

app.Run();
