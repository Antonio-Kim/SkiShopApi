using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public static class DbInitializer
{
	public static async Task Initialize(StoreContext context, UserManager<User> userManager)
	{
		if (!userManager.Users.Any())
		{
			var user = new User
			{
				UserName = "bob",
				Email = "bob@test.com"
			};

			await userManager.CreateAsync(user, "Pa$$w0rd");
			await userManager.AddToRoleAsync(user, "Member");

			var admin = new User
			{
				UserName = "admin",
				Email = "admin@test.com"
			};

			await userManager.CreateAsync(admin, "Pa$$w0rd");
			await userManager.AddToRolesAsync(admin, ["Member", "Admin"]);
		}
		if (context.Products.Any()) return;

		var products = new List<Product>
		{
			new Product
			{
				Name = "Ski Set - Black",
				Description =
					"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 20000,
				PictureUrl = "/images/products/ski-black.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Set - Blue",
				Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
				Price = 15000,
				PictureUrl = "/images/products/ski-blue.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Set - Grey",
				Description =
					"Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
				Price = 18000,
				PictureUrl = "/images/products/ski-grey.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Set - Orange",
				Description =
					"Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
				Price = 30000,
				PictureUrl = "/images/products/ski-orange.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Set - Red",
				Description =
					"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 25000,
				PictureUrl = "/images/products/ski-red.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Set - Yellow",
				Description =
					"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 22000,
				PictureUrl = "/images/products/ski-yellow.png",
				Brand = "Speedster",
				Type = "Ski",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Beanies - Blue",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1000,
				PictureUrl = "/images/products/beanies-blue.png",
				Brand = "Nique",
				Type = "Beanies",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Beanies - Green",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1500,
				PictureUrl = "/images/products/beanies-green.png",
				Brand = "Nique",
				Type = "Beanies",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Beanies - Pink",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1500,
				PictureUrl = "/images/products/beanies-pink.png",
				Brand = "Nique",
				Type = "Beanies",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Beanies - Purple",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1800,
				PictureUrl = "/images/products/beanies-purple.png",
				Brand = "Nique",
				Type = "Beanies",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Beanies - Red",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1500,
				PictureUrl = "/images/products/beanies-red.png",
				Brand = "Nique",
				Type = "Beanies",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Boots - Navy",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 16000,
				PictureUrl = "/images/products/boots-navy.png",
				Brand = "Adibas",
				Type = "Boots",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Boots - Pink",
				Description =
					"Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 14000,
				PictureUrl = "/images/products/boots-pink.png",
				Brand = "Adibas",
				Type = "Boots",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Boots - Red",
				Description =
					"Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
				Price = 25000,
				PictureUrl = "/images/products/boots-red.png",
				Brand = "Adibas",
				Type = "Boots",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Boots - Yellow",
				Description =
					"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 18999,
				PictureUrl = "/images/products/boots-yellow.png",
				Brand = "Adibas",
				Type = "Boots",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Gloves - Blue",
				Description =
					"Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
				Price = 1999,
				PictureUrl = "/images/products/gloves-blue.png",
				Brand = "OverArmour",
				Type = "Gloves",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Gloves - Green",
				Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
				Price = 1500,
				PictureUrl = "/images/products/gloves-green.png",
				Brand = "OverArmour",
				Type = "Gloves",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Gloves - Red",
				Description =
					"Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
				Price = 1800,
				PictureUrl = "/images/products/gloves-red.png",
				Brand = "OverArmour",
				Type = "Gloves",
				QuantityInStock = 100
			},
			new Product
			{
				Name = "Ski Gloves - Teal",
				Description =
					"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
				Price = 1999,
				PictureUrl = "/images/products/gloves-blue.png",
				Brand = "OverArmour",
				Type = "Gloves",
				QuantityInStock = 100
			},
		};

		foreach (var product in products)
		{
			context.Products.Add(product);
		}

		await context.SaveChangesAsync();
	}
}