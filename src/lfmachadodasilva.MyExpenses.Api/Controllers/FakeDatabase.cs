using System;
using System.Collections.Generic;
using lfmachadodasilva.MyExpenses.Api.Models;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    public class FakeDatabase
    {
        public static ICollection<LabelDto> Labels;
        public static ICollection<PaymentDto> Payments;
        public static ICollection<GroupDto> Groups;

        public FakeDatabase()
        {
            if (Labels == null || Payments == null || Groups == null)
            {
                Random rnd = new Random();

                Labels = new List<LabelDto>();
                Payments = new List<PaymentDto>();
                Groups = new List<GroupDto>();
                for (int i = 0; i < 20; i++)
                {
                    Labels.Add(new LabelWithValuesDto
                    {
                        Id = i,
                        Name = $"LabelName{i}",
                        CurrentValue = rnd.Next(1, 250),
                        AverageValue = rnd.Next(1, 250),
                        LastValue = rnd.Next(1, 250),
                        GroupId = rnd.Next(0, 5)
                    });
                    Payments.Add(new PaymentWithValueDto
                    {
                        Id = i,
                        Name = $"PaymentName{i}",
                        CurrentValue = rnd.Next(1, 250),
                        AverageValue = rnd.Next(1, 250),
                        LastValue = rnd.Next(1, 250),
                        GroupId = rnd.Next(0, 5)
                    });
                }
                for (int i = 0; i < 5; i++)
                {
                    Groups.Add(new GroupDto
                    {
                        Id = i,
                        Name = $"GroupName{i}"
                    });
                }
            }
        }
    }
}
