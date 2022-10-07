using BudgetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Repository
{
    public class DbSeeder
    {
        public static void InitializeDb(BudgetDbContext context, ILogger<DbSeeder> logger)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Przeprowadzanie zaległych migracji");
                context.Database.Migrate();
            }

            if (context.Items != null && context.Categories != null && context.Items.Count() == 0 && context.Categories.Count() == 0)
            {
                logger.LogInformation("Baza danych pusta, przystępujemy do zasilenia przykładowymi danymi");

                Category catSalary = new Category { Name = "Wypłata" };
                Category catBills = new Category { Name = "Opłaty" };
                Category catKids = new Category { Name = "Dzieci" };
                Category catShopping = new Category { Name = "Zakupy" };
                Category catCulture = new Category { Name = "Kultura" };

                context.Items.AddRange(new Item[]
                {
                    new Item
                    {
                        Name = "Wypłata z Instytutu Sekcji Zwłok",
                        DateTime = new DateTime(2022, 08, 26),
                        Expense = 3500,
                        Category = catSalary
                    },
                    new Item
                    {
                        Name = "Wypłata żony z Prosektorium",
                        DateTime = new DateTime(2022, 08, 25),
                        Expense = 2500,
                        Category = catSalary
                    },
                    new Item
                    {
                        Name = "Zwrot z Towarzystwa Budowlanego",
                        DateTime = new DateTime(2022, 08, 13),
                        Expense = 1000,
                        Category = catSalary
                    },
                    new Item
                    {
                        Name = "Wypłata żony z Prosektorium",
                        DateTime = new DateTime(2022, 07, 26),
                        Expense = 2500,
                        Category = catSalary
                    },
                    new Item
                    {
                        Name = "Zajęcia z języka Suahili dla dzieciaków",
                        DateTime = new DateTime(2022, 09, 12),
                        Expense = -500,
                        Category = catKids
                    },
                    new Item
                    {
                        Name = "Strój Batmana dla małego Miecia",
                        DateTime = new DateTime(2022, 07, 26),
                        Expense = -350,
                        Category = catKids
                    },
                    new Item
                    {
                        Name = "Buty dla małej Dżesiki",
                        DateTime = new DateTime(2022, 06, 20),
                        Expense = -150,
                        Category = catKids
                    },
                    new Item
                    {
                        Name = "Nauka pływania dla dzieci",
                        DateTime = new DateTime(2022, 06, 18),
                        Expense = -500,
                        Category = catKids
                    },
                    new Item
                    {
                        Name = "Opłata za telefon i internet",
                        DateTime = new DateTime(2022, 09, 04),
                        Expense = -160,
                        Category = catBills
                    },
                    new Item
                    {
                        Name = "Opłata za TV",
                        DateTime = new DateTime(2022, 09, 04),
                        Expense = -50,
                        Category = catBills
                    },
                    new Item
                    {
                        Name = "Opłata za czynsz",
                        DateTime = new DateTime(2022, 09, 03),
                        Expense = -580,
                        Category = catBills
                    },
                    new Item
                    {
                        Name = "Fundusz remontowy",
                        DateTime = new DateTime(2022, 09, 04),
                        Expense = -20,
                        Category = catBills
                    },
                    new Item
                    {
                        Name = "Dżem z czarnej porzeczki",
                        DateTime = new DateTime(2022, 09, 01),
                        Expense = -10,
                        Category = catShopping
                    },
                    new Item
                    {
                        Name = "Serek koryciński",
                        DateTime = new DateTime(2022, 09, 01),
                        Expense = -10,
                        Category = catShopping
                    },
                    new Item
                    {
                        Name = "Pieczywo",
                        DateTime = new DateTime(2022, 09, 01),
                        Expense = -35,
                        Category = catShopping
                    },
                    new Item
                    {
                        Name = "Perfumy Pani Walewska",
                        DateTime = new DateTime(2022, 08, 17),
                        Expense = -55,
                        Category = catShopping
                    },
                    new Item
                    {
                        Name = "Płyta Zenka Martyniuka z zespołem Autechre",
                        DateTime = new DateTime(2022, 09, 06),
                        Expense = -65,
                        Category = catCulture
                    },
                    new Item
                    {
                        Name = "Książka 365 Twarzy Greya",
                        DateTime = new DateTime(2022, 09, 06),
                        Expense = -35,
                        Category = catCulture
                    },
                    new Item
                    {
                        Name = "Album Tycjana",
                        DateTime = new DateTime(2022, 06, 11),
                        Expense = -125,
                        Category = catCulture
                    },
                    new Item
                    {
                        Name = "Wyjście do kina",
                        DateTime = new DateTime(2022, 07, 16),
                        Expense = -50,
                        Category = catCulture
                    },
                    new Item
                    {
                        Name = "Wyjście do teatru",
                        DateTime = new DateTime(2022, 07, 19),
                        Expense = -40,
                        Category = catCulture
                    }
                });
                context.SaveChanges();
                logger.LogInformation("Baza danych zasilona");
            }
            else
                logger.LogInformation("Baza danych została już wypełniona przykładowymi danymi");
        }
    }
}
