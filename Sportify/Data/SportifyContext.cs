﻿using System;
using System.ComponentModel.DataAnnotations;
using Sportify.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sportify.Data
{
    public class SportifyContext : DbContext
    {
        public SportifyContext(DbContextOptions<SportifyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutType> WorkoutTypes { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Добавляем начальные данные в типы тренировок
            modelBuilder.Entity<WorkoutType>().HasData(
                new WorkoutType { Id = 1, Title = "Кардіо", Description = "Вправи, що підвищують пульс і покращують роботу серця та легенів. Приклади: біг, ходьба, плавання, велоспорт, стрибки на скакалці. Вони допомагають спалювати калорії, підвищують витривалість і зміцнюють серцево-судинну систему.", ImageBase64 = "Згодом!!!!" },
                new WorkoutType { Id = 2, Title = "Силове", Description = "Тренування, спрямовані на зміцнення м''язів та підвищення їх сили. Основні види: вправи з вагою тіла (віджимання, присідання), заняття з гантелями, штангою або на тренажерах. Силові вправи допомагають наростити м''язову масу, покращують обмін речовин та підвищують загальну фізичну витривалість.", ImageBase64 = "Згодом!!!!" }
            );

            // Пароль: IvanIva12345
            User us1 = new User { Id = 1, UserName = "Іван І.", Email = "ivanivanchecko@gmail.com", Password = "$2a$11$H205KOxYKHc2rN5MajjyP.txIokyhcQRV95w.AQ/7/SkR/yLzvuqO", PhotoBase64 = "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", Gender = "Чоловік", Age = 25, Height = 179, Weight = 73.6 };

            // Пароль: OlgaKar12345
            User us2 = new User { Id = 2, UserName = "Ольга К.", Email = "olgakarpenko@gmail.com", Password = "$2a$11$IAArTmUY8k85vC20uRQPv.uQr493Dt9jbJPaD2FZXdclD7vtMxsfW", PhotoBase64 = "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", Gender = "Жінка", Age = 23, Height = 173, Weight = 67.1, Role = "trainer" };

            // Пароль: DmytroPod12345
            User us3 = new User { Id = 3, UserName = "Дмитро П.", Email = "dmytropodil@gmail.com", Password = "$2a$11$h4n7fBf9zlPSSRvtFCVdm..y0UytBUgni71uqubJEw0XLFVHA0E2a", PhotoBase64 = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxISEhUTExMWFRUVFxkWGBUXFRUdGBcXFRcaGBUaFxUYHSggGBolGxcXITEhJSkrLi4uFx8zODMsNygtLisBCgoKDg0OGhAQGi0lHR0tLS0vLS0tLSstLS0tLS0tLS0rLS0tLS0tLS0tLy0tLS0tLS0tLS0tLSstLS0tLS0tK//AABEIAOEA4QMBIgACEQEDEQH/xAAcAAEAAQUBAQAAAAAAAAAAAAAABgIDBAUHAQj/xABAEAABAgIHBQUGBAUDBQAAAAABAAIDEQQFEiExQVEGYXGB8CKRobHBBxMyQlLRcpLh8RQjYoKyFVPSFjNjc8L/xAAZAQEAAwEBAAAAAAAAAAAAAAAAAQIDBAX/xAAiEQEBAAICAgICAwAAAAAAAAAAAQIRAyESMTJBBBNRYXH/2gAMAwEAAhEDEQA/AO4oiICIiAiIgIiICIo3tdtpRavaDFdN7vhhtvcb5E6ABNkm0jJUVr/2h1fRJh8YPeDKxC7Tgd8rh3rhW1/tFplNcZvMOHlCY4ho/ERK2eKhrohUd1bUjttY+3IXiDRc7jEflva37rQH2z0+0TKHIzk2zhNoGO4385XrlyJo267R/bdSg1ofBhOIPad2haErpAYGc1tqo9t7SJUijmf1Q3Y8Wuw71wxJyTQ+rql2+q+lSDI4a5xkGRAWkndO44qTr4wg0lzSCCRJT3Zb2qU2iya53voYusRCSQJ/K/EHjPgmzUfSSLQ7JbWUesIZfBJBb8THStNnuBvG9b5SqIiICIiAiIgIiICIiAiIgIiICIiAiIg0W2m0DaDRHxyJn4WXfO4GzPdNfKlY058Z7nvcS5zi4knNxmV1H277TF8cUNhIbBE3yPxPeAQCNwlzJ0XJmMLjIBV/teT6UAK7Do5OAUvqHYePFAcWyB1uU9qf2fwmSLzM6D9VneWNZw37cfh1U8icisiHUUQ5dSXe4Oy9HEpsnxWZ/wBOwP8AbCp+2r/pj51fUzxdLDFY8erXtxaea+jnbMUcmfuxqrNM2Uo0QEFnMJ+2p/VHzW6EV5ZXbK09msEzsPLeImFC662EjQQS0WhqPsrzlil4b9I3UNcRaLFbFhOLXNMwR6jP9V9PbD7SNrCisjXB/wAMRo+V4xzwNxE9V8pxoRaSCJELpfsMr73VLMBxk2OJCf1tvaPMc1pKxs+n0CiIrKCIiAiIgIiICIiAiIgIiICIiAvCV6sKu4/u6PGf9EJ7vysJQfKm1lONIpkeLOfvIr3D8JcbPhIKU+znZxjyYzxOyeyDhPXeohBo5dElvXY9iaLZgtHNc/Jl1p1cWPe0nosG5bKFCVuCy5ZDVjpvtW1iuBq8aVWrRS1TZVMRiuqlyIlYUZkwtbHgrdOasOLDmq2NJXI/aJs4104rBJzcdD+qgVU0h0GPDiNudDe1wOhaZ4cl2va6B/LcNQuNU+j2HrfjvWmHLj3t9bUaMHsa8YOaHDgRMK4tHsPHL6voriZn3LAT+EWfRbxbuWiIiAiIgIiICIiAiIgIiICIiAtJtvEs0ClGcv5Th+YS9Vu1GfaU8iraRL6Wjve0HwUX0me3A6oonbmV1rZuDZhtniub1AycQZ6ro9BjSAXJyXt3cc6SVhV1pWHDdgsqGVVbS+Cq2lWwVdAVpFK9mqSvVQ5ylEeOViIrhKsxnKKvEc2lh2mOAxkuSVvQ5knQyXWq0N5UFr+jidoYHzTjvaOSdOt+zZ86to25rm9z3BSZQz2TRZ1eB9MR4G7A+qma656cOXsREUoEREBERAREQEREBERAREQFH9voVur6SP6J/lcD6KQLTV5TIL4UaC58iWOaSAbiWm6csVGVki2MtvThmz1z5blLYVPZCkXukJ3bzuUXqGF/OlK8AgrNrehmI8TJstGA1OK5M/e3dx3rTeUjbVjHSY2YAmXFUQfaQ0HtMu13KJuoNGb2XFzzOUpm86SzPBa6OIJtBsGQaZXuaHYyuaSHOzwU4zfqIzuvdjqlX7e0SK4NtyO9SajUwPE2mYXCqHVTSboZE5HF05HAkaHW8LpOxFIMjDOLfJVt70v49bS+kUiwCTgFDq39oEKEZBpdldqpFtGJQXcFzKnVXabOwNbTpyE+sEuWqYYeUX6Z7TXn4QG6TVui7axHEOc68HCeIzC0MaC2Ce2wyxFqTAZiYADWOMzgJyWxo9LhymWuhX2RbaHMmMiQAQr2XW9KzW9bSCk7Tw4pa03B909D+8lrq9bJrQdT4Kg1a2K21ZAOM24cQs6vWfy2OInrxIUYaRySxOPZM2VCd/7nf4MU1XOtiKxfBoYDWtE3OfM6GQwyw8VNajrVtJhW2yuJa6WFoabrwt8M5enLnx5SeX02CIi0ZCIiAiIgIiICIiAiIgIiIKIz7LSdAT3BclolOjNivt9qGXkTleDPE7iV1anCcN41Y7yK5vT6G9zA1jiA90zLQnCa5uf6eh+FJZlP500FVUUfxMU5TPipIaqD2netHAgmDFLXYuAcDuvHopZVcYEBZXtadWohDqIMjWy2ZGG5Zo2fgPfaILbyZBrTIn4pPImJ7tVM/wCGaclcbRmjJWwuWPqozmGXuNAKpY6y4tIsyDQJCQGA1l91lUChhsa0BKYvkttGkAsaiXvUX2tL0v1vCtQyDotX/pzHtaHAmzhIyW7p17CNywaEclNnaMb01FLqtjpWmumLg4OkZaTAvHHVa6l1K0tshgAE9+OJOp3qbe6C8MMaKbbrW0Txl3pFKqqYMbeMFhV3RQYTwMr1LqSQAojXVJstcd0lXGapndsCitdGhNgtdZaG9q8TOglop37PYBhwojf6gfOfko3QqjDbJzaPRTDZMf8AdGhaPA/dW4vmvz6nBZP6SBERdbyxERAREQEREBERAREQEREFMRswRqJKF0Sy1pBxa4y75jwIU2UQ2go/un2rM2uPeDiOI8pLHln26/xcpLcb9oZtXHE2RBix1l34HXCfMDvWwqilYHVWa/oLIkF9ltmYJnnMXjxWk2Up1tgByWE7jfPUy6dKo8SYWS1y0tXxrpLZwHoFLF0gceiVaq2FJa7aesjAsvkSJEGQJxwuCjNX7ZFr+0xwbqLx6EdyiXteY7xdJjMuWnYLLzfdotHWO2ADDZBJyDRMngtNVu08RzrLoRBdcLy51+oDZDvVsrKjHC67dKYblbivVMJ/YGsh5LHjxrk2pphVjHuUKrakWo0KHee1aIAxDb5cyJc1v61pcg4nJRao3iLS3uya2QPE/onqbR7siXVfS3ltogieov7ipVsmw2Yjj8xHr6SUdgQXRHsaCXSOH3U4oFFEJgbniTqTj1uV+HHvZ+TyTx8f5ZKIi6XniIiAiIgIiICIiAiIgIiICt0iA17S14BByPVyuIg0TtloBMyXlubJiR3XCcua4/WLRRKfSIY7LRFeWtFwDXG00AZCRXfVxL2s0X3VYB9m6LDa6eRImx3OQHesssZJ02w5Mre63VUVm1+Bw9VujTgxsyVymgVh7qKLM7M79CdeH2UyprTFIInINBABxJXNl07MO2XWNamKC1uN8jyIn1otIyqY5YAIRcR/Sd95V+i0yPDubR3T+qTD3EuWxh1xSpTMCLdmLJ8GkpG2PHa0sagRQRKE+YF8mm4zlz/ReUARYcQPIM5iYvv5LcN2ljxLhCjXf+MgnmseNXEf5qO8y1a0nwM0WvDlIkECvYZAtEDiq6ZSQATkoTHo8SPf7t0MAYOl5KqvKxdCorWz7REk+2GXUYW0Fal5LWnhvliugeyuq4bqCXRGNd7yK5wmAcA1l2l7T3rjzjdOd+A8j4r6G2PoXuaFR4ekME8X9s+Liujjjj5cmyo1EhwxJjGt4DHic1fRFswt2IiICIiAiIgIiICIiAiIgIiICIiAuee2erg+jQ4wHahPI/teL/FoXQ1GvaFBtUQj+tviCPVVy9LYfKOBQos3CeXoukbP0tpAzuC5pTqMYTi3C9SDZumWTInLLBc2cdvHXRxuC1tMrwQScQQrtW04OMvNX6yqmBGE3zM5C4kXHO7gVnNtt6aiHti1xlLmtjRKY19/oseFshRQ+bZi6fxG49+7xWXGobYUwDLTrjNTdnnbNbVR3gNM/Fc82rpAc4DIdeq3tb1iJYyCg1Y0q0TnepwnbPkvTZ7N1eaVSYEAD4nC1+Edpx/KCV9IASuXGvY7QLNIER2JY6W4SXZl1YenFyexERXZiIiAiIgIiICIiAiIgIiICIiAiIgLRbaNnRT+Jvmt6tJtg4fw5Grm+arn8ath8o5DXtVh40KjLXxIUSV895EuIXSqRRrQUdrKrGm5wnpqN4K5du2xi0GtJSJNx9L1tzXwIlO7PgAbut6icarogubJwGRMj191hRaPSTOUJwSYp89RNaBXeLnEzIljnifAkd69p9eFzRflnlfdPu8VBoFHpQkPduuM/v5eKyhApLriyzxIl5pcUTNfrynW7hoqKoqm24OcOzOYBzP281l1dU94L+0dMueqlNCou5TvSNbb7YCFZpA/A70XRlAdlJNpLN4cPAqfLfi+Ln5vkIiLRiIiICIiAiIgIiICIiAiIgIiICIiAo7tT24bpfIW+d/n4Lc0mmNYbM+0cG589AtdDZOYdeHCR5qLNxON1domxlywabRAQt7SaEYbi05YbxkVjRoUwuSzTvl2iMSrzNWRQHKSmCJr0UdEI3/AvVbKrKkrYAAVAhzTRbWpo1DkVt4UCQVUKBes1sJErVUwz79sspnuBU/gxLTQ7UKL1RRZB0Q59kevoO9barqVZBaZ6hdPHNYuTmu8m1RWoNIa+dlwMsRmOIxHNXVdkIiICIiAiIgIiICIiAiLx7gBMkAalB6i1dMr2EycjbO7D8y0VMrmLE+YNGjTj/dip0JJTa1hQrnOv+kXn9Oaj9Y7QveP5YsNzM+0b9cuS1Bb111eqIIkXAjGbgNx+Lcb546hTobmpIZPbOJu+/H9FuobFgVK3+W2ePan+YragILNKowiNkbiMD6HctBHgEEtIkRkpIVbpVFbFEjc4YOGI+4WXJhvuNePk8er6RGJBkVcbDWVT4DmGTxLRwwPNUwdFz6+nVvrbFdAQQlnSCtuao0bW4ELNZcCAXust5n6R91TRoTohss5uyC3sCjthNst5nMnetMMPJnyZ+P+qSwABrcBcFjOFlwKylbpMObV1RyMKuLLW28HggNcLjwmser9pXgyiC0NcCOt6xq3pRiGzKQaO853dYLVR3WBdibg3UnAdb00J9Q61hRfhcOBWauZQjYleRdrottQq+iQ85gZHRRoTdFqKHtBCfc7snwW1Y8ETBBGoUCpERAREQFZpNKZDE3uA8+5R6tNoyezCuH1HHkMuK0MVznEkkknUzU6Eip20n+03+4/ZaOlUl8Qzc5zufkMlYBOksLpa3+oVwN1v66vUimRPDrrkqrEruXoqmN8PAXfqF6GCcrt9/XU1IpAmMfPzXkdkwC03gzB9Dul1MKtwkLp+HgFUctOXrx8FA2mzVKtBzDc5sjI4gEX8sL9630lEKPFEKI2J9MwZZsPxDfrxAUwG68IKS1eNVxWqRFYwFz3BrRiXEAd5QVxYTXiy4Ag5FaePUzm3w+036Se0OBzHFW37VwSbMMOf/UGkN7zeVkQqa6JnIdwHFUywmS+Odx9NXFcG3OmDoRIq7QaviRjaPYZrmeA9VsItYQ7MiWukfml3yVJ2kgtuc4cp+SznD321vPddTttIEBrG2WiQHV6oilYVH2hor7hGYDo42f8pT5LPaJ3i/eto57VAYqi26SrkrFNj+7hvfm1pI3uwaO+SlCJU6K0PiOLhK0QORkPL97lgtYXG28GUrm4SBxJGpF0tLtVcZDNxcbThho26+zv343jgqpmUseEp43eXgpStkT14dc0sDDrxVyW707+s15LU936oLbhpj4T5c1fotOiw72uI8lbigdDU6q3fOWGOGPf1ggkVB2qNwiNnvbj9lvKHXEGJcHyP0uuP2PJQCyVQRPDHmo0OoouYfxTt/eijQyGNkq5AcurlUBoBLSa8iDL9+CkVNBOPddnu70aBwwn1lmvQ3lf+/qqmjLdj1wkgZdHrNJD9+t6qDd/cOuiqS4euHXQUj0kDXw/dAcvVGC/Ddf6+K8Djdj69YFQLgAM5ra0WuBDhBrmOcW3CVnD5byRhhyWpYSMBhr11eqQ7Lrr9c0HlPr2nPJENsKE3c608/3EWQZZWTxzWljQ7bgYznl07jFnLH5STZzNwyIW9ccvTrqStyuwBB3oNYBYAAE8c7jd44BetpzhkR37vsFkRKvZgG2fwEtkf7Zb+grX+m6PeN02+bgTkpFxtIc684deqtxaNax0z5eirZV8vmfjO+wPENF+Heqv4Jn9R323eTT1MINZGqoZyA10vJPBW6LAiQzOBEiCd82OIF9+M7Jy18VuW0NgN7G3fUJy5m/LHir9nlz06PioFugVxWDfiMKK3RwId+dsv8Vs6ZWro0MNLBDM5mTw4GWEjITE77wMAtdO+U+V/p1edyqc6X3nL06u1QeOv3ddDkqZDfPqfl4oQdR114FU2J8c9c9Or0Hto5nrgqTMn7jz8FUG/fDVUlt+PXp+6kUmU93W+X3mqXcirpHQ8etypIHQnhv6zQW7ON/j6ZZ9ypN27dfz63K4XbufGUsCvSABj9+se9ELczoPyuXir927d1yRBlNxHH7LwYcgiKEq4WCHEdar1EHkT4hwHoqYvxfm8yvUUoXPl/L5hYxwPB3qiKErzsRxP+L143HrREUiqDg7gfJeH4T185REFlmXXyhX/l7v/lERCzDx60Kp+cdZtREFwdd4XmnD0aiIKKN6ejVff8J5eb16iCmH8vAf4Ky3Lh/yRESuNxPPzKpiYjiPIoiC8MeXq1Wofxd3+Ll4iIYrsTxPqnz9ahEUClEREv/Z", Gender = "Чоловік", Age = 27, Height = 180, Weight = 75.0, Role = "admin" };

            Exercise ex1 = new Exercise { Id = 1, Name = "Присід", Description = "Вправа, що збільшує витривалість та силу ніг. Виконується у положені стоячи, згибаючи та розгибаючи коліна", UserId = 2, Unchanged = true };
            Exercise ex2 = new Exercise { Id = 2, Name = "Підтягування", Description = "Вправа на покращення силової якості верхні широкі м'язи спини (також має вплив на біцепс, грудні та плечові м'язи). Виконується на поперечені, згибаючи руки у локтях та плечах", UserId = 3, Unchanged = true };
            Exercise ex3 = new Exercise { Id = 3, Name = "Зкручування", Description = "Вправа, що розвиває м'язи живота. Викунується у положені лежачи, піднімаючи верхню частину тіла у вертикальне положення (або близько до нього) ", UserId = 2, Unchanged = true };
            Exercise ex4 = new Exercise { Id = 4, Name = "Біг на 100 метрів", Description = "Біг на відстань 100 метрів за найменший можливий час", UserId = 1, Unchanged = false };

            // Добавляем начальных пользователей
            modelBuilder.Entity<User>().HasData(
                us1, us2, us3
            );

            // Промежутачная таблица
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Exercises)
                .WithMany(e => e.Workouts)
                .UsingEntity<Dictionary<string, object>>(
                    "ExerciseWorkout",
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey("ExerciseId"),
                    j => j.HasOne<Workout>().WithMany().HasForeignKey("WorkoutId")
                );


            modelBuilder.Entity<Workout>()
                .HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Заполняем упражнения
            modelBuilder.Entity<Exercise>().HasData(
                ex1, ex2, ex3, ex4
            );

            // Заполняем тренировки
            modelBuilder.Entity<Workout>().HasData(
                new Workout
                {
                    Id = 1,
                    Name = "Ранкове кардіо",
                    Date = new DateTime(2024, 12, 15, 7, 30, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 5,
                    IsCompleted = true,
                    UserId = 1
                },
                new Workout
                {
                    Id = 2,
                    Name = "Вечірня силова",
                    Date = new DateTime(2024, 12, 18, 18, 0, 0),
                    WorkoutTypeId = 2,
                    WorkoutGoal = "Набір",
                    Complexity = 8,
                    IsCompleted = true,
                    UserId = 2
                },
                new Workout
                {
                    Id = 3,
                    Name = "Середнє кардіо",
                    Date = new DateTime(2025, 1, 5, 9, 0, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 6,
                    IsCompleted = false,
                    UserId = 3
                },
                new Workout
                {
                    Id = 4,
                    Name = "Важка силова",
                    Date = new DateTime(2024, 12, 20, 19, 30, 0),
                    WorkoutTypeId = 2,
                    WorkoutGoal = "Набір",
                    Complexity = 10,
                    IsCompleted = true,
                    UserId = 1
                },
                new Workout
                {
                    Id = 5,
                    Name = "Легка кардіо-розминка",
                    Date = new DateTime(2025, 1, 3, 8, 0, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 3,
                    IsCompleted = false,
                    UserId = 3
                },
                new Workout
                {
                    Id = 6,
                    Name = "Інтенсивне кардіо",
                    Date = new DateTime(2024, 12, 10, 6, 45, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 7,
                    IsCompleted = true,
                    UserId = 1
                },
                new Workout
                {
                    Id = 7,
                    Name = "Силова для новачків",
                    Date = new DateTime(2024, 12, 22, 19, 15, 0),
                    WorkoutTypeId = 2,
                    WorkoutGoal = "Набір",
                    Complexity = 4,
                    IsCompleted = true,
                    UserId = 2
                },
                new Workout
                {
                    Id = 8,
                    Name = "Кардіо на витривалість",
                    Date = new DateTime(2025, 1, 7, 8, 30, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 6,
                    IsCompleted = false,
                    UserId = 3
                },
                new Workout
                {
                    Id = 9,
                    Name = "Силове підняття ваги",
                    Date = new DateTime(2024, 12, 28, 17, 0, 0),
                    WorkoutTypeId = 2,
                    WorkoutGoal = "Набір",
                    Complexity = 9,
                    IsCompleted = true,
                    UserId = 3
                },
                new Workout
                {
                    Id = 10,
                    Name = "Розслаблююче кардіо",
                    Date = new DateTime(2025, 1, 2, 10, 0, 0),
                    WorkoutTypeId = 1,
                    WorkoutGoal = "Схуднення",
                    Complexity = 3,
                    IsCompleted = false,
                    UserId = 1
                }
            );

            modelBuilder.Entity("ExerciseWorkout").HasData(
                new { WorkoutId = 1, ExerciseId = 1 },
                new { WorkoutId = 1, ExerciseId = 2 },
                new { WorkoutId = 2, ExerciseId = 3 },
                new { WorkoutId = 2, ExerciseId = 2 },
                new { WorkoutId = 3, ExerciseId = 1 },
                new { WorkoutId = 3, ExerciseId = 3 },
                new { WorkoutId = 4, ExerciseId = 2 },
                new { WorkoutId = 4, ExerciseId = 3 },
                new { WorkoutId = 5, ExerciseId = 1 },
                new { WorkoutId = 6, ExerciseId = 1 },
                new { WorkoutId = 6, ExerciseId = 3 },
                new { WorkoutId = 7, ExerciseId = 2 },
                new { WorkoutId = 8, ExerciseId = 1 },
                new { WorkoutId = 8, ExerciseId = 2 },
                new { WorkoutId = 8, ExerciseId = 3 },
                new { WorkoutId = 9, ExerciseId = 1 },
                new { WorkoutId = 9, ExerciseId = 2 },
                new { WorkoutId = 9, ExerciseId = 3 },
                new { WorkoutId = 10, ExerciseId = 2 },
                new { WorkoutId = 10, ExerciseId = 4 }
            );

        }
    }
}