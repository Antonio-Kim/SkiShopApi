## Ski Shop API

![main](https://github.com/Antonio-Kim/SkiShopApi/actions/workflows/backend.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0.x-blue)

---

- This API uses cookies, which needs to be enabled inside Program.cs but also inside the controller. While this is not necessary for production sake, the CORS for cookies needs to be enabled for development, and this is added in app.UseCors():

```csharp
app.UseCors(opt =>
{
    opt.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowCredentials()
		.WithOrigins("http://localhost:3000");
});
```

Here are the steps for dockerizing the App.

1. Create production client and store into wwwroot
2. Inside the program.cs file, you will need to add the following. Note the order matters so see the program.cs file for this:

```csharp
// convert to PostgreSQL in service
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Configuring middleware
app.UseDefaultFiles();
app.UseStaticFiles();

// the middleware re-routes to Index controller endpoint as fallback. See the controller for more info
app.MapControllers();
app.MapFallbackToController("Index", "Fallback");
```

Ensure that appsetting.json has both jwt and PostgreSql routed properly to docker image. Note that instead of localhost, you will need to route to docker's path:

```json
"DefaultConnection": "Server=host.docker.internal
```

Create the dockerfile as shown inside here and type the following:

```bash
docker build -t <username/app_name> .
docker run --rm -it -p 8080:8080 <username/app_name>
```
