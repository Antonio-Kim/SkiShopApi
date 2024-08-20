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
